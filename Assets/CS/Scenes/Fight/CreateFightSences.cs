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
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateBtn(){
        GameObject btnShowPackage = (GameObject)Resources.Load("Prefab/CanvasPerfab/Button");
        btnShowPackage = Instantiate(btnShowPackage, CommonFuncs.GetMainCanvas().transform, false);
        btnShowPackage.GetComponent<RectTransform>().localPosition = new Vector3(500, -350, 0);
        btnShowPackage.GetComponent<UIButton>().SetTitle("背包");
        btnShowPackage.GetComponent<UIButton>().clickAction = ShowPackage;

        //攻击
        GameObject btnAttack = (GameObject)Resources.Load("Prefab/CanvasPerfab/Button");
        btnAttack = Instantiate(btnAttack, CommonFuncs.GetMainCanvas().transform, false);
        btnAttack.GetComponent<RectTransform>().localPosition = new Vector3(740, -130, 0);
        btnAttack.GetComponent<UIButton>().SetTitle("攻击");
        btnAttack.GetComponent<UIButton>().clickAction = () =>
        {
            GameObject.Find("Modao").SendMessage("AttackMove", SendMessageOptions.DontRequireReceiver);
        };
    }

    //显示或者消失背包
    void ShowPackage(){
        foreach (Transform child in CommonFuncs.GetMainCanvas().transform){
            if (child.gameObject.name == "PackageMianban(Clone)"){
                child.GetComponent<PackageMianban>().Hide();
                return;
            }
        }
        GameObject ShuxingUI = (GameObject)Resources.Load("Prefab/ScenePrefab/PackageMianban");
        ShuxingUI = Instantiate(ShuxingUI, GameObject.Find("Canvas").transform);
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
    }

    //创建哥布林
    void CreateGebulin(){
        for (int i = 0; i < 5; i++){
            GameObject gebulin1 = (GameObject)Resources.Load("Prefab/Gebulin/Npc_Gebulin_01");
            gebulin1 = Instantiate(gebulin1);
            gebulin1.transform.position = new Vector3(4, 0, 4 + i*4);

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
            gebulin2.transform.position = new Vector3(7 + i * 4, 0, 9);

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

    //回到主场景
    void BackToMainCityScene(){
        CommonFuncs.LoadTheScene("MainCity");
    }
}
