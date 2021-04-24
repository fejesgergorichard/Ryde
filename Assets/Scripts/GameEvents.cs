using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public event Action onMovingBlockTriggerEnter;
    public void MovingBlockTriggerEnter()
    {
        if (onMovingBlockTriggerEnter != null)
        {
            onMovingBlockTriggerEnter();
        }

    }
}
