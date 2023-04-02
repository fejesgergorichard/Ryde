using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnicycleControl : MonoBehaviour
{
    public float leanHeightOffset;
    public float leanSpeed;
    public float leanCorrectionSpeed;
    public float turnSpeed;
    public float minLeanTurnAmount;
    public float wheelRadius;
    public float bobbleSpeedIdle;
    public float bobbleAmountIdle;
    public float bobbleSpeedMove;
    public float bobbleAmountMove;
    public float gravityMultiplier;

    [HideInInspector] public bool getInput;
    [HideInInspector] public Vector3 overrideTurn;

    public Transform headRef;
    public Transform wheelRef;
    public Transform wheel;
    public Transform cam;
    public Transform unicyclePivot;
    public Transform leftPedal;
    public Transform rightPedal;

    private float wheelAngle;
    private Vector3 leanDir;
    private Vector3 leanOffsetDir;
    private Vector3 newRot;
    private Quaternion leanRotation;
    private Vector3 flattenedForward;
    private Vector3 flattenedVelocity;
    private Rigidbody rb;

    void Start()
    {
        getInput = true;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Lean();
        DontFallOver();
        Turn();
        SpinWheel();
        CounterspinPedals();
        Bobble();
        Fall();
    }

    void Lean()
    {
        if (getInput)
        {
            if (Input.GetKey(KeyCode.W))
            {
                leanDir.x = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                leanDir.x = -1;
            }
            else
            {
                leanDir.x = 0;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                leanDir.z = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                leanDir.z = 1;
            }
            else
            {
                leanDir.z = 0;
            }

            leanDir = leanDir.x * cam.forward + leanDir.z * cam.right;
            leanDir.y = 0;
            rb.AddForceAtPosition(leanDir.normalized * leanSpeed, transform.position + Vector3.up * leanHeightOffset);
        }
        else
        {
            leanDir = Vector3.zero;
        }
    }

    void DontFallOver()
    {
        leanOffsetDir = headRef.position - wheelRef.position;
        leanOffsetDir.y = 0;
        rb.AddForceAtPosition(leanOffsetDir *leanCorrectionSpeed, transform.position);
        var actualRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(actualRotation.eulerAngles.x, 0, actualRotation.eulerAngles.z);

        if (headRef.position.y < wheel.position.y)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    void Turn()
    {
        if (overrideTurn != Vector3.zero)
        {
            leanRotation = transform.rotation * Quaternion.LookRotation(overrideTurn);
            newRot.y = Quaternion.Slerp(unicyclePivot.transform.localRotation, leanRotation,
                turnSpeed * Time.deltaTime).y;
            unicyclePivot.transform.localRotation = Quaternion.Euler(newRot);
        }
        else if (leanOffsetDir.magnitude> minLeanTurnAmount)
        {
            leanRotation = transform.rotation * Quaternion.LookRotation(leanOffsetDir);
        }
    }

    void SpinWheel()
    {
        
    }

    void CounterspinPedals()
    {
        leftPedal.localRotation = Quaternion.Euler(-wheelAngle + 90, 0.0f, 0.0f);
        rightPedal.localRotation = Quaternion.Euler(-wheelAngle + 90, 0.0f, 0.0f);
    }

    void Bobble()
    {
        
    }

    void Fall()
    {
        rb.AddForce(Physics.gravity * (gravityMultiplier + 1));
    }

}