using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public Rigidbody playerRb;  //玩家的物体
    public static int count = 0;    //步数

    readonly int moveRate = 1;    //移动距离
    readonly float speed = 10;    //移动速度
    Vector3 playerEndPos;

    Vector3 boxEndPos;
    Rigidbody boxRb;

    // Use this for initialization
    void Start(){
        playerRb = GetComponent<Rigidbody>();
        playerEndPos = this.playerRb.transform.position;
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            RayDetect(1);   //射线检测方式，检测可否推动
        }else if (Input.GetKeyDown(KeyCode.DownArrow)){
            RayDetect(3);   //射线检测方式，检测可否推动
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            RayDetect(4);   //射线检测方式，检测可否推动
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)){
            RayDetect(2);   //射线检测方式，检测可否推动
        }
        MovePlayer();
    }


    void MovePlayer(){
        this.playerRb.transform.position = Vector3.MoveTowards(this.playerRb.transform.position,
                                                  playerEndPos,
                                                  Time.deltaTime * speed
                                                 );
        if (boxRb != null){
            this.boxRb.transform.position = Vector3.MoveTowards(this.boxRb.transform.position,
                                                  boxEndPos,
                                                  Time.deltaTime * speed
                                                 );
        }
    }


    //射线检测, d为方, 1上，2右，3下，4左
    //如果返回数量为2且距离大于1，或者没有碰撞，玩家动
    //如果返回数量为2且为box时，距离为0.5，玩家和箱子动
    //如果返回数量为4，玩家不能动
    void RayDetect(int d){

        Vector3 dir;
        switch (d){
            case 1: dir = Vector3.forward; break;
            case 2: dir = Vector3.right; break;
            case 3: dir = Vector3.back; break;
            case 4: dir = Vector3.left; break;
            default: dir = Vector3.zero; break;
        }

        //返回两个相同的碰撞体，以后解决？？？？？？？？？？
        RaycastHit[] hits = Physics.RaycastAll(this.playerRb.transform.position, dir, 2.1f);

        if(hits.Length == 0)//没有碰撞，玩家动
        { 
            boxRb = null;
            boxEndPos = Vector3.zero;
            playerEndPos = playerEndPos + TheDir(d);
            count++;
            GameObject.Find("Count").SendMessage("ChangeText");
        }
        else if(hits.Length == 2)//有碰撞
        { 
            if (hits[0].collider.gameObject.name == "Box1" && hits[0].distance < 1)
            { //有碰撞，Box，距离小于1，玩家和箱子动
                boxRb = hits[0].collider.gameObject.GetComponent<Rigidbody>();
                boxEndPos = hits[0].collider.gameObject.transform.position + TheDir(d);
                playerEndPos = playerEndPos + TheDir(d);
                count++;
                GameObject.Find("Count").SendMessage("ChangeText");
            }
            else if (hits[0].collider.gameObject.name != "Box1" && hits[0].distance < 1)
            {//有碰撞，不是Box，距离小于1，玩家不动
                boxRb = null;
                boxEndPos = Vector3.zero;
            }else{//有碰撞，不管是不是Box，距离大于1，玩家动
                boxRb = null;
                boxEndPos = Vector3.zero;
                playerEndPos = playerEndPos + TheDir(d);
                count++;
                GameObject.Find("Count").SendMessage("ChangeText");
            }
        }else if (hits.Length >= 4)
        {//有碰撞，有两个物体，都不动
            boxRb = null;
            boxEndPos = Vector3.zero;
        }
    }

    //根据方向，返回向量
    Vector3 TheDir(int d)
    {
        switch (d)
        {
            case 1: return new Vector3(0, 0, moveRate);
            case 2: return new Vector3(moveRate, 0, 0);
            case 3: return new Vector3(0, 0, -moveRate);
            default: return new Vector3(-moveRate, 0, 0);
        }
    }
}
