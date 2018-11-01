using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateKillScene : MonoBehaviour{

    protected GameObject AllParentGo;  //游戏元素的父节点，除了plane
    readonly float w = Screen.width;
    readonly float h = Screen.height;
    protected string sceneName;

    // Use this for initialization
    void Start () {
        AllParentGo = new GameObject
        {
            name = "FatherOfAll"
        };

        sceneName = SceneManager.GetActiveScene().name;
        int index = SceneManager.GetActiveScene().buildIndex; //0开始
        print("加载场景：" + sceneName + "，编号为：" + index);

        GameObject.Find("Main Camera").AddComponent<CameraFollow>();

        CreateThePlayer();
        CreateYaogan();
        CreateTheStone();
        CreateBtn();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Reload(){
        if (AllParentGo)
        {
            int c = AllParentGo.transform.childCount;
            for (int i = 0; i < c; i++)
            {
                Destroy(AllParentGo.transform.GetChild(i).gameObject);
            }
        }

        CreateThePlayer();
        CreateTheStone();

        GameObject mycamera = Map.GetMainCamera();
        mycamera.transform.position = new Vector3(0, 15, -10);
        mycamera.transform.rotation = Quaternion.Euler(45, 0, 0);
        mycamera.GetComponent<CameraFollow>().Start();
    }

    //创建一个自己的模型=   Player
    void CreateThePlayer()
    {
        GameObject go = (GameObject)Resources.Load("Prefab/Baolong");
        go = Instantiate(go);
        go.name = "Baolong";
        go.transform.parent = AllParentGo.transform;

        go.transform.position = new Vector3(0, 6f, 0);
        go.transform.localEulerAngles = new Vector3(0, 0, 0);

        go.AddComponent<Rigidbody>();
        go.GetComponent<Rigidbody>().useGravity = true;
        go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().center = new Vector3(0, 0.55f, 0);
        go.GetComponent<BoxCollider>().size = new Vector3(0.5f, 1.2f, 3f);

        go.AddComponent<BaolongAction>();
        go.GetComponent<BaolongAction>().playerRb = go.GetComponent<Rigidbody>();

        GameObject.Find("Main Camera").GetComponent<CameraFollow>().target = go.transform;
        GameObject.Find("Main Camera").SendMessage("GetOffset");
    }

    //创建一个内置的模型cube=   stone
    void CreateTheStone()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Stone";
        go.transform.parent = AllParentGo.transform;

        go.transform.position = new Vector3(0f, 3f, 0f);
        go.transform.localScale = new Vector3(3, 2.9f, 3);

        go.GetComponent<BoxCollider>().isTrigger = false;

        go.AddComponent<Rigidbody>();
        go.GetComponent<Rigidbody>().useGravity = true;
        go.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePositionY;
        go.GetComponent<Rigidbody>().freezeRotation = true;

        Material mat = new Material(Shader.Find("Shader Forge/StoneShader"));
        go.GetComponent<MeshRenderer>().material = mat;

        //上表面
        GameObject goUpFace = GameObject.CreatePrimitive(PrimitiveType.Cube);
        goUpFace.name = "StoneUpFace";
        goUpFace.transform.parent = go.transform;

        goUpFace.transform.position = new Vector3(0f, 4f, 0f);
        goUpFace.transform.localScale = new Vector3(1, 0.1f, 1);

        goUpFace.GetComponent<BoxCollider>().isTrigger = false;

        goUpFace.AddComponent<Rigidbody>();
        goUpFace.GetComponent<Rigidbody>().useGravity = true;
        goUpFace.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePositionY;
        goUpFace.GetComponent<Rigidbody>().freezeRotation = true;

        goUpFace.GetComponent<MeshRenderer>().material = mat;

        go.AddComponent<Stone>();
        goUpFace.AddComponent<Stone>();
    }

    void CreateYaogan(){
        GameObject Yaogan = (GameObject)Resources.Load("Prefab/YaoganBack");
        Yaogan = Instantiate(Yaogan, GameObject.Find("Canvas").transform);
    }

    void CreateBtn()
    {
        //跳
        GameObject btnJump = (GameObject)Resources.Load("Prefab/Button");
        btnJump = Instantiate(btnJump, Map.GetMainCanvas().transform, false);
        btnJump.GetComponent<RectTransform>().localPosition = new Vector3(600, -300, 0);
        btnJump.GetComponent<UIButton>().SetTitle("Jump");
        btnJump.GetComponent<UIButton>().clickAction = () =>
        {
            GameObject.Find("Baolong").SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
        };

        //加速
        GameObject btnAcc = (GameObject)Resources.Load("Prefab/Button");
        btnAcc = Instantiate(btnJump, Map.GetMainCanvas().transform, false);
        btnAcc.GetComponent<RectTransform>().localPosition = new Vector3(600, -200, 0);
        btnAcc.GetComponent<UIButton>().SetTitle("ACC");
        btnAcc.GetComponent<UIButton>().clickAction = () =>
        {
            GameObject.Find("Baolong").SendMessage("Accelerate", SendMessageOptions.DontRequireReceiver);
        };
        btnAcc.GetComponent<UIButton>().upAction = () =>
        {
            GameObject.Find("Baolong").SendMessage("AccelerateEnd", SendMessageOptions.DontRequireReceiver);
        };

        //攻击
        GameObject btnAttack = (GameObject)Resources.Load("Prefab/Button");
        btnAttack = Instantiate(btnJump, Map.GetMainCanvas().transform, false);
        btnAttack.GetComponent<RectTransform>().localPosition = new Vector3(600, -100, 0);
        btnAttack.GetComponent<UIButton>().SetTitle("Atk");
        btnAttack.GetComponent<UIButton>().clickAction = () =>
        {
            GameObject.Find("Baolong").SendMessage("AttackMove", SendMessageOptions.DontRequireReceiver);
        };
    }

    private void OnGUI()
    {
        //重新开始按钮
        if (GUI.Button(new Rect(w * 0.04f, h * 0.04f, w * 0.12f, h * 0.1f), "重新开始"))
        {
            Reload();
        }
        if (GUI.Button(new Rect(w * 0.04f, h * 0.16f, w * 0.12f, h * 0.1f), "跳过"))
        {
            LoadNextScene();
        }
    }

    //加载下一个场景
    void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex; //0开始
        SceneManager.LoadScene(index - 1);
    }
}
