using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
public class LinePrint : MonoBehaviour
{
    public LogManager logManager;
    public StoryManager storyManager;
    public CharacterManager characterManager;
    public BackgroundManager backgroundManager;
    public BGMmanager bgmManager;
    public ESManager esManager;
    public EffectManager effectManager;
    public ChoiceManager choiceManager;
    public SaveManager saveManager;
    public DataSet dataSet;
    public TextMeshProUGUI nameText;    //이름 나오는 텍스트
    public TextMeshProUGUI mainText;    //대사 나오는 텍스트
    private int scriptIndex = -1;        //현재 몇번째 대사인가, 세이브/로드 구현 시 바뀔 부분
    private float typingSpeed = 0.025f;       //글자 출력 주기
    private float scriptbgmVolume = 1;
    private string scriptTitle;
    public Text t;                      //나중에 없앨거
    private bool onTyping = false;      //글자가 나오는 중인가?
    int lineActionIndex = 0;            //한 대사 안에서 몇번째 연출을 수행중인가
    // bool onTimer = false;               //일시정지중인가?
    bool onAction = false;      //연출 나오는중인가?
    bool onAuto = false;        //오토모드 돌리고있는 중인가?
    bool onSkip = false;        //스킵 돌리는 중인가?
    bool onInit = false;        //방금 시작했는가?
    List<List<string>> script = new List<List<string>>();   //대사만 모아놓은거
    List<List<List<string>>> action = new List<List<List<string>>>();   //연출만 모아놓은거
    // Start is called before the first frame update
    void Start()
    {
        onInit = true;

        bgmManager = GameObject.Find("BGAudioSource").GetComponent<BGMmanager>();
        esManager = GameObject.Find("ESAudioSource").GetComponent<ESManager>();

        scriptTitle = SettingManager.instance.initDataSet.saveScriptName;   //저장된 대본 이름
        scriptIndex = SettingManager.instance.initDataSet.saveScriptIndex;  //저장된 대본 위치

        nameText.text = SettingManager.instance.initDataSet.saveCurrentCharacter;   //저장된 시점의 말하는 캐릭터 이름
        mainText.text = SettingManager.instance.initDataSet.saveDialogue;        //저장된 시점의 대사

        if(SettingManager.instance.initDataSet.saveIsAnim){
            backgroundManager.BackgroundAnimOn(SettingManager.instance.initDataSet.saveBackgroundImage);    //저장된 시점의 애니메이션
        }
        else{
            backgroundManager.BackgroundImageOn(SettingManager.instance.initDataSet.saveBackgroundImage);    //저장된 시점의 배경
        }

        SettingManager.instance.mainVolume = SettingManager.instance.initDataSet.saveBGMVolume; //저장된 시점의 브금 볼륨
        scriptbgmVolume = SettingManager.instance.initDataSet.saveScriptBGMVolume;              //저장된 시점의 브금 볼륨(대본상)
        SettingManager.instance.esVolume = SettingManager.instance.initDataSet.saveESVolume;    //저장된 시점의 효과음 볼륨
        bgmManager.playBGM(SettingManager.instance.initDataSet.saveBGMName, scriptbgmVolume);   //저장된 시점의 브금 이름

        for (int i = 0; i < SettingManager.instance.initDataSet.saveCharacterList.Count; i++)   //저장된 시점의 캐릭터들
        {
            CharacterSet cha = SettingManager.instance.initDataSet.saveCharacterList[i];
            characterManager.setCharacter(cha.characterName, cha.characterFace, cha.characterBody,
            cha.characterXpos, cha.characterYpos);
        }

        Debug.Log(scriptTitle);
        KeyValuePair<List<List<string>>, List<List<List<string>>>> a = storyManager.storyFileRead(scriptTitle);
        script = a.Key;
        action = a.Value;
    
        PrintController();

        onInit = false;
    }
    public void NewScript(String scriptName)        //새로운 대본 파일을 읽어오는 함수
    {
        scriptTitle = scriptName;
        KeyValuePair<List<List<string>>, List<List<List<string>>>> a = storyManager.storyFileRead(scriptName);
        script = a.Key;
        action = a.Value;

        scriptIndex = -1;
    }
    private void Update()
    {
        t.text = scriptIndex.ToString();
    }
    public void PrintController()
    {
        if (onTyping == true)   //대사 출력중일때
        {
            onTyping = false;   //대사 출력 애니메이션 스킵

            // onTimer = false;    //연출 스킵
            for (int i = 0; i < 10; i++)
            {    //연출 스킵
                ActionPlay();
            }

            DOTween.KillAll(true);
        }

        // else if (onTimer == true)
        // {   //대사는 끝났는데 연출은 안 끝났을 때(보통 이런경우는 별로 없음)
        //     onTimer = false;    //연출 스킵
        //     for (int i = 0; i < 10; i++)
        //     {    //연출 스킵
        //         ActionPlay();
        //         effectManager.SequenceKill();
        //     }

        //     scriptIndex += 1;   //대사 출력중이 아니라면 다음 대사로
        //     lineActionIndex = 0;    //연출도 다음 대사의 첫번째 연출부터 진행되도록
        //     ActionPlay();   //다음 대사 연출 시작
        //     nameText.text = script[scriptIndex][1];     //이름 변경
        //     StartCoroutine(PrintLine(script[scriptIndex][2]));  //대사 출력
        // }

        else    //대사 출력중이 아니라면 다음 대사로
        {
            scriptIndex += 1;
            lineActionIndex = 0;    //연출도 그 대사의 첫번째 연출부터 진행되도록
            ActionPlay();   //다음 대사 연출 시작
            nameText.text = script[scriptIndex][1];     //이름 변경
            StartCoroutine(PrintLine(script[scriptIndex][2]));  //대사 출력
            logManager.LogMaker(script[scriptIndex][1], script[scriptIndex][2]);
        }
    }
    public void ActionPlay()    //클릭했을 때 다음 대사에 맞는 연출을 띄우는 함수
    {
        if (lineActionIndex < action[scriptIndex].Count)
        {
            onAction = true;
            List<string> oneAction = action[scriptIndex][lineActionIndex];
            lineActionIndex += 1;

            switch (oneAction[0])
            {
                case "등장":    //<등장, 캐릭터 이름, 표정, 자세, x좌표(0 ~ 1), y좌표(0 ~ 1)>
                    if (onInit == false)
                    {    //이거 없으면 로드되는 순간에 캐릭터 복제됨
                        characterManager.setCharacter(oneAction[1], oneAction[2], oneAction[3],
                        float.Parse(oneAction[4]), float.Parse(oneAction[5]));
                    }
                    ActionPlay();
                    break;
                case "표정":    //<표정, 바꿀 캐릭터, 바꿀 표정>
                    characterManager.SetFace(oneAction[1], oneAction[2]);
                    ActionPlay();
                    break;
                case "퇴장":    //<퇴장, 퇴장할 캐릭터 이름>
                    characterManager.CharacterKill(oneAction[1]);
                    ActionPlay();
                    break;
                case "배경":    //<배경, 배경이름>
                    backgroundManager.BackgroundImageOn(oneAction[1]);
                    ActionPlay();
                    break;
                case "속도":    //<속도, 타이핑 속도>
                    typingSpeed = int.Parse(oneAction[1]);
                    ActionPlay();
                    break;
                case "크기":    //<크기, 글자 크기>
                    mainText.fontSize = Int32.Parse(oneAction[1]);
                    ActionPlay();
                    break;
                case "페이드아웃":    //<페이드아웃, 캐릭터, 시간>
                    characterManager.FadeOut(oneAction[1], float.Parse(oneAction[2]));
                    ActionPlay();
                    break;
                case "페이드인":    //<페이드인, 캐릭터, 표정, 자세, x좌표(0 ~ 1), y좌표(0 ~ 1), 시간>
                    characterManager.FadeIn(oneAction[1], oneAction[2], oneAction[3], float.Parse(oneAction[4]),
                    float.Parse(oneAction[5]), float.Parse(oneAction[6]));
                    ActionPlay();
                    break;
                case "이동":        //<이동, 캐릭터, x좌표(0 ~ 1), y좌표(0 ~ 1), 시간>
                    characterManager.CharacterMove(oneAction[1], float.Parse(oneAction[2]),
                    float.Parse(oneAction[3]), float.Parse(oneAction[4]));
                    ActionPlay();
                    break;
                case "브금":        //<브금, 브금 제목, 음량(크게 틀건지 작게 틀건지 / 0 ~ 1), 페이드 아웃 시간(기본값 0)>
                    scriptbgmVolume = float.Parse(oneAction[2]);
                    bgmManager.playBGM(oneAction[1], float.Parse(oneAction[2]));
                    ActionPlay();
                    break;
                case "효과음":      //<효과음, 효과음 이름, 음량(크게 틀건지 작게 틀건지 / 0 ~ 1)>
                    esManager.playES(oneAction[1], float.Parse(oneAction[2]));
                    ActionPlay();
                    break;
                case "흔들":        //<흔들, 시간, 강도>
                    effectManager.Shake(float.Parse(oneAction[1]), float.Parse(oneAction[2]));
                    ActionPlay();
                    break;
                case "애니메이션":
                    backgroundManager.BackgroundAnimOn(oneAction[1]);
                    break;
                // case "타이머":      //<타이머, 시간> 
                //     onTimer = true;
                //     StartCoroutine(Timer(float.Parse(oneAction[1])));
                //     break;
                case "선택지":      //<선택지, 선택지 이름, 옵션1, 옵션2, 옵션3(없어도 됨)>
                    if (oneAction.Count == 4)
                    {
                        choiceManager.ChoiceAppear(oneAction[1], oneAction[2], oneAction[3]);
                    }
                    else if (oneAction.Count == 5)
                    {
                        choiceManager.ChoiceAppear(oneAction[1], oneAction[2], oneAction[3], oneAction[4]);
                    }
                    break;
            }
        }
        else
        {
            onAction = false;
        }
    }
    private IEnumerator PrintLine(string line)
    {
        mainText.text = "";
        WaitForSeconds wfs = new WaitForSeconds(typingSpeed);   //캐싱(최적화)
        onTyping = true;

        for (int i = 0; i < line.Length; i++)
        {
            if (onTyping == false)
            {
                mainText.text = line;
                break;
            }
            mainText.text = line.Substring(0, i + 1);
            yield return wfs;
        }

        onTyping = false;
    }
    // private IEnumerator Timer(float time)
    // {
    //     yield return new WaitForSeconds(time);
    //     if (onTimer == true)
    //     {
    //         onTimer = false;
    //         ActionPlay();
    //     }
    // }
    public void OnAuto()
    {
        onAuto = !onAuto;
        StartCoroutine(AutoPlaying());
    }
    public IEnumerator AutoPlaying()
    {
        WaitForSeconds auto = new WaitForSeconds(0.7f);       //캐싱(최적화)
        bool aaa = false;

        while (onAuto)
        {
            if (onAction == false && onTyping == false)
            {
                aaa = true;
            }
            yield return auto;
            if (aaa == true)
            {
                aaa = false;
                PrintController();
            }
        }

    }
    public void OnSkip()
    {
        onSkip = !onSkip;
        StartCoroutine(SkipPlaying());
    }
    public IEnumerator SkipPlaying()
    {
        WaitForSeconds skiper = new WaitForSeconds(0.02f);
        while (onSkip)
        {
            PrintController();
            yield return skiper;
        }
    }
    public void SaveClicked(string index)
    {
        dataSet = new DataSet(characterManager.getCharacterList(), backgroundManager.isAnim, mainText.text, nameText.text, scriptIndex - 1, scriptTitle,
        backgroundManager.backgroundName, bgmManager.BGMname, SettingManager.instance.mainVolume, scriptbgmVolume,
        SettingManager.instance.esVolume);

        saveManager.GameSave(dataSet, index);
    }
}
