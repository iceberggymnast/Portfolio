using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Interaction_Base : MonoBehaviour
{
    // 플레이어는 해당 델리게이트로 접근해서 인터렉션 하면 됨

    PlayerInteraction interaction;

    public Action action;

    public Transform spawnPoint;

    public float time = 1.5f;
    public string intername = "분리수거 통";
    public bool useTrue = false;
    private GameObject _player;
    public GameObject player
    {
        get => _player;
        set
        {
            _player = value;
            OnPlayerChanged?.Invoke(_player); // player가 변경될 때 이벤트 발생
        }
    }

    // player 값이 변경될 때 발생하는 이벤트
    public event Action<GameObject> OnPlayerChanged;

    // 아웃라인 기능
    public Outline outline;
    private void Start()
    {
        outline = GetComponent<Outline>();
        transform.gameObject.layer = 10;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            useTrue = false;
        }

        if (outline.OutlineWidth > 0)
        {
            if (player == null)
            {
                interaction = null;
            }
            if (player != null)
            {
                interaction = player.GetComponent<PlayerInteraction>();
                RaycastHit hit = interaction.OutlineCheck();

                if (hit.collider == null || hit.collider.gameObject != this.gameObject)
                {
                    SetOutlineWidth(1);
                }
            }
        }
    }

    public void SetUseTrue()
    {
        useTrue = false;
    }

    public void SetOutlineWidth(float vlaue)
    {
       outline.OutlineWidth = vlaue;
    }
}
