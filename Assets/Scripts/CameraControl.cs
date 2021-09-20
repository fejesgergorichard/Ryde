using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    public GameObject Target;
    public float Offset;

    private Seethrough currentlyTransparent;
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
    }

    private void FixedUpdate()
    {
        RayCastPlayer();
        RayCastPlayerWithOffset(Offset);
        RayCastPlayerWithOffset(-Offset);
    }

    private void RayCastPlayerWithOffset(float offset)
    {
        Vector3 direction = (Target.transform.position + (offset * Target.transform.forward)) - transform.position;
        float length = Vector3.Distance(Target.transform.position, transform.position);

        Debug.DrawRay(transform.position, direction * length, Color.red);

        RaycastHit currentHit;
        if (Physics.Raycast(transform.position, direction, out currentHit, length, LayerMask.GetMask("Default")))
        {
            Seethrough seethroughInstance = currentHit.transform.GetComponent<Seethrough>();
            if (seethroughInstance) // if its not null
            {
                // when we hit a different object
                if (currentlyTransparent && currentlyTransparent.gameObject != seethroughInstance.gameObject)
                {
                    currentlyTransparent.ChangeTransparency(false);
                }
                seethroughInstance.ChangeTransparency(true);
                currentlyTransparent = seethroughInstance;
            }
        }
        else
        {
            //If nothing is hit and there is a previous object hit
            if (currentlyTransparent)
            {
                currentlyTransparent.ChangeTransparency(false);
            }
        }
    }

    private void RayCastPlayer()
    {
        Vector3 direction = Target.transform.position - transform.position;
        float length = Vector3.Distance(Target.transform.position, transform.position);

        Debug.DrawRay(transform.position, direction * length, Color.red);

        RaycastHit currentHit;
        if (Physics.Raycast(transform.position, direction, out currentHit, length, LayerMask.GetMask("Default")))
        {
            Seethrough seethroughInstance = currentHit.transform.GetComponent<Seethrough>();
            if (seethroughInstance) // if its not null
            {
                // when we hit a different object
                if (currentlyTransparent && currentlyTransparent.gameObject != seethroughInstance.gameObject)
                {
                    currentlyTransparent.ChangeTransparency(false);
                }
                seethroughInstance.ChangeTransparency(true);
                currentlyTransparent = seethroughInstance;
            }
        }
        else
        {
            //If nothing is hit and there is a previous object hit
            if (currentlyTransparent)
            {
                currentlyTransparent.ChangeTransparency(false);
            }
        }
    }
}
