using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BGMmanager : MonoBehaviour
{
    public Object[] BGMList;
    //배경 사운드들이 들어갈 리스트
    public string BGMname;
    public AudioSource BGaudioSource;
    //브금을 출력시키는 스피커
    public Slider BGslider;
    //브금 볼륨값 조절 슬라이더
    float textVolume = 1;   //대본에 적혀있는 볼륨값

    private void Awake() {
        BGMList = Resources.LoadAll("Sounds/BGM");
        BGslider.value = SettingManager.instance.mainVolume;
        BGaudioSource.volume = SettingManager.instance.mainVolume;
        playBGM("Title", 1);
    }
    public void playBGM(string name, float scriptVolume){  //대본에 적힌 브금과 볼륨으로 브금 틀기
        BGMname = name;
        BGaudioSource.Stop();   //일단 브금을 멈추고

        AudioClip bgAudio = findBGM(name);  //브금 이름으로 BGAudioClips에서 브금을 찾아옴
        
        BGaudioSource.clip = bgAudio;       //노래를 바꾸고
        textVolume = scriptVolume;
        bgmVolume();
        BGaudioSource.loop = true;          //루프를 켜고
        BGaudioSource.Play();               //브금을 다시 틀어줍니다.
    }

    public void playBGM(string name, float scriptVolume, float fadeTime = 0){
        BGMname = name;
        BGaudioSource.Stop();   //일단 브금을 멈추고

        AudioClip bgAudio = findBGM(name);  //브금 이름으로 BGAudioClips에서 브금을 찾아옴

        BGaudioSource.clip = bgAudio;       //노래를 바꾸고
        textVolume = scriptVolume;
        bgmVolume();
        BGaudioSource.loop = true;          //루프를 켜고
        BGaudioSource.Play();               //브금을 다시 틀어줍니다.
        BGaudioSource.DOFade(0, fadeTime);
    }

    public void stopBGM(){
        BGaudioSource.Stop();   //브금 멈춰!
    }

    AudioClip findBGM(string name){     //브금 이름을 통해 브금을 리턴하는 함수
        foreach(var i in BGMList){     //BGAudioClips를 돌면서
            if(i.name == name){         //해당하는 이름의 브금을 찾으면 리턴합니다
                return i as AudioClip;
            }
        }

        Debug.LogFormat(this, "{0}이라는 브금이 없습니다.", name);  //없으면 망한거지
        return null;
    }

    public void volumeChange(float volume){
        BGaudioSource.volume = SettingManager.instance.mainVolume * volume;
    }

    public void bgmVolume(){
        SettingManager.instance.mainVolume = BGslider.value;    //슬라이더 값이 바뀌면 실시간으로 볼륨을 바꿔주는 함수(에디터에서 씀)
        BGaudioSource.volume = BGslider.value * textVolume;
        
        PlayerPrefs.SetFloat("bgmVolume", SettingManager.instance.mainVolume);
    }
}
