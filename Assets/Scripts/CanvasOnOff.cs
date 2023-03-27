using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOnOff : MonoBehaviour
{
    public void canvasOnOff(GameObject canvas){
        canvas.SetActive(!canvas.activeSelf);
    }
}
