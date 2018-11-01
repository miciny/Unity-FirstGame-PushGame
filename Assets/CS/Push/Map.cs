using System.Collections;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class Map : MonoBehaviour {

    //1=stone, 2=box, 3=aim, 4=box And aim, 5= player
    //2==3


    //第一关
    static readonly int[,] the_map_1 = {
        {0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
        {0, 0, 1, 0, 0, 0, 1, 0, 0, 0},
        {0, 1, 1, 0, 1, 0, 1, 1, 1, 0},
        {0, 1, 0, 3, 2, 5, 0, 0, 1, 0},
        {0, 1, 0, 1, 0, 0, 1, 0, 1, 0},
        {0, 1, 0, 1, 0, 3, 2, 0, 1, 0},
        {0, 1, 0, 0, 0, 1, 1, 1, 1, 0},
        {0, 1, 1, 1, 1, 1, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };
    static readonly int the_map_1_score = 2;


    //第二关
    static readonly int[,] the_map_2 = {
        {0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
        {0, 0, 1, 0, 0, 0, 1, 0, 0, 0},
        {0, 1, 1, 0, 1, 0, 1, 1, 1, 0},
        {0, 1, 0, 5, 2, 0, 0, 0, 1, 0},
        {0, 1, 0, 1, 0, 0, 1, 0, 1, 0},
        {0, 1, 0, 1, 3, 3, 2, 0, 1, 0},
        {0, 1, 0, 0, 0, 1, 1, 1, 1, 0},
        {0, 1, 1, 1, 1, 1, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };
    static readonly int the_map_2_score = 2;


    //第三关
    static readonly int[,] the_map_3 = {
        {0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
        {0, 0, 1, 0, 0, 0, 1, 0, 0, 0},
        {0, 1, 1, 2, 1, 0, 1, 1, 1, 0},
        {0, 1, 0, 0, 3, 2, 5, 0, 1, 0},
        {0, 1, 0, 1, 0, 0, 1, 0, 1, 0},
        {0, 1, 0, 1, 3, 3, 2, 0, 1, 0},
        {0, 1, 0, 0, 0, 1, 1, 1, 1, 0},
        {0, 1, 1, 1, 1, 1, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };
    static readonly int the_map_3_score = 3;





    //需要添加进来
    public static readonly ArrayList MapArray = new ArrayList
    {
        the_map_1, 
        the_map_2,
        the_map_3
    };
    public static readonly int[] LevelAimNoArray = { 
        the_map_1_score, 
        the_map_2_score,
        the_map_3_score
    };





    //协同，延时
    public static IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

    //移动 旋转 协程
    public static IEnumerator MovePlayer(Rigidbody p, Vector3 target, float moveSpeed, Action doneAction)
    {
        while (p.position != target)
        {
            p.position = Vector3.MoveTowards(p.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        doneAction();

    }
    //移动 旋转 协程
    public static IEnumerator RotationPlayer(Rigidbody p, Quaternion target, float rotationSpeed, Action doneAction)
    {
        while (p.rotation != target)
        {
            p.rotation = Quaternion.RotateTowards(p.rotation, target, Time.time * rotationSpeed);
            yield return null;
        }
        doneAction();
    }

    //传入map中的位置，转为实际位置，i对应z,j对应x
    public static float[] PositionCulculate(int i, int j)
    {
        float x = j - 4.5f;
        float z = 4.5f - i;
        float[] p = {x, z};
        return p;
    }

    //检测是否点击到ui
    public static bool IsPointerOverUIObject(Vector2 screenPosition)
    {
        //实例化点击事件
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
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

    //移动端，计算缩放用
    public static float IsEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2){
        //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        return leng1 - leng2;   //小于0，放大手势
    }


    public static float ClampAngle(float angle, float min, float max){
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    public static GameObject GetMainCamera(){
        return GameObject.Find("Main Camera");
    }
    public static GameObject GetMainCanvas(){
        return GameObject.Find("Canvas");
    }
}
