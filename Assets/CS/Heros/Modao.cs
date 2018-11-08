using System.Collections.Generic;
using UnityEngine;

public class Modao : MonoBehaviour {

    ParticleSystem[] particleSystems;
    ParticleSystem lightParticle;

    Rigidbody Player;

    bool moveFlag = false;
    Vector3 targetDir; //运动的转向
    Quaternion targetRotation;
    float oriAngleY;

    readonly float moveRate = 10f;
    readonly float rotationSpeed = 40f;
    string level;
    Dictionary<string, string> Shuxing;

    // Use this for initialization
    void Start () {
        Player = GetComponent<Rigidbody>();
        particleSystems = GetComponentsInChildren<ParticleSystem>(); 
        foreach (ParticleSystem ps in particleSystems){
            if (ps.name == "LightParticle"){
                lightParticle = ps;
                lightParticle.Stop();
            }
        }

        targetDir = new Vector3(0, 0, 0);
        targetRotation = Quaternion.Euler(targetDir);


        Shuxing = ModaoData.GetHeroShuxing();
        //加血条
        GameObject bloodShow = (GameObject)Resources.Load("Prefab/Blood/Canvas_Show");
        bloodShow = Instantiate(bloodShow, transform);
        bloodShow.GetComponent<ControlHP>().maxHP = int.Parse(Shuxing["bloodStr"]);
        level = Shuxing["levelStr"];
    }
	
	// Update is called once per frame
	void Update () {

        IsClickedModao(); //是否点击了模型

        //发波
        if (Input.GetKeyDown(KeyCode.Space)){
            AttackMove();
        }

        //保持运动
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            PlayerMove(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)){
            PlayerMove(3);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            PlayerMove(4);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)){
            PlayerMove(2);
        }

        //停止运动
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.UpArrow)
                     || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow)){
            EndMove();
        }

        //旋转
        Player.rotation = Quaternion.RotateTowards(Player.rotation, targetRotation, Time.time * rotationSpeed);
    }

    void FixedUpdate()
    {
        //移动，直接给速度
        if (moveFlag){
            Vector3 moveVelocity = Player.transform.forward * moveRate;
            moveVelocity.y = Player.velocity.y;  //不控制y的速度
            Player.velocity = moveVelocity;
        }
    }

    //按键的动作
    void PlayerMove(int d){
        BeginMove();
        oriAngleY = CommonFuncs.GetMainCamera().transform.eulerAngles.y;
        switch (d){
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


    void AttackMove(){
        lightParticle.Play();
        Attacked(1000);
    }

    void Attacked(int damage){
        foreach (Transform can in transform){
            if (can.gameObject.name == "Canvas_Show(Clone)"){
                can.gameObject.GetComponent<ControlHP>().SendMessage("GetHitted", damage);
                break;
            }
        }

        GameObject pla = GameObject.Find("PlayerInfo");
        if(pla){
            pla.GetComponent<PlayerInfo>().Reload();
        }
    }

    //开始，结束时的处理
    void BeginMove(){
        moveFlag = true;
        HideShuxingMianban();
    }

    void EndMove(){
        moveFlag = false;
        Player.velocity = Vector3.zero;
    }

    //遥感的代理
    //传入 Horizontal Vertical，改变方向
    void OnDrag(Vector2 hv){
        float y = Quaternion.LookRotation(new Vector3(hv.x, 0, hv.y)).eulerAngles.y;
        oriAngleY = CommonFuncs.GetMainCamera().transform.eulerAngles.y;
        targetDir = new Vector3(0, oriAngleY + y, 0); //朝向
        targetRotation = Quaternion.Euler(targetDir);
    }

    //开始拖拽
    void BeginDrag(){
        BeginMove();
    }

    void EndDrag(){
        EndMove();
    }


    //判断是否点击在模型上，触发事件
    void IsClickedModao(){
        if (Input.GetMouseButtonDown(0)){//判断是否是点击事件
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo)){
                if (hitInfo.collider.gameObject.name == "Modao"){
                    ShowShuxingMianban(Input.mousePosition);
                }
            }
        }

        //如果是一根手指触摸屏幕而且是刚开始触摸屏幕 
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began){
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo)){
                if (Input.GetTouch(0).tapCount == 1 && hitInfo.collider.gameObject.name == "Modao"){//判断点击的次数
                    ShowShuxingMianban(Input.GetTouch(0).position);
                }
            }
        }
    }


    //显示属性面板
    void ShowShuxingMianban(Vector3 touchPos){
        foreach (Transform child in CommonFuncs.GetMainCanvas().transform){
            if (child.gameObject.name == "ShuxingMianban(Clone)"){
                return;
            }
        }

        GameObject ShuxingUI = (GameObject)Resources.Load("Prefab/ScenePrefab/ShuxingMianban");
        ShuxingUI = Instantiate(ShuxingUI, GameObject.Find("Canvas").transform);
        ShuxingUI.GetComponent<RectTransform>().localPosition = touchPos + new Vector3(-400, -touchPos.y, 0);

        ShuxingMianban ShuxingCS = ShuxingUI.GetComponent<ShuxingMianban>();
        ShuxingCS.atk.text = Shuxing["atkStr"];
        ShuxingCS.blood.text = Shuxing["bloodStr"];
        ShuxingCS.level.text = level;
    }

    void HideShuxingMianban(){
        foreach (Transform child in CommonFuncs.GetMainCanvas().transform){
            if (child.gameObject.name == "ShuxingMianban(Clone)"){
                child.GetComponent<ShuxingMianban>().Hide();
                return;
            }
        }
    }


    //碰撞
    void OnCollisionEnter(Collision collision){
        print("魔导碰到了：" + collision.gameObject.name);
        if (collision.gameObject.name == "Terrain"){
            AddPosYConstraint();
            print("落地");
        }
    }

    void AddPosYConstraint(){
        Player.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;
    }

    //死亡
    void Dead(){
        Destroy(gameObject);
    }
}
