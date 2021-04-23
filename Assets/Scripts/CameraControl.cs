using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject Target;
    public float rotation;
    private bool rotateCamera;
    public bool RotateCamera
    {
        get => rotateCamera;
        set => rotateCamera = value;
    }

    //public Vector3 result;


    void Start()
    {
        QualitySettings.vSyncCount = 0;
        QualitySettings.antiAliasing = 0;
        Application.targetFrameRate = 60;

        transform.LookAt(Target.transform);
    }

    void Update()
    {
        transform.LookAt(Target.transform);

        if (RotateCamera)
        {
            Vector3 newRotation = new Vector3(transform.rotation.eulerAngles.x,
                                               transform.rotation.eulerAngles.y,
                                               transform.rotation.eulerAngles.z - DeviceTiltAngle.Get());
            transform.rotation = Quaternion.Euler(newRotation);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position + result);
    //}
}
