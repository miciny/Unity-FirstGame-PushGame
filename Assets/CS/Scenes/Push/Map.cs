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
    public static float[] PositionCulculate(int i, int j){
        float x = j - 4.5f;
        float z = 4.5f - i;
        float[] p = {x, z};
        return p;
    }
}
