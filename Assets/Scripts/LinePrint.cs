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
    public ImageEffectManager imageEffectManager;
    public ChoiceManager choiceManager;
    public SaveManager saveManager;
    public ParticleManager particleManager;
    public DataSet dataSet;
    public CanvasOnOff canvasOnOff;
    public TextMeshProUGUI nameText;    //이름 나오는 텍스트
    public TextMeshProUGUI mainText;    //지금 대사 나오는 텍스트
    public List<GameObject> mainTextList;   //대사 나오는 텍스트들(아래, 가운데)
    public GameObject scriptCanvas;     //UI 있는 캔버스 1번
    public GameObject effectCanvas;     //UI 있는 캔버스 2번
    public GameObject settingCanvas;    //설정창
    private int scriptIndex = -1;        //현재 몇번째 대사인가, 세이브/로드 구현 시 바뀔 부분
    private float typingSpeed = 0.025f;       //글자 출력 주기
    private float scriptbgmVolume = 1;
    private string scriptTitle = "Chapter1";         //현재 대본 이름
    public Text t;                      //나중에 없앨거
    private bool onTyping = false;      //글자가 나오는 중인가?
    int lineActionIndex = 0;            //한 대사 안에서 몇번째 연출을 수행중인가
    // bool onTimer = false;               //일시정지중인가?
    bool onAction = false;      //연출 나오는중인가?
    bool onAuto = false;        //오토모드 돌리고있는 중인가?
    bool onSkip = false;        //스킵 돌리는 중인가?
    bool onInit = false;        //방금 시작했는가?
    bool onTimer = false;
    List<List<string>> script = new List<List<string>>();   //대사만 모아놓은거
    List<List<List<string>>> action = new List<List<List<string>>>();   //연출만 모아놓은거
    // Start is called before the first frame update
    void Start()
    {
        onInit = true;

        bgmManager = GameObject.Find("BGAudioSource").GetComponent<BGMmanager>();
        esManager = GameObject.Find("ESAudioSource").GetComponent<ESManager>();

        if (SettingManager.instance.isNewGame == false)
        {
            scriptTitle = SettingManager.instance.initDataSet.saveScriptName;   //저장된 대본 이름
            scriptIndex = SettingManager.instance.initDataSet.saveScriptIndex;  //저장된 대본 위치

            nameText.text = SettingManager.instance.initDataSet.saveCurrentCharacter;   //저장된 시점의 말하는 캐릭터 이름
            mainText.text = SettingManager.instance.initDataSet.saveDialogue;        //저장된 시점의 대사

            if (SettingManager.instance.initDataSet.saveIsAnim)
            {
                backgroundManager.BackgroundAnimOn(SettingManager.instance.initDataSet.saveBackgroundImage);    //저장된 시점의 애니메이션
            }
            else
            {
                backgroundManager.BackgroundImageOn(SettingManager.instance.initDataSet.saveBackgroundImage);    //저장된 시점의 배경
            }
            scriptbgmVolume = SettingManager.instance.initDataSet.saveScriptBGMVolume;              //저장된 시점의 브금 볼륨(대본상)
            bgmManager.playBGM(SettingManager.instance.initDataSet.saveBGMName, scriptbgmVolume);   //저장된 시점의 브금 이름

            for (int i = 0; i < SettingManager.instance.initDataSet.saveCharacterList.Count; i++)   //저장된 시점의 캐릭터들
            {
                CharacterSet cha = SettingManager.instance.initDataSet.saveCharacterList[i];
                if (cha.characterEffect == "")
                {
                    characterManager.setCharacter(cha.characterName, cha.characterBody,
                cha.characterXpos, cha.characterYpos);
                }
                else
                {
                    characterManager.setCharacter(cha.characterName, cha.characterEffect, cha.characterBody,
                cha.characterXpos, cha.characterYpos);
                }
            }

            if(SettingManager.instance.initDataSet.saveIsParticle == true){
                foreach(string par in SettingManager.instance.initDataSet.saveParticleNameList){
                    particleManager.ParticleOn(par);
                }
            }
        }

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

        if(Input.GetKey(KeyCode.LeftControl) && onSkip == false){
            OnSkip();
        }

        if(Input.GetKeyUp(KeyCode.LeftControl) && onSkip == true){
            OnSkip();
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            OnAuto();
        }

        if(Input.GetKeyDown(KeyCode.H)){
            canvasOnOff.canvasOnOff(effectCanvas);
            canvasOnOff.canvasOnOff(scriptCanvas);
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            canvasOnOff.canvasOnOff(settingCanvas);
        }
    }
    public void PrintController()
    {
        if(onTimer == true){
            onTimer = false;
            StopCoroutine("Timer");

            PrintController();
        }

        if (onTyping == true)   //대사 출력중일때
        {
            for (int i = 0; i < 2; i++)
            {
                DOTween.CompleteAll(true);
                ActionPlay();
            }

            DOTween.CompleteAll(true);
            onTyping = false;  //대사 출력 애니메이션 스킵
        }
        // else if (onTimer == true)
        // { //대사는 끝났는데 타이머로 인해 연출은 안 끝났을 때
            

        //     for (int i = 0; i < 10; i++)
        //     {    //연출 스킵
        //         ActionPlay();
        //     }

        //     DOTween.KillAll(true, new string[] { "fadeImage" });

        //     PrintController();
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
                case "등장":    //<등장, 캐릭터 이름, (표정), 자세, x좌표(0 ~ 1), y좌표(0 ~ 1)>
                    if (onInit)
                    {
                        break;
                    }
                    if (oneAction.Count == 6)
                    {
                        characterManager.setCharacter(oneAction[1], oneAction[2], oneAction[3],
                        float.Parse(oneAction[4]), float.Parse(oneAction[5]));
                    }
                    else if (oneAction.Count == 5)
                    {
                        characterManager.setCharacter(oneAction[1], oneAction[2],
                        float.Parse(oneAction[3]), float.Parse(oneAction[4]));
                    }
                    ActionPlay();
                    break;
                case "이펙트":    //<이펙트, 바꿀 캐릭터, 넣을 이펙트>
                    if (onInit)
                    {
                        break;
                    }
                    characterManager.SetEffect(oneAction[1], oneAction[2]);
                    ActionPlay();
                    break;
                case "캐릭블러":    //<캐릭블러, 바꿀 캐릭터, 강도>
                    characterManager.CharacterBlur(oneAction[1], float.Parse(oneAction[2]));
                    ActionPlay();
                    break;
                case "캐릭색깔":    //<캐릭색깔, r, g, b, 시간>
                    characterManager.CharacterColor(oneAction[1], float.Parse(oneAction[2]), float.Parse(oneAction[3]),
                    float.Parse(oneAction[4]), float.Parse(oneAction[5]));
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
                case "배경효과":  //<배경효과, 효과 이름, 배경이름, 시간>
                    if(oneAction.Count == 3){
                        backgroundManager.EffectSwitch(oneAction[1], oneAction[2]);
                    }
                    else if(oneAction.Count == 4){
                        backgroundManager.EffectSwitch(oneAction[1], oneAction[2], float.Parse(oneAction[3]));
                    }
                    ActionPlay();
                    break;
                case "파티클":
                    particleManager.ParticleOn(oneAction[1]);
                    ActionPlay();
                    break;
                case "파티클삭제":
                    particleManager.ParticleOff(oneAction[1]);
                    ActionPlay();
                    break;
                case "속도":    //<속도, 타이핑 속도>
                    typingSpeed = float.Parse(oneAction[1]);
                    ActionPlay();
                    break;
                case "크기":    //<크기, 글자 크기>
                    mainText.fontSize = Int32.Parse(oneAction[1]);
                    ActionPlay();
                    break;
                case "색깔":    //<색깔, R, G, B, A>
                    mainText.color = new Color(float.Parse(oneAction[1]), float.Parse(oneAction[2]), float.Parse(oneAction[3]), float.Parse(oneAction[4]));
                    break;
                case "페이드아웃":    //<페이드아웃, 캐릭터, 시간>
                    characterManager.FadeOut(oneAction[1], float.Parse(oneAction[2]));
                    ActionPlay();
                    break;
                case "페이드인":    //<페이드인, 캐릭터, (이펙트), 자세, x좌표(0 ~ 1), y좌표(0 ~ 1), 시간>
                    if (onInit)
                    {
                        break;
                    }
                    if (oneAction.Count == 7)
                    {
                        characterManager.FadeIn(oneAction[1], oneAction[2], oneAction[3], float.Parse(oneAction[4]),
                        float.Parse(oneAction[5]), float.Parse(oneAction[6]));
                    }
                    else if (oneAction.Count == 6)
                    {
                        characterManager.FadeIn(oneAction[1], oneAction[2], float.Parse(oneAction[3]),
                        float.Parse(oneAction[4]), float.Parse(oneAction[5]));
                    }
                    else if (oneAction.Count == 5)
                    {
                        characterManager.FadeIn(oneAction[1], float.Parse(oneAction[2]),
                        float.Parse(oneAction[3]), float.Parse(oneAction[4]));
                    }
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
                case "브금볼륨":    //<브금볼륨, 음량> (브금은 유지하고 볼륨값을 바꿔줌)
                    bgmManager.volumeChange(float.Parse(oneAction[1]));
                    ActionPlay();
                    break;
                case "브금종료":    //<브금종료>
                    bgmManager.stopBGM();
                    ActionPlay();
                    break;
                case "효과음":      //<효과음, 효과음 이름, 음량(크게 틀건지 작게 틀건지 / 0 ~ 1), (페이드 시간)>
                    if(onSkip == true){
                        ActionPlay();
                        break;
                    }
                    if(oneAction.Count == 3){
                        esManager.playES(oneAction[1], float.Parse(oneAction[2]));
                    }
                    else if(oneAction.Count == 4){
                        esManager.playES(oneAction[1], float.Parse(oneAction[2]), float.Parse(oneAction[3]));
                    }
                    ActionPlay();
                    break;
                case "효과음반복":  //<효과음반복, 효과음 이름, 볼륨>
                    if(onSkip == true){
                        ActionPlay();
                        break;
                    }
                    esManager.playLoopES(oneAction[1], float.Parse(oneAction[2]));
                    ActionPlay();
                    break;
                case "효과음종료":  //반복 효과음 종료
                    esManager.StopES();
                    ActionPlay();
                    break;
                case "흔들":        //<흔들, 시간, 강도>
                    effectManager.Shake(float.Parse(oneAction[1]), float.Parse(oneAction[2]));
                    ActionPlay();
                    break;
                case "애니메이션":  //<애니메이션, 애니메이션 이름>
                    backgroundManager.BackgroundAnimOn(oneAction[1]);
                    ActionPlay();
                    break;
                case "사진등장":    //<사진등장, 사진 이름, 등장 페이드 시간>
                    imageEffectManager.EffectImageOn(oneAction[1], float.Parse(oneAction[2]));
                    ActionPlay();
                    break;
                case "사진이동":    //<사진이동, x좌표, y좌표, 이동시간>
                    imageEffectManager.EffectImageMove(float.Parse(oneAction[1]), float.Parse(oneAction[2]), float.Parse(oneAction[3]));
                    ActionPlay();
                    break;
                case "사진퇴장":    //<사진퇴장, 퇴장 페이드 시간>
                    imageEffectManager.EffectImageOff(float.Parse(oneAction[1]));
                    ActionPlay();
                    break;
                case "다음챕터":    //<다음챕터, 파일 이름>
                    NewScript(oneAction[1]);
                    PrintController();
                    break;
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
                case "바운스":  //캐릭터 위아래 떨림, <바운스, 강도>
                    characterManager.characterBounce(oneAction[1], float.Parse(oneAction[2]));
                    ActionPlay();
                    break;
                case "확대":    //<캐릭터, 크기, 시간>
                    characterManager.CharacterSize(oneAction[1], float.Parse(oneAction[2]), float.Parse(oneAction[3]));
                    ActionPlay();
                    break;
                case "UI숨김":  //<UI숨김>
                    canvasOnOff.canvasOnOff(scriptCanvas);
                    ActionPlay();
                    break;
                case "타이머":  //<타이머, 시간>
                    StartCoroutine(Timer(float.Parse(oneAction[1])));
                    break;
                case "대사위치":    //<대사위치, 가운데/아래>
                    effectManager.centerUISwitch();
                    if (oneAction[1] == "가운데")
                    {
                        mainText = mainTextList[1].GetComponent<TextMeshProUGUI>();
                    }
                    else if (oneAction[1] == "아래")
                    {
                        mainText = mainTextList[0].GetComponent<TextMeshProUGUI>();
                    }
                    ActionPlay();
                    break;
                case "화면페이드":  //<화면페이드, r, g, b, a, 시간>
                    effectManager.FadeAll(float.Parse(oneAction[1]), float.Parse(oneAction[2]),
                    float.Parse(oneAction[3]), float.Parse(oneAction[4]), float.Parse(oneAction[5]));
                    ActionPlay();
                    break;
                case "배경블러":    //<배경블러, 강도, 시간>
                    effectManager.BackgroundBlur(float.Parse(oneAction[1]), float.Parse(oneAction[2]));
                    ActionPlay();
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
    private IEnumerator Timer(float time)
    {
        onTimer = true;
        yield return new WaitForSeconds(time);
        onTimer = false;
        ActionPlay();
    }
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

        if(onSkip == true){
            effectManager.skipCursor.SetActive(true);
            effectManager.SkipCursorCallback(true);
        }
        else{
            effectManager.SkipCursorCallback(false);
            effectManager.skipCursor.SetActive(false);
        }

        StartCoroutine(SkipPlaying());
    }
    public IEnumerator SkipPlaying()
    {
        WaitForSeconds skiper = new WaitForSeconds(0.05f);
        while (onSkip)
        {
            PrintController();
            yield return skiper;
        }
    }
    public void SaveClicked(string index)
    {
        dataSet = new DataSet(characterManager.getCharacterList(), particleManager.isParticle, 
        particleManager.curParticleNameList, backgroundManager.isAnim, mainText.text, nameText.text, 
        scriptIndex - 1, scriptTitle, backgroundManager.backgroundName, bgmManager.BGMname, 
        scriptbgmVolume, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), storyManager.chapterIndex);

        saveManager.GameSave(dataSet, index);
    }
}
