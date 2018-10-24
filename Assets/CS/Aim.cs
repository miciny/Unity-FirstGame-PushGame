using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour {

    public Rigidbody rb;
    public static int score = 0;

    Vector3 oldPos;
    readonly float moveDel = -0.05f;

    int flag = 0; //防止trigger消息多次触发

    // Use this for initialization
    void Start (){
        oldPos = this.rb.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Box1"){
            this.rb.transform.position = oldPos +
                new Vector3(0, moveDel, 0);
            if (flag == 0)
            {
                flag = 1;
                score++;
                Material mat = new Material(Shader.Find("Custom/BoxOnAim"));
                other.GetComponent<MeshRenderer>().material = mat;

                //延时0.1秒判断是否胜利
                StartCoroutine(Map.DelayToInvokeDo(() =>
                {
                    GameObject.Find("Plane").SendMessage("LoadTheScene");
                }, 0.1f));
            }
        }
    }


    private void OnTriggerExit(Collider other){
        if (other.gameObject.name == "Box1"){
            this.rb.transform.position = oldPos;

            if (flag == 1)
            {
                flag = 0;
                score--;
                Material mat = new Material(Shader.Find("Custom/Box"));
                other.GetComponent<MeshRenderer>().material = mat;
            }
        }
    }

}
