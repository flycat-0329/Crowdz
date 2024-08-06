using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHide : MonoBehaviour
{
    public GameObject scriptCanvas;
    public GameObject effectCanvas;
    private void Update() {
        if(scriptCanvas.activeSelf == false && Input.GetMouseButtonDown(0)){
            scriptCanvas.SetActive(true);
            effectCanvas.SetActive(true);
        }
    }

}
