using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderStateMachine : MonoBehaviour
{
    [SerializeField] StateMachineAction _build, _gatherResource;
    BuilderStats _stats;
    private void Awake()
    {
        _stats = GetComponent<BuilderStats>();
    }

    public IEnumerator Tick()
    {
        // if (_stats.IsTired)
        //     yield return _rest.Execute();
        if (!_stats.HasResource)
            yield return _gatherResource.Execute();
        else
            yield return _build.Execute();
    }
}
