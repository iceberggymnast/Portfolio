using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Prototype_UI : MonoBehaviour
{
    public GameObject player;
    //public GameObject monster;
    PlayerManager playerManager;
    //MonsterState monsterState;

    //public Text playerhp;
    //public Text monsterhp;
    public TMP_Text itemnumber;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = player.GetComponent<PlayerManager>();
        //monsterState = monster.GetComponent<MonsterState>();

    }

    // Update is called once per frame
    void Update()
    {
        //playerhp.text = playerManager.hp.ToString();
        //monsterhp.text = monsterState.hp.ToString();

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Cursor.lockState = CursorLockMode.None;
        //}
        itemnumber.text = playerManager.healingpostioncount.ToString();
    }
}
