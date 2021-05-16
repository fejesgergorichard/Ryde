using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    public GameObject[] ParticlePrefabs;


    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlaySound("Coin");
        GameManager.Instance.CoinFlyEffect(transform.position);

        InstantiateParticles();
        Destroy(gameObject);
    }

    private void InstantiateParticles()
    {
        foreach (var particlePrefab in ParticlePrefabs)
        {
            Instantiate(particlePrefab, transform.position, Quaternion.identity);
        }
    }
}
