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
    public GameObject background;    //현재 배경
    public GameObject backgroundSecond; //디졸브, 페이드용 배경 사진
    public GameObject backgroundAnimImage;
    public GameObject backgroundEffectImage;
    public string backgroundName;
    public bool isAnim;
    public VideoPlayer videoPlayer;
    private Sequence dissolveInSequence;


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
        background.SetActive(true);
        backgroundName = bgName;
        background.GetComponent<Image>().sprite = FindBG(bgName);     //이미지 자체가 아닌 이미지의 이름으로 이미지를 찾아옴
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
    public void BackgroundChangeDissolve(string bgName, float time, bool anim = false){
        isAnim = false;
        backgroundName = bgName;
        backgroundAnimImage.SetActive(false);
        background.SetActive(true);
        backgroundSecond.SetActive(true);

        backgroundSecond.GetComponent<Image>().sprite = FindBG(bgName);
        backgroundSecond.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        dissolveInSequence = DOTween.Sequence()
        .Append(backgroundSecond.GetComponent<Image>().DOFade(1, time))
        .OnComplete(() => {
            background.GetComponent<Image>().sprite = FindBG(bgName);
            backgroundSecond.SetActive(false);
        })
        .SetId("dissolveSequence");
    }
    public void BackgroundChangePoint(string name, float time, bool anim = false){
        isAnim = false;
        backgroundName = name;
        backgroundEffectImage.SetActive(true);
        backgroundEffectImage.GetComponent<Image>().sprite = FindEffect("확대");
        backgroundEffectImage.transform.localScale = new Vector3(0, 0, 0);
        backgroundEffectImage.transform.localPosition = new Vector3(0, 0, 0);
        
        Sequence pointSequence = DOTween.Sequence()
        .Append(backgroundEffectImage.transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), time))
        .AppendCallback(() => {
            backgroundAnimImage.SetActive(false);
            background.SetActive(true);
            background.GetComponent<Image>().sprite = FindBG(name);
        })
        .AppendInterval(0.3f)
        .Append(backgroundEffectImage.transform.DOScale(new Vector3(0, 0, 0), time))
        .OnComplete(() => {
            backgroundEffectImage.SetActive(false);
        })
        .SetId("pointSequence");
    }

    public void BackgroundChangeSide(string name, bool anim = false){
        isAnim = false;
        backgroundName = name;
        backgroundEffectImage.SetActive(true);
        backgroundEffectImage.GetComponent<Image>().sprite = FindEffect("사이드");
        backgroundEffectImage.transform.localScale = new Vector3(2, 1, 1);
        backgroundEffectImage.transform.localPosition = new Vector3(-3200, 0, 0);
        
        Sequence sideseqnence = DOTween.Sequence()
        .Append(backgroundEffectImage.transform.DOLocalMoveX(0, 0.4f))
        .AppendCallback(() => {
            backgroundAnimImage.SetActive(false);
            background.SetActive(true);
        })
        .AppendInterval(0.3f)
        .Append(backgroundEffectImage.transform.DOLocalMoveX(3200, 0.4f))
        .SetId("sideSequence");
    }

    public void BackgroundChangeFade(string name, float time, bool anim = false){
        isAnim = false;
        backgroundName = name;
        backgroundSecond.SetActive(true);
        backgroundSecond.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        Sequence FadeSequence = DOTween.Sequence()
        .Append(backgroundSecond.GetComponent<Image>().DOFade(1, time))
        .AppendCallback(() => {
            backgroundAnimImage.SetActive(false);
            background.SetActive(true);
            background.GetComponent<Image>().sprite = FindBG(name);
        })
        .Append(backgroundSecond.GetComponent<Image>().DOFade(0, time))
        .OnComplete(() => {
            backgroundSecond.SetActive(false);
        })
        .SetId("backgroundFadeSequence");
    }

    public void EffectSwitch(string effectName = "페이드", string backgroundName = "검은배경", float time = 1){
        switch (effectName){
            case "디졸브":
                BackgroundChangeDissolve(backgroundName, time);
                break;
            case "확대":
                BackgroundChangePoint(backgroundName, time);
                break;
            case "사이드":
                BackgroundChangeSide(backgroundName);
                break;
            case "페이드":
                BackgroundChangeFade(backgroundName, time);
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
                background.SetActive(false);
                break;
            }
        }
    }
}
