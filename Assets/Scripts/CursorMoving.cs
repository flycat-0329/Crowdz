using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CursorMoving : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sequence cursorMove = DOTween.Sequence()
        .Append(this.transform.DOLocalMoveY(-50, 0.5f).SetLoops(int.MaxValue, LoopType.Yoyo))
        .SetId("CursorMove");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
