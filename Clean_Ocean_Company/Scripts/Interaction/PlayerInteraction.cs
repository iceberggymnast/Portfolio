using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviourPun
{
    public enum RayType
    {
        Lobby, Main
    }

    public RayType rayType;

    public RawImage E_guageCircle;

    public GameObject rayStartObject; // Ray 가 발사 시작되는 오브젝트 위치. 이 경우는 플레이어 Model
    public GameObject interactionCanvas; // 상호작용 Canvas
    //private Slider outlineSlider; // 상호작용 Canvas를 감싸는 Slider
    Image outlineImage;
    Image InteractionImage;
    private TMP_Text interactionText; // 상호작용 오브젝트 name을 표시하는 Text
    private string interactionName; // 상호작용 오브젝트 name을 받을 string
    public float rayDistance = 5f; // ray 길이
    public bool pointHitTrue = false; // ray 가 충돌이 되었는지 체크
    private float currentTime = 0; // 상호작용 버튼(Space) 몇초 눌렀는지 체크

    // 레이 쏠 백터 offset
    public Vector3 offset = new Vector3(0, 1, 0);

    public Camera mainCamera;

    Vector3 rayPos; // Ray를 쏠 방향

    float defaultImageX = 0;

    public PhotonView pv;

    Interaction_Base lastInteraction;

    private void Awake()
    {
        interactionCanvas = GameObject.Find("InteractionCanvas");
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => interactionCanvas != null);

        outlineImage = interactionCanvas.transform.GetChild(2).GetComponent<Image>();
        outlineImage.fillAmount = 0;
        interactionText = interactionCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
        InteractionImage = interactionCanvas.transform.GetChild(0).GetComponent<Image>();
        
        RectTransform tr = InteractionImage.GetComponent<RectTransform>();
        defaultImageX = tr.sizeDelta.x;

        if (mainCamera == null)
        {
            mainCamera = PlayerInfo.instance.player.transform.GetChild(1).GetChild(0).GetComponent<Camera>();
            SwitchRayType();
        }
    }

    private void Update()
    {
        if (rayType == RayType.Lobby)
        {
            InteractionFun();
        }
        if (pv == null) return;

        if (pv.IsMine && rayType == RayType.Main)
        {
            InteractionFun();
        }
        
    }

    void SwitchRayType()
    {
        if (mainCamera == null) return;
        rayPos = rayType == RayType.Main ? mainCamera.transform.forward : rayStartObject.transform.forward;
    }


    //플레이어 앞 부분으로 레이를 쏴서 레이어에 따라, 상호작용(ui띄우는 건 동일함)
    public void InteractionFun()
    {
        SwitchRayType();
        RaycastHit raycastHit;



        Vector3 startRay = rayStartObject.transform.position + rayStartObject.transform.TransformDirection(offset);
        if (Physics.SphereCast(startRay, 0.5f, rayPos, out raycastHit, rayDistance, (1 << 10)))
        {
            var interaction = raycastHit.collider.GetComponent<Interaction_Base>();
            if (interaction != null)
            {
                if (lastInteraction != interaction || lastInteraction == null)
                {
                    lastInteraction = interaction;
                    interaction.player = gameObject;
                    #region interactionText 가 null일 경우
                    if (interactionText == null)
                    {
                        if(interactionCanvas == null) interactionCanvas = GameObject.Find("InteractionCanvas");
                        interactionText = interactionCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
                        interactionText.text = lastInteraction.intername;
                        if (InteractionImage == null)
                        {
                            InteractionImage = interactionCanvas.transform.GetChild(0).GetComponent<Image>();
                            RectTransform tr = InteractionImage.GetComponent<RectTransform>();
                            defaultImageX = tr.sizeDelta.x;
                        }
                        interaction.SetOutlineWidth(7);
                        ResetOutlineFill();
                        UpdateInteractionUI(interactionText.preferredWidth);
                        SetBillboardCamera();
                        HandleInteractionInput(lastInteraction);
                        return;
                    }
                    #endregion
                    interactionText.text = lastInteraction.intername;
                    interaction.SetOutlineWidth(7);
                    ResetOutlineFill();
                    UpdateInteractionUI(interactionText.preferredWidth);
                    SetBillboardCamera();
                }


                
                interactionCanvas.transform.position = interaction.spawnPoint != null ? interaction.spawnPoint.position : raycastHit.point;


                if (!interactionCanvas.activeSelf)
                {
                    interactionCanvas.SetActive(true);
                    SetBillboardCamera();
                    interaction.player = gameObject;
                    interactionText.text = interaction.intername;
                    interaction.SetOutlineWidth(7);
                    ResetOutlineFill();
                    UpdateInteractionUI(interactionText.preferredWidth);
                }

                HandleInteractionInput(lastInteraction);
            }
        }
        else
        {
            HideInteractionCanvas();
        }
    }

    // 헬퍼 메서드 정의
    void UpdateInteractionUI(float preferredWidth)
    {
        RectTransform textTr = interactionText.GetComponent<RectTransform>();
        RectTransform imageTr = InteractionImage.GetComponent<RectTransform>();
        if (preferredWidth > 74.12f)
        {
            float addWidthValue = preferredWidth - 74.12f;
            textTr.sizeDelta = new Vector2(preferredWidth, textTr.sizeDelta.y);
            imageTr.sizeDelta = new Vector2(281.39f + addWidthValue + 20, imageTr.sizeDelta.y);
        }
        else if(preferredWidth <= 74.12f)
        {
            textTr.sizeDelta = new Vector2(74.12f, textTr.sizeDelta.y);
            imageTr.sizeDelta = new Vector2(281.39f, imageTr.sizeDelta.y);
        }
    }

    void SetBillboardCamera()
    {
        var billBoard = interactionCanvas.GetComponent<Interaction_BillBoard>();
        if (PlayerInfo.instance.player != null)
        {
            var cameraRotate = PlayerInfo.instance.player.GetComponentInChildren<CameraRotate>();
            billBoard.SetUICamera(cameraRotate.uiCamera);
        }
    }

    void HandleInteractionInput(Interaction_Base interaction)
    {
        if (Input.GetKey(KeyCode.Space) && !interaction.useTrue)
        {
            currentTime += Time.deltaTime;
            outlineImage.fillAmount = Mathf.Clamp01(currentTime / interaction.time);

            if (currentTime >= interaction.time)
            {
                interaction.action();
                HideInteractionCanvas();
                interaction.useTrue = true;
                ResetOutlineFill();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            ResetOutlineFill();
        }
    }

    void HideInteractionCanvas()
    {
        if (interactionCanvas == null) return;
        if (interactionCanvas.activeSelf)
        {
            interactionCanvas.SetActive(false);
            pointHitTrue = false;
        }
    }

    void ResetOutlineFill()
    {
        if (outlineImage == null) return;
        currentTime = 0;
        outlineImage.fillAmount = currentTime;
    }

    public RaycastHit OutlineCheck()
    {
        Vector3 startRay = rayStartObject.transform.position + (rayStartObject.transform.TransformDirection(offset));
        RaycastHit raycastHit;

        Physics.SphereCast(startRay, 0.5f, rayPos, out raycastHit, rayDistance, (1 << 10));

        return raycastHit;
    }

    private void OnDrawGizmos()
    {
        if (rayPos == null)
        {
            SwitchRayType();
        }

        if (rayPos != null && rayStartObject != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayStartObject.transform.position + (rayStartObject.transform.TransformDirection(offset)), rayPos * rayDistance);

            RaycastHit raycastHit;
            if (Physics.SphereCast(rayStartObject.transform.position + (rayStartObject.transform.TransformDirection(offset)), 0.5f, rayPos, out raycastHit, rayDistance))
            {
                Gizmos.DrawSphere(raycastHit.point, 0.5f);
            }
        }
    }

}
