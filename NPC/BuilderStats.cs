using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderStats : MonoBehaviour
{
    public List<(GameObject, WonderStage)> BuildList { get { return _objectsToBuild; } }
    List<(GameObject, WonderStage)> _objectsToBuild = new List<(GameObject, WonderStage)>();
    public bool HasResource { get { return _objectsToBuild.Count != 0; } }
    public Vector3 PosToMove;

    // int _maxExhaustion = 5;
    // int _currentExhaustion = 0;
    // public void IncreaseExhaustion(int change)
    // {
    //     _currentExhaustion += change;
    //     if (_currentExhaustion < _maxExhaustion) return;
    //     _currentExhaustion = 0;
    // }
    public void SetList(List<(GameObject, WonderStage)> objs)
    {
        _objectsToBuild = objs;
    }

    public void AddToList((GameObject, WonderStage) stageObject)
    {
        if (stageObject != (null, null))
            _objectsToBuild.Add(stageObject);
    }
    public (GameObject, WonderStage) GetItem()
    {
        (GameObject, WonderStage) retObject = _objectsToBuild[_objectsToBuild.Count - 1];
        return retObject;
    }
    public (GameObject, WonderStage) GetItemAndRemove()
    {
        (GameObject, WonderStage) retObject = _objectsToBuild[_objectsToBuild.Count - 1];
        _objectsToBuild.Remove(retObject);
        return retObject;
    }
}
