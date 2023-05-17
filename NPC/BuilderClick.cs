using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuilderClick : MonoBehaviour
{
    [SerializeField] GameObject _trail;
    InputReader _reader;
    Coroutine _timeDecayRoutine;
    NavMeshAgent _agent;
    UpgradeManager _manager;
    public TrailRenderer SpeedUpTrail;
    public float BaseSpeed { set { _baseSpeed = value; } }
    float _baseSpeed;

    void Awake()
    {
        _reader = FindObjectOfType<InputReader>();
        _agent = GetComponent<NavMeshAgent>();
        _manager = FindObjectOfType<UpgradeManager>();
    }
    private void Start()
    {
        _baseSpeed = _manager.BaseAgentSpeed;
        _agent.speed = _baseSpeed / 2f;
    }
    private void OnEnable() => _reader.ClickHandler += OnClick;
    private void OnDisable() => _reader.ClickHandler -= OnClick;

    private void OnClick()
    {
        if (_timeDecayRoutine != null)
            StopCoroutine(_timeDecayRoutine);

        _timeDecayRoutine = StartCoroutine(TimeDecay());
    }
    IEnumerator TimeDecay()
    {
        if (SpeedUpTrail.emitting==false)
        {
           // _trail.SetActive(true);
            SpeedUpTrail.emitting = true;
            _agent.speed = _baseSpeed;
        }

        yield return new WaitForSeconds(.25f);

        //_trail.SetActive(false);
       SpeedUpTrail.emitting = false;
        _agent.speed = _baseSpeed / 2f;

        _timeDecayRoutine = null;
    }
}
