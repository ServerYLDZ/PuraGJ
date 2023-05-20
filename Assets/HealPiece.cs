using UnityEngine;

public class HealPiece : MonoBehaviour
{
    public GameObject healPrefab;
    public float speed = 5f;
    public float gravity = 9.8f;

    public void SpawnHeal(Vector3 spawnPosition)
    {
        GameObject heal = Instantiate(healPrefab, spawnPosition, Quaternion.identity);
        Rigidbody healRigidbody = heal.GetComponent<Rigidbody>();

        // Rastgele bir yönde hareket etmek için rastgele bir vektör oluþturulur
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        // Hýz vektörüne rastgele yöndeki hýz eklenir
        Vector3 velocity = randomDirection * speed;

        // Hýz vektörüne yerçekimi eklenir
        velocity.y -= gravity;

        // Heal prefabýnýn rigidbody bileþenine hýz vektörü atanýr
        healRigidbody.velocity = velocity;
    }
}
