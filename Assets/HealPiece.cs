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

        // Rastgele bir y�nde hareket etmek i�in rastgele bir vekt�r olu�turulur
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        // H�z vekt�r�ne rastgele y�ndeki h�z eklenir
        Vector3 velocity = randomDirection * speed;

        // H�z vekt�r�ne yer�ekimi eklenir
        velocity.y -= gravity;

        // Heal prefab�n�n rigidbody bile�enine h�z vekt�r� atan�r
        healRigidbody.velocity = velocity;
    }
}
