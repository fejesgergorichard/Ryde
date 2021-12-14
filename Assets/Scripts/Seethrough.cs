using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Seethrough : MonoBehaviour
{
    private bool transparent = false;
    private Renderer rend;
    private CancellationTokenSource cts;
    private const string materialTransparencyProperty = "_Fade";


    private void Start()
    {
        cts = new CancellationTokenSource();

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

        cts?.Cancel();
        cts = new CancellationTokenSource();

        try
        {
            if (transparent)
            {

                StartCoroutine(FadeTransparency(0.4f, 0.6f, cts.Token));
            }
            else
            {
                StartCoroutine(FadeTransparency(1f, 0.5f, cts.Token));
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Fading canceled");
        }
    }

    private IEnumerator FadeTransparency(float targetValue, float duration, CancellationToken token)
    {
        Material[] materials = rend.materials;
        foreach (Material material in materials)
        {
            float startValue = material.GetFloat(materialTransparencyProperty);
            float time = 0;

            while (time < duration)
            {
                token.ThrowIfCancellationRequested();

                material.SetFloat(materialTransparencyProperty, Mathf.Lerp(startValue, targetValue, time / duration));
                time += Time.deltaTime;
                yield return null;
            }

            material.SetFloat(materialTransparencyProperty, targetValue);
        }
    }
}
