using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    public int ForceMultiplier = 2;
    Ray ray;
    RaycastHit hit;

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                string name = hit.collider.name;
                Debug.Log(name);
                if (name == "CarBody")
                {
                    var mass = hit.collider.attachedRigidbody.mass;
                    Vector3 upwardsForce = new Vector3(0, ForceMultiplier * mass, 0);
                    hit.collider.attachedRigidbody.AddForceAtPosition(upwardsForce, hit.point);
                }
            }
        }
    }
}
