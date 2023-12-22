using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EffectManager : MonoBehaviour
{
    Object[] EffectImageList;
    public List<GameObject> objects;
    public GameObject fadePanel;
    public CharacterManager characterManager;
    public GameObject effectImage;
    public GameObject dialoguePanel;    //정상적인 대사 ui
    public GameObject centerDialoguePanel;    //가운데에 나오는 대사 ui
    Sequence fadeSequence;
    Sequence characterFadeInSequence;
    Sequence characterFadeOutSequence;
    Sequence fadeImageSequence;
    Sequence characterBounceSequence;
    Sequence characterSizeSequence;
    private void Awake()
    {
        EffectImageList = Resources.LoadAll("Images/Effect");
    }
    private void Start()
    {
        FadeAll();
    }
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

    public void CharacterBounce(GameObject character){  //캐릭터 떨림
        characterBounceSequence = DOTween.Sequence()
        .Append(character.transform.DOLocalJump(new Vector3(0, 0f, 0), 70, 1, 0.16f).SetRelative())
        .Append(character.transform.DOLocalJump(new Vector3(0, 0f, 0), 50, 1, 0.16f).SetRelative())
        .Append(character.transform.DOLocalJump(new Vector3(0, 0f, 0), 30, 1, 0.16f).SetRelative())
        .SetId("characterBounce");
    }

    public void FadeAll()
    {
        fadeSequence = DOTween.Sequence()
        .Append(fadePanel.GetComponent<Image>().DOFade(1.0f, 1))
        .Append(fadePanel.GetComponent<Image>().DOFade(0.0f, 1))
        .SetId("fadeAll");
    }

    public void CharacterSize(GameObject character, Vector3 size, float time){
        if (character.transform.GetChild(0).GetComponent<Image>().sprite == null)
        {
            characterSizeSequence = DOTween.Sequence()
            .Append(character.transform.GetChild(0).GetComponent<Image>().DOFade(0, time))
            .Join(character.transform.GetChild(1).GetComponent<Image>().DOFade(0, time))
            .Append(character.transform.DOScale(size, 0f))
            .AppendInterval(0.5f)
            .Append(character.transform.GetChild(1).GetComponent<Image>().DOFade(1.0f, time))
            .SetId("characterSize");
        }
        else
        {   characterSizeSequence = DOTween.Sequence()
            .Append(character.transform.GetChild(0).GetComponent<Image>().DOFade(0, time))
            .Join(character.transform.GetChild(1).GetComponent<Image>().DOFade(0, time))
            .Append(character.transform.DOScale(size, 0f))
            .AppendInterval(0.5f)
            .Append(character.transform.GetChild(0).GetComponent<Image>().DOFade(1.0f, time))
            .Join(character.transform.GetChild(1).GetComponent<Image>().DOFade(1.0f, time))
            .SetId("characterSize");
        }
    }

    public void FadeInCharacter(GameObject character, float time)
    {
        if (character.transform.GetChild(0).GetComponent<Image>().sprite == null)
        {
            characterFadeInSequence = DOTween.Sequence()
            .Append(character.transform.GetChild(1).GetComponent<Image>().DOFade(1.0f, time))
            .SetId("characterFadeIn");
        }
        else
        {
            characterFadeInSequence = DOTween.Sequence()
            .Append(character.transform.GetChild(0).GetComponent<Image>().DOFade(1.0f, time))
            .Join(character.transform.GetChild(1).GetComponent<Image>().DOFade(1.0f, time))
            .SetId("characterFadeIn");
        }
    }

    public void FadeOutCharacter(GameObject character, float time)
    {
        characterFadeOutSequence = DOTween.Sequence()
        .Append(character.transform.GetChild(0).GetComponent<Image>().DOFade(0, time))
        .Join(character.transform.GetChild(1).GetComponent<Image>().DOFade(0, time))
        .OnComplete(() =>
        {
            characterManager.CharacterKill(character.name);
        })
        .SetId("characterFadeOutKill");
    }
    public void FadeOutMoveCharacter(GameObject character, float xpos, float ypos, float time)
    {
        characterFadeOutSequence = DOTween.Sequence()
        .Append(character.transform.GetChild(0).GetComponent<Image>().DOFade(0, time))
        .Join(character.transform.GetChild(1).GetComponent<Image>().DOFade(0, time))
        .Join(character.transform.DOLocalMove(new Vector3((-0.5f + xpos) * Screen.width, (-0.5f + ypos) * Screen.height, 0), time))
        .OnComplete(() =>
        {
            characterManager.CharacterKill(character.name);
        })
        .SetId("characterFadeOutKill");
    }
    public void fadeImageEffect(string imageName)   //화면에 사진 띄우는거
    {
        effectImage.GetComponent<Image>().sprite = findEffectImage(imageName);

        fadeImageSequence = DOTween.Sequence()
        .Append(effectImage.GetComponent<Image>().DOFade(1.0f, 0.5f))
        .AppendInterval(0.8f)
        .Append(effectImage.GetComponent<Image>().DOFade(0.0f, 0.5f))
        .SetId("fadeImage");
    }

    public void EffectImageOn(string imageName, float fadeTime){
        effectImage.GetComponent<Image>().sprite = findEffectImage(imageName);

        effectImage.GetComponent<Image>().DOFade(1.0f, fadeTime);
    }

    public void EffectImageOff(float fadeTime){
        effectImage.GetComponent<Image>().DOFade(0f, fadeTime);

        effectImage.transform.position = new Vector3(0, 0, 0);
    }
    public void EffectImageMove(float xpos, float ypos, float time){
        effectImage.transform.DOLocalMove(new Vector3((-0.5f + xpos) * Screen.width, (-0.5f + ypos) * Screen.height, 0), time);
    }


    public void centerUISwitch(){
        dialoguePanel.SetActive(!dialoguePanel.activeSelf);
        centerDialoguePanel.SetActive(!centerDialoguePanel.activeSelf);
    }

    public Sprite findEffectImage(string spriteName)
    {
        foreach (var i in EffectImageList)
        {
            if (i.name == spriteName)
            {
                Sprite sprite = Sprite.Create((i as Texture2D), new Rect(0, 0, (i as Texture2D).width, (i as Texture2D).height), new Vector2(0.5f, 0.5f));
                return sprite;
            }
        }

        Debug.LogFormat(this, "{0}이라는 페이드 이미지가 없습니다.", name);
        return null;
    }
}
