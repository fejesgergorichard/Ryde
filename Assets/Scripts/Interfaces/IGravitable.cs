using UnityEngine;

public interface IGravitable
{
    bool IsGravitated { get; set; }
    bool IsFlipped { get; set; }
    Rigidbody Rigidbody { get; }
}
