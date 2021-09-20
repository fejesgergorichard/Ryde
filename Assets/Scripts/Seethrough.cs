using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seethrough : MonoBehaviour
{
    private bool transparent = false;
    private Renderer rend;

    private void Start()
    {
        //Get the renderer of the object
        rend = GetComponent<Renderer>();
        rend.material.EnableKeyword("_ALPHABLEND_ON");
        rend.material.EnableKeyword("_ALPHATEST_ON");
        rend.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");

    }

    public void ChangeTransparency(bool transparent)
    {
        //Avoid to set the same transparency twice
        if (this.transparent == transparent) return;

        //Set the new configuration
        this.transparent = transparent;


        if (transparent)
        {
            Material origi = rend.material;

            // Make transparent
            origi.SetFloat("_Fade", 0.5f); // 1 alapbol

        }
        else
        {
            Material origi = rend.material;

            // Make transparent
            origi.SetFloat("_Fade", 1f); // 1 alapbol

        }
    }
}
