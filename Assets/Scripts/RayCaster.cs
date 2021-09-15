using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    public int ForceMultiplier = 2;
    [Range(0, 200)]
    public int AddForceTimeout;
    private DateTime forceApplyTime;
    private Ray ray;
    private RaycastHit hit;

    private void Start()
    {
        forceApplyTime = DateTime.Now;
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                ApplyForce();
            }
        }
    }

    private void ApplyForce()
    {
        string name = hit.collider.name;
        if (name == "CarBody")
        {
            if (DateTime.Compare(forceApplyTime.AddMilliseconds(AddForceTimeout), DateTime.Now) < 0)
            {
                forceApplyTime = DateTime.Now;
                var mass = hit.collider.attachedRigidbody.mass;
                Vector3 upwardsForce = new Vector3(0, ForceMultiplier * mass, 0);
                hit.collider.attachedRigidbody.AddForceAtPosition(upwardsForce, hit.point);
            }
        }
    }
}
