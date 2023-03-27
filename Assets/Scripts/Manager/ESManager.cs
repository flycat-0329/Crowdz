using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESManager : MonoBehaviour
{
    public Object[] ESaudioClips;
    public AudioSource ESaudioSource;
    public Slider ESslider;
    float textVolume;   //대본에 적혀있는 볼륨값
    private void Start() {
        ESaudioClips = Resources.LoadAll("Sounds/Effect");
        ESslider.value = SettingManager.instance.esVolume;
        esVolume();
    }

    public void playES(string name, float scriptVolume){
        AudioClip audioClip = findES(name);
        ESaudioSource.volume = SettingManager.instance.esVolume * scriptVolume;
        ESaudioSource.clip = audioClip;
        textVolume = scriptVolume;
        ESaudioSource.Play();
    }

    AudioClip findES(string name){
        foreach(var i in ESaudioClips){
            Debug.Log(i.name);
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
    }
}
