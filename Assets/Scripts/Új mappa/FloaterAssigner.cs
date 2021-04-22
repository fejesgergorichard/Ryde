using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FloaterAssigner : MonoBehaviour
{
    async void Start()
    {
        // Add the Floater.cs script to all top level children
        foreach (Transform child in transform)
        {
            if (child != transform)
            child.gameObject.AddComponent<Floater>();
            await Task.Run(()=> Thread.Sleep(50));
        }
    }
}
