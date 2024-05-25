using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class TitleMenuOnOff : MonoBehaviour
{
    public GameObject titleMenuPanel;
    public AudioSource bgm;

    private void Start() {
        // titleMenuPanel.transform.localScale = new Vector3(1, ((float)Screen.height / Screen.width) * 16 / 9, 1);
        // titleMenuPanel.transform.localPosition = new Vector3(0, -(int)(((float)Screen.height / Screen.width) * 1920), 0);
        bgm.clip = Resources.Load("Sounds/BGM/Title") as AudioClip;
        bgm.volume = 1f;
        bgm.Play();
    }
    public void TitleMenuOn(){
        titleMenuPanel.transform.DOMoveY(0, 1);
    }

    public void TitleMenuOff(){
        titleMenuPanel.transform.DOLocalMoveY(-1080 * 1920, 1);
    }
}
