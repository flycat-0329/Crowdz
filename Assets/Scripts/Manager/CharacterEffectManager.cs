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
    
    public void CharacterBounce(GameObject character){  //캐릭터 떨림
        characterBounceSequence = DOTween.Sequence()
        .Append(character.transform.DOLocalJump(new Vector3(0, 0f, 0), 70, 1, 0.16f).SetRelative())
        .Append(character.transform.DOLocalJump(new Vector3(0, 0f, 0), 50, 1, 0.16f).SetRelative())
        .Append(character.transform.DOLocalJump(new Vector3(0, 0f, 0), 30, 1, 0.16f).SetRelative())
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
    public void FadeOutMoveCharacter(GameObject character, float xpos, float ypos, float time)
    {       //페이드아웃 하면서 캐릭터 움직임(보통 퇴장용)
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
}
