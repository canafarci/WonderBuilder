using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : StateMachineAction
{
    protected override void Enter()
    {
        print("Resting...");
    }
    public override IEnumerator Execute()
    {
        Enter();
        yield return new WaitForSeconds(3f);
        Exit();
    }
    protected override void Exit()
    {
        print("Finished Resting...");
    }
}
