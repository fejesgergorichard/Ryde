using System;
using System.Collections;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    private readonly Vector3 _defaultGravity = new Vector3(0, -9.81f, 0);

    public Vector3 NewGravityDirection = new Vector3(0, 9.81f, 0);

    private void Awake()
    {
        GameEvents.Instance.onGravityTriggerEnter += EnterAction;
        GameEvents.Instance.onGravityTriggerExit += ExitAction;
    }

    private void Start()
    {
        Physics.gravity = _defaultGravity;
    }

    private void EnterAction()
    {
        Physics.gravity = NewGravityDirection;
        StartCoroutine(RotateCamera(180, -2, 1));
        //StartCoroutine(RotatePlayer(180, 0.3f, 0.3f));
        RotatePlayer(180, 0.5f, 0.3f);
    }

    private void ExitAction()
    {
        Physics.gravity = _defaultGravity;
        StartCoroutine(RotateCamera(0, 0, 1));
        //StartCoroutine(RotatePlayer(0, 0.3f, 0.3f));
        RotatePlayer(0, 0.5f, 0.3f);
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
    }

    private void RotatePlayer(float targetRotation, float duration, float delay = 0f)
    {
        var player = GameObject.Find("Player");

        Vector3 newRotation = new Vector3(targetRotation, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z);
        LeanTween.rotate(player, newRotation, duration).setDelay(delay);
    }

    //private IEnumerator RotatePlayer(float targetRotation, float duration, float delay = 0f)
    //{
    //    var player = GameObject.Find("Player");

    //    float startRotation = player.transform.rotation.eulerAngles.x;
    //    float time = 0;

    //    while (time < delay)
    //    {
    //        time += Time.deltaTime;
    //        yield return null;
    //    }

    //    time = 0;

    //    while (time < duration)
    //    {
    //        float x = Mathf.Lerp(startRotation, targetRotation, time / duration);
    //        float rotSpeed = targetRotation / duration;

    //        //Vector3 newRotation = new Vector3(x, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z);
    //        //player.transform.rotation = Quaternion.Euler(newRotation);
    //        player.transform.Rotate(rotSpeed, 0, 0);

    //        time += Time.deltaTime;
    //        yield return null;
    //    }
    //}
}

