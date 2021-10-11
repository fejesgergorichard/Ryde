using System;
using UnityEngine;

public class MovingBlockTriggerArea : TriggerAreaBase
{
    public override Action<int> InvokedEnterEvent => GameEvents.Instance.MovingBlockTriggerEnter;
    public override Action<int> InvokedExitEvent => GameEvents.Instance.MovingBlockTriggerExit;

    public override Action<Collider> ActionOnEnterEvent => EnterAction;
    public override Action<Collider> ActionOnExitEvent => ExitAction;

    private void EnterAction(Collider other)
    {
        if (other.tag == "Player")
        {
            // Set the parent of the player to our parent
            other.transform.parent.transform.parent = transform.parent;
        }
    }
    
    private void ExitAction(Collider other)
    {
        if (other.tag == "Player")
        {
            // Reset the player parent
            other.transform.parent.transform.parent = null;
        }
    }
}

