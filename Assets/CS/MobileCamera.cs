using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileCamera : MonoBehaviour
{

    public float phiBoundMin = -89f;
    public float phiBoundMax = 89f;

    //用于绑定参照物对象
    public Transform target;
    //缩放系数
    float distance = 10.0f;
    //左右滑动移动速度
    readonly float xSpeed = 250.0f;
    readonly float ySpeed = 120.0f;
    //缩放限制系数
    readonly float yMinLimit = -20;
    readonly float yMaxLimit = 80;
    //摄像头的位置
    private Vector2 euler;
    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
    private Vector2 oldPosition1 = new Vector2();
    private Vector2 oldPosition2 = new Vector2();

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        euler.x = angles.y;
        euler.y = angles.x;
        euler.y = Mathf.Repeat(euler.y + 180f, 360f) - 180f;
    }

    // Update is called once per frame
    void Update()
    {
        //判断触摸数量为单点触摸
        if (Input.touchCount == 1)
        {
            //触摸类型为移动触摸
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                float dx = Input.GetAxis("Mouse X");
                float dy = Input.GetAxis("Mouse Y");
                //根据触摸点计算X与Y位置
                euler.x += dx * xSpeed * 0.02f;
                euler.y -= dy * ySpeed * 0.02f;
                euler.y = ClampAngle(euler.y, phiBoundMin, phiBoundMax);
            }
        }

        //判断触摸数量为多点触摸
        if (Input.touchCount > 1)
        {
            //前两只手指触摸类型都为移动触摸
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                //计算出当前两点触摸点的位置
                Vector3 tempPosition1 = Input.GetTouch(0).position;
                Vector3 tempPosition2 = Input.GetTouch(1).position;
                //函数返回真为放大，返回假为缩小
                if (IsEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {
                    //放大系数超过3以后不允许继续放大
                    //这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                    if (distance > 3)
                    {
                        distance -= 0.5f;
                    }
                }
                else
                {
                    //缩小洗漱返回18.5后不允许继续缩小
                    //这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                    if (distance < 18.5)
                    {
                        distance += 0.5f;
                    }
                }
                //备份上一次触摸点的位置，用于对比
                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;
            }
        }
    }


    bool IsEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        {
            //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
            float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
            float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
            if (leng1 < leng2)
            {
                //放大手势
                return true;
            }
            else
            {
                //缩小手势
                return false;
            }
        }
    }
    

    void LateUpdate(){
        //target为我们绑定的箱子变量，缩放旋转的参照物
        if (target)
        {
            //重置摄像机的位置
            euler.y = ClampAngle(euler.y, yMinLimit, yMaxLimit);
            Quaternion rotation = Quaternion.Euler(euler.y, euler.x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}