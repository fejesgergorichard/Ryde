using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelPieceBehaviourAssigner : MonoBehaviour
{
    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                child.gameObject.AddComponent<VoxelPieceBehaviour>();
            }
        }
    }
}
