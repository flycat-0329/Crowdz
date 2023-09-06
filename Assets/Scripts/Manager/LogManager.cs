using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LogManager : MonoBehaviour
{
    public GameObject logImage;
    public GameObject content;
    public GameObject scrollView;
    public void LogMaker(string name, string body){
        GameObject l = Instantiate(logImage);
        l.transform.SetParent(content.transform);
        l.transform.localScale = new Vector3(1, 1, 1);

        l.transform.GetChild(0).GetComponent<Text>().text = name;
        l.transform.GetChild(1).GetComponent<Text>().text = body;
    }

    public void BottomAnchor(){
        scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }
}
