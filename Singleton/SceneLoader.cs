using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    Coroutine _loadRoutine;
    public static SceneLoader Instance;
    void Awake()
    {
        InitSingleton();
    }
    private void InitSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void DelayedLoadScene(int index, float delay, Action callback = null)
    {
        if (_loadRoutine != null)
            StopCoroutine(_loadRoutine);
        _loadRoutine = StartCoroutine(DelayedLoadSceneCoroutine(index, delay, callback));
    }
    IEnumerator DelayedLoadSceneCoroutine(int index, float delay, Action callback = null)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(index);
        if (callback != null)
            callback();

        _loadRoutine = null;
    }

}
