using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AudioRecorder : MonoBehaviour
{
    private AudioClip _audioClip;
    private string _microphone;
    private bool _isRecording = false;

    void Start()
    {
        // 첫 번째 사용 가능한 마이크 선택
        if (Microphone.devices.Length > 0)
        {
            _microphone = Microphone.devices[0];
        }
        else
        {
            Debug.LogError("마이크를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartRecording();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            StopRecordingAndSave(Application.dataPath + "/test.wav");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HttpManager.GetInstance().UploadFileByFormDataAsync(
                new HttpRequest<DownloadHandler>
                {
                    Url = "http://192.168.1.19:2222/upload",
                    RequestBody = Application.dataPath + "/test.wav",
                    OnComplete = handler => { print(handler.text); }
                },
                "검정색"
            );
        }
    }

    public void StartRecording()
    {
        if (_microphone != null && !_isRecording)
        {
            _audioClip = Microphone.Start(_microphone, false, 2, 16000);
            _isRecording = true;
        }
    }

    public void StopRecordingAndSave(string filePath)
    {
        if (_isRecording)
        {
            Microphone.End(_microphone);
            _isRecording = false;

            if (_audioClip != null)
            {
                SaveWavFile(_audioClip, filePath);
            }
        }
    }

    private void SaveWavFile(AudioClip clip, string filePath)
    {
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);

        byte[] wavFile = ConvertToWav(clip, samples);
        File.WriteAllBytes(filePath, wavFile);
        Debug.Log("파일이 저장되었습니다: " + filePath);
    }

    private byte[] ConvertToWav(AudioClip clip, float[] samples)
    {
        int sampleCount = samples.Length;
        int frequency = clip.frequency;
        int channels = clip.channels;

        int byteCount = sampleCount * sizeof(short);
        int headerSize = 44;

        using (var memoryStream = new MemoryStream())
        {
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                // WAV 파일 헤더 작성
                binaryWriter.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));
                binaryWriter.Write(headerSize + byteCount - 8);
                binaryWriter.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"));
                binaryWriter.Write(System.Text.Encoding.UTF8.GetBytes("fmt "));
                binaryWriter.Write(16);
                binaryWriter.Write((short)1); // 오디오 포맷: PCM
                binaryWriter.Write((short)channels);
                binaryWriter.Write(frequency);
                binaryWriter.Write(frequency * channels * sizeof(short));
                binaryWriter.Write((short)(channels * sizeof(short)));
                binaryWriter.Write((short)16); // 비트 깊이
                binaryWriter.Write(System.Text.Encoding.UTF8.GetBytes("data"));
                binaryWriter.Write(byteCount);

                // 샘플 데이터를 WAV 형식으로 변환하여 작성
                foreach (var sample in samples)
                {
                    short intData = (short)(sample * short.MaxValue);
                    binaryWriter.Write(intData);
                }
            }

            return memoryStream.ToArray();
        }
    }
}