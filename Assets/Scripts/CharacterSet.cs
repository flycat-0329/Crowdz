using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterSet
{
    public string characterName;
    public string characterBody;
    public string characterFace;
    public float characterXpos;
    public float characterYpos;

    public CharacterSet(string name, string body, string face, float xpos, float ypos){
        characterName = name;
        characterBody = body;
        characterFace = face;
        characterXpos = xpos;
        characterYpos = ypos;
    }
}
