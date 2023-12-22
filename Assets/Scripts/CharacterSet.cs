using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterSet
{
    public string characterName;
    public string characterBody;
    public string characterEffect;
    public float characterXpos;
    public float characterYpos;

    public CharacterSet(string name, string body, string effect, float xpos, float ypos){
        characterName = name;
        characterBody = body;
        characterEffect = effect;
        characterXpos = xpos;
        characterYpos = ypos;
    }

    public CharacterSet(string name, string body, float xpos, float ypos){
        characterName = name;
        characterBody = body;
        characterXpos = xpos;
        characterYpos = ypos;
    }
}
