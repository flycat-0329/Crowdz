using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class TitleMenuOnOff : MonoBehaviour
{
    public GameObject titleMenuPanel;
    public AudioSource bgm;

    public void TitleMenuOn(){
        titleMenuPanel.transform.DOMoveY(0, 1);
    }

    public void TitleMenuOff(){
        titleMenuPanel.transform.DOLocalMoveY(-Screen.height, 1);
    }
}
