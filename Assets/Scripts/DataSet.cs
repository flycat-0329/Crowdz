using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataSet
{
    public List<CharacterSet> saveCharacterList;
    public bool saveIsParticle;
    public List<string> saveParticleNameList;
    public bool saveIsAnim;
    public string saveDialogue;
    public string saveCurrentCharacter;
    public int saveScriptIndex;
    public string saveScriptName;
    public string saveBackgroundImage;
    public string saveBGMName;
    public float saveScriptBGMVolume;
    public string saveTime;
    public string saveChapterIndex;

    public DataSet(List<CharacterSet> characterList, bool isParticle, List<string> particleNameList, bool isAnim, string dialogue, string character, int scriptIndex, string scriptName, string backgroundImage, string bgm, float scriptbgmVolume, string time, string chapterIndex){
        saveIsParticle = isParticle;
        saveParticleNameList = particleNameList;
        saveIsAnim = isAnim;
        saveDialogue = dialogue;
        saveCurrentCharacter = character;
        saveCharacterList = characterList;
        saveScriptIndex = scriptIndex;
        saveScriptName = scriptName;
        saveBackgroundImage = backgroundImage;
        saveBGMName = bgm;
        saveScriptBGMVolume = scriptbgmVolume;
        saveTime = time;
        saveChapterIndex = chapterIndex;
    }
}
    
