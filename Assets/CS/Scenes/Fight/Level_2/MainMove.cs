using UnityEngine;

public class MainMove : MonoBehaviour
{

    public Rigidbody playerRb;  //玩家的物体
    Animation baolong_animation;

    bool jumpFlag = false;
    bool dragFlag = false;
    bool keyFlag = false;
    bool moveFlag = false;

    Vector3 targetDir; //运动的转向
    Quaternion targetRotation;

    float moveRate = 10f;
    readonly float rotationSpeed = 40f;
    public float forceRate = 12f;
    float oriAngleY;

    // Use this for initialization
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        baolong_animation = GetComponent<Animation>(); //动画控制器
        baolong_animation.PlayQueued("idle", QueueMode.PlayNow);//上来直接播放

        targetDir = new Vector3(0, 0, 0);
        targetRotation = Quaternion.Euler(targetDir);
    }

    // Update is called once per frame
    void Update()
    {
        if (!jumpFlag)
        {
            //运动时，能跳
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Accelerate();
            }

            //保持运动
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                BaolongMove(1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                BaolongMove(3);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                BaolongMove(4);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                BaolongMove(2);
            }
        }

        //停止运动
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.UpArrow)
                     || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow)
                || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow)))
            {
                EndMove();
                keyFlag = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            AccelerateEnd();
        }

        //旋转
        playerRb.rotation = Quaternion.RotateTowards(playerRb.rotation, targetRotation, Time.time * rotationSpeed);
    }

    void FixedUpdate()
    {
        //移动，直接给速度
        if (moveFlag)
        {
            Vector3 moveVelocity = playerRb.transform.forward * moveRate;
            moveVelocity.y = playerRb.velocity.y;  //不控制y的速度
            playerRb.velocity = moveVelocity;
        }
    }


    //=================动作================
    void Jump()
    {
        if (jumpFlag)
        {
            return;
        }

        //跳的时候，解除Y限制
        RemovePosYConstraint();
        playerRb.AddForce(Vector3.up * forceRate, ForceMode.Impulse);

        jumpFlag = true;
        baolong_animation.PlayQueued("jump", QueueMode.PlayNow);
        //直接接着播放，动作不协调
        StartCoroutine(CommonFuncs.DelayToInvokeDo(() => {
            if (dragFlag || keyFlag)
            {
                baolong_animation.PlayQueued("walk", QueueMode.PlayNow);
            }
            else
            {
                baolong_animation.PlayQueued("idle", QueueMode.PlayNow);
            }
        }, 1.23f));
    }

    //加速
    void Accelerate()
    {
        moveRate = moveRate * 3;
        baolong_animation.PlayQueued("walk", QueueMode.PlayNow);
    }
    void AccelerateEnd()
    {
        moveRate = moveRate / 3;
        baolong_animation.PlayQueued("walk", QueueMode.PlayNow);
    }

    //攻击
    void AttackMove()
    {
        if (!jumpFlag)
        {
            baolong_animation.PlayQueued("attack", QueueMode.PlayNow);

            if (dragFlag || keyFlag)
            {
                baolong_animation.PlayQueued("walk", QueueMode.CompleteOthers);
            }
            else
            {
                baolong_animation.PlayQueued("idle", QueueMode.CompleteOthers);
            }
        }
    }

    //=================移动================
    void BaolongMove(int d)
    {
        BeginMove();
        oriAngleY = CommonFuncs.GetMainCamera().transform.eulerAngles.y;
        keyFlag = true;
        switch (d)
        {
            case 1:
                targetDir = new Vector3(0, oriAngleY + 0, 0);
                break;
            case 2:
                targetDir = new Vector3(0, oriAngleY + 90, 0);
                break;
            case 3:
                targetDir = new Vector3(0, oriAngleY + 180, 0);
                break;
            case 4:
                targetDir = new Vector3(0, oriAngleY - 90, 0);
                break;
            default: break;
        }
        targetRotation = Quaternion.Euler(targetDir);
    }

    //开始，结束时的处理
    void BeginMove()
    {
        moveFlag = true;
        baolong_animation.PlayQueued("walk", QueueMode.PlayNow);
    }

    void EndMove()
    {
        moveFlag = false;
        if (!jumpFlag)
        { //在地下，变成0，在控制，速度不变，
            playerRb.velocity = Vector3.zero;
            baolong_animation.PlayQueued("idle", QueueMode.PlayNow);
        }
    }


    //遥感的代理
    //传入 Horizontal Vertical，改变方向
    void OnDrag(Vector2 hv)
    {
        if (!jumpFlag)
        {
            float y = Quaternion.LookRotation(new Vector3(hv.x, 0, hv.y)).eulerAngles.y;
            oriAngleY = CommonFuncs.GetMainCamera().transform.eulerAngles.y;
            targetDir = new Vector3(0, oriAngleY + y, 0); //朝向
            targetRotation = Quaternion.Euler(targetDir);
        }
    }

    //开始拖拽
    void BeginDrag()
    {
        dragFlag = true;
        BeginMove();
    }

    void EndDrag()
    {
        dragFlag = false;
        EndMove();
    }


    //=================碰撞================
    private void OnCollisionEnter(Collision collision)
    {
        print("Player碰到了：" + collision.gameObject.name);
        //落地了，jumpFlag变成false
        if (collision.gameObject.name == "Terrain" ||
            collision.gameObject.name == "StoneUpFace" ||
            collision.gameObject.name == "pllz" ||
            collision.gameObject.name == "Fountain")
        {
            jumpFlag = false;
            //落地时，Y限制，防止抖动
            AddPosYConstraint();
            if (!moveFlag)
            {
                playerRb.velocity = Vector3.zero;
            }
            print("落地");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Terrain" ||
            collision.gameObject.name == "StoneUpFace" ||
            collision.gameObject.name == "pllz" ||
            collision.gameObject.name == "Fountain")
        {
            RemovePosYConstraint();
            print("离地");
        }
    }

    //
    void AddPosYConstraint()
    {
        playerRb.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;
    }
    void RemovePosYConstraint()
    {
        playerRb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
