using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FloaterAssigner : MonoBehaviour
{
    public float OffsetPerItem = 30.0f;

    void Start()
    {
        int i = 0;

        // Add the Floater.cs script to all top level children
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                child.gameObject.AddComponent<Floater>().StartOffset = OffsetPerItem * i;
                i++;
            }
        }
    }
}
