using UnityEngine;
using UnityEngine.SceneManagement;
using Common;

public class CreatePushScene : MonoBehaviour{

    protected int[,] map;
    protected string sceneName;
    protected int aimScore;

    public static int level = 1; //当前关卡
    int levelNm; //关卡数量
    readonly float w = Screen.width;
    readonly float h = Screen.height;

    protected GameObject AllParentGo;  //游戏元素的父节点，除了plane

    // Use this for initialization
    void Start(){

        AllParentGo = new GameObject
        {
            name = "FatherOfAll"
        };

        levelNm = Map.MapArray.Count;

        //游戏动态场景
        sceneName = SceneManager.GetActiveScene().name; 
        int index = SceneManager.GetActiveScene().buildIndex; //0开始
        print("加载场景："+ sceneName + "，编号为：" + index);
        ProgressData();

        //游戏静态场景
        CreateTexture("Level", "level: 1", new Vector3(6f, 2.5f, 5f));
        CreateTexture("Count", "count: 0", new Vector3(6, 1, 5));
        GameObject.Find("Level").SendMessage("ChangeText");

        //数据存储读取
        if (PlayerPrefs.HasKey("level"))
        {
            level = PlayerPrefs.GetInt("level");
        }
        else
        {
            PlayerPrefs.SetInt("level", level);
        }
    }


    // Update is called once per frame
    void Update(){

    }

