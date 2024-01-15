using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AnimManager : MonoBehaviour
{
    public GameObject myVideo;
    public VideoPlayer videoPlayer;
    // public GameObject backgroundImage;
    // public GameObject backgroundAnimImage;

    // public void AnimOn(string animName){
    //     backgroundImage.SetActive(true);
    //     backgroundAnimImage.GetComponent<Animator>().Play(animName);
    // }

    public void playVideo(){
        

        myVideo.SetActive(true);
        videoPlayer.Play();
    }
}
