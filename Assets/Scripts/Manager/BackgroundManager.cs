using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class BackgroundManager : MonoBehaviour
{
    public Object[] backgroundList;
    public Object[] backgroundAnimList;
    public Image background;    //현재 배경
    public GameObject backgroundSecond; //페이드용 배경 사진
    public GameObject backgroundAnimImage;
    public string backgroundName;
    public bool isAnim;
    public VideoPlayer videoPlayer;
    private Sequence fadeInSequence;
    private Sequence fadeOutSequence;

    private void Awake()
    {
        backgroundList = Resources.LoadAll("Images/Background/Sprite");
        backgroundAnimList = Resources.LoadAll("Images/Background/Video");
    }

    public void BackgroundImageOn(string bgName)
    {       //대본에 배경 이름을 넣으면 그 배경으로 바뀌도록 하는 함수
        isAnim = false;
        backgroundAnimImage.SetActive(false);
        background.gameObject.SetActive(true);
        backgroundName = bgName;
        background.sprite = FindBG(bgName);     //이미지 자체가 아닌 이미지의 이름으로 이미지를 찾아옴
    }

    public void BackgroundImageFadeIn(string bgName, float time){   //일정 시간에 걸쳐 이미지 배경 페이드인
        isAnim = false;
        backgroundAnimImage.SetActive(false);
        background.gameObject.SetActive(true);
        background.color = new Color(1, 1, 1, 0);
        backgroundName = bgName;
        background.sprite = FindBG(bgName);     //이미지 자체가 아닌 이미지의 이름으로 이미지를 찾아옴

        fadeInSequence = DOTween.Sequence()
        .Append(background.DOFade(1, time));
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

    public void BackgroundImageFadeOut(float time){  //일정 시간에 걸쳐 이미지 배경 페이드아웃
        fadeOutSequence = DOTween.Sequence()
        .Append(background.DOFade(0, time));
    }

    public void BackgroundAnimOn(string animName)
    {
        isAnim = true;
        backgroundName = animName;

        backgroundAnimImage.SetActive(true);
        videoPlayer.clip = FindAnim(animName);
        videoPlayer.Play();
        StartCoroutine(videoDelay());
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
