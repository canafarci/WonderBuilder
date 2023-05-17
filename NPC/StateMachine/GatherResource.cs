using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GatherResource : StateMachineAction
{
    NavMeshNPC _npc;
    Animator _animator;
    Wonder _wonder;
    BuilderFX _fx;
    bool _noObjectsLeft;
    protected override void Awake()
    {
        base.Awake();
        _npc = GetComponent<NavMeshNPC>();
        _animator = GetComponentInChildren<Animator>();
        _wonder = FindObjectOfType<Wonder>();
        _fx = GetComponent<BuilderFX>();
    }
    public override IEnumerator Execute()
    {
        Enter();
        //move to pile
        if (_noObjectsLeft) yield break;
        ResourcePile pile = GetPile();
        yield return _npc.GetToPos(pile.GetPickupPoint());
        //gather

        BuilderFX.ChangedAnimationBlend(_animator, 1f, 0.25f);
        _builderStats.AddToList(_wonder.GetNextBuildObject());
        //error check
        if (!_builderStats.HasResource)
        {
            _noObjectsLeft = true;
            yield break;
        }
        //move to build location
        _builderStats.PosToMove = _wonder.GetDestination(_builderStats.GetItem().Item1, 1);
        yield return new WaitForSeconds(1f);
        pile.TakeFromPile();
        _fx.EnableBuildObject(pile.PileType);

        Exit();
    }
    ResourcePile GetPile()
    {
        ResourcePile[] piles = References.Instance.Piles;
        int randInt = Random.Range(0, piles.Length);
        return piles[randInt];
    }
    protected override void Enter()
    {
        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();
        BuilderFX.ChangedAnimationBlend(_animator, 0.5f, 0.1f);
    }
    protected override void Exit()
    {
        BuilderFX.ChangedAnimationBlend(_animator, 0.5f, 0.1f);
    }
}