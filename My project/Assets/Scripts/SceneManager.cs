using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance {get; private set;}

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    public void TitleScene()
    {
        LoadScene("TitleScene");
    }

    public void MainGameScene()
    {
        LoadScene("MainGameSceneStage1");
    }

    public void Stage2Scene()
    {
        LoadScene("MainGameSceneStage2");
    }

    public void Stage3Scene()
    {
        LoadScene("MainGameSceneStage3");
    }

    public void CheckCurrentScene()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        if(currentScene == "MainGameSceneStage1") Stage2Scene();
        else if(currentScene == "MainGameSceneStage2") Stage3Scene();
        else LoadScene("EndScene");
    }
    
    private void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}