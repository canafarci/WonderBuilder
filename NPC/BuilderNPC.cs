using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderNPC : NavMeshNPC
{
    public int Tier = 0;
    public float BuildSpeed = 1f;
    BuilderStateMachine _stateMachine;
    Coroutine _stateTick;
    Wonder _wonder;
    protected override void Awake()
    {
        base.Awake();
        _stateMachine = GetComponent<BuilderStateMachine>();
        _wonder = FindObjectOfType<Wonder>();
    }
    private void Start()
    {
        _stateTick = StartCoroutine(Tick());
    }
    public void Merge(Vector3 target, Vector3 midPoint, List<(GameObject, WonderStage)> objs)
    {
        StopAllCoroutines();
        foreach (StateMachineAction ac in GetComponentsInChildren<StateMachineAction>())
        {
            ac.StopAllCoroutines();
        }
        _stateMachine.StopAllCoroutines();

        Action callback = () =>
            {
                FindObjectOfType<NPCSpawner>().Spawn(Tier + 1, midPoint, objs, GetComponentInChildren<Animator>());
            };

        GetToPos(target, callback);
    }
    IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(ConstantValues.NPC_POLL_RATE);
            yield return _stateMachine.Tick();
        }
    }
    public void StageOver()
    {
        StopAllCoroutines();
    }


}
