using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSys : MonoBehaviour
{
    /*

        저장시 - 해당 컴포넌트가 있는지 체크 후 저장
        불러오기 - 해당 키 값이 있는지 체크 -> 해당 컴포넌트가 없는지 체크 -> 있으면 해당 컴포넌트 내에 불러오기
                   없을 경우 현재 스크립트에 저장

    */
    public static SaveSys saveSys;

    ES3Settings settings;

    // 저장할 데이터
    public Interaction_Shop shopData;
    public List<InventoryCupon> loadDataInventory;

    // 업그레이드 기록
    public playerupgrades playerupgrades;

    private void Awake()
    {
        if (saveSys == null)
        {
            saveSys = this;
            settings = new ES3Settings();
            settings.path = "none";
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (settings.path == "none")
        {
            settings.path = PhotonNetwork.NickName + ".save";
            DataLoad();
        }

        DataSave();
    }

    public void setPath()
    {
        settings.path = PhotonNetwork.NickName + ".save";
    }

    public void DataSave()
    {
        if (settings.path == "none")
        {
            print("계정 지정되지 않음");
            return;
        }

        // 포인트 저장
        if (PlayerInfo.instance != null)
        {
            ES3.Save("point", PlayerInfo.instance.point, settings);
        }

        // 인벤토리 저장
        if (shopData == null)
        {
            shopData = GameObject.FindAnyObjectByType<Interaction_Shop>();
        }
        if (shopData != null)
        { 
            ES3.Save("Inventory", shopData.inventoryCupons, settings); 
        }

        // 업그레이드 내역 저장
        if (playerupgrades == null)
        {
            playerupgrades = new playerupgrades();
            ES3.Save("Upgrades", playerupgrades, settings);
        }
        else
        {
            ES3.Save("Upgrades", playerupgrades, settings);
        }


        print("저장되었습니다");
    }

    public void DataLoad()
    {
        if (settings.path == "none")
        {
            print("계정 지정되지 않음");
            return;
        }

        // 포인트 불러오기
        if (ES3.KeyExists("point", settings))
        {
            if (PlayerInfo.instance != null)
            {
                PlayerInfo.instance.point = ES3.Load<int>("point", settings);
            }
        }

        // 인벤토리 불러오기
        if (ES3.KeyExists("Inventory", settings))
        {
            if (shopData == null)
            {
                shopData = GameObject.FindAnyObjectByType<Interaction_Shop>();
            }

            if (shopData != null)
            {
                shopData.inventoryCupons = ES3.Load<List<InventoryCupon>>("Inventory", settings);
            }
            else
            {
                loadDataInventory = ES3.Load<List<InventoryCupon>>("Inventory", settings);
            }
        }
        else
        {
            loadDataInventory = new List<InventoryCupon>();
        }

        //업그레이드 불러오기
        if (ES3.KeyExists("Upgrades", settings))
        {
            playerupgrades = ES3.Load<playerupgrades>("Upgrades", settings);
            interaction_Equipment iE = GameObject.FindFirstObjectByType<interaction_Equipment>();
            if (iE != null)
            {
                iE.Setvalue();
            }
        }

        print("불러오기 완료");
    }

    public void SaveQuizType(string value)
    {
        setPath();

        if (settings.path == "none")
        {
            print("계정 지정되지 않음");
            return;
        }
        ES3.Save("QuizType", value, settings);
    }

    public string LoadQuizType()
    {
        if (settings.path == "none")
        {
            print("계정 지정되지 않음");
            return null;
        }
        return ES3.Load<string>("QuizType", settings);
    }

}

[Serializable]
public class playerupgrades
{
    [SerializeField]
    public List<int> upgradesLV = new List<int>() { 0, 0, 0, 0 };
}
