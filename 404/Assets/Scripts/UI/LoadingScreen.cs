﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private Image panel;

    [SerializeField]
    private Text progressValue;

    [SerializeField]
    private CanvasGroup loadingScreenGroup;

    [SerializeField]
    private int speed;

    private string toLoad;
    private string toUnload;

    private void Awake()
    {
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
    }

    public void StartLoadingProcess(string toLoad, string toUnload)
    {
        this.toLoad = toLoad;
        this.toUnload = toUnload;

        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        // first blacken the screen
        while (panel.color.a < 1)
        {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a + speed * Time.deltaTime);
            loadingScreenGroup.alpha += speed * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        // unload active scene
        AsyncOperation op2 = SceneManager.UnloadSceneAsync(toUnload);
        while (!op2.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        // load active scene
        AsyncOperation op = SceneManager.LoadSceneAsync(toLoad, LoadSceneMode.Additive);
        while (!op.isDone)
        {
            int progress = (int)(op.progress * 100);
            progressValue.text = progress.ToString();
            yield return new WaitForEndOfFrame();
        }

        // set loaded scene canvas to 0
        /*Canvas[] canvases = FindObjectsOfType<Canvas>();
        CanvasGroup toLoadCanvas = null;
        foreach(Canvas c in canvases)
        {
            if (c.gameObject.scene.name == toLoad )
            {
                toLoadCanvas = c.GetComponent<CanvasGroup>();
                toLoadCanvas.alpha = 0;
            }
        }*/

        progressValue.text = "100";

        float time = 0;
        while (time < 1.0f)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


        while (panel.color.a > 0)
        {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - speed * Time.deltaTime);
            loadingScreenGroup.alpha -= speed * 3 * Time.deltaTime;

            /*if (toLoadCanvas)
                toLoadCanvas.alpha += speed * Time.deltaTime;*/

            yield return new WaitForEndOfFrame();
        }

        SceneManager.UnloadSceneAsync("loadingScreen");
    }

}
