using UnityEngine;

public class Gebulin : MonoBehaviour {

    void Attacked(int damage){
        foreach (Transform can in transform){
            if (can.gameObject.name == "Canvas_Show(Clone)"){
                can.gameObject.GetComponent<ControlHP>().SendMessage("GetHitted", damage);
                break;
            }
        }
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "Terrain"){
            AddPosYConstraint();
        }
    }

    //被粒子系统碰到
    void OnParticleCollision(GameObject other){
        print(other.tag + ": " + gameObject.name);
        if (other.tag == "Modao_Light"){
            int atk = ModaoData.GetModaoATK();
            Attacked(atk);
        }
    }

    //死亡
    void Dead(){
        Destroy(gameObject);
    }

    void AddPosYConstraint(){
        GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;
    }

}
