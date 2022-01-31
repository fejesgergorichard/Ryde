using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelPieceBehaviour : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "VoxelPiece")
        {
            AudioManager.PlaySound($"Bell{Random.Range(0,12)}");
        }
    }
}
