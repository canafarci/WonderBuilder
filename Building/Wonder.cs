using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wonder : MonoBehaviour
{
    public List<WonderStage> Stages;
    WonderStage _currentStage;
    public WonderStage CurrentStage { get { return _currentStage; } }
    [SerializeField] string _uniqueIdentifier;
    int _currentIndex;
    bool _calledEndGame = false;
    private void Start()
    {
        TinySauce.OnGameStarted(_uniqueIdentifier);
        Load();
    }
    public void Build((GameObject, WonderStage) buildTarget)
    {
        buildTarget.Item2.Build(buildTarget.Item1);
    }
    public (GameObject, WonderStage) GetNextBuildObject()
    {
        return _currentStage.GetNextBuildObject();
    }
    public Vector3 GetDestination(GameObject obj, int radius)
    {
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(obj.transform.position, out myNavHit, radius, -1))
        {
            return myNavHit.position;
        }
        else
        {
            return GetDestination(obj, radius * 2);
        }
    }
    public (GameObject, WonderStage) StageComplete()
    {
        _currentIndex++;
        Save();
        if (_currentIndex < Stages.Count)
        {
            _currentStage = Stages[_currentIndex];
            return _currentStage.GetNextBuildObject();
        }
        else
        {
            if (!_calledEndGame)
            {
                TinySauce.OnGameFinished(true, _currentIndex, _uniqueIdentifier);
                FindObjectOfType<EndGame>().OnGameOver();
                _calledEndGame = true;
            }
            return (null, null);
        }
    }

    protected void Load()
    {
        if (PlayerPrefs.HasKey(_uniqueIdentifier))
            _currentIndex = PlayerPrefs.GetInt(_uniqueIdentifier);
        else
        {
            PlayerPrefs.SetInt(_uniqueIdentifier, 0);
            _currentIndex = 0;
        }
        _currentStage = Stages[_currentIndex];
    }

    protected void Save()
    {
        PlayerPrefs.SetInt(_uniqueIdentifier, _currentIndex);
    }

    private void OnApplicationQuit()
    {
        TinySauce.OnGameFinished(false, _currentIndex, _uniqueIdentifier);
    }
}