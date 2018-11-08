using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CommonFuncs : MonoBehaviour
{
    //加载场景
    public static void LoadTheScene(string sceneName){
        Globe.nextSceneName = sceneName;
        SceneManager.LoadScene("Loading");
    }


    //获得主Canvas
    public static GameObject GetMainCanvas(){
        return GameObject.Find("Canvas");
    }

    public static GameObject GetMainCamera(){
        return GameObject.Find("Main Camera");
    }

    //协同，延时
    public static IEnumerator DelayToInvokeDo(Action action, float delaySeconds){
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

   //角度
    public static float ClampAngle(float angle, float min, float max){
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    //移动端，计算缩放用
    public static float IsEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2){
        //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        return leng1 - leng2;   //小于0，放大手势
    }

    //检测是否点击到ui
    public static bool IsPointerOverUIObject(Vector2 screenPosition){
        //实例化点击事件
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current){
            //将点击位置的屏幕坐标赋值给点击事件
            position = screenPosition
        };
        //获取画布上的 GraphicRaycaster 组件
        GraphicRaycaster uiRaycaster = GameObject.Find("Canvas").GetComponent<Canvas>().gameObject.GetComponent<GraphicRaycaster>();

        List<RaycastResult> results = new List<RaycastResult>();
        // GraphicRaycaster 发射射线
        uiRaycaster.Raycast(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
