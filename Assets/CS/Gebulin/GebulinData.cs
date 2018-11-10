using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GebulinData : MonoBehaviour {

    static readonly int atk = 1400;
    static readonly int blood = 3000;


    public static Dictionary<string, string> GetGebulinShuxing(){
        Dictionary<string, string> Shuxing = new Dictionary<string, string>{
            { "atkStr", atk.ToString() },
            { "bloodStr", blood.ToString() }
        };
        return Shuxing;
    }

    public static int GetGebulinATK(){
        return atk;
    }
}
