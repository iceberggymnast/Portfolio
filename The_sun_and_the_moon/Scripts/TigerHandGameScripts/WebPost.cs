using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.UI;

public class WebPost : MonoBehaviour
{
    bool sanding;

    void Start()
    {
        path = Application.dataPath + "/images/testimg.jpg";
        //PostImage();
    }

    void Update()
    {
        if (!sanding)
        {
            PostImage();
        }
    }

    public string path;
    public string url;
    public string text_response;
    public HandsPunchList handsPunchList;
    public CamTest camtest;

    public void PostImage()
    {
        StartCoroutine(PostImageRequest(url));
    }

    IEnumerator PostImageRequest(string url)
    {
        sanding = true;
        // 바이트 배열로 데이터를 읽어올 때
        byte[] imageBinaries = camtest.bytes;

        /*
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.SetRequestHeader("file", "application/jpg");
        //request.SetRequestHeader("Content-Type", "multipart/form-data");
        request.uploadHandler = new UploadHandlerRaw(imageBinaries);
        request.downloadHandler = new DownloadHandlerBuffer();
        */

        // WWWForm을 사용하여 파일 데이터 추가
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", imageBinaries, Path.GetFileName(path), "image/jpeg");

        // UnityWebRequest를 사용하여 HTTP POST 요청 생성
        UnityWebRequest request = UnityWebRequest.Post(url, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            text_response = response;
            if (handsPunchList != null)
            {
                handsPunchList.CreateFromJSON(text_response);
            }
        }
        else
        {
            text_response = $"{request.responseCode} - {request.error}";
            Debug.LogError($"{request.responseCode} - {request.error}");
        }
        sanding = false;

    }

}