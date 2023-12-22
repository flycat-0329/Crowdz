using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterManager : MonoBehaviour
{
    public Dictionary<string, GameObject> currentCharacter = new Dictionary<string, GameObject>();  //현재 존재하는 캐릭터들
    public List<CharacterSet> characterList = new List<CharacterSet>();
    Object[] EffectImageList; //시작할때 불러오는 캐릭터 얼굴 이미지
    Object[] characterImageList;    //시작할때 불러오는 캐릭터 몸통 이미지
    public GameObject character;    //캐릭터 오리지널 형태
    public Canvas characterCanvas;  //캐릭터가 있는 캔버스
    public EffectManager effectManager;
    private Sequence characterMoveSequence; //dotween

    private void Awake()
    {
        EffectImageList = Resources.LoadAll("Images/Character/Effect");
        characterImageList = Resources.LoadAll("Images/Character/Body");
    }
    public void setCharacter(string characterName, string characterEffect, string characterBody, float xpos, float ypos)
    {
        GameObject newActor = Instantiate(character);
        newActor.transform.SetParent(characterCanvas.transform);
        newActor.name = characterName;

        newActor.transform.localPosition = new Vector3((-0.5f + xpos) * Screen.width, (-0.5f + ypos) * Screen.height, 0);
        newActor.transform.GetChild(1).transform.localPosition = new Vector3(0, 0, 0);
        newActor.transform.GetChild(0).transform.localPosition = new Vector3(0, 135, 0);

        newActor.transform.localScale = new Vector3(1, 1, 1);
        newActor.transform.GetChild(1).transform.localScale = new Vector3(1, 1, 1);
        newActor.transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);

        newActor.transform.GetChild(1).GetComponent<Image>().sprite = FindCha(characterName + "_" + characterBody);    //캐릭터의 몸 이미지
        currentCharacter[characterName] = newActor.gameObject;
        SetEffect(characterName, characterEffect);

        characterList.Add(new CharacterSet(characterName, characterBody, characterEffect, xpos, ypos));
    }

    public void setCharacter(string characterName, string characterBody, float xpos, float ypos)
    {
        GameObject newActor = Instantiate(character);
        newActor.transform.SetParent(characterCanvas.transform);
        newActor.name = characterName;

        newActor.transform.localPosition = new Vector3((-0.5f + xpos) * Screen.width, (-0.5f + ypos) * Screen.height, 0);
        newActor.transform.GetChild(1).transform.localPosition = new Vector3(0, 0, 0);
        newActor.transform.GetChild(0).transform.localPosition = new Vector3(0, 135, 0);

        newActor.transform.localScale = new Vector3(1, 1, 1);
        newActor.transform.GetChild(1).transform.localScale = new Vector3(1, 1, 1);
        newActor.transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);

        newActor.transform.GetChild(1).GetComponent<Image>().sprite = FindCha(characterName + "_" + characterBody);    //캐릭터의 몸 이미지
        currentCharacter[characterName] = newActor.gameObject;
        newActor.transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        characterList.Add(new CharacterSet(characterName, characterBody, xpos, ypos));
    }

    public void FadeIn(string name, string effect, string body, float xpos, float ypos, float time)
    {
        setCharacter(name, effect, body, xpos, ypos);
        currentCharacter[name].transform.GetChild(1).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        currentCharacter[name].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        effectManager.FadeInCharacter(currentCharacter[name], time);
    }

    public void FadeIn(string name, string body, float xpos, float ypos, float time)
    {
        setCharacter(name, body, xpos, ypos);
        currentCharacter[name].transform.GetChild(1).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        currentCharacter[name].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        effectManager.FadeInCharacter(currentCharacter[name], time);
    }

    public void FadeOut(string name, float time)
    {
        effectManager.FadeOutCharacter(currentCharacter[name], time);
    }

    public void FadeOutMove(string name, float xpos, float ypos, float time){
        effectManager.FadeOutMoveCharacter(currentCharacter[name], xpos, ypos, time);
    }

    public void characterBounce(string name){
        effectManager.CharacterBounce(currentCharacter[name]);
    }

    public void CharacterSize(string name, float size, float time){
        effectManager.CharacterSize(currentCharacter[name], new Vector3(size, size, 1), time);
    }

    public void CharacterMove(string name, float xpos, float ypos, float time)
    {
        characterMoveSequence = DOTween.Sequence()
        .Append(currentCharacter[name].transform.DOLocalMove(new Vector3((-0.5f + xpos) * Screen.width, (-0.5f + ypos) * Screen.height, 0), time))
        .OnComplete(() => 
        {
            foreach(CharacterSet c in characterList){
                if(c.characterName == name){
                    c.characterXpos = xpos;
                    c.characterYpos = ypos;
                }
            }
        });
    }

    public Sprite FindEffect(string name)
    {
        foreach (var i in EffectImageList)
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

    public void SetEffect(string name, string effectName)
    {
        GameObject character = currentCharacter[name];
        character.transform.GetChild(0).GetComponent<Image>().sprite = FindEffect(character.name + effectName);
        for(int i = 0; i < characterList.Count; i++){
            if(characterList[i].characterName == name){
                characterList[i].characterEffect = effectName;
            }
        }

        character.transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void CharacterKill(string name)
    {
        Destroy(currentCharacter[name]);
        currentCharacter.Remove(name);

        for(int i = 0; i < characterList.Count; i++){
            if(characterList[i].characterName == name){
                characterList.Remove(characterList[i]);
                break;
            }
        }
    }
    public Dictionary<string, GameObject> getCurrentCharacter()
    {
        return this.currentCharacter;
    }

    public void setCurrentCharacter(Dictionary<string, GameObject> newDict)
    {
        currentCharacter = newDict;
    }

    public List<CharacterSet> getCharacterList()
    {
        return this.characterList;
    }

    public void setCharacterList(List<CharacterSet> characterList)
    {
        this.characterList = characterList;
    }
} 
