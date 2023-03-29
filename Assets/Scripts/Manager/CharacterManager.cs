using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterManager : MonoBehaviour
{
    public Dictionary<string, GameObject> currentCharacter = new Dictionary<string, GameObject>();  //현재 존재하는 캐릭터들
    public List<string> characterOne = new List<string>(); //저장할때 쓰이는 변수
    public List<string> characterTwo = new List<string>(); //저장할때 쓰이는 변수
    Object[] faceImageList; //시작할때 불러오는 캐릭터 얼굴 이미지
    Object[] characterImageList;    //시작할때 불러오는 캐릭터 몸통 이미지
    public GameObject character;    //캐릭터 오리지널 형태
    public Canvas characterCanvas;  //캐릭터가 있는 캔버스
    private Sequence fadeOutSequence;   //dotween
    private Sequence fadeInSequence;    //dotween
    private Sequence characterMoveSequence; //dotween

    private void Start()
    {
        faceImageList = Resources.LoadAll("Images/Character/Face");
        characterImageList = Resources.LoadAll("Images/Character/Body");
    }
    public void setCharacter(string characterName, string characterFace, string characterBody, float xpos, float ypos)
    {
        GameObject newActor = Instantiate(character);
        newActor.transform.SetParent(characterCanvas.transform);
        newActor.name = characterName;

        newActor.transform.localPosition = new Vector3((-0.5f + xpos) * Screen.width, (-0.5f + ypos) * Screen.height, 0);
        newActor.transform.GetChild(0).transform.localPosition = new Vector3(0, 235, 0);

        newActor.transform.localScale = new Vector3(1, 1, 1);
        newActor.transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);

        newActor.GetComponent<Image>().sprite = FindCha(characterName + "_" + characterBody);    //캐릭터의 몸 이미지
        SetFace(newActor, characterFace);
        currentCharacter[characterName] = newActor.gameObject;
        print(characterName);

        if(currentCharacter.Count == 0){
            characterOne.Add(characterName);
            characterOne.Add(xpos.ToString());
            characterOne.Add(ypos.ToString());
            characterOne.Add(characterBody);
            characterOne.Add(characterFace);    //저장할때 쓰이는 데이터
        }
        else if(currentCharacter.Count == 1){
            characterTwo.Add(characterName);
            characterTwo.Add(xpos.ToString());
            characterTwo.Add(ypos.ToString());
            characterTwo.Add(characterBody);
            characterTwo.Add(characterFace);    //저장할때 쓰이는 데이터
        }
        
    }

    public void FadeIn(string name, string face, string body, float xpos, float ypos, float time)
    {
        setCharacter(name, face, body, xpos, ypos);
        currentCharacter[name].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        currentCharacter[name].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 0);

        fadeInSequence = DOTween.Sequence()
        .Append(currentCharacter[name].GetComponent<Image>().DOFade(1.0f, time))
        .Join(currentCharacter[name].transform.GetChild(0).GetComponent<Image>().DOFade(1.0f, time));
    }

    public void FadeOut(string name, float time)
    {
        fadeOutSequence = DOTween.Sequence()
        .Append(currentCharacter[name].GetComponent<Image>().DOFade(0, time))
        .Join(currentCharacter[name].transform.GetChild(0).GetComponent<Image>().DOFade(0, time))
        .OnComplete(() =>
        {
            CharacterKill(name);
        });
    }

    public void CharacterMove(string name, float xpos, float ypos, float time)
    {
        characterMoveSequence = DOTween.Sequence()
        .Append(currentCharacter[name].transform.DOLocalMove(new Vector3((-0.5f + xpos) * Screen.width, (-0.5f + ypos) * Screen.height, 0), time));
    }

    public Sprite FindFace(string name)
    {
        foreach (var i in faceImageList)
        {
            if (i.name == name)
            {
                Sprite sprite = Sprite.Create((i as Texture2D), new Rect(0, 0, (i as Texture2D).width, (i as Texture2D).height), new Vector2(0.5f, 0.5f));
                return sprite;
            }
        }

        Debug.LogFormat(this, "{0}이라는 표정이 없습니다.", name);
        return null;
    }

    public Sprite FindCha(string name)
    {
        foreach (var i in characterImageList)
        {
            if (i.name == name)
            {
                Sprite sprite = Sprite.Create((i as Texture2D), new Rect(0, 0, (i as Texture2D).width, (i as Texture2D).height), new Vector2(0.5f, 0.5f));
                return sprite;
            }
        }

        Debug.LogFormat(this, "{0}이라는 캐릭터가 없습니다.", name);
        return null;
    }

    public void SetFace(GameObject character, string faceName)
    {
        character.transform.GetChild(0).GetComponent<Image>().sprite = FindFace(character.name + faceName);
    }

    public void CharacterKill(string name)
    {
        Destroy(currentCharacter[name]);
        currentCharacter.Remove(name);
    }
    public Dictionary<string, GameObject> getCurrentCharacter()
    {
        return this.currentCharacter;
    }

    public void setCurrentCharacter(Dictionary<string, GameObject> newDict)
    {
        currentCharacter = newDict;
    }
}
