#nullable enable
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows;

public class HttpManager : MonoBehaviour
{
 
    private static HttpManager _instance;

    // ReSharper disable Unity.PerformanceAnalysis
    public static HttpManager GetInstance()
    {
        if (_instance == null)
        {
            GameObject go = new GameObject();
            go.name = "HttpManager";
            var t  =go.AddComponent<HttpManager>(); // Awake will be called at this moment
            _instance = t;
        }

        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void GetJsonAsync<T>(HttpRequest<T> requestInfo)
    {
        StartCoroutine(GetJson<T>(requestInfo));
    }

    public void GetAsyncWithDH(HttpRequest<DownloadHandler> requestInfo)
    {
        StartCoroutine(GetWithDownloadHandler(requestInfo));
    }

    private IEnumerator GetWithDownloadHandler(HttpRequest<DownloadHandler> requestInfo)
    {
        using (var webRequest = UnityWebRequest.Get(requestInfo.Url))
        {
            yield return webRequest.SendWebRequest();
            // response success - result or error
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                requestInfo.OnComplete?.Invoke(webRequest.downloadHandler);
            }
            else
            {
                Debug.LogError($"Net Error: {webRequest.error}");
            }
        }
    }

    // Http GET Method => 데이터 조회
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator GetJson<T>(HttpRequest<T> requestInfo)
    {
        using (var webRequest = UnityWebRequest.Get(requestInfo.Url))
        {
            yield return webRequest.SendWebRequest();
            // response success - result or error
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                print(webRequest.downloadHandler.text);
                requestInfo.OnComplete?.Invoke(JsonUtility.FromJson<T>(webRequest.downloadHandler.text));
            }
            else
            {
                Debug.LogError($"Net Error: {webRequest.error}");
            }
        }
    }

    public void PostJsonAsync<T> (HttpRequest<T> requestInfo)
    {
        StartCoroutine(PostJson(requestInfo));
    }
    public IEnumerator PostJson<T>(HttpRequest<T> requestInfo)
    {
        if(requestInfo.ContentType == "")
        {
            requestInfo.ContentType = "application/json";
        }
        using (var webRequest = UnityWebRequest.Post(requestInfo.Url, requestInfo.RequestBody, requestInfo.ContentType))
        {
            yield return webRequest.SendWebRequest();
            // response success - result or error
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                print(webRequest.downloadHandler.text);
                requestInfo.OnComplete?.Invoke(JsonUtility.FromJson<T>(webRequest.downloadHandler.text));
            }
            else
            {
                Debug.LogError($"Net Error: {webRequest.error}");
            }
        }
    }
    
    public void UploadFileByFormDataAsync(HttpRequest<DownloadHandler> requestInfo, string correctAnswer)
    {
        StartCoroutine(UploadFileByFormData( requestInfo, correctAnswer));
    }

    public IEnumerator UploadFileByFormData(HttpRequest<DownloadHandler> requestInfo, string correctAnswer)
    {
        byte[] data = System.IO.File.ReadAllBytes(requestInfo.RequestBody);
        requestInfo.ContentType = "multipart/form-data";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>
        {
            new MultipartFormFileSection("file", data, "data.wav", requestInfo.ContentType),
            new MultipartFormDataSection("target_text", correctAnswer) 
        };

        using (var webRequest = UnityWebRequest.Post(requestInfo.Url, formData))
        {
            yield return webRequest.SendWebRequest();
            // response success - result or error
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                print(webRequest.downloadHandler.text);
                requestInfo.OnComplete?.Invoke(webRequest.downloadHandler);
            }
            else
            {
                Debug.LogError($"Net Error: {webRequest.error}");
            }
        } 
    }

    void DoneRequest<T>(UnityWebRequest webRequest, HttpRequest<T> requestInfo)
    {

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            print(webRequest.downloadHandler.text);
            requestInfo.OnComplete?.Invoke(JsonUtility.FromJson<T>(webRequest.downloadHandler.text));
        }
        else
        {
            Debug.LogError($"Net Error: {webRequest.error}");
        }
    }

    public IEnumerator UploadFileByByte(HttpRequest<DownloadHandler> requestInfo)
    {
        byte[] data = System.IO.File.ReadAllBytes(requestInfo.RequestBody);
        
        using (var webRequest = new UnityWebRequest(requestInfo.Url, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(data)
            {
                contentType = requestInfo.ContentType
            };

            webRequest.downloadHandler = new DownloadHandlerBuffer()
            {
            };
            
            yield return webRequest.SendWebRequest();
            // response success - result or error
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                print(webRequest.downloadHandler.text);
                requestInfo.OnComplete?.Invoke(webRequest.downloadHandler);
            }
            else
            {
                Debug.LogError($"Net Error: {webRequest.error}");
            }
        }  
    }

    public IEnumerator DownloadSprite(HttpRequest<Sprite> requestInfo)
    {
        byte[] data = System.IO.File.ReadAllBytes(requestInfo.RequestBody);
        
        using (var webRequest = new UnityWebRequest(requestInfo.Url, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(data)
            {
                contentType = requestInfo.ContentType
            };

            webRequest.downloadHandler = new DownloadHandlerTexture();
            
            yield return webRequest.SendWebRequest();
            // response success - result or error
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                var texture = DownloadHandlerTexture.GetContent(webRequest) ?? throw new InvalidOperationException();
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                requestInfo.OnComplete?.Invoke(sprite);
            }
            else
            {
                Debug.LogError($"Net Error: {webRequest.error}");
            }
        }

    }
}