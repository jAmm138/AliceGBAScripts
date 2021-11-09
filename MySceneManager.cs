using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public SceneData[] scenes;
    public SceneData currentScene;
    public SceneData previousScene;
    public bool isLoading;
    public bool debugSkip;
    public string debugSkipLevel;
    public static MySceneManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ResetSceneData();
        LoadScene(scenes[0].sceneId);

        if (debugSkip)
            LoadScene(debugSkipLevel);
        else
            LoadScene(scenes[0].sceneId);
    }

    #region Controls

    void ResetSceneData()
    {
        foreach (SceneData sd in scenes)
        {
            sd.isLoaded = false;
        }
    }

    public void LoadNextScene()
    {
        if (currentScene.nextScene != null)
            LoadScene(currentScene.nextScene);
    }

    public void LoadSetActiveScene(string s)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(s));
    }

    public void ReloadCurrentLevel()
    {
        UnloadScene(currentScene.sceneId);
        LoadScene(currentScene.sceneId);
    }

    public void LoadScene(string targetScene)
    {
        if (currentScene != null && currentScene != scenes[1])
            previousScene = currentScene;

        SceneData s = GetMyScene(targetScene);
        currentScene = s;

        if (s.isLoaded)
            return;

        StartCoroutine(LoadScene(targetScene, LoadSceneMode.Additive, false));
        s.isLoaded = true;
    }

    public void LoadAndUnloadScenes(string load, string unload)
    {
        SceneData loadScene = GetMyScene(load);
        SceneData unloadScene = GetMyScene(unload);

        if (string.IsNullOrEmpty(load) == false)
        {
            LoadScene(load);
            isLoading = false;
        }

        if (string.IsNullOrEmpty(unload) == false)
            UnloadScene(unload);

    }

    #endregion

    #region Loading Logic

    public void UnloadCurrentScene()
    {
        UnloadScene(currentScene.sceneId);
    }

    public void UnloadScene(string targetScene)
    {
        SceneData s = GetMyScene(targetScene);
        if (s.isLoaded)
            s.isLoaded = false;
        else
            return;

        SceneManager.UnloadSceneAsync(targetScene);
    }

    IEnumerator LoadScene(string targetScene, LoadSceneMode mode, bool isActiveScene = false)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene, mode);
        yield return operation;

        while (!operation.isDone)
        {
            isLoading = true;
            float progress = Mathf.Clamp01(operation.progress / .9f);
            //UI Loading Icon

            yield return null;
        }

        //if (operation.isDone)
        //  isLoading = false;

    }

    #endregion

    #region Helper Methods

    public SceneData GetMyScene(string sceneId)
    {
        SceneData r = null;

        for (int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i].sceneId == sceneId)
            {
                r = scenes[i];
            }
        }

        return r;
    }

    #endregion
}
