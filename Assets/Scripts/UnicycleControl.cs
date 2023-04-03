using System;
using UnityEngine;

public class UnicycleControl : MonoBehaviour
{
    public float leanHeightOffset;
    public float leanSpeed;
    public float leanCorrectionSpeed;
    public float turnSpeed;
    public float minLeanTurnAmount;
    public float wheelRadius;
    public float gravityMultiplier;
    public float rotationOffset;
    public float moveForceMultiplier;

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
        cam = GameObject.Find("Main Camera").transform;
    }

    void FixedUpdate()
    {
        Lean();
        DontFallOver();
        Turn();
        SpinWheel();
        SpinPedals();
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
            var force = leanDir.normalized * leanSpeed * rb.mass;
            var forcePosition = transform.position + Vector3.up * leanHeightOffset;
            Debug.DrawLine(forcePosition, forcePosition+force, Color.red);
            rb.AddForceAtPosition(force, forcePosition);
            
            
            rb.AddForce(force);
        }
        else
        {
            leanDir = Vector3.zero;
        }
    }

    void DontFallOver()
    {
        // az az irány amerre a fej dől a kerékhez képest, függőleges komponens nélkül
        leanOffsetDir = headRef.position - wheelRef.position;
        leanOffsetDir.y = 0;
        Debug.DrawLine(headRef.position, headRef.position + leanOffsetDir, Color.yellow);
        var force = leanOffsetDir * -leanCorrectionSpeed;
        var forcePosition = wheelRef.position + Vector3.up * 2;
        // var forcePosition = transform.position;
        rb.AddForceAtPosition(force, forcePosition);
        Debug.DrawLine(forcePosition, forcePosition + force, Color.magenta);
        var actualRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(actualRotation.eulerAngles.x, 0, actualRotation.eulerAngles.z);

        if (headRef.position.y < wheel.position.y)
        {
            transform.rotation = Quaternion.identity;
        }
        
        rb.AddForceAtPosition(force * moveForceMultiplier, transform.position);
        Debug.DrawLine(transform.position, transform.position + force * -1, Color.blue);
    }

    void Turn()
    {
        // if (overrideTurn != Vector3.zero)
        // {
        //     leanRotation = transform.rotation * Quaternion.LookRotation(overrideTurn);
        //     newRot.y = Quaternion.Slerp(unicyclePivot.transform.localRotation, leanRotation,
        //         turnSpeed * Time.deltaTime).y;
        //     unicyclePivot.transform.localRotation = Quaternion.Euler(newRot);
        // }
        // else
        if (leanOffsetDir.magnitude > minLeanTurnAmount)
        {
            leanRotation = transform.rotation * Quaternion.LookRotation(leanOffsetDir);
            var y = Quaternion.Slerp(unicyclePivot.transform.localRotation, leanRotation,
                    turnSpeed * Time.deltaTime).eulerAngles.y;

            unicyclePivot.transform.localRotation = Quaternion.Euler(unicyclePivot.transform.localRotation.eulerAngles.x, y, unicyclePivot.transform.localRotation.eulerAngles.z)
                                                    * Quaternion.Euler(0.0f, rotationOffset, 0.0f);
        }
    }

    void SpinWheel()
    {
        flattenedForward = unicyclePivot.forward;
        flattenedForward.y = 0;
        flattenedVelocity = rb.velocity;
        flattenedVelocity.y = 0;

        if (Vector3.Angle(leanOffsetDir, flattenedForward) > 90)
        {
            wheelAngle -= flattenedVelocity.magnitude / (2 * Mathf.PI * wheelRadius);
        }
        else
        {
            wheelAngle += flattenedVelocity.magnitude / (2 * Mathf.PI * wheelRadius);
        }
        wheel.localRotation = Quaternion.Euler(0.0f, 0.0f, wheelAngle);
    }

    void SpinPedals()
    {
        leftPedal.localRotation = Quaternion.Euler(0.0f, 0.0f, wheelAngle + 90);
        rightPedal.localRotation = Quaternion.Euler(0.0f, 0.0f, wheelAngle + 90);
    }

    void Fall()
    {
        rb.AddForce(Physics.gravity * (gravityMultiplier + 1));
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawSphere(wheelRef.position, wheelRadius);
    // }
}