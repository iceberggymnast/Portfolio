using UnityEngine;
using System.IO;
using System.Diagnostics;
using System.Text;

public class TestPythonManager : MonoBehaviour
{
    public enum PythonName
    {
        test,
        nullType,
        ref3,
        mp
    }

    public PythonName pythonName = PythonName.test;

    private bool testTrue = false;


    // Python 파일 실행 함수
    public void ExecutePythonFile()
    {
        // StreamingAssets 폴더에 있는 Python 파일 경로를 가져옴
        string pythonFilePath = Path.Combine(Application.streamingAssetsPath, $"{pythonName.ToString()}.py");

        if (File.Exists(pythonFilePath))
        {
            // Python 실행 파일 경로 (사용자의 Python 설치 경로)
            //string pythonPath = @"C:\Users\dfjm\AppData\Local\Programs\Python\Python38\python.exe";
            string pythonPath = Path.Combine(Application.streamingAssetsPath, "PythonFile\\Python\\Python38\\python.exe");

            // 프로세스 실행 설정
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = pythonPath; // Python 실행 파일 경로
            processInfo.Arguments = pythonFilePath; // 실행할 Python 파일 경로
            processInfo.RedirectStandardOutput = true; // 출력 리디렉션
            processInfo.StandardOutputEncoding = Encoding.UTF8; // UTF-8 인코딩 사용
            processInfo.UseShellExecute = false; // 셸 사용 비활성화
            processInfo.CreateNoWindow = true; // 창 생성 안 함

            // 프로세스 실행
            using (Process process = Process.Start(processInfo))
            {
                // Python 파일의 출력 결과를 UTF-8로 읽음
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    UnityEngine.Debug.Log("Python 실행 결과: " + result);
                }
            }
        }
        else
        {
            UnityEngine.Debug.LogError("Python 파일을 찾을 수 없습니다: " + pythonFilePath);
        }
    }

    private void Start()
    {
        print(PythonName.test.ToString());
    }

    private void Update()
    {
        // V 키 누르면 Python 파일 실행
        if (Input.GetKeyDown(KeyCode.V) && !testTrue)
        {
            testTrue = true;
            ExecutePythonFile();
        }
    }
}
