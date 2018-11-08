using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModaoData {

    static readonly int atk = 100;
    static readonly int blood = 1000;
    static readonly int level = 13;
    static readonly string name = "miciny";
    static readonly GameObject mainWeapon = (GameObject)Resources.Load("Prefab/WeaponPerfab/Sword1"); //主武器


    public static Dictionary<string, string> GetHeroShuxing(){
        int atkTemp = atk * level;
        if (mainWeapon){
            atkTemp += mainWeapon.GetComponent<Sword1Data>().atk;
        }

        int bloodTemp = blood * level;

        Dictionary<string, string> Shuxing = new Dictionary<string, string>{
            { "atkStr", atkTemp.ToString() },
            { "bloodStr", bloodTemp.ToString() },
            { "levelStr", level.ToString() },
            { "nameStr", name }
        };
        return Shuxing;
    }
}
