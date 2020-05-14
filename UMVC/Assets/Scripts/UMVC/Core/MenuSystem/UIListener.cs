﻿using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class UIListener : MonoBehaviour
{
    public static bool CanEscape = true;

    protected virtual void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    protected virtual void OnSceneLoaded(UnityEngine.SceneManagement.Scene loadedScene, LoadSceneMode loadSceneMode)
    {
    }

    protected virtual void OnSceneUnloaded(UnityEngine.SceneManagement.Scene loadedScene)
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CanEscape)
            UIMenuManager.Instance.OnBackPressed();
    }
}
