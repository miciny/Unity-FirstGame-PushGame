using UnityEngine;
using UnityEngine.UI;
using Common;

public class UIMessage : MonoBehaviour {

    public Text Title;
    public Text Content;    //这个是Content下的text
    public Button SureBtn;
    public Button CancleBtn;

    void Start()
    {
        print("绑定确认框事件");
        SureBtn.onClick.AddListener(MessageBox.SureClick);
        CancleBtn.onClick.AddListener(MessageBox.CancleClick);
        Title.text = MessageBox.TitleStr;
        Content.text = MessageBox.ContentStr;
    }
}
