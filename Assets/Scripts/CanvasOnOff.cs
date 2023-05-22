using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOnOff : MonoBehaviour
{
    public void canvasOnOff(GameObject canvas)
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
