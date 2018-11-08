using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {

    public Text playerName;
    public Text level;
    public Image icon;
    public Slider blood;
    public Text bloodStr;

    int maxHP;

    Dictionary<string, string> Shuxing;

    // Use this for initialization
    void Start () {
        Shuxing = ModaoData.GetHeroShuxing();
        playerName.text = Shuxing["nameStr"];
        level.text = "Level: " + Shuxing["levelStr"];
        bloodStr.text = "HP: " + Shuxing["bloodStr"];
        blood.maxValue = int.Parse(Shuxing["bloodStr"]);
        maxHP = int.Parse(Shuxing["bloodStr"]);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reload(){
        GameObject player = GameObject.Find("Modao");
        if(player){
            foreach (Transform can in player.transform){
                if (can.gameObject.name == "Canvas_Show(Clone)"){
                    blood.value = can.gameObject.GetComponent<ControlHP>().GetCurrentHP();
                    bloodStr.text = "HP: " + (maxHP - blood.value).ToString();
                    break;
                }
            }
        }
    }
}
