using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFightSences : MonoBehaviour{

    readonly float w = Screen.width;
    readonly float h = Screen.height;

    // Use this for initialization
    void Start () {
        CreateBtn();
        CreateYaogan();
        CreateGebulin();
        CreatePlayerInfo();
    }


    //=========================Set Canvas==========================
    void CreateBtn(){
        //背包
        GameObject btnShowPackage = (GameObject)Resources.Load("Prefab/CanvasPerfab/Button");
        btnShowPackage = Instantiate(btnShowPackage, CommonFuncs.GetMainCanvas().transform, false);
        btnShowPackage.GetComponent<RectTransform>().localPosition = new Vector3(5 * w / 12, -2 * h / 5, 0);
        btnShowPackage.GetComponent<RectTransform>().localScale = new Vector2(w / 1200, h / 1000);
        btnShowPackage.GetComponent<UIButton>().SetTitle("背包");
        btnShowPackage.GetComponent<UIButton>().clickAction = ShowPackage;

        //攻击
        GameObject btnAttack = (GameObject)Resources.Load("Prefab/CanvasPerfab/Button");
        btnAttack = Instantiate(btnAttack, CommonFuncs.GetMainCanvas().transform, false);
        btnAttack.GetComponent<RectTransform>().localPosition = new Vector3(5 * w / 12, -3 * h / 10, 0);
        btnAttack.GetComponent<RectTransform>().localScale = new Vector2(w / 1200, h / 1000);
        btnAttack.GetComponent<UIButton>().SetTitle("攻击");
        btnAttack.GetComponent<UIButton>().clickAction = () =>
        {
            GameObject.Find("Modao").SendMessage("AttackMove", SendMessageOptions.DontRequireReceiver);
        };
    }

    void CreatePlayerInfo(){
        GameObject playerInfoUI = (GameObject)Resources.Load("Prefab/CanvasPerfab/PlayerInfo");
        playerInfoUI = Instantiate(playerInfoUI, GameObject.Find("Canvas").transform);
        playerInfoUI.GetComponent<RectTransform>().localScale = new Vector2(w / 1400, h / 840);
        playerInfoUI.GetComponent<RectTransform>().position = new Vector3(w/8, 11*h/12, 0);
    }

    //显示或者消失背包
    void ShowPackage(){
        foreach (Transform child in CommonFuncs.GetMainCanvas().transform){
            if (child.gameObject.name == "PackageMianban(Clone)"){
                child.GetComponent<PackageMianban>().Hide();
                return;
            }
        }
        GameObject packageUI = (GameObject)Resources.Load("Prefab/CanvasPerfab/PackageMianban");
        packageUI = Instantiate(packageUI, GameObject.Find("Canvas").transform);
        packageUI.GetComponent<RectTransform>().localScale = new Vector2(w / 2000, h / 1100);
    }


    void OnGUI(){
        if (GUI.Button(new Rect(w * 0.89f, h * 0.05f, w * 0.1f, h * 0.07f), "返回主场景")){
            BackToMainCityScene();
        }
    }

    void CreateYaogan(){
        GameObject Yaogan = (GameObject)Resources.Load("Prefab/CanvasPerfab/YaoganBack");
        Yaogan = Instantiate(Yaogan, GameObject.Find("Canvas").transform);
        Yaogan.GetComponent<Yaogan>().playerName = "Modao";
        Yaogan.GetComponent<RectTransform>().localScale = new Vector2(w / 2400, w / 2400);
        Yaogan.GetComponent<RectTransform>().localPosition = new Vector3(-w / 3, w/12-2 * h / 5, 0);
    }



    //=========================Set Sence==========================
    //创建哥布林
    void CreateGebulin(){
        for (int i = 0; i < 5; i++){
            GameObject gebulin1 = (GameObject)Resources.Load("Prefab/Gebulin/Npc_Gebulin_01");
            gebulin1 = Instantiate(gebulin1);
            gebulin1.transform.position = new Vector3(-7, 0, -9 + i*4);
            gebulin1.tag = "Gubulin_01";
            gebulin1.AddComponent<Gebulin>();

            Dictionary<string, string> GShuxing = GebulinData.GetGebulinShuxing();

            //加血条
            GameObject bloodShow = (GameObject)Resources.Load("Prefab/Blood/Canvas_Show");
            bloodShow = Instantiate(bloodShow, gebulin1.transform);
            bloodShow.transform.localScale = new Vector3(1, 1, 1);
            bloodShow.transform.position = gebulin1.transform.position;
            bloodShow.GetComponent<ControlHP>().maxHP = int.Parse(GShuxing["bloodStr"]);
        }
        for (int i = 0; i < 5; i++){
            GameObject gebulin2 = (GameObject)Resources.Load("Prefab/Gebulin/Npc_Gebulin_02");
            gebulin2 = Instantiate(gebulin2);
            gebulin2.transform.position = new Vector3(-7 + i * 4, 0, -9);
            gebulin2.tag = "Gubulin_02";

            gebulin2.AddComponent<Gebulin>();

            Dictionary<string, string> GShuxing = GebulinData.GetGebulinShuxing();
            //加血条
            GameObject bloodShow = (GameObject)Resources.Load("Prefab/Blood/Canvas_Show");
            bloodShow = Instantiate(bloodShow, gebulin2.transform);
            bloodShow.transform.localScale = new Vector3(1, 1, 1);
            bloodShow.transform.position = gebulin2.transform.position;
            bloodShow.GetComponent<ControlHP>().maxHP = int.Parse(GShuxing["bloodStr"]);
        }
    }




    //=========================Common Funcs==========================
    //回到主场景
    void BackToMainCityScene(){
        CommonFuncs.LoadTheScene("MainCity");
    }
}
