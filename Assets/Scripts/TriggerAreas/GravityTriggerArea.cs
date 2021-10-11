using System;
using UnityEngine;

public class GravityTriggerArea : TriggerAreaBase
{
    public override Action<Collider> ActionOnEnterEvent => EnterAction;
    public override Action<Collider> ActionOnExitEvent => ExitAction;

    private void EnterAction(Collider other)
    {
        if (other.tag == "Player")
        {
            Physics.gravity = new Vector3(0, 9.81f, 0);
        }
    }
    
    private void ExitAction(Collider other)
    {
        if (other.tag == "Player")
        {
            //Physics.gravity = new Vector3(0, -9.81f, 0);
        }
    }
}

