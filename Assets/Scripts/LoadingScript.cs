using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Text LoadingPercentage;
    public Image LoadingProgressBar;
    public GameObject hideObjecte;
    private int scene=0;
    private int hideNum=0;
    private static SceneTransition instance;
    private static bool shouldPlayOpeningAnimation = false; 
    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperation;


    public void SceneSet(int _scene) {
        scene = _scene; 
        if (_scene == 2){
            hideNum++;
        }
    }
    public static void SwitchTheScene(int sceneIndex){
        SceneManager.LoadScene(sceneIndex);
    }

    public static void SwitchToScene(int sceneIndex)
    {
        instance.componentAnimator.SetTrigger("SceneStart");

        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneIndex);
        
        // Чтобы сцена не начала переключаться пока играет анимация closing:
        instance.loadingSceneOperation.allowSceneActivation = false;
        
        instance.LoadingProgressBar.fillAmount = 0;
    }
    
    private void Start()
    {
        instance = this;
        
        try{
            componentAnimator = GetComponent<Animator>();
            
            if (shouldPlayOpeningAnimation) 
            {
                componentAnimator.SetTrigger("SceneEnd");
                instance.LoadingProgressBar.fillAmount = 1;
                
                // Чтобы если следующий переход будет обычным SceneManager.LoadScene, не проигрывать анимацию opening:
                shouldPlayOpeningAnimation = false; 
            }

        }catch{
        }
    }

    private void Update()
    {
        if (loadingSceneOperation != null)
        {
            LoadingPercentage.text = Mathf.RoundToInt(loadingSceneOperation.progress * 100) + "%";
            
            // Просто присвоить прогресс:
            //LoadingProgressBar.fillAmount = loadingSceneOperation.progress; 
            
            // Присвоить прогресс с быстрой анимацией, чтобы ощущалось плавнее:
            LoadingProgressBar.fillAmount = Mathf.Lerp(LoadingProgressBar.fillAmount, loadingSceneOperation.progress,
                Time.deltaTime * 5);
        }
        if (hideNum >= 5){
            hideObjecte.SetActive(true);
        }
    }

    public void OnAnimationOver()
    {
        // Чтобы при открытии сцены, куда мы переключаемся, проигралась анимация opening:
        shouldPlayOpeningAnimation = true;
        
        loadingSceneOperation.allowSceneActivation = true;
    }
    void OnDestroy()
    {
        MyVariables._int = scene;
    }
}