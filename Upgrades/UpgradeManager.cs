using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    NPCSpawner _spawner;
    List<BuilderNPC> _npcs = new List<BuilderNPC>();
    public float BaseAgentSpeed;
    public event Action<List<BuilderNPC>> NPCCountChangeHandler;
    public void SetAgentSpeed(float speed)
    {
        BaseAgentSpeed = speed;

        foreach (BuilderClick npc in FindObjectsOfType<BuilderClick>())
        {
            npc.BaseSpeed = BaseAgentSpeed;
        }
    }
    private void Awake()
    {
        _spawner = FindObjectOfType<NPCSpawner>();
    }
    public void InitListAfterLoad()
    {
        foreach (BuilderNPC npc in FindObjectsOfType<BuilderNPC>())
        {
            _npcs.Add(npc);
        }
        NPCCountChangeHandler.Invoke(_npcs);
    }
    public void AddToList(BuilderNPC npc)
    {
        _npcs.Add(npc);
        NPCCountChangeHandler.Invoke(_npcs);
    }
    public void RemoveFromList(BuilderNPC npc)
    {
        _npcs.Remove(npc);
        NPCCountChangeHandler.Invoke(_npcs);
    }
    public void UpdateText()
    {
        //TODO: Update Passive Income Text
    }
    public void SpawnWorker(int level)
    {
        StartCoroutine(SpawnCoroutine(level));
    }
    IEnumerator SpawnCoroutine(int level)
    {
        BuilderNPC npc = _spawner.Spawn();
        _npcs.Add(npc);
        NPCCountChangeHandler.Invoke(_npcs);
        yield return new WaitForSeconds(ConstantValues.NPC_SPAWN_RATE);
    }
    public void OnMergeBuilders(List<BuilderNPC> npcs)
    {
        Vector3 midPoint = (References.Instance.MergePoints[0].position + References.Instance.MergePoints[1].position + References.Instance.MergePoints[2].position) / 3f;

        List<BuilderNPC> sortedList = new List<BuilderNPC>();
        sortedList = npcs.OrderBy(x => Vector3.Distance(x.transform.position, midPoint)).ToList();
        BuilderNPC[] buildersToMerge = new BuilderNPC[3] { sortedList[0], sortedList[1], sortedList[2] };

        List<(GameObject, WonderStage)> objs = new List<(GameObject, WonderStage)>();

        for (int i = 0; i < buildersToMerge.Length; i++)
            objs.AddRange(buildersToMerge[i].GetComponent<BuilderStats>().BuildList);

        for (int i = 0; i < buildersToMerge.Length; i++)
            buildersToMerge[i].Merge(References.Instance.MergePoints[i].position, midPoint, objs);
    }
}




// while (npcs.Count > 2)
// {
//     BuilderNPC npc1 = npcs[0];
//     float dist = Mathf.Infinity;
//     BuilderNPC closestNPC = null;

//     for (int i = 1; i < npcs.Count; i++)
//     {
//         float curDist = Vector3.Distance(npc1.transform.position, npcs[i].transform.position);
//         if (curDist < dist)
//         {
//             closestNPC = npcs[i];
//             dist = curDist;
//         }
//     }

//     Vector3 midPoint = (npc1.transform.position + closestNPC.transform.position) / 2f;

//     List<GameObject> objs = new List<GameObject>(npc1.GetComponent<BuilderStats>().BuildList);
//     objs.AddRange(closestNPC.GetComponent<BuilderStats>().BuildList);

//     npc1.Merge(midPoint, () =>
//     {
//         _spawner.Spawn(npc1.Tier + 1, midPoint, objs);
//         npc1.GetComponent<BuilderFX>().PlayMergeFX();
//         Destroy(npc1.gameObject, 0.1f);
//     });
//     closestNPC.Merge(midPoint, () => Destroy(closestNPC.gameObject, 0.1f));

//     npcs.Remove(npc1);
//     npcs.Remove(closestNPC);
// }
