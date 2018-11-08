using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Globe
{
    public static string nextSceneName;

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    /// <param name="isAsync">是否使用异步加载</param>
    /// 
    public static void LodingScene(string sceneName, bool isAsync = true)
    {
        //清理Unity垃圾
        Resources.UnloadUnusedAssets();
        //清理GC垃圾
        System.GC.Collect();
        //是否使用异步加载
        if (isAsync){
            //赋值加载场景名称
            nextSceneName = sceneName;
            //跳转到LoadingScene场景
            SceneManager.LoadScene("Loading");
        }else{
            SceneManager.LoadScene(sceneName);
        }
    }

}
/// <summary>
/// 异步加载脚本
/// </summary>
public class AsyncLoadScene : MonoBehaviour
{
    public Slider loadingSlider;
    public Text loadingText;
    AsyncOperation operation;
    int displayProgress;
    int toProgress;

    void Start()
    {
        loadingSlider.value = 0;
        loadingSlider.maxValue = 100;

        if (SceneManager.GetActiveScene().name == "Loading"){
            //启动协程  
            StartCoroutine(AsyncLoading());
        }
    }

    private IEnumerator AsyncLoading(){
        operation = SceneManager.LoadSceneAsync(Globe.nextSceneName);
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f){
            toProgress = (int)operation.progress * 100;
            while (displayProgress < toProgress){
                ++displayProgress;
                SetLoadingPercentage(displayProgress);
                yield return new WaitForEndOfFrame();
            }
        }
        toProgress = 100;

        while (displayProgress < toProgress){
            ++displayProgress;
            SetLoadingPercentage(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        operation.allowSceneActivation = true;
    }

    private void SetLoadingPercentage(int Percentage)
    {
        loadingSlider.value = Percentage;
        loadingText.text = Percentage + "%";
    }
}