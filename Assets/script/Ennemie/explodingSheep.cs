using UnityEngine;

public class explodingSheep : MonoBehaviour
{
// 1. Assign your "WoolExplosion" prefab here in the Inspector
    public GameObject explosionEffectPrefab; 

    // Optional: Add a health system if you want
    public int health = 1;

    // Call this function when the sheep needs to blow up (e.g., from an enemy attack)
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Explode();
        }
    }

    // Example trigger: Explode on contact with a "Projectile"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the thing we hit is tagged "Projectile"
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    void Explode()
    {
        // 1. Create the visual "poof" at the sheep's current position
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 2. You could also apply the Physics Force logic from the previous answer here
        // (If you want the explosion to push other things away).

        // 3. Remove the sheep sprite immediately
        Destroy(gameObject);
    }
}
