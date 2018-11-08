using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackageMianban : MonoBehaviour {

    public Image image0;
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;
    public Image image5;
    public Image image6;
    public Image image7;

    Dictionary<int, ItemModel> allItem = new Dictionary<int, ItemModel>();
    ArrayList allImage = new ArrayList();

    // Use this for initialization
    void Start () {
        allItem = PackageData.GetHeroPackage();
        allImage.Add(image0);
        allImage.Add(image1);
        allImage.Add(image2);
        allImage.Add(image3);
        allImage.Add(image4);
        allImage.Add(image5);
        allImage.Add(image6);
        allImage.Add(image7);

        for (int i = 0; i < allItem.Count; i++){
            string n = allItem[i+1].name;
            Sprite btnShowPackage = (Sprite)Resources.Load("Prefab/WeaponPerfab/"+n+"_pic", typeof(Sprite));
            Image image = (Image)allImage[i];
            image.sprite = btnShowPackage;
        }
    }

    public void Hide(){
        Destroy(gameObject);
    }
}
