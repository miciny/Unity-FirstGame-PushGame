using System.Collections;
using UnityEngine;
using System;

public class Map : MonoBehaviour {

    //1=stone, 2=box, 3=aim, 4=box And aim, 5= player
    //2==3

    public static readonly int[,] the_map_1 = {
        {0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
        {0, 0, 1, 0, 0, 0, 1, 0, 0, 0},
        {0, 1, 1, 2, 1, 0, 1, 1, 1, 0},
        {0, 1, 0, 0, 0, 5, 0, 0, 1, 0},
        {0, 1, 0, 1, 0, 0, 1, 0, 1, 0},
        {0, 1, 0, 1, 3, 3, 2, 0, 1, 0},
        {0, 1, 0, 0, 0, 1, 1, 1, 1, 0},
        {0, 1, 1, 1, 1, 1, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };
    public static readonly int the_map_1_score = 2;

    public static readonly int[,] the_map_2 = {
        {0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
        {0, 0, 1, 0, 0, 0, 1, 0, 0, 0},
        {0, 1, 1, 2, 1, 0, 1, 1, 1, 0},
        {0, 1, 0, 5, 0, 0, 0, 0, 1, 0},
        {0, 1, 0, 1, 0, 3, 1, 0, 1, 0},
        {0, 1, 0, 1, 0, 3, 2, 0, 1, 0},
        {0, 1, 0, 0, 0, 1, 1, 1, 1, 0},
        {0, 1, 1, 1, 1, 1, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };
    public static readonly int the_map_2_score = 2;


    //协同，延时
    public static IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
