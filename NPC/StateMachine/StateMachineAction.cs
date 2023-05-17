using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineAction : MonoBehaviour
{
    protected BuilderStats _builderStats;
    protected virtual void Awake()
    {
        _builderStats = GetComponent<BuilderStats>();
    }
    public abstract IEnumerator Execute();
    protected abstract void Enter();
    protected abstract void Exit();
}
