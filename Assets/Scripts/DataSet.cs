using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataSet
{
    public List<CharacterSet> saveCharacterList;
    public int saveScriptIndex;
    public string saveScriptName;
    public string saveBackgroundImage;
    public string saveBGMName;
    public float saveBGMVolume;
    public float saveESVolume;

    public DataSet(List<CharacterSet> characterList, int scriptIndex, string scriptName, string backgroundImage, string bgm, float bgmVolume, float esVolume){
        saveCharacterList = characterList;
        saveScriptIndex = scriptIndex;
        saveScriptName = scriptName;
        saveBackgroundImage = backgroundImage;
        saveBGMName = bgm;
        saveBGMVolume = bgmVolume;
        saveESVolume = esVolume;
    }
}
    
