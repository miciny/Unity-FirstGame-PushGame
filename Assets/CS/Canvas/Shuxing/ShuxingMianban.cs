using UnityEngine;
using UnityEngine.UI;

public class ShuxingMianban : MonoBehaviour {

    public Text level;
    public Text atk;
    public Text blood;
    public Button hideBtn;

    public RawImage mainWeapon;

    GameObject textureFather;

    // Use this for initialization
    void Start () {
        hideBtn.onClick.AddListener(Hide);

        textureFather = new GameObject{
            name = "AllTextureFather"
        };

        CreateWeaponUI();
    }
	
    public void Hide(){
        Destroy(gameObject);
        Destroy(textureFather);
    }

    void CreateWeaponUI(){
        RenderTexture weaponTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        weaponTexture.Create();

        GameObject CameraObj = new GameObject("WeaponCamera");
        Camera mycamera = CameraObj.AddComponent<Camera>();
        mycamera.targetTexture = weaponTexture;
        mycamera.transform.position = new Vector3(0, -101, 77);
        mycamera.transform.parent = textureFather.transform;

        GameObject theWeapon = (GameObject)Resources.Load("Prefab/WeaponPerfab/Sword1");
        theWeapon = Instantiate(theWeapon, textureFather.transform, false);
        theWeapon.transform.position = new Vector3(0, -100, 80);

        mainWeapon.GetComponent<RawImage>().texture = weaponTexture;
    }
}
