using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageEffectManager : MonoBehaviour
{   
    public GameObject effectImage;
    Object[] EffectImageList;
    Sequence fadeImageSequence;
    private void Awake() 
    {
        EffectImageList = Resources.LoadAll("Images/Effect");
    }
    
    public void fadeImageEffect(string imageName)   //사진페이드(나타났다가 사라짐)
    {
        Sprite sprite = findEffectImage(imageName);
        effectImage.GetComponent<Image>().sprite = sprite;
        effectImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sprite.bounds.size.x);
        effectImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sprite.bounds.size.y);

        fadeImageSequence = DOTween.Sequence()
        .Append(effectImage.GetComponent<Image>().DOFade(1.0f, 0.5f))
        .AppendInterval(0.8f)
        .Append(effectImage.GetComponent<Image>().DOFade(0.0f, 0.5f))
        .SetId("fadeImage");
    }

    public void EffectImageOn(string imageName, float fadeTime){        //사진 등장(이펙트 이미지 페이드인)
        Sprite sprite = findEffectImage(imageName);
        effectImage.GetComponent<Image>().sprite = sprite;
        effectImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sprite.bounds.size.x);
        effectImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sprite.bounds.size.y);

        effectImage.GetComponent<Image>().DOFade(1.0f, fadeTime);
    }

    public void EffectImageOff(float fadeTime){     //사진 퇴장(이펙트 이미지 페이드아웃)
        effectImage.GetComponent<Image>().DOFade(0f, fadeTime);

        effectImage.transform.position = new Vector3(0, 0, 0);
    }
    public void EffectImageMove(float xpos, float ypos, float time){    //사진이동(이펙트 이미지 움직이기)
        effectImage.transform.DOLocalMove(new Vector3((-0.5f + xpos) * Screen.width, (-0.5f + ypos) * Screen.height, 0), time);
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
