using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOnOff : MonoBehaviour
{
    public void canvasOnOff(GameObject canvas)      //UI숨기면 LinePrint에서 다시 켬
    {
        canvas.SetActive(!canvas.activeSelf);
    }

    public void LoadCanvasOnOff()
    {
        GameObject canvas = SettingManager.instance.LoadCanvas;
        canvas.SetActive(!canvas.activeSelf);
    }

    public void ClickOff(GameObject obj)
    {
        if (Input.GetMouseButtonDown(0) && (obj.activeSelf == true))
        {
            obj.SetActive(false);
        }
    }
}
