using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PodiumRotator : MonoBehaviour
{
    public float RotateSpeed = 10f;
    public int StepsToKillVelocity = 100;

    private float touchPosLatest;
    private float touchPosNew;

    private Vector3 mousePosLatest;
    private Vector3 mousePosNew;

    private float velocity;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchPosLatest = touch.position.x;
                    break;
                case TouchPhase.Moved:
                    touchPosNew = touch.position.x;
                    velocity = touchPosNew - touchPosLatest;
                    touchPosLatest = touchPosNew;
                    break;
                case TouchPhase.Ended:
                    Task.Run(() => KillVelocity());
                    Debug.Log("Touch Phase Ended.");
                    break;
            }
        }

        // Mouse

        if (Input.GetMouseButtonDown(0))
        {
            mousePosLatest = Input.mousePosition;
        }

        else if (Input.GetMouseButton(0))
        {
            mousePosNew = Input.mousePosition;
            velocity = (mousePosNew - mousePosLatest).x;
            mousePosLatest = mousePosNew;
        }

        else if(Input.GetMouseButtonUp(0))
        {
            Task.Run(() => KillVelocity());
        }

        if (velocity != 0)
            transform.Rotate(Vector3.down, velocity * RotateSpeed * 0.001f);
    }

    private void KillVelocity()
    {
        var decreaseSpeed = velocity / StepsToKillVelocity;
        var steps = 0;

        while (velocity != 0)
        {
            if (steps == StepsToKillVelocity)
            {
                velocity = 0;
                return;
            }

            velocity -= decreaseSpeed;
            Thread.Sleep(20);
            steps++;
        }
    }
}
