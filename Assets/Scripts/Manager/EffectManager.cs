using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectManager : MonoBehaviour
{
    public List<GameObject> objects;
    private Sequence shakeSequence;
    public void Shake(float time, float force){
        objects.Clear();
        
        foreach(Transform child in GameObject.Find("CharacterCanvas").transform){   //캐릭터 리스트
            objects.Add(child.gameObject);
        }

        foreach(Transform child in GameObject.Find("ScriptCanvas").transform){      //UI 오브젝트 리스트
            objects.Add(child.gameObject);
        }

        for(int i = 0; i < objects.Count; i++){
            objects[i].transform.DOShakePosition(time, force);      //흔들흔들 개꿀잼
        }
    }
}
