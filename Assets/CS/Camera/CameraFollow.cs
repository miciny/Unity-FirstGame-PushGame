using UnityEngine;
using UnityEngine.EventSystems;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    readonly float thetaSpeed = 40.0f;
    readonly float phiSpeed = 30.0f;
    readonly float phiBoundMin = -70; //最大最小角度
    readonly float phiBoundMax = 89f;
    readonly float rotateSmoothing = 0.06f;//旋转速度
    readonly float moveSmoothing = 0.7f;
    readonly float zoomSpeed = 5.0f;
    Vector3 distanceVec = new Vector3(0, 0, 0);
    readonly float yDel = 1.5f; //射线机对着玩家的中心点，Y轴偏移量

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
                    bool touch1 = CommonFuncs.IsPointerOverUIObject(Input.GetTouch(0).position);
                    bool touch2 = CommonFuncs.IsPointerOverUIObject(Input.GetTouch(1).position);

                    //都在UI上，返回
                    if (touch1 && touch2)
                    {
                        return;
                    }
                    //第一个手指在UI，第二个手指不在，需要移动镜头
                    else if (!touch1 && touch2){
                        //触摸类型为移动触摸
                        if (Input.GetTouch(0).phase == TouchPhase.Moved){
                            SetEuler();
                        }
                    }
                    //第一个手指在UI，第二个手指不在，需要移动镜头
                    else if (touch1 && !touch2)
                    { //触摸类型为移动触摸
                        if (Input.GetTouch(1).phase == TouchPhase.Moved){
                            SetEuler();
                        }
                    }
                }
            }

            //单点触摸
            if (Input.touchCount == 1){
                if (canvas != null){
                    if (CommonFuncs.IsPointerOverUIObject(Input.GetTouch(0).position)){
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
        }
    }

    //旋转用
    public void FixedUpdate(){
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSmoothing);
        Vector3 nextPosition = GetNewPos();

       //滑动时，检测
        if (transform.rotation != targetRot){
            bool grounded = IsHitFloor(nextPosition);

            if (!grounded){
                if (targetDist < 22){
                    targetDist += 0.03f * zoomSpeed;
                }
            }

            int i = 0;  //基本上循环100次还找不到位置，就放弃
            while (grounded){
                targetDist -= 0.01f * zoomSpeed;
                nextPosition = GetNewPos();
                grounded = IsHitFloor(nextPosition);
                i++;
                if (i > 100) { grounded = false; break; }
            }
        }

        transform.position = nextPosition;
    }

    //是否碰到了地面
    bool IsHitFloor(Vector3 nextPosition){
        RaycastHit hit;
        Vector3 V = (nextPosition - transform.position).normalized;
        bool grounded = Physics.Raycast(transform.position, V, out hit, 4f);
        return grounded && (hit.collider.gameObject.name == "Terrain" || 
                            hit.collider.gameObject.transform.parent.gameObject.name == "pllz" || 
                            hit.collider.gameObject.transform.parent.gameObject.name == "Fountain") ? true : false;
    }


    //旋转屏幕时的计算
    void SetEuler(){
        float k = 1;
        if (PC) k = 6;
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");
        dx = dx * thetaSpeed * 0.02f*k;
        dy = dy * phiSpeed * 0.02f*k;
        euler.x += dx;
        euler.y -= dy;
        euler.y = CommonFuncs.ClampAngle(euler.y, phiBoundMin, phiBoundMax);
        targetRot = Quaternion.Euler(euler.y, euler.x, 0);
    }

    Vector3 GetNewPos(){
        dis = moveSmoothing * targetDist + (1 - moveSmoothing) * dis;
        distanceVec.z = dis;
        return GetTargetPosition() - transform.rotation * distanceVec;
    }


    //传入target用
    void GetOffset(){
        targetRot = transform.rotation;
        targetDist = (transform.position - GetTargetPosition()).magnitude;
    }

    //计算Y偏移量
    Vector3 GetTargetPosition(){
        return target.position + new Vector3(0, yDel, 0);
    }

    //镜头角度跟随
    void SetTargetRot(){
        euler.x = target.transform.localEulerAngles.y;
        targetRot = Quaternion.Euler(new Vector3(euler.y, euler.x, 0));
    }
}
