using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlHP : MonoBehaviour {
    public int maxHP = 200;
    public int currentHP;
    public Slider HPSlider; 

	// Use this for initialization
	void Start () {
        currentHP = maxHP;
        HPSlider.maxValue = maxHP;
        HPSlider.value = 0;
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Camera.main.transform.rotation;
	}

    void GetHitted(int damage){
        currentHP -= damage;
        HPSlider.value = maxHP - currentHP;

        if(currentHP <= 0){
            transform.parent.SendMessage("Dead");
        }
    }


    public int GetCurrentHP(){
        return maxHP - currentHP;
    }
}
