using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class DataSet : MonoBehaviour
{
    List<string> saveCharacterOne; //이름, x좌표, y좌표, 몸, 얼굴
    List<string> saveCharacterTwo; //이름, x좌표, y좌표, 몸, 얼굴

    int saveScriptIndex = 0;
    string saveScriptName;
    string saveBackgroundImage;
    string saveBGMName;
    float saveBGMVolume = 1;
    float saveESVolume = 1;

    //화면에 캐릭터가 없음
    public DataSet(int scriptIndex, string scriptName, string backgroundImage, string bgm, float bgmVolume, float esVolume){
        saveScriptIndex = scriptIndex;
        saveScriptName = scriptName;
        saveBackgroundImage = backgroundImage;
        saveBGMName = bgm;
        saveBGMVolume = bgmVolume;
        saveESVolume = esVolume;
    }

    public DataSet(List<string> saveOne, int scriptIndex, string scriptName, string backgroundImage, string bgm, float bgmVolume, float esVolume){
        saveCharacterOne = saveOne;
        saveScriptIndex = scriptIndex;
        saveScriptName = scriptName;
        saveBackgroundImage = backgroundImage;
        saveBGMName = bgm;
        saveBGMVolume = bgmVolume;
        saveESVolume = esVolume;
    }

    public DataSet(List<string> saveOne, List<string> saveTwo, int scriptIndex, string scriptName, string backgroundImage, string bgm, float bgmVolume, float esVolume){
        saveCharacterOne = saveOne;
        saveCharacterTwo = saveTwo;
        saveScriptIndex = scriptIndex;
        saveScriptName = scriptName;
        saveBackgroundImage = backgroundImage;
        saveBGMName = bgm;
        saveBGMVolume = bgmVolume;
        saveESVolume = esVolume;
    }
}
    
