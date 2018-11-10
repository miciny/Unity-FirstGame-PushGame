using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateMainCityScene : MonoBehaviour{

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
        CreatePlayerInfo();
    }

    //=========================Common Funcs==========================
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

        GameObject mycamera = CommonFuncs.GetMainCamera();
        mycamera.transform.position = new Vector3(0, 15, 24);
        mycamera.transform.rotation = Quaternion.Euler(10, 180, 0);
        mycamera.GetComponent<CameraFollow>().Start();
    }


    //=========================Set Sence==========================
    //创建一个自己的模型=   Player
    void CreateThePlayer()
    {
        GameObject go = (GameObject)Resources.Load("Prefab/Heros/Baolong");
        go = Instantiate(go);
        go.name = "Baolong";
        go.transform.parent = AllParentGo.transform;

        go.transform.position = new Vector3(0, 6f, 0);
        go.transform.localEulerAngles = new Vector3(0, 0, 0);

        go.AddComponent<Rigidbody>();
        go.GetComponent<Rigidbody>().useGravity = true;
        go.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
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
    void CreateTheStone(){
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


    //=========================Set Canvas==========================
    void CreateYaogan(){
        GameObject Yaogan = (GameObject)Resources.Load("Prefab/CanvasPerfab/YaoganBack");
        Yaogan = Instantiate(Yaogan, GameObject.Find("Canvas").transform);
        Yaogan.GetComponent<Yaogan>().playerName = "Baolong";
        Yaogan.GetComponent<RectTransform>().localScale = new Vector2(w / 2400, w / 2400);
        Yaogan.GetComponent<RectTransform>().localPosition = new Vector3(-w / 3, w / 12 - 2 * h / 5, 0);
    }

    void CreatePlayerInfo(){
        GameObject playerInfoUI = (GameObject)Resources.Load("Prefab/CanvasPerfab/PlayerInfo");
        playerInfoUI = Instantiate(playerInfoUI, GameObject.Find("Canvas").transform);
        playerInfoUI.GetComponent<RectTransform>().localScale = new Vector2(w / 1400, h / 840);
        playerInfoUI.GetComponent<RectTransform>().position = new Vector3(w / 8, 11 * h / 12, 0);
    }

    void CreateBtn(){
        //跳
        GameObject btnJump = (GameObject)Resources.Load("Prefab/CanvasPerfab/Button");
        btnJump = Instantiate(btnJump, CommonFuncs.GetMainCanvas().transform, false);
        btnJump.GetComponent<RectTransform>().localPosition = new Vector3(500, -370, 0);
        btnJump.GetComponent<RectTransform>().localPosition = new Vector3(5 * w / 12, -2 * h / 5, 0);
        btnJump.GetComponent<RectTransform>().localScale = new Vector2(w / 1200, h / 1000);
        btnJump.GetComponent<UIButton>().SetTitle("跳");
        btnJump.GetComponent<UIButton>().clickAction = () =>
        {
            GameObject.Find("Baolong").SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
        };

        //加速
        GameObject btnAcc = (GameObject)Resources.Load("Prefab/CanvasPerfab/Button");
        btnAcc = Instantiate(btnAcc, CommonFuncs.GetMainCanvas().transform, false);
        btnAcc.GetComponent<RectTransform>().localPosition = new Vector3(620, -250, 0);
        btnAcc.GetComponent<RectTransform>().localPosition = new Vector3(5 * w / 12, -3 * h / 10, 0);
        btnAcc.GetComponent<RectTransform>().localScale = new Vector2(w / 1200, h / 1000);
        btnAcc.GetComponent<UIButton>().SetTitle("加速");
        btnAcc.GetComponent<UIButton>().clickAction = () =>
        {
            GameObject.Find("Baolong").SendMessage("Accelerate", SendMessageOptions.DontRequireReceiver);
        };
        btnAcc.GetComponent<UIButton>().upAction = () =>
        {
            GameObject.Find("Baolong").SendMessage("AccelerateEnd", SendMessageOptions.DontRequireReceiver);
        };

        //攻击
        GameObject btnAttack = (GameObject)Resources.Load("Prefab/CanvasPerfab/Button");
        btnAttack = Instantiate(btnAttack, CommonFuncs.GetMainCanvas().transform, false);
        btnAttack.GetComponent<RectTransform>().localPosition = new Vector3(740, -130, 0);
        btnAttack.GetComponent<RectTransform>().localPosition = new Vector3(5 * w / 12, -1 * h / 5, 0);
        btnAttack.GetComponent<RectTransform>().localScale = new Vector2(w / 1200, h / 1000);
        btnAttack.GetComponent<UIButton>().SetTitle("攻击");
        btnAttack.GetComponent<UIButton>().clickAction = () =>
        {
            GameObject.Find("Baolong").SendMessage("AttackMove", SendMessageOptions.DontRequireReceiver);
        };
    }

    private void OnGUI(){
        //重新开始按钮
        if (GUI.Button(new Rect(w * 0.89f, h * 0.04f, w * 0.1f, h * 0.08f), "重新开始")){
            Reload();
        }
    }
}
