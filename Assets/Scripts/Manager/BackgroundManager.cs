using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class BackgroundManager : MonoBehaviour
{
    public UnityEngine.Object[] backgroundList;
    public UnityEngine.Object[] backgroundAnimList;
    public UnityEngine.Object[] backgroundEffectList;
    public Image background;    //현재 배경
    public GameObject backgroundSecond; //페이드용 배경 사진
    public GameObject backgroundAnimImage;
    public GameObject backgroundEffectImage;
    public string backgroundName;
    public bool isAnim;
    public VideoPlayer videoPlayer;
    private Sequence fadeInSequence;
    private Sequence fadeOutSequence;

    private void Awake()
    {
        backgroundList = Resources.LoadAll("Images/Background/Sprite");
        backgroundAnimList = Resources.LoadAll("Images/Background/Video");
        backgroundEffectList = Resources.LoadAll("Images/Background/Effect");
    }

    public void BackgroundImageOn(string bgName)
    {       //대본에 배경 이름을 넣으면 그 배경으로 바뀌도록 하는 함수
        isAnim = false;
        backgroundAnimImage.SetActive(false);
        background.gameObject.SetActive(true);
        backgroundName = bgName;
        background.sprite = FindBG(bgName);     //이미지 자체가 아닌 이미지의 이름으로 이미지를 찾아옴
    }
    public void BackgroundAnimOn(string animName)   //애니메이션 배경 재생
    {
        isAnim = true;
        backgroundName = animName;

        backgroundAnimImage.SetActive(true);
        videoPlayer.clip = FindAnim(animName);
        videoPlayer.Play();
        StartCoroutine(videoDelay());
    }
    public void BackgroundChangeFade(string bgName, float time){
        Debug.Log(bgName);
        isAnim = false;
        backgroundName = bgName;
        backgroundAnimImage.SetActive(false);
        background.gameObject.SetActive(true);
        backgroundSecond.SetActive(true);

        backgroundSecond.GetComponent<Image>().sprite = FindBG(bgName);
        backgroundSecond.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        fadeInSequence = DOTween.Sequence()
        .Append(backgroundSecond.GetComponent<Image>().DOFade(1, time))
        .OnComplete(() => {
            background.sprite = FindBG(bgName);
            backgroundSecond.SetActive(false);
        })
        .SetId("backgroundChangeFade");
    }
    public void BackgroundChangePoint(string name, float time){
        backgroundEffectImage.SetActive(true);
        backgroundEffectImage.GetComponent<Image>().sprite = FindEffect("확대");
        backgroundEffectImage.transform.localScale = new Vector3(0, 0, 0);
        
        Sequence pointOnSequence = DOTween.Sequence()
        .Append(backgroundEffectImage.transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), time))
        .OnComplete(() => {
            background.sprite = FindBG(name);
        })
        .SetId("pointOnSequence");

        Sequence pointOutSequence = DOTween.Sequence()
        .Insert(1.0f, backgroundEffectImage.transform.DOScale(new Vector3(0, 0, 0), time))
        .OnComplete(() => {
            backgroundEffectImage.SetActive(false);
        })
        .SetId("pointOutSequence");
    }


    public void EffectSwitch(string effectName = "페이드", string backgroundName = "검은배경", float time = 1){
        switch (effectName){
            case "페이드":
                BackgroundChangeFade(backgroundName, time);
                break;
            case "확대":
                BackgroundChangePoint(backgroundName, time);
                break;
        }
    }

    Sprite FindBG(string name)
    {         //이미지의 이름을 받아서 이미지를 리턴하는 함수
        foreach (var i in backgroundList)
        {       //backgroundList를 돌면서
            if (i.name == name)
            {         //만약 backgroundList에 이 이미지가 있다면 리턴
                Sprite sprite = Sprite.Create((i as Texture2D), new Rect(0, 0, (i as Texture2D).width, (i as Texture2D).height), new Vector2(0.5f, 0.5f));
                return sprite;
            }
        }

        Debug.LogFormat(this, "{0}이라는 배경이 없습니다.", name);  //없으면 ㅈ된거지 뭐
        return null;
    }

    Sprite FindEffect(string name){
        foreach(var i in backgroundEffectList){
            if(i.name == name){
                Sprite sprite = Sprite.Create((i as Texture2D), new Rect(0, 0, (i as Texture2D).width, (i as Texture2D).height), new Vector2(0.5f, 0.5f));
                return sprite;
            }
        }

        Debug.LogFormat(this, "{0}이라는 이펙트가 없습니다.", name);
        return null;
    }

    VideoClip FindAnim(string name)
    {
        foreach (var i in backgroundAnimList)
        {
            if (i.name == name)
            {
                VideoClip videoClip = (VideoClip)i;
                return videoClip;
            }
        }

        Debug.LogFormat(this, "{0}이라는 배경이 없습니다.", name);  //없으면 ㅈ된거지 뭐
        return null;
    }

    IEnumerator videoDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (videoPlayer.isPlaying == true)
            {
                background.gameObject.SetActive(false);
                break;
            }
        }
    }
}
