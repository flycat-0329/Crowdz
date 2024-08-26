using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterEffectManager : MonoBehaviour
{
    public CharacterManager characterManager;
    Sequence characterFadeInSequence;
    Sequence characterFadeOutSequence;
    Sequence characterBounceSequence;
    Sequence characterSizeSequence;
    Sequence characterColorSequence;
    
    public void CharacterBounce(GameObject character, float pow){  //캐릭터 떨림
        characterBounceSequence = DOTween.Sequence()
        .Append(character.transform.DOLocalJump(new Vector3(0, 0f, 0), 70 * pow, 1, 0.16f).SetRelative())
        .Append(character.transform.DOLocalJump(new Vector3(0, 0f, 0), 50 * pow, 1, 0.16f).SetRelative())
        .Append(character.transform.DOLocalJump(new Vector3(0, 0f, 0), 30 * pow, 1, 0.16f).SetRelative())
        .SetId("characterBounce");
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

    public void BlurCharacter(GameObject character, Material material, float size){
        character.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().material = material;
        character.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        material.SetFloat("_Size", size);
        character.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
    }

    public void CharacterShaderNull(GameObject character){
        character.GetComponent<Image>().material = null;
    }

    public void CharacterColor(GameObject character, float r = 0, float g = 0, float b = 0, float time = 0){
        characterColorSequence = DOTween.Sequence()
        .Append(character.transform.GetChild(1).gameObject.GetComponent<Image>().DOColor(new Color(r, g, b, 1), time))
        .SetId("CharacterColor");
    }
}
