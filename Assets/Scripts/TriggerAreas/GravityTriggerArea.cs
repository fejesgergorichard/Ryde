using System;
using UnityEngine;

public class GravityTriggerArea : TriggerAreaBase
{
    public GravityTriggerType GravityTriggerType = GravityTriggerType.Enter;

    private Action<Collider> _actionOnEnterEvent;
    public override Action<Collider> ActionOnEnterEvent
    {
        get => _actionOnEnterEvent;
        protected set => _actionOnEnterEvent = value;
    }

    private Action<Collider> _actionOnExitEvent;
    public override Action<Collider> ActionOnExitEvent
    {
        get => _actionOnExitEvent;
        protected set => _actionOnExitEvent = value;
    }

    private void Awake()
    {
        switch (GravityTriggerType)
        {
            case GravityTriggerType.Enter:
                ActionOnEnterEvent = EnterAction;
                ActionOnExitEvent = null;
                break;
            case GravityTriggerType.Exit:
                ActionOnEnterEvent = ExitAction;
                ActionOnExitEvent = null;
                break;
            default:
                ActionOnEnterEvent = null;
                ActionOnExitEvent = null;
                break;
        }
    }

    private void EnterAction(Collider other)
    {
        if (other.tag == "Player")
        {
            GameEvents.Instance.GravityTriggerEnter();
        }
    }

    private void ExitAction(Collider other)
    {
        if (other.tag == "Player")
        {
            GameEvents.Instance.GravityTriggerExit();
        }
    }
}

public enum GravityTriggerType
{
    Enter,
    Exit
}
