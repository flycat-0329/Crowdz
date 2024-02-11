using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TitleMenuOnOff : MonoBehaviour
{
    public GameObject titleMenuPanel;
    public AudioSource bgm;

    private void Start() {
        titleMenuPanel.transform.localPosition = new Vector3(0, -Screen.height, 0);
        bgm.clip = Resources.Load("Sounds/BGM/Title") as AudioClip;
        bgm.volume = 1f;
        bgm.Play();
    }
    public void TitleMenuOn(){
        titleMenuPanel.transform.DOMoveY(0, 1);
    }

    public void TitleMenuOff(){
        titleMenuPanel.transform.DOLocalMoveY(-Screen.height, 1);
    }
}
