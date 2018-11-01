using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public Rigidbody playerRb;  //玩家的物体

    readonly int moveRate = 1;    //移动距离
    readonly float moveSpeed = 10;    //移动速度
    readonly float rotationSpeed = 20f;    //移动速度

    Vector3 playerEndPos;
    Quaternion playerEndDir;

    Vector3 boxEndPos;
    Rigidbody boxRb;
    Animation baolong_animation;

    bool canMoveFlag = true; //是否接收移动操作，动作未做完，不允许动
    public static int count = 0;    //步数

    // Use this for initialization
    void Start(){
        playerRb = GetComponent<Rigidbody>();
        playerEndPos = this.playerRb.transform.position;
        canMoveFlag = true;
        count = 0;
        boxRb = null;
        boxEndPos = Vector3.zero;

        baolong_animation = GetComponent<Animation>(); //动画控制器
        baolong_animation.PlayQueued("Wait", QueueMode.PlayNow);//上来直接播放
        print("重新加载Player的脚本");

    }

    // Update is called once per frame
    void Update()
    {
        if (canMoveFlag){
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RayDetect(1);   //射线检测方式，检测可否推动
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                RayDetect(3);   //射线检测方式，检测可否推动
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RayDetect(4);   //射线检测方式，检测可否推动
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RayDetect(2);   //射线检测方式，检测可否推动
            }
        }
    }

    //射线检测, d为方, 1上，2右，3下，4左
    //如果返回数量为2且距离大于1，或者没有碰撞，玩家动
    //如果返回数量为2且为box时，距离为0.5，玩家和箱子动
    //如果返回数量为4，玩家不能动
    void RayDetect(int d){
        playerEndDir = TheDirection(d); //按了方向，直接变方向

        Vector3 dir;
        switch (d){
            case 1: dir = Vector3.forward; break;
            case 2: dir = Vector3.right; break;
            case 3: dir = Vector3.back; break;
            case 4: dir = Vector3.left; break;
            default: dir = Vector3.zero; break;
        }

        //返回两个相同的碰撞体，以后解决？？？？？？？？？？
        //高度问题，不然会检测到aim
        Vector3 theBegain = this.playerRb.transform.position + new Vector3(0, 1, 0);
        RaycastHit[] hits = Physics.RaycastAll(theBegain, dir, 2.1f);

        if(hits.Length == 0)//没有碰撞，玩家动
        {
            print("没检测到物体,走");
            boxRb = null;
            boxEndPos = Vector3.zero;
            playerEndPos = playerEndPos + ThePosition(d);

            JustWalk();
        }
        else if(hits.Length == 2)//有碰撞
        { 
            if (hits[0].collider.gameObject.name == "Box1" && hits[0].distance < 1)
            { //有碰撞，Box，距离小于1，玩家和箱子动
                print("检测到箱,可推,攻击-走");
                boxRb = hits[0].collider.gameObject.GetComponent<Rigidbody>();
                boxEndPos = hits[0].collider.gameObject.transform.position + ThePosition(d);
                playerEndPos = playerEndPos + ThePosition(d);

                PushWalk();
            }
            else if (hits[0].collider.gameObject.name != "Box1" && hits[0].distance < 1)
            {//有碰撞，不是Box，距离小于1，玩家不动
                print("检测到不是箱子,吼");
                boxRb = null;
                boxEndPos = Vector3.zero;

                NotMove();
            }
            else
            {//有碰撞，不管是不是Box，距离大于1，玩家动
                print("没检测到物体,走");
                boxRb = null;
                boxEndPos = Vector3.zero;
                playerEndPos = playerEndPos + ThePosition(d);

                JustWalk();
            }
        }else if (hits.Length >= 4)
        {//有碰撞，有两个物体，都不动
            print("检测到两个物体,吼");
            boxRb = null;
            boxEndPos = Vector3.zero;

            NotMove();
        }
    }

    //根据方向，返回位置变化
    Vector3 ThePosition(int d)
    {
        switch (d)
        {
            case 1: return new Vector3(0, 0, moveRate);
            case 2: return new Vector3(moveRate, 0, 0);
            case 3: return new Vector3(0, 0, -moveRate);
            default: return new Vector3(-moveRate, 0, 0);
        }
    }

    //根据方向，返回方向变化
    Quaternion TheDirection(int d){
        switch (d){
                case 1: return Quaternion.Euler(0, 0, 0);
                case 2: return Quaternion.Euler(0, 90, 0);
                case 3: return Quaternion.Euler(0, 180, 0);
                default: return Quaternion.Euler(0, -90, 0);
        }
    }

    //不动时得动作
    void NotMove()
    {
        StartCoroutine(Map.RotationPlayer(playerRb, playerEndDir, rotationSpeed, () => {
            canMoveFlag = true;
            baolong_animation.PlayQueued("Bellow", QueueMode.PlayNow);
            baolong_animation.PlayQueued("Wait", QueueMode.CompleteOthers);
        })); //旋转
    }
    //直接走的动作
    void JustWalk()
    {
        canMoveFlag = false;
        StartCoroutine(Map.RotationPlayer(playerRb, playerEndDir, rotationSpeed, () => {
            StartCoroutine(Map.MovePlayer(playerRb, playerEndPos, moveSpeed, () => {
                canMoveFlag = true;
                baolong_animation.PlayQueued("Wait", QueueMode.PlayNow);
                count++;
                GameObject.Find("Count").SendMessage("ChangeText");
            })); //走
            baolong_animation.PlayQueued("Walk", QueueMode.PlayNow);
        })); //旋转
    }
    //箱子和人一起走
    void PushWalk()
    {
        canMoveFlag = false;
        StartCoroutine(Map.RotationPlayer(playerRb, playerEndDir, rotationSpeed, () => {
            baolong_animation.PlayQueued("Attack", QueueMode.PlayNow);
            //Attack 0.2之后,box走
            StartCoroutine(Map.DelayToInvokeDo(() =>
            {
                StartCoroutine(Map.MovePlayer(boxRb, boxEndPos, moveSpeed, null)); //box走
            }, 0.2f));
            //Attack 0.9之后,走
            StartCoroutine(Map.DelayToInvokeDo(() =>
            {
                boxRb = null;
                boxEndPos = Vector3.zero;
                baolong_animation.PlayQueued("Walk", QueueMode.PlayNow);
                StartCoroutine(Map.MovePlayer(playerRb, playerEndPos, moveSpeed, () => {
                    canMoveFlag = true;
                    baolong_animation.PlayQueued("Wait", QueueMode.PlayNow);
                    count++;
                    GameObject.Find("Count").SendMessage("ChangeText");
                    GameObject.Find("Plane").SendMessage("Vectory");
                })); //走
            }, 0.9f));
        })); //旋转
    }
}
