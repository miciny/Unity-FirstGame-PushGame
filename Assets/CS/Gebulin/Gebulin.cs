using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gebulin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //发波
        if (Input.GetKeyDown(KeyCode.Space)){
            Attacked(200);
        }

    }

    void Attacked(int damage){
        foreach (Transform can in transform){
            if (can.gameObject.name == "Canvas_Show(Clone)"){
                can.gameObject.GetComponent<ControlHP>().SendMessage("GetHitted", damage);
                break;
            }
        }
    }

    //死亡
    void Dead(){
        Destroy(gameObject);
    }


}
