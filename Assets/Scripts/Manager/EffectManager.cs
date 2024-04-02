using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EffectManager : MonoBehaviour
{
    public List<GameObject> objects;
    public GameObject fadePanel;
    public GameObject blurPanel;
    public GameObject dialoguePanel;    //정상적인 대사 ui
    public GameObject centerDialoguePanel;    //가운데에 나오는 대사 ui
    Sequence fadeSequence;
    Sequence blurSequence;
    public void Shake(float time, float force)
    {
        objects.Clear();

        foreach (Transform child in GameObject.Find("CharacterCanvas").transform)
        {   //캐릭터 리스트
            objects.Add(child.gameObject);
        }

        foreach (Transform child in GameObject.Find("ScriptCanvas").transform)
        {      //UI 오브젝트 리스트
            objects.Add(child.gameObject);
        }

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.DOShakePosition(time, force);      //흔들흔들 개꿀잼
        }
    }
    public void FadeAll(float r, float g, float b, float a, float time)
    {
        fadePanel.GetComponent<Image>().color = new Color(r , g, b);

        fadeSequence = DOTween.Sequence()
        .Append(fadePanel.GetComponent<Image>().DOFade(a, time/2))
        .Append(fadePanel.GetComponent<Image>().DOFade(0.0f, time/2))
        .SetId("fadeAll");
    }
    public void centerUISwitch(){   //자막 가운데 띄우기
        dialoguePanel.SetActive(!dialoguePanel.activeSelf);
        centerDialoguePanel.SetActive(!centerDialoguePanel.activeSelf);
    }

    public void BackgroundBlur(float pow, float time){
        if(blurPanel.activeSelf == false){
            blurPanel.SetActive(true);
            blurPanel.GetComponent<Image>().material.SetFloat("_Size", 0);
        }
        
        blurSequence = DOTween.Sequence()
        .Append(blurPanel.GetComponent<Image>().material.DOFloat(pow * 3.2f, "_Size", time))
        .SetId("BackgroundBlur")
        .OnComplete(() => {
            if(pow == 0){
                blurPanel.SetActive(false);
            }
        });
    }   
}
