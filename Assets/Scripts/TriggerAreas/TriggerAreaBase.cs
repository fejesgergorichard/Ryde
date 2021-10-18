using System;
using UnityEngine;

public abstract class TriggerAreaBase : MonoBehaviour
{
    protected int id;

    public abstract Action<Collider> ActionOnEnterEvent { get; protected set; }
    public abstract Action<Collider> ActionOnExitEvent { get; protected set; }

    private void Awake()
    {
        id = transform.parent.GetComponent<MovingCubeController>().Id;
    }

    public void OnTriggerEnter(Collider other)
    {
        ActionOnEnterEvent?.Invoke(other);
    }

    public void OnTriggerExit(Collider other)
    {
        ActionOnExitEvent?.Invoke(other);
    }
}
