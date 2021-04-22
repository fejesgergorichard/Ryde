using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceTiltAngle : MonoBehaviour
{
    private static bool gyroInitialized = false;
    private static Vector3 initialDeviceTilt;

    public static bool HasGyroscope
    {
        get
        {
            return SystemInfo.supportsGyroscope;
        }
    }

    public static float Get()
    {
        if (!gyroInitialized)
        {
            InitGyro();
        }

        return HasGyroscope
            ? ReadTiltAngle()
            : 0.0f;
    }

    private static void InitGyro()
    {
        if (HasGyroscope)
        {
            Input.gyro.enabled = true;                // enable the gyroscope
            Input.gyro.updateInterval = 0.0167f;      // set the update interval to it's highest value (60 Hz)
        }
        gyroInitialized = true;
    }

    public void Start()
    {
        initialDeviceTilt = Input.acceleration;
    }


    private static float ReadTiltAngle()
    {
        var currentDeviceTilt = Input.acceleration;
        return (currentDeviceTilt.x - initialDeviceTilt.x) * 90;
    }
}
