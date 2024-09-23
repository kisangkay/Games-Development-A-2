using UnityEngine;

public class RedsphereBullet : MonoBehaviour
{
    public int damage = 100;

    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Player" or "EnemyTank" tag
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("RedTank"))
        {
            // Apply damage to the object
            obj.SendMessage("ApplyDamage", damage);
        }
    }

	
}
