using UnityEngine;
using UnityEngine.EventSystems;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    readonly float thetaSpeed = 250.0f;
    readonly float phiSpeed = 120.0f;
    readonly float phiBoundMin = 10f; //最大最小角度
    readonly float phiBoundMax = 89f;
    readonly float rotateSmoothing = 0.5f;//旋转速度
    readonly float moveSmoothing = 0.7f;
    Vector3 distanceVec = new Vector3(0, 0, 0);

    readonly float zoomSpeed = 10.0f;

    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
    private Vector2 oldPosition1 = new Vector2();
    private Vector2 oldPosition2 = new Vector2();

    float dis;
    bool PC = true;
    Vector2 euler;
    Quaternion targetRot;
    float targetDist;

    // Use this for initialization
    public void Start(){
        Vector3 angles = transform.eulerAngles;
        euler.x = angles.y;
        euler.y = angles.x;
        euler.y = Mathf.Repeat(euler.y + 180f, 360f) - 180f;

        PC &= (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer);

        oldPosition1 = new Vector2();
        oldPosition2 = new Vector2();
        dis = 0;
        targetRot = Quaternion.Euler(0,0,0);
        targetDist = 0;
        gameObject.GetComponent<Camera>().nearClipPlane = 0.8f;

        if (target) GetOffset();
    }

    // Update is called once per frame
    void Update(){
        //移动
        if (!PC){
            //点到UI了，不旋转 //传递画布组件，传递触摸手势坐标
            GameObject canvas = GameObject.Find("Canvas");
            //多点触摸
            if (Input.touchCount == 2){

                if (canvas != null){
                    //都在UI上，返回
                    if (Map.IsPointerOverUIObject(Input.GetTouch(0).position) && Map.IsPointerOverUIObject(Input.GetTouch(1).position)){
                        return;
                    }

                    //如果都不在
                    else if (!(Map.IsPointerOverUIObject(Input.GetTouch(0).position) || Map.IsPointerOverUIObject(Input.GetTouch(1).position))){
                        //前两只手指触摸类型都为移动触摸
                        if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved){
                            //计算出当前两点触摸点的位置
                            Vector3 tempPosition1 = Input.GetTouch(0).position;
                            Vector3 tempPosition2 = Input.GetTouch(1).position;
                            SetScale(tempPosition1, tempPosition2);
                        }
                    }
                    //第一个手指在UI，第二个手指不在，需要移动镜头
                    else if (!Map.IsPointerOverUIObject(Input.GetTouch(0).position) && Map.IsPointerOverUIObject(Input.GetTouch(1).position)){
                        //触摸类型为移动触摸
                        if (Input.GetTouch(0).phase == TouchPhase.Moved){
                            SetEuler();
                        }
                    }
                    //第一个手指在UI，第二个手指不在，需要移动镜头
                    else if (Map.IsPointerOverUIObject(Input.GetTouch(0).position) && !Map.IsPointerOverUIObject(Input.GetTouch(1).position)){ //触摸类型为移动触摸
                        if (Input.GetTouch(1).phase == TouchPhase.Moved){
                            SetEuler();
                        }
                    }
                }
                else{
                    //前两只手指触摸类型都为移动触摸
                    if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved){
                        //计算出当前两点触摸点的位置
                        Vector3 tempPosition1 = Input.GetTouch(0).position;
                        Vector3 tempPosition2 = Input.GetTouch(1).position;
                        SetScale(tempPosition1, tempPosition2);
                    }
                }
            }

            //单点触摸
            if (Input.touchCount == 1){
                if (canvas != null){
                    if (Map.IsPointerOverUIObject(Input.GetTouch(0).position)){
                        return;
                    }
                }

                //触摸类型为移动触摸
                if (Input.GetTouch(0).phase == TouchPhase.Moved){
                    SetEuler();
                }
            }
        }

        //PC
        else{
            //点到UI了，不旋转
            if (EventSystem.current.IsPointerOverGameObject() == true){
                return;
            }

            //旋转
            if (target){
                //左键，旋转屏幕
                if (Input.GetMouseButton(0)){
                    SetEuler();
                }
            }

            targetDist -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * 0.5f;
        }
        targetDist = Mathf.Clamp(targetDist, 4f, 50f);
    }

    //旋转用
    public void FixedUpdate(){
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSmoothing);

        dis = moveSmoothing * targetDist + (1 - moveSmoothing) * dis;
        distanceVec.z = dis;
        transform.position = target.position - transform.rotation * distanceVec;
    }

    //旋转屏幕时的计算
    void SetEuler(){
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");
        dx = dx * thetaSpeed * 0.02f;
        dy = dy * phiSpeed * 0.02f;
        euler.x += dx;
        euler.y -= dy;
        euler.y = Map.ClampAngle(euler.y, phiBoundMin, phiBoundMax);
        targetRot = Quaternion.Euler(euler.y, euler.x, 0);
    }

    //计算缩放时
    void SetScale(Vector2 tempPosition1, Vector2 tempPosition2){
        //函数返回float<0.放大
        float zoom = Map.IsEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2);

        if (zoom < 0) { targetDist -= 0.2f * zoomSpeed; } else { targetDist += 0.2f * zoomSpeed; }
        //备份上一次触摸点的位置，用于对比
        oldPosition1 = tempPosition1;
        oldPosition2 = tempPosition2;
    }

    //传入target用
    void GetOffset(){
        targetRot = transform.rotation;
        targetDist = (transform.position - target.position).magnitude;
    }

    //镜头角度跟随
    void SetTargetRot(){
        euler.x = target.transform.localEulerAngles.y;
        targetRot = Quaternion.Euler(new Vector3(euler.y, euler.x, 0));
    }
}
