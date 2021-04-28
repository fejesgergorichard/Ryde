using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FloaterAssigner : MonoBehaviour
{
    public float OffsetPerItem = 30.0f;
    public float AmplitudeModifierOfItems = 0.2f;

    void Start()
    {
        int i = 0;

        // Add the Floater.cs script to all top level children
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                Floater childFloater = child.gameObject.AddComponent<Floater>();
                childFloater.StartOffset = OffsetPerItem * i;
                childFloater.AmplitudeModifier = AmplitudeModifierOfItems;
                i++;
            }
        }
    }
}
