using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageData : MonoBehaviour {

    public static Dictionary<int, ItemModel> GetHeroPackage(){
        ItemModel sword1 = new ItemModel{
            name = "Sword1",
            describe = "第一把剑"
        };

        ItemModel sword2 = new ItemModel{
            name = "Sword2",
            describe = "第二把剑"
        };

        Dictionary<int, ItemModel> a = new Dictionary<int, ItemModel>{
            { 1, sword1 },
            { 2, sword2 }
        };
        return a;
    }
}
