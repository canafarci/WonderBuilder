using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] Transform _spawnPoint;
    [SerializeField] GameObject[] _prefabs;
    UpgradeManager _manager;
    List<Animator> _animators = new List<Animator>();
    MergeUpgrade _upgrader;
    [SerializeField] UnityEvent _event1, _event2, _event3;
    private void Awake()
    {
        _manager = FindObjectOfType<UpgradeManager>();
        _upgrader = FindObjectOfType<MergeUpgrade>();
    }
    private void OnEnable()
    {
        _manager.NPCCountChangeHandler += OnNPCChange;
    }

    private void Start()
    {
        Load();
    }
    private void OnDisable()
    {
        _manager.NPCCountChangeHandler += OnNPCChange;

    }
    public BuilderNPC Spawn()
    {
        return GameObject.Instantiate(_prefabs[0], _spawnPoint.transform.position, _spawnPoint.transform.rotation, transform).GetComponent<BuilderNPC>();
    }
    public BuilderNPC Spawn(int tier)
    {
        return GameObject.Instantiate(_prefabs[tier], _spawnPoint.transform.position, _spawnPoint.transform.rotation, transform).GetComponent<BuilderNPC>();
    }
    public void Spawn(int tier, Vector3 position, List<(GameObject, WonderStage)> objects, Animator animator)
    {
        _animators.Add(animator);
        if (_animators.Count == 3)
        {
            StartCoroutine(MergeCoroutine(tier, position, objects));
        }
    }

    IEnumerator MergeCoroutine(int tier, Vector3 position, List<(GameObject, WonderStage)> objects)
    {
        CameraController.Instance.ActivateCamera("MergeCam");
        yield return new WaitForSeconds(.2f);
        _event1.Invoke();

        Camera.main.cullingMask = LayerMask.GetMask("Navmesh");
        _animators.ForEach(x => x.Play("Jump"));
        yield return new WaitForSeconds(1.25f);
        _event2.Invoke();

        _animators.ForEach(x =>
        {
            _manager.RemoveFromList(x.GetComponentInParent<BuilderNPC>());
            Destroy(x.gameObject, 0.1f);
        });
        _animators = new List<Animator>();

        BuilderStats npc = GameObject.Instantiate(_prefabs[tier], position, _prefabs[tier].transform.rotation, transform).GetComponent<BuilderStats>();
        npc.SetList(objects);
        npc.GetComponent<BuilderFX>().PlayMergeFX();
        _manager.AddToList(npc.GetComponent<BuilderNPC>());
        yield return new WaitForSeconds(0.5f);
        CameraController.Instance.ActivateCamera("Main Camera");
        Camera.main.cullingMask = -1;
        //yield return new WaitForSeconds(2f);
        _event3.Invoke();
        yield return new WaitForSeconds(0.5f);
        _upgrader.OnMergeFinished();
    }
    void Load()
    {
        string scene = SceneManager.GetActiveScene().buildIndex.ToString();
        if (!PlayerPrefs.HasKey(scene + "LEVEL0"))
        {
            Spawn();
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                if (!PlayerPrefs.HasKey(scene + "LEVEL" + i.ToString())) continue;
                int count = PlayerPrefs.GetInt(scene + "LEVEL" + i.ToString());

                for (int j = 0; j < count; j++)
                {
                    Spawn(i);
                }
            }
        }

        _manager.InitListAfterLoad();
    }
    private void OnNPCChange(List<BuilderNPC> lst)
    {
        string scene = SceneManager.GetActiveScene().buildIndex.ToString();
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(scene + "LEVEL" + i.ToString(), 0);
        }

        foreach (BuilderNPC npc in lst)
        {
            int count = PlayerPrefs.GetInt(scene + "LEVEL" + npc.Tier.ToString());
            count += 1;
            PlayerPrefs.SetInt(scene + "LEVEL" + npc.Tier.ToString(), count);
        }
    }
}