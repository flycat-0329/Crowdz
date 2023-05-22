using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using DG.Tweening;

public class StoryManager : MonoBehaviour
{
    public GameObject mailText;
    public GameObject mailPanel;
    public KeyValuePair<List<List<string>>, List<List<List<string>>>> storyFileRead(string fileName)
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/Storys/Script/" + fileName);   //대본 파일(후에 리스트로 관리해야 함)
        List<List<string>> scriptList = new List<List<string>>();   //전체 대사 리스트
        List<List<List<string>>> actionList = new List<List<List<string>>>();   //전체 연출 리스트
        List<string> scriptLineList = new List<string>();           //대사 한줄을 임시로 저장하는 리스트
        List<string> actionLineList = new List<string>();           //연출 한줄을 임시로 저장하는 리스트
        List<List<string>> actionSetList = new List<List<string>>(); //대사 한줄에 따른 연출들을 저장하는 리스트(진짜 구조 야랄났다)
        int index = 0;      //대사를 기준으로 몇번째 대사인가?

        while (true)
        {
            string line = sr.ReadLine();    //대본 한 줄 읽어옴

            if (line == null)       //대본 끝났다
            {
                sr.Close();
                return new KeyValuePair<List<List<string>>, List<List<List<string>>>>(scriptList, actionList);
            }

            if (line[0] == '<')
            {      //대본의 내용이 연출이라면
                line = line.Replace("<", "");
                line = line.Replace(">", "");
                line = line.Replace(" ", "");

                actionLineList = new List<string>(line.Split(','));     //연출에 필요한 파라미터를 쉼표를 기준으로 나눈 것
                actionSetList.Add(actionLineList);
            }

            else
            {   //대사라면
                List<string> lineArray = new List<string>();    //최종적으로 정리되는 대사 한 줄
                lineArray.Add(index.ToString());    //맨 앞에 인덱스 추가
                scriptLineList = new List<string>(line.Split(':'));     //사람 이름과 대사를 나눈 것

                if (scriptLineList.Count == 1)
                {   //나레이션이라면
                    scriptLineList.Clear();
                    scriptLineList.Add("");
                    scriptLineList.Add(line);
                }

                lineArray.AddRange(scriptLineList);     //본문과 인덱스를 합침
                scriptList.Add(lineArray);     //대사를 대본 파일에 추가

                actionList.Add(actionSetList);      //대사 한줄에 해당하는 연출 등록
                actionSetList = new List<List<string>>();
                index += 1;
            }
        }
    }

    public string MailRead(string mailName){
        StreamReader sr = new StreamReader(Application.dataPath + "/Storys/Mail/" + mailName + ".txt");

        string mail = sr.ReadToEnd();
        Debug.Log(mail);
        return mail;
    }

    public void Mailmake(string mailName){
        mailPanel.SetActive(true);
        string mail = MailRead(mailName);
        mailText.GetComponent<Text>().text = mail;
    }
}
