using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ESManager : MonoBehaviour
{
    public Object[] ESaudioClips;
    public AudioSource ESaudioSource;
    public Slider ESslider;
    float textVolume = 1;   //대본에 적혀있는 볼륨값
    public AudioClip clickSound;
    AudioClip esAudioclip;
    string esName;   //반복 효과음 이름
    float esVol;    //반복 효과음 크기
    private void Awake() {
        ESaudioClips = Resources.LoadAll("Sounds/Effect");
        ESslider.value = SettingManager.instance.esVolume;
        ESaudioSource.volume = SettingManager.instance.esVolume;
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            ESaudioSource.PlayOneShot(clickSound);
        }
    }

    public void playES(string name, float scriptVolume){
        AudioClip audioClip = findES(name);
        textVolume = scriptVolume;
        esVolume();

        ESaudioSource.PlayOneShot(audioClip);
    }
    public void playES(string name, float scriptVolume, float fadeTime){
        AudioClip audioClip = findES(name);
        textVolume = scriptVolume;
        esVolume();
        ESaudioSource.PlayOneShot(audioClip);

        ESaudioSource.DOFade(0, fadeTime);
    }

    public void playLoopES(string name, float scriptVolume){    //효과음 반복
        esAudioclip = findES(name);
        esVol = scriptVolume;
        InvokeRepeating("ESLoopPlay", 0, esAudioclip.length);
    }

    void ESLoopPlay(){  //인보크 수행용
        textVolume = esVol;
        esVolume();

        ESaudioSource.PlayOneShot(esAudioclip);
    }

    public void StopES(){   //효과음 반복 취소
        CancelInvoke("ESLoopPlay");
        ESaudioSource.Stop();
    }
    AudioClip findES(string name){
        foreach(var i in ESaudioClips){
            if(name == i.name){
                return i as AudioClip;
            }
        }

        Debug.LogFormat(this, "{0}이라는 효과음이 없습니다.", name);
        return null;
    }

    public void esVolume(){
        SettingManager.instance.esVolume = ESslider.value;
        ESaudioSource.volume = ESslider.value * textVolume;

        PlayerPrefs.SetFloat("esVolume", SettingManager.instance.esVolume);
    }
}
