using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Reference to the player 
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
            // Enemy is scared and running away — flatten Y
            Vector3 fleeTarget = new Vector3(player.position.x, transform.position.y, player.position.z);
            direction = (transform.position - fleeTarget).normalized;

            // Decrease flee timer
            fleeTimer -= Time.deltaTime;

            // Stop fleeing when timer is up
            if (fleeTimer <= 0f)
            {
                isFleeing = false;
            }
        }
        else
        {
            // Enemy is chasing — flatten Y
            Vector3 chaseTarget = new Vector3(player.position.x, transform.position.y, player.position.z);
            direction = (chaseTarget - transform.position).normalized;
        }

        // Choose speed
        float speed = isFleeing ? fleeSpeed : moveSpeed;

        // Move the enemy on the horizontal plane (XZ only)
        transform.position += direction * speed * Time.deltaTime;

        // Optional: rotate to face the player (on XZ only)
        Vector3 lookTarget = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookTarget);
    }

    // Called by the flashlight or cone when enemy is hit
    public void Scare()
    {
        isFleeing = true;             // Switch to fleeing mode
        fleeTimer = fleeDuration;     // Reset flee timer
    }
}
