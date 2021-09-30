using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    public GameObject Target;

    [Range(-1f, 1f)]
    public float UpwardsOffset;
    public List<float> Offsets;

    private Seethrough[] trackedObjects;
    private bool[] trackedTransparencies;

    public bool RotateCamera { get; set; }

    public float ZRotationOffset { get; set; }

    void Start()
    {
        Target = GameObject.Find("Player");
        transform.LookAt(Target.transform);
        ZRotationOffset = 0f;

        trackedObjects = new Seethrough[Offsets.Count];
        trackedTransparencies = new bool[Offsets.Count];
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

        if (ZRotationOffset != 0)
        {
            Vector3 newRotation = new Vector3(transform.rotation.eulerAngles.x,
                                               transform.rotation.eulerAngles.y,
                                               ZRotationOffset);

            Vector3 actualRotation = Vector3.Slerp(transform.rotation.eulerAngles, newRotation, 1f * Time.deltaTime);

            transform.rotation = Quaternion.Euler(actualRotation);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < Offsets.Count; i++)
        {
            RayCast(Offsets[i], i);
        }
    }

    private void RayCast(float offset, int i)
    {
        Vector3 direction = (Target.transform.position + (offset * Target.transform.forward) + (UpwardsOffset * Target.transform.up)) - transform.position;
        float length = Vector3.Distance(Target.transform.position, transform.position);

        //Debug.DrawRay(transform.position, direction * length, Color.red);

        RaycastHit currentHit;
        if (Physics.Raycast(transform.position, direction, out currentHit, length, LayerMask.GetMask("Default")))
        {
            Seethrough hitSeethrough = currentHit.transform.GetComponent<Seethrough>();
            if (hitSeethrough) // if its not null
            {
                // when we hit a different object
                if (trackedObjects[i] && trackedObjects[i].gameObject != hitSeethrough.gameObject)
                {
                    trackedTransparencies[i] = false;
                    TryChangeTransparency(i, false);
                }

                trackedObjects[i] = hitSeethrough;
                trackedTransparencies[i] = true;
                TryChangeTransparency(i, true);
            }
            // If the hit object is not seethrough
            else
            {
                if (trackedObjects[i] != null)
                {
                    trackedTransparencies[i] = false;
                    TryChangeTransparency(i, false);
                    trackedObjects[i] = null;
                }
            }
        }
        // If we did not hit anything
        else
        {
            if (trackedObjects[i] != null)
            {
                trackedTransparencies[i] = false;
                TryChangeTransparency(i, false);
                trackedObjects[i] = null;   
            }
        }
    }

    public void TryChangeTransparency(int i, bool value)
    {
        List<KeyValuePair<Seethrough, bool>> transparencyMap = CreateTransparencyMap();

        if (trackedObjects[i] != null)
        {
            var trackedCopies = transparencyMap.Where(x => x.Key == trackedObjects[i]);

            if (trackedCopies.All(x => x.Value == false) && value == false)
                trackedObjects[i].ChangeTransparency(false);

            else if (value)
                trackedObjects[i].ChangeTransparency(true);
        }
    }

    private List<KeyValuePair<Seethrough, bool>> CreateTransparencyMap()
    {
        var transparencyMap = new List<KeyValuePair<Seethrough, bool>>();

        for (int j = 0; j < trackedObjects.Length; j++)
        {
            transparencyMap.Add(new KeyValuePair<Seethrough, bool>(trackedObjects[j], trackedTransparencies[j]));
        }

        return transparencyMap;
    }
}
