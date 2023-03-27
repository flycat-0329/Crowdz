using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHide : MonoBehaviour
{
    public GameObject scriptCanvas;
    private void Update() {
        #if UNITY_EDITOR
        if(scriptCanvas.activeSelf == false && Input.GetMouseButtonDown(0)){
            scriptCanvas.SetActive(true);
        }
        #elif UNITY_ANDROID
        if(scriptCanvas.activeSelf == false && Input.touchCount == 1){
            scriptCanvas.SetActive(true);
        }
        #endif
    }
}
