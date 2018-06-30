using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup pauseMenu;

    [SerializeField]
    private CanvasGroup gameplayUI;

    [SerializeField]
    private CanvasGroup optionsMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.gameObject.activeInHierarchy)
            {
                ResumeButtonPressed();
            }
            else
            {
                Time.timeScale = 0.0f;
                gameplayUI.alpha = 0.5f;
                pauseMenu.gameObject.SetActive(true);
            }
        }
    }

    public void OptionButtonPressed()
    {
        optionsMenu.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
    }

    public void ResumeButtonPressed()
    {
        pauseMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        gameplayUI.alpha = 1.0f;
    }

    public void MainMenuButtonPressed()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(SwitchScene("menuPrincipal", SceneManager.GetActiveScene().name));
    }

    IEnumerator SwitchScene(string toLoad, string toUnload = "")
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("loadingScreen", LoadSceneMode.Additive);
        while (!op.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        LoadingScreen loadingScreen = FindObjectOfType<LoadingScreen>();
        loadingScreen.StartLoadingProcess(toLoad, toUnload);
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }

}
