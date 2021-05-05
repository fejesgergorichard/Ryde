using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    public GameObject ParticlePrefab;
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlaySound("Coin");
        var particle = Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
