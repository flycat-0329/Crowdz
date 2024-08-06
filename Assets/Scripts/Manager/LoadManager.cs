using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public GameObject loadButton;
    public GameObject loadPanel;
    public GameObject loadPageText;
    private int loadPanelIndex;
    string path;

    private void Start()
    {
        for (int i = 0; i < 60; i++)
        {
            GameObject sB = GameObject.Instantiate(loadButton);
            sB.name = i.ToString();
            sB.transform.SetParent(loadPanel.transform);
            sB.transform.localScale = new Vector3(1, 1, 1);
            sB.GetComponent<Button>().onClick.AddListener(() => LoadGame(sB.name));
            sB.SetActive(false);
        }

        pageChange(-1);
    
        this.gameObject.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        pageChange(0);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < 10; i++)
        {
            pageChange(-1);
        }

        // this.gameObject.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void pageChange(int a)
    {
        //원래 있던것들 일단 지우고
        for (int i = loadPanelIndex * 6; i < 6 * (loadPanelIndex + 1); i++)
        {
            loadPanel.transform.GetChild(i).gameObject.SetActive(false);
        }

        //첫페이지에서 뒤로 가거나 끝페이지에서 앞으로 가는거 아니면 페이지 넘어감
        if ((loadPanelIndex != 0 && a == -1) || (loadPanelIndex != 9 && a == 1))
        {
            loadPanelIndex += a;
        }

        //넘어간 페이지에 있는 세이브 슬롯
        for (int i = loadPanelIndex * 6; i < 6 * (loadPanelIndex + 1); i++)
        {
            loadPanel.transform.GetChild(i).gameObject.SetActive(true);
            
            if(SettingManager.instance.allDataList[i] != null){
                loadPanel.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = SettingManager.instance.allDataList[i].saveTime;
                loadPanel.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = "챕터 " + SettingManager.instance.allDataList[i].saveChapterIndex;
            }
            else{
                loadPanel.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = "저장된 데이터가 없습니다.";
            }
        }

        loadPageText.GetComponent<Text>().text = (loadPanelIndex + 1).ToString() + "/10";
    }

    public void LoadAllData(){
        for(int i = 0; i < 60; i++){
            string p = Application.persistentDataPath + "saveData" + i + ".json";

            if(File.Exists(p)){ 
                FileStream fileStream = new FileStream(p, FileMode.Open);
                byte[] data = new byte[fileStream.Length];
                fileStream.Read(data, 0, data.Length);
                fileStream.Close();

                string j = Encoding.UTF8.GetString(data);
                SettingManager.instance.allDataList.Add(JsonUtility.FromJson<DataSet>(j));
            }
            else{
                SettingManager.instance.allDataList.Add(null);
            }
        }
    }

    public void LoadGame(string index)
    {
        SettingManager.instance.isNewGame = false;
        path = Application.persistentDataPath + "saveData" + index + ".json";

        FileStream fileStream = new FileStream(path, FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();

        string json = Encoding.UTF8.GetString(data);
        SettingManager.instance.initDataSet = JsonUtility.FromJson<DataSet>(json);

        SceneManager.LoadScene("GameScene");
        loadPanel.transform.parent.gameObject.SetActive(false);
        
        SettingManager.instance.mainVolume = PlayerPrefs.GetFloat("bgmVolume", 1);
        SettingManager.instance.esVolume = PlayerPrefs.GetFloat("esVolume", 1);
    }

    public void LoadNewGame(string scriptName)
    {
        SettingManager.instance.isNewGame = true;
        TextAsset ta = Resources.Load("InitData/" + scriptName) as TextAsset;
        SettingManager.instance.initDataSet = JsonUtility.FromJson<DataSet>(ta.ToString());



        SceneManager.LoadScene("GameScene");
    }
}
