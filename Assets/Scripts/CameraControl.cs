using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    public GameObject Target;
    public float Offset;

    public GameObject sphere;
    public LayerMask mylayermask;

    ////private Seethrough currentlyTransparent;

    //private List<KeyValuePair<Seethrough, bool>> trackedObjects = new List<KeyValuePair<Seethrough, bool>>()
    //                                                        {
    //    new KeyValuePair<Seethrough, bool>(null, false),
    //    new KeyValuePair<Seethrough, bool>(null, false)
    //};

    public bool RotateCamera { get; set; }

    void Start()
    {
        Target = GameObject.Find("Player");
        transform.LookAt(Target.transform);
    }
    
    void Update()
    {
        if (Target != null)
        {
            transform.LookAt(Target.transform);
        }

        if (RotateCamera && !PauseMenu.GameIsPaused)
        {
            Vector3 newRotation = new Vector3(transform.rotation.eulerAngles.x,
                                               transform.rotation.eulerAngles.y,
                                               transform.rotation.eulerAngles.z + (MobileInput.GyroRotation.eulerAngles.z - MobileInput.InitialGyroRotation.eulerAngles.z));
            transform.rotation = Quaternion.Euler(newRotation);
        }

        Vector3 direction = sphere.transform.position - transform.position;
        float length = Vector3.Distance(sphere.transform.position, transform.position);

        Debug.DrawRay(transform.position, direction * length, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(transform.position,
            direction,
            out hit, length, mylayermask))
        {
            if (hit.collider.gameObject.tag == "Spheremask")
            {
                sphere.transform.LeanScale(new Vector3(0,0,0), 1f);
            }
            else
            {
                sphere.transform.LeanScale(new Vector3(6, 6, 6), 1f);
            }
        }


    }

    //private void FixedUpdate()
    //{
    //    RayCastPlayerWithOffset(Offset, 0);
    //    RayCastPlayerWithOffset(-Offset, 1);
    //}

    //private void RayCastPlayerWithOffset(float offset, int index)
    //{
    //    Vector3 direction = (Target.transform.position + (offset * Target.transform.forward)) - transform.position;
    //    float length = Vector3.Distance(Target.transform.position, transform.position);

    //    Debug.DrawRay(transform.position, direction * length, Color.red);

    //    RaycastHit currentHit;
    //    if (Physics.Raycast(transform.position, direction, out currentHit, length, LayerMask.GetMask("Default")))
    //    {
    //        Seethrough objectHit = currentHit.transform.GetComponent<Seethrough>();
    //        if (objectHit) // if we hit an object with a  Seethrough script attached
    //        {
    //            // when we hit a different object than before
    //            if (trackedObjects[index].Key && trackedObjects[index].Key.gameObject != objectHit.gameObject)
    //            {
    //                trackedObjects[index] = new KeyValuePair<Seethrough, bool>(objectHit, false);
    //                DisableIfAllIsDisabled(index);
    //                //trackedObjects[index].Key.ChangeTransparency(false);
    //            }
    //            objectHit.ChangeTransparency(true);
    //            trackedObjects[index] = new KeyValuePair<Seethrough, bool>(objectHit, true);
    //        }
    //    }
    //    else
    //    {
    //        //If nothing is hit and there is a previous object hit
    //        if (trackedObjects[index].Key)
    //        {
    //            trackedObjects[index] = new KeyValuePair<Seethrough, bool>(trackedObjects[index].Key, false);
    //            DisableIfAllIsDisabled(index);
    //            //trackedObjects[index].Key.ChangeTransparency(false);
    //        }
    //    }
    //}

    //private void DisableIfAllIsDisabled(int index)
    //{
    //    if (trackedObjects[0].Key != trackedObjects[1].Key ||
    //        ((trackedObjects[0].Key == trackedObjects[1].Key) && (!trackedObjects[0].Value && !trackedObjects[1].Value)))
    //    {
    //        trackedObjects[index].Key.ChangeTransparency(false);
    //        trackedObjects[index] = new KeyValuePair<Seethrough, bool>(null, false);
    //    }
    //}
}
