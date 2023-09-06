using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EffectManager : MonoBehaviour
{
    public List<GameObject> objects;
    public GameObject fadePanel;
    public CharacterManager characterManager;
    Sequence currentSequence;
    Sequence fadeSequence;
    Sequence characterFadeInSequence;
    Sequence characterFadeOutSequence;
    private void Start() {
        FadeAll();
    }
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

    public void FadeAll(){
        currentSequence = fadeSequence;

        fadeSequence = DOTween.Sequence()
        .Append(fadePanel.GetComponent<Image>().DOFade(1.0f, 1))
        .Append(fadePanel.GetComponent<Image>().DOFade(0.0f, 1));
    }

    public void FadeInCharacter(GameObject character, float time){
        currentSequence = characterFadeInSequence;

        characterFadeInSequence = DOTween.Sequence()
        .Append(character.GetComponent<Image>().DOFade(1.0f, time))
        .Join(character.transform.GetChild(0).GetComponent<Image>().DOFade(1.0f, time));
    }

    public void FadeOutCharacter(GameObject character, float time){
        currentSequence = characterFadeOutSequence;

        characterFadeOutSequence = DOTween.Sequence()
        .Append(character.GetComponent<Image>().DOFade(0.0f, time))
        .Join(character.transform.GetChild(0).GetComponent<Image>().DOFade(0.0f, time))
        .OnComplete(() => {
            characterManager.CharacterKill(character.name);
        });
    }

    public void SequenceKill(){
        currentSequence.Kill();
    }
}
