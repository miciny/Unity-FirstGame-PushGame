using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Text Title;

    bool isPointDown = false;
    float lastInvokeTime;
    public float interval = 0.1f;

    public Action clickAction; //按下事件
    public Action donwAction;  //持续按下事件
    public Action upAction;  //抬起事件


    public void SetTitle(string title){
        Title.text = title;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointDown = true;
        if (clickAction != null){
            clickAction();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointDown = false;
        if (upAction != null)
        {
            upAction();
        }
    }


    void Update(){
        if (isPointDown){
            if (Time.time - lastInvokeTime > interval)
            {
                //触发点击;
                lastInvokeTime = Time.time;
                if (donwAction != null)
                {
                    donwAction();
                }
            }
        }
    }
}
