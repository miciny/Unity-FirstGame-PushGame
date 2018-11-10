using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    public delegate void Confim();
    public class MessageBox : MonoBehaviour
    {
        static GameObject Messagebox;
        public static Confim confim;
        public static string TitleStr;
        public static string ContentStr;

        public static void Show(string content)
        {
            ContentStr = "    " + content;
            Messagebox = (GameObject)Resources.Load("Prefab/CanvasPerfab/MessageBackground");
            Messagebox = Instantiate(Messagebox, GameObject.Find("Canvas").transform);
            Messagebox.transform.localScale = new Vector3(1, 1, 1);
            Messagebox.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            Messagebox.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            Messagebox.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            Time.timeScale = 1;
        }

        public static void Show(string title, string content)
        {
            TitleStr = title;
            ContentStr = "    " + content;
            Messagebox = (GameObject)Resources.Load("Prefab/CanvasPerfab/MessageBackground");
            Messagebox = Instantiate(Messagebox, GameObject.Find("Canvas").transform);
            Messagebox.transform.localScale = new Vector3(1, 1, 1);
            Messagebox.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            Messagebox.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            Messagebox.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            Time.timeScale = 1;
        }

        public static void SureClick()
        {
            if (confim != null)
            {
                TitleStr = "标题";
                ContentStr = null;
                confim();
                Destroy(Messagebox);
                print("执行确认事件，销毁自己");
            }
        }

        public static void CancleClick()
        {
            TitleStr = "标题";
            ContentStr = null;
            Destroy(Messagebox);
        }
    }
}