    //处理数据，加载物体
    void ProgressData(){
        int index = level - 1;
        PlayerPrefs.SetInt("level", level);
        print(PlayerPrefs.GetInt("level"));
        map = (int[,])Map.MapArray[index];
        aimScore = Map.LevelAimNoArray[index];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 1)
                {
                    CreateTheStone(i, j);
                }
                if (map[i, j] == 2)
                {
                    CreateTheBox1(i, j);
                }
                if (map[i, j] == 3)
                {
                    CreateTheAim(i, j);
                }
                if (map[i, j] == 4)
                {
                    CreateTheBox1(i, j);
                    CreateTheAim(i, j);
                }
                if (map[i, j] == 5)
                {
                    CreateThePlayer(i, j);
                }
            }
        }
    }

    //加载关卡，清掉所有子对象，重新添加子对象
    void LoadLevel()
    {
        Aim.score = 0;
        PlayerMove.count = 0;
        GameObject.Find("Level").SendMessage("ChangeText");
        GameObject.Find("Count").SendMessage("ChangeText");

        if (AllParentGo){
            int c = AllParentGo.transform.childCount;
            for (int i = 0; i < c; i++){
                Destroy(AllParentGo.transform.GetChild(i).gameObject);
            }
        }
        ProgressData();
    }

    //胜利时，加载下一关，或者到下一个场景
    void Vectory(){
        if (aimScore == Aim.score)
        {
            if (level < levelNm)
            {
                level++;
                MessageBox.Show("恭喜", "过了第"+ (level-1)+"关");
                MessageBox.confim = LoadLevel;
            }
            else
            {
                MessageBox.Show("恭喜", "过了第一个场景，下面进入第二个场景");
                MessageBox.confim = LoadNextScene;
            }
        }
    }

    //加载下一个场景
    void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex; //0开始
        SceneManager.LoadScene(index+1);
    }

    //创建一个内置的模型cube=   stone
    void CreateTheStone(int i, int j)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Stone";
        go.transform.parent = AllParentGo.transform;

        float[] p = Map.PositionCulculate(i, j);
        go.transform.position = new Vector3(p[0], 0.5f, p[1]);

        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().isTrigger = false;

        go.AddComponent<Rigidbody>();
        go.GetComponent<Rigidbody>().useGravity = true;
        go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        go.GetComponent<Rigidbody>().freezeRotation = true;

        Material mat = new Material(Shader.Find("Shader Forge/StoneShader"));
        go.GetComponent<MeshRenderer>().material = mat;
    }

    //创建一个内置的模型cube=   Box1
    void CreateTheBox1(int i, int j)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Box1";
        go.transform.parent = AllParentGo.transform;

        float[] p = Map.PositionCulculate(i, j);
        go.transform.position = new Vector3(p[0], 0.5f, p[1]);
        go.transform.localScale = new Vector3(0.9f, 1.0f, 0.9f);

        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<BoxCollider>().size = new Vector3(0.5f, 1f, 0.5f);

        go.AddComponent<Rigidbody>();
        go.GetComponent<Rigidbody>().useGravity = false;
        go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        go.GetComponent<Rigidbody>().freezeRotation = true;

        Material mat = new Material(Shader.Find("Shader Forge/Box"));
        go.GetComponent<MeshRenderer>().material = mat;
    }

    //创建一个内置的模型cube=   Aim
    void CreateTheAim(int i, int j)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Aim";
        go.transform.parent = AllParentGo.transform;

        float[] p = Map.PositionCulculate(i, j);
        go.transform.position = new Vector3(p[0], 0.05f, p[1]);
        go.transform.localScale = new Vector3(0.9f, 0.1f, 0.9f);

        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<BoxCollider>().size = new Vector3(0.05f, 10f, 0.05f);

        go.AddComponent<Rigidbody>();
        go.GetComponent<Rigidbody>().useGravity = false;
        go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        go.GetComponent<Rigidbody>().freezeRotation = true;

        go.AddComponent<Aim>();
        go.GetComponent<Aim>().rb = go.GetComponent<Rigidbody>();

        Material mat = new Material(Shader.Find("Shader Forge/Aim"));
        go.GetComponent<MeshRenderer>().material = mat;
    }


    //创建一个自己的模型=   Player
    void CreateThePlayer(int i, int j)
    {
        GameObject go = (GameObject)Resources.Load("Prefab/Baolong");
        go = Instantiate(go);
        go.name = "Player";
        go.transform.parent = AllParentGo.transform;

        float[] p = Map.PositionCulculate(i, j);
        go.transform.position = new Vector3(p[0], 0, p[1]);
        go.transform.localEulerAngles = new Vector3(0, 0, 0);

        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().isTrigger = false;
        go.GetComponent<BoxCollider>().size = new Vector3(0.001f, 0.01f, 0.001f);

        go.AddComponent<Rigidbody>();
        go.GetComponent<Rigidbody>().useGravity = false;
        go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        go.GetComponent<Rigidbody>().freezeRotation = true;

        go.AddComponent<PlayerMove>();
        go.GetComponent<PlayerMove>().playerRb = go.GetComponent<Rigidbody>();

        GameObject.Find("Main Camera").AddComponent<CameraFixed>();
    }


    //创建text，关卡 步数
    void CreateTexture(string the_name, string text, Vector3 position){
        GameObject go = (GameObject)Resources.Load("Prefab/LevelAndCount");
        go = Instantiate(go);
        go.name = the_name;
        go.transform.position = position;
        go.GetComponent<TextMesh>().text = text;

        go.AddComponent<TheLabel>();
        go.GetComponent<TheLabel>().rb = go;
    }


    private void OnGUI()
    {
       //移动按钮
        if (GUI.Button(new Rect(w * 0.15f, h * 0.7f, w * 0.1f, h * 0.1f), "↑"))
        {
            PlayerRayDetect(1);
        }
        if (GUI.Button(new Rect(w * 0.04f, h * 0.82f, w * 0.1f, h * 0.1f), "←"))
        {
            PlayerRayDetect(4);
        }
        if (GUI.Button(new Rect(w * 0.15f, h * 0.82f, w * 0.1f, h * 0.1f), "↓"))
        {
            PlayerRayDetect(3);
        }
        if (GUI.Button(new Rect(w * 0.26f, h * 0.82f, w * 0.1f, h * 0.1f), "→"))
        {
            PlayerRayDetect(2);
        }

        //重新开始按钮
        if (GUI.Button(new Rect(w * 0.04f, h * 0.04f, w * 0.12f, h * 0.1f), "重新开始"))
        {
            LoadLevel();
        }
        if (GUI.Button(new Rect(w * 0.04f, h * 0.16f, w * 0.12f, h * 0.1f), "跳过"))
        {
            LoadNextScene();
        }
    }

    //移动人物检测
    void PlayerRayDetect(int d){
        GameObject.Find("Player").SendMessage("RayDetect", d, SendMessageOptions.DontRequireReceiver);
    }
}