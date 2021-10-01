using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public Vector3 PullBox;
    public Vector3 FlipBox;
    public float PullForce = 600000;
    public bool FlipEntryPoint = false;
    public float CameraRotationSpeed = 1f;

    private bool _flipped = false;

    private void FixedUpdate()
    {
        foreach (Collider collider in Physics.OverlapBox(transform.position, PullBox/2, Quaternion.identity))
        {
            if (collider.tag == "Player")
            {
                Vector3 forceDirection = transform.position - collider.transform.position;
                Debug.DrawRay(transform.position, -forceDirection * 4, Color.green);

                // Sucks the player towards the centre.
                //collider.transform.parent.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * PullForce * Time.fixedDeltaTime);
                // This creates a force field that pushes the player back
                //collider.transform.parent.GetComponent<Rigidbody>().AddForce(transform.right * PullForce * Time.fixedDeltaTime);
                collider.transform.parent.GetComponent<Rigidbody>().AddForce(Vector3.up * PullForce * Time.fixedDeltaTime);
            }
        }
        
        foreach (Collider collider in Physics.OverlapBox(transform.position, FlipBox/2, Quaternion.identity))
        {
            if (collider.tag == "Player")
            {
                Vector3 forceDirection = transform.position - collider.transform.position;
                Debug.DrawRay(transform.position, -forceDirection * 2, Color.blue);

                if (!_flipped && FlipEntryPoint)
                {
                    var rb = collider.transform.parent.GetComponent<Rigidbody>();
                    rb.AddRelativeTorque(new Vector3(0.5f * rb.mass, 0, 0/*5 * rb.mass*/), ForceMode.Impulse);
                    _flipped = true;
                    FlipCamera();
                }
            }
        }
    }

    private void FlipCamera()
    {
        StartCoroutine(RotateCamera(180,-2, 1));
    }

    private IEnumerator RotateCamera(float targetRotation, float targetYPosition, float duration)
    {
        var cam = GameObject.Find("Main Camera");
        var cameraControl = cam.GetComponent<CameraControl>();

        float startZRotation = cam.transform.rotation.eulerAngles.z;
        float startYPos = cam.transform.position.y;
        float time = 0;

        while (time < duration)
        {
            cameraControl.ZRotationOffset = Mathf.Lerp(startZRotation, targetRotation, time / duration);
            cameraControl.YPositionOffset = Mathf.Lerp(startYPos, targetYPosition, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        cameraControl.ZRotationOffset = 180;
        cameraControl.YPositionOffset = targetYPosition;
        
        //cam.transform.position = new Vector3(cam.transform.position.x,
        //targetYPosition,
        //cam.transform.position.z);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, PullBox);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, FlipBox);
    }
}