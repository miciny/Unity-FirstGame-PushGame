using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GebulinData : MonoBehaviour {

    static readonly int atk = 200;
    static readonly int blood = 1500;


    public static Dictionary<string, string> GetGebulinShuxing(){
        Dictionary<string, string> Shuxing = new Dictionary<string, string>{
            { "atkStr", atk.ToString() },
            { "bloodStr", blood.ToString() }
        };
        return Shuxing;
    }
}
