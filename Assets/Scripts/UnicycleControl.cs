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
    public float moveForceMultiplier;

    public Transform headRef;
    public Transform wheelRef;
    public Transform wheel;
    public Transform cam;
    public Transform unicyclePivot;
    public Transform leftPedal;
    public Transform rightPedal;

    private float wheelAngle;
    private Vector3 leanOffsetDir;
    private Quaternion leanRotation;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GameObject.Find("Main Camera").transform;
    }

    void FixedUpdate()
    {
        LeanAxis();
        CalculateLeanOffsetDir();
        DontFallOver();
        Turn();
        SpinWheelAndPedals();
        Fall();
    }

    private void CalculateLeanOffsetDir()
    {
        // Calculate the direction of leaning from the position of the head and the wheelbase without Y component
        leanOffsetDir = headRef.position - wheelRef.position;
        leanOffsetDir.y = 0;
    }

    void LeanAxis()
    {
        var x = Input.GetAxis("Vertical");
        x = (Math.Abs(x) > 0.3f) ? 0.3f * Math.Sign(x) : x;
        
        var z = Input.GetAxis("Horizontal");
        z = (Math.Abs(z) > 0.3f) ? 0.3f * Math.Sign(z) : z;
        
        var leanDir = new Vector3(Input.GetAxis("Vertical"), 
            0f,
            Input.GetAxis("Horizontal"));
        
        leanDir = leanDir.x * cam.forward + leanDir.z * cam.right;
        leanDir.y = 0;
        var force = MassEffectiveForce(leanDir.normalized * leanSpeed);
        var forcePosition = transform.position + Vector3.up * leanHeightOffset;
        
        rb.AddForceAtPosition(force, forcePosition);
        rb.AddForce(force);

        Debug.DrawLine(forcePosition, forcePosition+force, Color.red);
    }

    void DontFallOver()
    {
        Debug.DrawLine(headRef.position, headRef.position + leanOffsetDir, Color.yellow);
        var force = leanOffsetDir * -leanCorrectionSpeed;
        var forcePosition = wheelRef.position + Vector3.up * 2;
        
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
        Debug.DrawLine(unicyclePivot.transform.position, unicyclePivot.transform.position + unicyclePivot.forward * 5f, Color.black);
        
        if (leanOffsetDir.magnitude > minLeanTurnAmount)
        {
            // Slerp unicyclePivot's y rotation towards the velocity vector
            leanRotation = Quaternion.LookRotation(rb.velocity.normalized, unicyclePivot.up);
            var localRotation = unicyclePivot.transform.localRotation;

            float newY = Quaternion.Slerp(localRotation, leanRotation, turnSpeed * Time.deltaTime).eulerAngles.y;

            unicyclePivot.transform.localRotation = Quaternion.Euler(
                localRotation.eulerAngles.x,
                newY,
                localRotation.eulerAngles.z);
        }
    }

    void SpinWheelAndPedals()
    {
        var flattenedForward = unicyclePivot.forward;
        flattenedForward.y = 0;
        var flattenedVelocity = rb.velocity;
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
        leftPedal.localRotation = Quaternion.Euler(0.0f, 0.0f, wheelAngle + 90);
        rightPedal.localRotation = Quaternion.Euler(0.0f, 0.0f, wheelAngle + 90);
    }

    void Fall()
    {
        rb.AddForce(Physics.gravity * (gravityMultiplier + 1));
    }

    private Vector3 MassEffectiveForce(Vector3 force)
    {
        return force * rb.mass;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawSphere(wheelRef.position, wheelRadius);
    // }
}