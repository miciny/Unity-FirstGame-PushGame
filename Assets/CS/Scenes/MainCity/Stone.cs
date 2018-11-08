using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour {

    //碰撞
    private void OnCollisionEnter(Collision collision)
    {   //落地了，jumpFlag变成false
        if (collision.gameObject.name == "Plane" || collision.gameObject.name == "Stone")
        {
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            ////Stone 和 StoneUpFace，如果挨得太近，会抖动？？？？
            if (this.gameObject.name == "StoneUpFace"){
                this.gameObject.transform.position += new Vector3(0, 0.15f, 0);
            }
        }
    }
}
