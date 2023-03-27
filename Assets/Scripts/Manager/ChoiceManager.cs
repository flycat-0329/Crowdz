using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    public LinePrint linePrint;
    public List<GameObject> choiceButtons;
    public GameObject choiceCanvas;

    public void ChoiceAppear(string choiceID, string optionA, string optionB){
        choiceCanvas.SetActive(true);

        choiceButtons[0].transform.GetChild(0).GetComponent<Text>().text = optionA;
        choiceButtons[0].transform.name = choiceID + "A";
        choiceButtons[1].transform.GetChild(0).GetComponent<Text>().text = optionB;
        choiceButtons[1].transform.name = choiceID + "B";
        
        choiceButtons[0].SetActive(true);
        choiceButtons[1].SetActive(true);
        choiceButtons[2].SetActive(false);
    }

    public void ChoiceAppear(string choiceID, string optionA, string optionB, string optionC){
        choiceCanvas.SetActive(true);

        choiceButtons[0].transform.GetChild(0).GetComponent<Text>().text = optionA;
        choiceButtons[0].transform.name = choiceID + "A";
        choiceButtons[1].transform.GetChild(0).GetComponent<Text>().text = optionB;
        choiceButtons[1].transform.name = choiceID + "B";
        choiceButtons[2].transform.GetChild(0).GetComponent<Text>().text = optionC;
        choiceButtons[2].transform.name = choiceID + "C";
        
        choiceButtons[0].SetActive(true);
        choiceButtons[1].SetActive(true);
        choiceButtons[2].SetActive(true);
    }

    public void OptionSelected(int index){      //선택지 선택했을 때의 함수
        choiceCanvas.SetActive(false);
        linePrint.NewScript(choiceButtons[index].transform.name);
        linePrint.PrintController();
    }
}
