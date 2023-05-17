using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Build : StateMachineAction
{
    BuilderNPC _npc;
    Wonder _wonder;
    GameObject _objectToBuild;
    Animator _animator;
    BuilderFX _fx;
    protected override void Awake()
    {
        base.Awake();
        _npc = GetComponent<BuilderNPC>();
        _animator = GetComponentInChildren<Animator>();
        _wonder = FindObjectOfType<Wonder>();
        _fx = GetComponent<BuilderFX>();
    }
    protected override void Enter()
    {
        BuilderFX.ChangedAnimationBlend(_animator, 0.5f, 0.1f);
    }
    public override IEnumerator Execute()
    {
        Enter();
        if (!_builderStats.HasResource) yield break;
        yield return _npc.GetToPos(_builderStats.PosToMove);

        BuilderFX.ChangedAnimationBlend(_animator, 0f, 0.25f);

        float buildTime = _wonder.CurrentStage.GetBuildTime(_npc.BuildSpeed);
        _fx.FillSlider(buildTime);
        _fx.FaceObject(_builderStats.BuildList[0].Item1.transform.position);
        yield return new WaitForSeconds(buildTime);

        Exit();
    }
    protected override void Exit()
    {

        _wonder.Build(_builderStats.GetItemAndRemove());
        _fx.DisableBuildObject();
        _fx.PlayUnloadFX();
        float reward = _wonder.CurrentStage.GetReward(_npc.Tier);
        _fx.MoneyGainFX(reward);
        //Get money
        Resource.Instance.OnMoneyChange(reward);
        BuilderFX.ChangedAnimationBlend(_animator, .5f, 0.1f);

    }
}