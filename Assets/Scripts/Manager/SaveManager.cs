using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SaveManager : MonoBehaviour
{
    string path;
    public void GameSave(DataSet dataSet, string saveIndex){
        Debug.Log(dataSet.saveCharacterList.Count);
        string jsonData = JsonUtility.ToJson(dataSet);
        Debug.Log(jsonData);

        #if UNITY_EDITOR
        path = Application.dataPath + "/Data/" + "saveData" + saveIndex + ".json";
        #elif UNITY_ANDROID
        path = Application.persistentDataPath + "/Data/" + "saveData" + saveIndex + ".json";
        #endif

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