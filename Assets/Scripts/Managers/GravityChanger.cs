using System;
using System.Collections;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    private readonly Vector3 _defaultGravity = new Vector3(0, -9.81f, 0);
    private float _initialCameraY;

    public Vector3 NewGravityDirection = new Vector3(0, 9.81f, 0);
    public float TargetCameraY;

    private void Start()
    {
        GameEvents.Instance.onGravityTriggerEnter += EnterAction;
        GameEvents.Instance.onGravityTriggerExit += ExitAction;
        Physics.gravity = _defaultGravity;
        var cam = GameObject.Find("Main Camera");
        _initialCameraY = cam.transform.position.y;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onGravityTriggerEnter -= EnterAction;
        GameEvents.Instance.onGravityTriggerExit -= ExitAction;
        Physics.gravity = _defaultGravity;
    }

    private void EnterAction()
    {
        Physics.gravity = NewGravityDirection;
        StartCoroutine(RepositionCamera(180, TargetCameraY, 1f));
        RotatePlayer(180, 0.5f, 0.3f);
    }

    private void ExitAction()
    {
        Physics.gravity = _defaultGravity;
        StartCoroutine(RepositionCamera(0, _initialCameraY, 1f));
        RotatePlayer(0, 0.5f, 0.3f);
    }

    private IEnumerator RepositionCamera(float targetRotation, float targetYPosition, float duration)
    {
        var cam = GameObject.Find("Main Camera");
        var cameraControl = cam.GetComponent<CameraControl>();

        float startZRotation = cam.transform.rotation.eulerAngles.z;
        float startYPos = cam.transform.position.y;
        float time = 0;

        while (time < duration)
        {
            cameraControl.TargetZRotation = Mathf.Lerp(startZRotation, targetRotation, time / duration);
            cameraControl.TargetYPosition = Mathf.Lerp(startYPos, targetYPosition, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        cameraControl.TargetZRotation = 180;
        cameraControl.TargetYPosition = targetYPosition;
    }

    private void RotatePlayer(float targetRotation, float duration, float delay = 0f)
    {
        var player = GameObject.Find("Player");

        Vector3 newRotation = new Vector3(targetRotation, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z);
        LeanTween.rotate(player, newRotation, duration).setDelay(delay);
    }
}

