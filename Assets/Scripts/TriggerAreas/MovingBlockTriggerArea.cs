using System;
using UnityEngine;

public class MovingBlockTriggerArea : TriggerAreaBase
{
    private Action<int> _enterEvent => GameEvents.Instance.MovingBlockTriggerEnter;
    private Action<int> _exitEvent => GameEvents.Instance.MovingBlockTriggerExit;

    public override Action<Collider> ActionOnEnterEvent => EnterAction;
    public override Action<Collider> ActionOnExitEvent => ExitAction;

    private void EnterAction(Collider other)
    {
        if (other.tag == "Player")
        {
            // Set the parent of the player to our parent
            other.transform.parent.transform.parent = transform.parent;
            _enterEvent?.Invoke(id);
        }
    }
    
    private void ExitAction(Collider other)
    {
        if (other.tag == "Player")
        {
            // Reset the player parent
            other.transform.parent.transform.parent = null;
            _exitEvent?.Invoke(id);
        }
    }
}

