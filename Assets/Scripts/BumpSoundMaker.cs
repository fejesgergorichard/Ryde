using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpSoundMaker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.ToLower().Contains("podium")
            && !other.name.ToLower().Contains("coin")
            && !other.name.ToLower().Contains("crystal"))
        {
            AudioManager.PlaySuspensionSound();
        }
    }
}
