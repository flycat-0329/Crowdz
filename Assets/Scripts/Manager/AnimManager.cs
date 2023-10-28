using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    public GameObject backgroundImage;
    public GameObject backgroundAnimImage;

    public void AnimOn(string animName){
        backgroundImage.SetActive(false);
        backgroundAnimImage.GetComponent<Animator>().Play(animName);
    }
}
