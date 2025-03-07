using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


public class WebcamSample : MonoBehaviour
{
    public RawImage display;
    WebCamTexture camTexture;
    private int currentIndex = 0;

    private void Start()
    {
        if (camTexture != null)
        {
            display.texture = null;
            camTexture.Stop();
            camTexture = null;
        }
        WebCamDevice device = WebCamTexture.devices[currentIndex];
        camTexture = new WebCamTexture(device.name);
        display.texture = camTexture;
        camTexture.Play();
    }

    public RawImage texturePicRaw;

    public void save ()
    {
        Texture texture2D = texturePicRaw.texture as Texture;
        SaveTextureToPNGFile(texture2D, "/Assets/img", "save");
    }


    public void SaveTextureToPNGFile(Texture texture, string directoryPath, string fileName)
    {
        if (string.IsNullOrEmpty(directoryPath))
        {
            Debug.LogError("Directory path is null or empty.");
            return;
        }

        if (!Directory.Exists(directoryPath))
        {
            Debug.Log("Directory does not exist, creating directory.");
            Directory.CreateDirectory(directoryPath);
        }

        // Texture를 Texture2D로 변환
        int width = texture.width;
        int height = texture.height;

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture copiedRenderTexture = new RenderTexture(width, height, 0);

        Graphics.Blit(texture, copiedRenderTexture);

        RenderTexture.active = copiedRenderTexture;

        Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRenderTexture;

        byte[] texturePNGBytes = texture2D.EncodeToPNG();

        // directoryPath가 '/'로 끝나지 않으면 추가
        if (!directoryPath.EndsWith("/"))
        {
            directoryPath += "/";
        }

        string filePath = directoryPath + fileName + ".png";

        try
        {
            File.WriteAllBytes(filePath, texturePNGBytes);
            Debug.Log("File saved successfully at " + filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save file: " + ex.Message);
        }
    }

}
