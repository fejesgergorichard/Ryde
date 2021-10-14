using System;
using System.Collections;
using UnityEngine;

public class GravityTriggerArea : TriggerAreaBase
{
    public override Action<Collider> ActionOnEnterEvent => EnterAction;
    public override Action<Collider> ActionOnExitEvent => null;

    private void EnterAction(Collider other)
    {
        if (other.tag == "Player")
        {
            GameEvents.Instance.GravityTriggerEnter();
            Physics.gravity = new Vector3(0, 9.81f, 0);
            FlipCamera();
            StartCoroutine(RotatePlayer(180, 0.3f, 0.3f));
        }
    }
    
    private void ExitAction(Collider other)
    {
        if (other.tag == "Player")
        {
            GameEvents.Instance.GravityTriggerExit();
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
    }

    private void FlipCamera()
    {
        StartCoroutine(RotateCamera(180, -2, 1));
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

    private IEnumerator RotatePlayer(float targetRotation, float duration, float delay = 0f)
    {
        var player = GameObject.Find("Player");

        float startRotation = player.transform.rotation.eulerAngles.x;
        float time = 0; 
        
        while (time < delay)
        {
            time += Time.deltaTime;
            yield return null;
        }

        time = 0;

        while (time < duration)
        {
            float x = Mathf.Lerp(startRotation, targetRotation, time / duration);

            Vector3 newRotation = new Vector3(x, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z);
            player.transform.rotation = Quaternion.Euler(newRotation);

            time += Time.deltaTime;
            yield return null;
        }
    }
}

