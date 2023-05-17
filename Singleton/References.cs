using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    public GameConfig GameConfig { get { return _config; } }
    [SerializeField] GameConfig _config;
    public ResourcePile[] Piles;
    public AnimationClip BuildClip;
    public Transform[] MergePoints;
    public static References Instance;
    void Awake()
    {
        InitSingleton();
        Init();
    }

    private void Init()
    {
        Piles = FindObjectsOfType<ResourcePile>();
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
}
