using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public LinePrint linePrint;
    string path;
    public GameObject savePanel;
    public GameObject saveButton;
    public GameObject savePageText;
    int savePanelIndex = 0;
    

    private void Start() {
        savePageText.GetComponent<Text>().text = "1/10";

        for(int i = 0; i < 60; i++){
            GameObject sB = GameObject.Instantiate(saveButton);
            sB.name = i.ToString();
            sB.transform.SetParent(savePanel.transform);
            sB.transform.localScale = new Vector3(1, 1, 1);
            sB.GetComponent<Button>().onClick.AddListener(() => linePrint.SaveClicked(sB.name));
            sB.SetActive(false);
        }

        pageChange(-1);
    }

    public void pageChange(int a){
        //원래 있던것들 일단 지우고
        for(int i = savePanelIndex * 6; i < 6 * (savePanelIndex + 1); i++){
            savePanel.transform.GetChild(i).gameObject.SetActive(false);
        }

        //첫페이지에서 뒤로 가거나 끝페이지에서 앞으로 가는거 아니면 페이지 넘어감
        if((savePanelIndex != 0 && a == -1) || (savePanelIndex != 9 && a == 1)){    
            savePanelIndex += a;
        }

        //넘어간 페이지에 있는 세이브 슬롯
        for(int i = savePanelIndex * 6; i < 6 * (savePanelIndex + 1); i++){
            savePanel.transform.GetChild(i).gameObject.SetActive(true);
        }

        savePageText.GetComponent<Text>().text = (savePanelIndex + 1).ToString() + "/10";
    }

    public void GameSave(DataSet dataSet, string saveIndex){
        Debug.Log(dataSet.saveCharacterList.Count);
        string jsonData = JsonUtility.ToJson(dataSet);
        Debug.Log(jsonData);

        path = Application.dataPath + "/Resources/Data/saveData" + saveIndex;

        FileInfo fi = new FileInfo(path);
        if(fi.Exists){
            System.IO.File.Delete(path);
        }

        FileStream fileStream = new FileStream(path, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
}
