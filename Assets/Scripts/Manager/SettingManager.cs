using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;      //싱글톤
    public float mainVolume = 1;        //메인 볼륨
    public float esVolume = 1;      //효과음 볼륨
    public bool isNewGame = false;
    public DataSet initDataSet;
    public GameObject LoadCanvas;
    void Awake(){
        DontDestroyOnLoad(LoadCanvas);

        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }
}
