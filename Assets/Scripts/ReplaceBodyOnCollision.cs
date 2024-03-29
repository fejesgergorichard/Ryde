﻿using System.Collections.Generic;
using UnityEngine;

public class ReplaceBodyOnCollision : MonoBehaviour
{
    private bool _isBodyReplaced = false;

    public List<string> TagsToBreakTo = new List<string>() { "Player" };
    public GameObject NormalBody;
    public GameObject ReplacementBody;
    public float VelocityConstraint = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (TagsToBreakTo.Contains(collision.collider.tag))
        {
            Debug.Log(collision.relativeVelocity.magnitude);
            if (collision.relativeVelocity.magnitude > VelocityConstraint)
            {
                if (!_isBodyReplaced)
                {
                    NormalBody.SetActive(false);
                    ReplacementBody.SetActive(true);
                    if (gameObject.GetComponent<Tilter>())
                    {
                        gameObject.GetComponent<Tilter>().enabled = false;
                    }
                }
            }
        }
    }
}
