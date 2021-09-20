using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject Target;

    private Seethrough currentTransparentObject;
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
        //Calculate the Vector direction 
        Vector3 direction = Target.transform.position - transform.position;
        //Calculate the length
        float length = Vector3.Distance(Target.transform.position, transform.position);
        //Draw the ray in the debug
        Debug.DrawRay(transform.position, direction * length, Color.red);
        //The first object hit reference
        RaycastHit currentHit;
        //Cast the ray and report the firt object hit filtering by "Wall" layer mask
        if (Physics.Raycast(transform.position, direction, out currentHit, length, LayerMask.GetMask("Default")))
        {
            //Getting the script to change transparency of the hit object
            Seethrough transparentWall = currentHit.transform.GetComponent<Seethrough>();
            //If the object is not null
            if (transparentWall)
            {
                //If there is a previous wall hit and it's different from this one
                if (currentTransparentObject && currentTransparentObject.gameObject != transparentWall.gameObject)
                {
                    //Restore its transparency setting it not transparent
                    currentTransparentObject.ChangeTransparency(false);
                }
                //Change the object transparency in transparent.
                transparentWall.ChangeTransparency(true);
                currentTransparentObject = transparentWall;
            }
        }
        else
        {
            //If nothing is hit and there is a previous object hit
            if (currentTransparentObject)
            {
                //Restore its transparency setting it not transparent
                currentTransparentObject.ChangeTransparency(false);
            }
        }
    }
}
