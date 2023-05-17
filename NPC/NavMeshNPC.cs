using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshNPC : MonoBehaviour
{
    protected NavMeshAgent _agent;
    protected Coroutine _moveCoroutine;
    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    public Coroutine GetToPos(Vector3 target, Action callback = null)
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(GetToPosCoroutine(target, callback));
        return _moveCoroutine;
    }

    IEnumerator GetToPosCoroutine(Vector3 target, Action callback = null)
    {
        _agent.destination = target;

        while (Vector3.Distance(transform.position, target) > _agent.stoppingDistance + ConstantValues.NAVMESH_STOP_OFFSET)
            yield return new WaitForSeconds(ConstantValues.NPC_POLL_RATE);

        if (callback != null)
            callback();

        _moveCoroutine = null;
    }
}
