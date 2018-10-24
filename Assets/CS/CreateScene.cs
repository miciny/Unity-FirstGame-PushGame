using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateScene : MonoBehaviour{

    protected int[,] map;
    protected string sceneName;
    protected int aimScore;
    public GameObject Player;  //动态添加的物体
    public GameObject Text;

    public static int level;

    protected string test = "开始：";  //测试用

    // Use this for initialization
    void Start(){

        //游戏动态场景
        sceneName = SceneManager.GetActiveScene().name;
        test += "加载场景："+ sceneName + "\n";
        if (sceneName == "FirstLevel"){
            map = Map.the_map_1;
            aimScore = Map.the_map_1_score;
            level = 1;
        }
        else if (sceneName == "NextLevel"){
            map = Map.the_map_2;
            aimScore = Map.the_map_2_score;
            level = 2;
        }

        test += "地图长宽：" + map.GetLength(0) + "," + map.GetLength(1) + "\n";

        for (int i = 0; i < map.GetLength(0); i++){
            for (int j = 0; j < map.GetLength(1); j++){
                if (map[i, j] == 1){
                    test += "加载石头：[" + i + "," + j + "]\n";
                    CreateTheStone(i, j); 
                }
                if (map[i, j] == 2)
                {
                    test += "加载箱子：[" + i + "," + j + "]\n";
                    CreateTheBox1(i, j);
                }
                if (map[i, j] == 3)
                {
                    test += "加载目标：[" + i + "," + j + "]\n";
                    CreateTheAim(i, j);
                }
                if (map[i, j] == 4)
                {
                    test += "加载目标和箱子：[" + i + "," + j + "]\n";
                    CreateTheBox1(i, j);
                    CreateTheAim(i, j);
                }
                if (map[i, j] == 5)
                {
                    test += "加载玩家：[" + i + "," + j + "]\n";
                    CreateThePlayer(i, j);
                }
            }
        }

        //游戏静态场景
        CreateTexture("Level", "level: 1", new Vector3(6, 0, 5));
        test += "加载关卡" + "\n";
        CreateTexture("Count", "count: 0", new Vector3(6, 0, 2));
        test += "加载步数" + "\n";
        GameObject.Find("Level").SendMessage("ChangeText");
    }

    // Update is called once per frame
    void Update(){

    }


    void LoadTheScene(){
        if (aimScore == Aim.score)
        {
            if (sceneName == "FirstLevel")
            {
                Aim.score = 0;
                PlayerMove.count = 0;
                SceneManager.LoadScene("NextLevel");
            }
            else if (sceneName == "NextLevel")
            {
                Aim.score = 0;
                PlayerMove.count = 0;
                SceneManager.LoadScene("FirstLevel");
            }
        }
    }

    //创建一个内置的模型cube=   stone
    void CreateTheStone(int i, int j)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Stone";
        float x = j - 4.5f;
        float z = 4.5f - i;
        go.transform.position = new Vector3(x, 0.5f, z);
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

        float x = j - 4.5f;
        float z = 4.5f - i;
        go.transform.position = new Vector3(x, 0.5f, z);
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

        float x = j - 4.5f;
        float z = 4.5f - i;
        go.transform.position = new Vector3(x, 0.05f, z);
        go.transform.localScale = new Vector3(0.9f, 0.1f, 0.9f);

        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<BoxCollider>().size = new Vector3(0.05f, 1f, 0.05f);

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
        GameObject go = Instantiate(Player);
        go.name = "Player";

        float x = j - 4.5f;
        float z = 4.5f - i;
        go.transform.position = new Vector3(x, 0.75f, z);
        go.transform.localEulerAngles = new Vector3(-90, -90, 0);
        go.transform.localScale = new Vector3(1f, 1f, 1f);

        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().isTrigger = false;

        go.AddComponent<Rigidbody>();
        go.GetComponent<Rigidbody>().useGravity = false;
        go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        go.GetComponent<Rigidbody>().freezeRotation = true;

        go.AddComponent<PlayerMove>();
        go.GetComponent<PlayerMove>().playerRb = go.GetComponent<Rigidbody>();
    }


    //创建text，关卡 步数
    void CreateTexture(string the_name, string text, Vector3 position){
        GameObject go = Instantiate(Text);
        go.name = the_name;
        go.transform.position = position;
        go.GetComponent<TextMesh>().text = text;

        go.AddComponent<Text>();
        go.GetComponent<Text>().rb = go;
    }


    private void OnGUI()
    {
        float w = Screen.width;
        float h = Screen.height;

       //移动按钮
        if (GUI.Button(new Rect(w * 0.15f, h * 0.7f, w * 0.1f, h * 0.1f), "↑"))
        {
            GameObject.Find("Player").SendMessage("RayDetect", 1, SendMessageOptions.DontRequireReceiver);
        }
        if (GUI.Button(new Rect(w * 0.04f, h * 0.82f, w * 0.1f, h * 0.1f), "←"))
        {
            GameObject.Find("Player").SendMessage("RayDetect", 4, SendMessageOptions.DontRequireReceiver);
        }
        if (GUI.Button(new Rect(w * 0.15f, h * 0.82f, w * 0.1f, h * 0.1f), "↓"))
        {
            GameObject.Find("Player").SendMessage("RayDetect", 3, SendMessageOptions.DontRequireReceiver);
        }
        if (GUI.Button(new Rect(w * 0.26f, h * 0.82f, w * 0.1f, h * 0.1f), "→"))
        {
            GameObject.Find("Player").SendMessage("RayDetect", 2, SendMessageOptions.DontRequireReceiver);
        }

        //重新开始按钮
        if (GUI.Button(new Rect(w * 0.04f, h * 0.04f, w * 0.12f, h * 0.1f), "重新开始"))
        {
            Aim.score = 0;
            PlayerMove.count = 0;
            SceneManager.LoadScene("FirstLevel");
        }

        //test
        GUI.Label(new Rect(w * 0.70f, h * 0.04f, w * 0.25f, h * 0.9f), test);
    }
}