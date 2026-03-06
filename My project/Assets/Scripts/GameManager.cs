using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    
    public void OnStartButtonClick()
    {
        SceneManager.Instance.MainGameScene();
    }

    public void OnOptionsButtonClick()
    {
        Debug.Log("옵션창");
    }

    public void OnEndButtonClick()
    {
        Application.Quit();
    }

    public void OnReturnButtonClick()
    {
        SceneManager.Instance.TitleScene();
    }
}
