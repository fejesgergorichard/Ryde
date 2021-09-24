using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Seethrough : MonoBehaviour
{
    private bool transparent = false;
    private Renderer rend;
    private CancellationTokenSource cts;

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

        cts.Cancel();
        cts = new CancellationTokenSource();

        if (transparent)
        {
            StartCoroutine(FadeTransparency(0.4f, 0.6f, cts.Token));
        }
        else
        {
            StartCoroutine(FadeTransparency(1f, 0.5f, cts.Token));
        }
    }

    private IEnumerator FadeTransparency(float targetValue, float duration, CancellationToken token)
    {
        float startValue = rend.material.GetFloat("_Fade");
        float time = 0;

        while (time < duration)
        {
            token.ThrowIfCancellationRequested();

            rend.material.SetFloat("_Fade", Mathf.Lerp(startValue, targetValue, time / duration));
            time += Time.deltaTime;
            yield return null;
        }

        rend.material.SetFloat("_Fade", targetValue);
    }
}
