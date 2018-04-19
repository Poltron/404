using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{    
    public void ButtonPlayPress()
    {
        StartCoroutine(StartPlaying());
    }

    IEnumerator StartPlaying()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("loadingScreen", LoadSceneMode.Additive);
        while (!op.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        LoadingScreen loadingScreen = FindObjectOfType<LoadingScreen>();
        loadingScreen.StartLoadingProcess("main", "menuPrincipal");
    }

    public void ButtonQuitPress()
    {
        Application.Quit();
    }
}
