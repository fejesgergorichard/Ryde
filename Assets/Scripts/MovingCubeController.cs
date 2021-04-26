using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingCubeController : MonoBehaviour
{
    private float initialYPosition;
    private LTSpline spline;

    public int Id;
    public float TargetDistanceY;
    public List<Transform> Points;
    
    private void Start()
    {
        // select every transform.position from the List<Transform> Points
        spline = new LTSpline(Points.Select(transform => transform.position).ToArray());

        LeanTween.moveSpline(gameObject, spline.pts, 16f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setOrientToPath(false);


        GameEvents.Instance.onMovingBlockTriggerEnter += OnMovingBlockTriggerEnter;
        GameEvents.Instance.onMovingBlockTriggerExit += OnMovingBlockTriggerExit;
        initialYPosition = transform.position.y;
    }

    private void OnMovingBlockTriggerEnter(int id)
    {

        if (id == Id)
        {
            LeanTween.cancel(gameObject);

            LeanTween.moveY(gameObject, initialYPosition + TargetDistanceY, 5f).setEaseOutQuad().setDelay(1.5f);
            LeanTween.moveLocal(gameObject, new Vector3(-8.5f, -1.6f, 4.94f), 7f).setEaseInQuad().setEaseOutQuad().setDelay(6.7f);
        }
    }

    private void OnMovingBlockTriggerExit(int id)
    {
        if (id == Id)
        {
            LeanTween.cancel(gameObject);

            LeanTween.moveY(gameObject, initialYPosition, 5f).setEaseOutQuad().setDelay(1f);
            //LeanTween.moveX(gameObject, -8f, 8f).setEaseOutQuad();
        }
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onMovingBlockTriggerEnter -= OnMovingBlockTriggerEnter;
        GameEvents.Instance.onMovingBlockTriggerExit -= OnMovingBlockTriggerExit;
    }

    void OnDrawGizmos()
    {
        if (spline != null)
        {
            Gizmos.color = Color.red;
            spline.gizmoDraw(); // debug aid to be able to see the path in the scene inspector
        }
    }
}
