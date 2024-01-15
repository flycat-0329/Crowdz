using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BackgroundManager : MonoBehaviour
{
    public Object[] backgroundList;
    public Object[] backgroundAnimList;
    public Image background;    //현재 게임 배경
    public GameObject backgroundAnimImage;
    public string backgroundName;
    public bool isAnim;
    public VideoPlayer videoPlayer;

    private void Awake()
    {
        backgroundList = Resources.LoadAll("Images/Background/Sprite");
        backgroundAnimList = Resources.LoadAll("Images/Background/Video");
    }
    private void Update()
    {
        Debug.Log(videoPlayer.isPlaying);
    }

    public void BackgroundImageOn(string bgName)
    {       //대본에 배경 이름을 넣으면 그 배경으로 바뀌도록 하는 함수
        isAnim = false;
        backgroundAnimImage.SetActive(false);
        background.gameObject.SetActive(true);
        backgroundName = bgName;
        background.sprite = FindBG(bgName);     //이미지 자체가 아닌 이미지의 이름으로 이미지를 찾아옴
    }

    public void BackgroundAnimOn(string animName)
    {
        isAnim = true;
        backgroundName = animName;

        backgroundAnimImage.SetActive(true);
        videoPlayer.clip = FindAnim(animName);
        videoPlayer.Play();
        Debug.Log("시작함");
        
        StartCoroutine(videoDelay());

        Debug.Log("탈출함");
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
