using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataSet
{
    public List<CharacterSet> saveCharacterList;
    public string saveDialogue;
    public string saveCurrentCharacter;
    public int saveScriptIndex;
    public string saveScriptName;
    public string saveBackgroundImage;
    public string saveBGMName;
    public float saveBGMVolume;
    public float saveScriptBGMVolume;
    public float saveESVolume;

    public DataSet(List<CharacterSet> characterList, string dialogue, string character, int scriptIndex, string scriptName, string backgroundImage, string bgm, float bgmVolume, float scriptbgmVolume, float esVolume){
        saveDialogue = dialogue;
        saveCurrentCharacter = character;
        saveCharacterList = characterList;
        saveScriptIndex = scriptIndex;
        saveScriptName = scriptName;
        saveBackgroundImage = backgroundImage;
        saveBGMName = bgm;
        saveBGMVolume = bgmVolume;
        saveScriptBGMVolume = scriptbgmVolume;
        saveESVolume = esVolume;
    }
}
    
