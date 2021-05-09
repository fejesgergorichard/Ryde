using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public GameObject ParticlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        GameEvents.Instance.CrystalTriggerEnter();
        var particle = Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
    }
}
