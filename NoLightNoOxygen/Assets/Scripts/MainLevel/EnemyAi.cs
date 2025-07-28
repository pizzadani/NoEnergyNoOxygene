using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Reference  the player 
    public Transform player;

    // Movement speeds
    public float moveSpeed = 3f;     
    public float fleeSpeed = 5f;    

    // How long the enemy flees
    public float fleeDuration = 3f;

    // Internal state tracking
    private bool isFleeing = false;   
    private float fleeTimer = 0f;     

    void Update()
    {
        // Don’t do anything if player is not set
        if (player == null) return;

        Vector3 direction;

        if (isFleeing)
        {
            //  Enemy is scared running 
            direction = (transform.position - player.position).normalized;

            // Decrease flee timer
            fleeTimer -= Time.deltaTime;

            // stop fleeing
            if (fleeTimer <= 0f)
            {
                isFleeing = false;
            }
        }
        else
        {
            //  Enemy is chasing 
            direction = (player.position - transform.position).normalized;
        }

        // Choose speed
        float speed;
        if (isFleeing)
        {
            speed = fleeSpeed;
        }
        else
        {
            speed = moveSpeed;
        }

        // Move the enemy in the right direction
        transform.position += direction * speed * Time.deltaTime;

        // Optional: rotate to face the player (can disable if using animations)
        transform.LookAt(player.position);
    }

    //  Called by the cone when  enemy is hit
    public void Scare()
    {
        Debug.Log("Enemy scared!");
        isFleeing = true;              // Switch to fleeing mode
        fleeTimer = fleeDuration;      // Reset flee timer
    }
}