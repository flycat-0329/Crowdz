using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public Object[] backgroundList;
    public Image background;    //현재 게임 배경
    public string backgroundName;

    private void Awake() {
        backgroundList = Resources.LoadAll("Images/Background");
    }

    public void changeBG(string bgName){       //대본에 배경 이름을 넣으면 그 배경으로 바뀌도록 하는 함수
        backgroundName = bgName;
        background.sprite = FindBG(bgName);     //이미지 자체가 아닌 이미지의 이름으로 이미지를 찾아옴
    }

    Sprite FindBG(string name){         //이미지의 이름을 받아서 이미지를 리턴하는 함수
        foreach(var i in backgroundList){       //backgroundList를 돌면서
            if(i.name == name){         //만약 backgroundList에 이 이미지가 있다면 리턴
                Sprite sprite = Sprite.Create((i as Texture2D), new Rect(0, 0, (i as Texture2D).width, (i as Texture2D).height), new Vector2(0.5f, 0.5f));
                return sprite;
            }
        }

        Debug.LogFormat(this, "{0}이라는 배경이 없습니다.", name);  //없으면 ㅈ된거지 뭐
        return null;
    }
}
