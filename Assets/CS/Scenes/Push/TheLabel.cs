using UnityEngine;

public class TheLabel : MonoBehaviour {

    public GameObject rb;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangeText()
    {
        if (this.rb.gameObject.name == "Count")
        {
            int count = PlayerMove.count;
            this.rb.gameObject.GetComponent<TextMesh>().text = "count: " + count;
        }else if (this.rb.gameObject.name == "Level")
        {
            int level = CreatePushScene.level;
            this.rb.gameObject.GetComponent<TextMesh>().text = "level: " + level;
        }

    }
}
