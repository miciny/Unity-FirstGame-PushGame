using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraFixed : MonoBehaviour
{
    public Transform target;

    bool PC = true;

    readonly float thetaSpeed = 250.0f;
    readonly float phiSpeed = 120.0f;
    readonly float moveSmoothing = 0.7f;
    readonly Vector3 pivotPoint = new Vector3(0, 2, 0);
    readonly float rotateSmoothing = 0.5f;//旋转速度
    readonly float moveBounds = 100f;
    readonly bool useMoveBounds = true;
    readonly float phiBoundMin; //最大最小角度
    readonly float phiBoundMax = 89f;
    readonly float moveSpeed = 10.0f;
    readonly float zoomSpeed = 10.0f;

    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
    Vector2 oldPosition1 = new Vector2();
    Vector2 oldPosition2 = new Vector2();

    float targetDist;
    Vector3 targetLookAt;
    public float distance = 2.0f;
    Quaternion targetRot;
    Vector2 euler;
    Vector3 distanceVec = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start () {
        Vector3 angles = transform.eulerAngles;
        euler.x = angles.y;
        euler.y = angles.x;
        //unity only returns positive euler angles but we need them in -90 to 90
        euler.y = Mathf.Repeat(euler.y + 180f, 360f) - 180f;

        GameObject go = new GameObject("_FreeCameraTarget")
        {
            hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector
        };

        target = go.transform;
        target.position = pivotPoint;
        targetDist = (transform.position - target.position).magnitude;

        targetRot = transform.rotation;
        targetLookAt = target.position;

        PC &= (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer);

    }

    // Update is called once per frame
    void Update () {

        //移动
        if (!PC)
        {
            //点到UI了，不旋转 //传递画布组件，传递触摸手势坐标
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)//面板存在就判断
            {
                if (CommonFuncs.IsPointerOverUIObject(Input.GetTouch(0).position))
                {
                    print("当前触摸在UI上"); return;
                }
            }

            //多点触摸
            if (Input.touchCount > 1)
            {
                //前两只手指触摸类型都为移动触摸
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    //计算出当前两点触摸点的位置
                    Vector3 tempPosition1 = Input.GetTouch(0).position;
                    Vector3 tempPosition2 = Input.GetTouch(1).position;
                    //函数返回float<0.放大
                    float zoom = CommonFuncs.IsEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2);
                    if (zoom < 0) { targetDist -= 0.2f * zoomSpeed; } else { targetDist += 0.2f * zoomSpeed; }
                  
                    //备份上一次触摸点的位置，用于对比
                    oldPosition1 = tempPosition1;
                    oldPosition2 = tempPosition2;
                }
            }

            if (Input.touchCount == 1)
            {
                //触摸类型为移动触摸
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    SetEuler();
                }
            }
        }

        else{
            //点到UI了，不旋转
            if (EventSystem.current.IsPointerOverGameObject() == true)
            {
                return;
            }

            if (target)
            {
                //左键，旋转屏幕
                if (Input.GetMouseButton(0))
                {
                    SetEuler();
                }

                if (Input.GetMouseButton(2))
                {
                    float dx = Input.GetAxis("Mouse X");
                    float dy = Input.GetAxis("Mouse Y");
                    dx = dx * moveSpeed * 0.005f * targetDist;
                    dy = dy * moveSpeed * 0.005f * targetDist;
                    targetLookAt -= transform.up * dy + transform.right * dx;
                    if (useMoveBounds)
                    {
                        targetLookAt.x = Mathf.Clamp(targetLookAt.x, -moveBounds, moveBounds);
                        targetLookAt.y = Mathf.Clamp(targetLookAt.y, -moveBounds, moveBounds);
                        targetLookAt.z = Mathf.Clamp(targetLookAt.z, -moveBounds, moveBounds);
                    }
                }
            }

            targetDist -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * 0.5f;
        }
        targetDist = Mathf.Clamp(targetDist, 4f, 50f);
    }

    //旋转屏幕时的计算
    void SetEuler()
    {
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");
        dx = dx * thetaSpeed * 0.02f;
        dy = dy * phiSpeed * 0.02f;
        euler.x += dx;
        euler.y -= dy;
        euler.y = CommonFuncs.ClampAngle(euler.y, phiBoundMin, phiBoundMax);
        targetRot = Quaternion.Euler(euler.y, euler.x, 0);
    }

    //旋转用
    public void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSmoothing);

        distance = moveSmoothing * targetDist + (1 - moveSmoothing) * distance;
        target.position = Vector3.Lerp(target.position, targetLookAt, moveSmoothing);
        distanceVec.z = distance;
        transform.position = target.position - transform.rotation * distanceVec;
    }
}
