using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;



public class AiProtocol : MonoBehaviour

{
    // 이미지를 보낼 url
    [Tooltip ("이미지를 보낼 서버 주소")]
    public string url;

    // url 뒤에 붙을 string
    [Tooltip("주소 뒤에 붙는 단어 ex) tether")]
    public string urlPath;

    // 통신의 결과 값
    [Tooltip("포스트를 하고 리턴 받은 json값")]
    public string text_response;

    [Tooltip("플레이어가 1번인가 2번인가")]
    [Range (1, 2)]
    public int actorId;

    // 이미지를 저장할 바이트 배열 변수
    byte[] bytes;
    // 웹캠 용 이미지
    public RawImage rawimage;  //Image for rendering what the camera sees.
    // 웹 캠 텍스쳐
    WebCamTexture webcamTexture = null;

    // 보내고 있는지 확인
    bool sanding;

    // 사용 할 웹 캠의 번호
    public int webCamNum = 0;

    // 불러온 캠 리스트
    WebCamDevice[] camDevices;

    public JsonToClass jsonToClass;



    void Start()
    {
        // JsonToClass 컴포넌트에 델리게이트에 추가
        jsonToClass.jsonReturnDel = ResponseReturn;

        //컴퓨터에 있는 웹 캠 리스트를 가져옴
        camDevices = WebCamTexture.devices;
        print("카메라 갯수 : " + camDevices.Length);

        // 캠이 하나라도 있으면..
        if (camDevices.Length != 0)
        {
            //Get the used camera name for the WebCamTexture initialization.
            string camName = camDevices[webCamNum].name;
            webcamTexture = new WebCamTexture(camName);

            //Render the image in the screen.
            rawimage.texture = webcamTexture;
            rawimage.material.mainTexture = webcamTexture;
            print("캠이 재생됩니다..");
            webcamTexture.Play();
        }
        else
        {
            print("캠이 없습니다.");
        }
    }

    void Update()
    {
        if (camDevices.Length != 0)
        {
            PostImage();
        }
    }

    // 포스트 할때 바이트 같이 저장
    public void PostImage()
    {
        if (!sanding)
        {
            RawImgToBytes();
            StartCoroutine(PostImageRequest(url + urlPath + actorId));
        }
    }

    // 로우 이미지를 바이트 배열로 변경
    void RawImgToBytes()
    {
        //Create a Texture2D with the size of the rendered image on the screen.
        Texture2D texture = new Texture2D(rawimage.texture.width, rawimage.texture.height, TextureFormat.ARGB32, false);

        //Save the image to the Texture2D
        texture.SetPixels(webcamTexture.GetPixels());
        texture.Apply();

        //Encode it as a JPG.
        bytes = texture.EncodeToJPG();
    }

    // 해당 주소로 Post 시도하고 리턴 받기
    IEnumerator PostImageRequest(string url)
    {
        // 코루틴이 실행중이면 또 실행하지 못하게 하기
        sanding = true;

        // 바이트 배열로 데이터를 읽어온다.
        byte[] imageBinaries = bytes;

        // WWWForm을 사용하여 파일 데이터 추가
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", imageBinaries);

        // UnityWebRequest를 사용하여 HTTP POST 요청 생성
        UnityWebRequest request = UnityWebRequest.Post(url, form);

        // 답신을 기다린다.
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            text_response = response;
        }
        else
        {
            text_response = $"{request.responseCode} - {request.error}";
            Debug.LogError($"{request.responseCode} - {request.error}");
        }
        sanding = false;

    }

    private void OnDestroy()
    {
        webcamTexture.Stop();
        webcamTexture = null;
        print("캠을 초기화합니다.");
        // UnityWebRequest를 사용하여 HTTP Get 요청 생성
        UnityWebRequest request = UnityWebRequest.Get(url + "reset");
        request.SendWebRequest();
    }

    public string ResponseReturn()
    {
        return text_response;
    }




}
