using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    //private Quaternion initialDeviceRotation;
    private Vector3 initialDeviceRotation;


    public GameObject Target;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(Target.transform);
        Input.gyro.enabled = true;
        initialDeviceRotation = DeviceRotation.Get().eulerAngles;
        

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deviceRotation = DeviceRotation.Get().eulerAngles;

        transform.LookAt(Target.transform);

        Vector3 newRotation = new Vector3(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z + (deviceRotation.z - initialDeviceRotation.z)
            );

        transform.rotation = Quaternion.Euler(newRotation);
        //transform.rotation = Quaternion.Euler(deviceRotation);
    }
}
