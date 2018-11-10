using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Yaogan : ScrollRect{
    float _mRadius; // 半径
    const float Dis = 0.5f;// 距离

    public string playerName;

    protected override void Start(){
        base.Start();
        _mRadius = content.sizeDelta.x * Dis;// 能移动的半径 = 摇杆的宽 * Dis
    }

    public override void OnDrag(PointerEventData eventData){
        base.OnDrag(eventData);
             
         // 获取摇杆，根据锚点的位置。
        Vector2 contentPosition = content.anchoredPosition;
 
        // 判断摇杆的位置 是否大于 半径
        if (contentPosition.magnitude > _mRadius)
        {   
             // 设置摇杆最远的位置
            contentPosition = contentPosition.normalized* _mRadius;
            SetContentAnchoredPosition(contentPosition);
        }
 
         // 最后 v2.x/y 就跟 Input中的 Horizontal Vertical 获取的值一样 
        Vector2 v2 = content.anchoredPosition.normalized;

        GameObject.Find(playerName).SendMessage("OnDrag", v2, SendMessageOptions.DontRequireReceiver);
    }

    //开始拖拽
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        GameObject.Find(playerName).SendMessage("BeginDrag", SendMessageOptions.DontRequireReceiver);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        GameObject.Find(playerName).SendMessage("EndDrag", SendMessageOptions.DontRequireReceiver);
    }

}
