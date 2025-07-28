using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light spotlight;               // Assign your spotlight here in Inspector
    public int Energy = 100;              // Starts at full power (100 seconds)
    public LayerMask enemyLayer;          // Only detect enemies on this layer

    [Header("Detection Settings")]
    public float coneAngle = 30f;         // Half-angle used for detection logic
    public float detectionRange = 10f;

    [Header("Scaling Parameters")]
    public float maxIntensity = 1f;       // Light intensity at 100 Energy
    public float maxRange = 20f;          // Light range at 100 Energy
    public float maxAngle = 60f;          // Light angle at 100 Energy (full angle)

    private bool isOn = true;
    private float secondTimer = 0f;

    void Start()
    {
        spotlight.enabled = true;
        UpdateLightProperties(); // Ensure correct values on start
    }

    void Update()
    {
        if (!isOn) return;

        // Count down every 1 second
        secondTimer += Time.deltaTime;
        if (secondTimer >= 1f)
        {
            Energy = Mathf.Max(0, Energy - 1);  // Clamp to 0
            secondTimer = 0f;

            Debug.Log("Flashlight time left: " + Energy + " seconds");

            UpdateLightProperties();  // Adjust brightness, range, angle

            if (Energy <= 0)
            {
                spotlight.enabled = false;
                isOn = false;
                return;
            }
        }

        DetectEnemies();
    }

    void UpdateLightProperties()
    {
        float percent = Energy / 100f;

        spotlight.intensity = maxIntensity * percent;
        spotlight.range = maxRange * percent;
        spotlight.spotAngle = maxAngle * percent;

        // Detection cone uses half the visual spotlight angle (to match Vector3.Angle logic)
        detectionRange = maxRange * percent;
        coneAngle = (maxAngle * percent) / 2f;
    }

    void DetectEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);

        foreach (Collider hit in hits)
        {
            Vector3 toTarget = (hit.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, toTarget);

            if (angle <= coneAngle)
            {
                EnemyAI enemy = hit.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.Scare();
                }
            }
        }
    }

    public void RestoreEnergy(int amount)
    {
        Energy = Mathf.Clamp(amount, 0, 100); // Reset to 100 but not more
        spotlight.enabled = true;
        isOn = true;
        secondTimer = 0; // Reset timer
        Debug.Log("Battery picked up! Energy restored to " + Energy);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw flashlight cone lines in red for angle reference
        Vector3 forward = transform.forward * detectionRange;
        Quaternion leftRotation = Quaternion.Euler(0, -coneAngle, 0);
        Quaternion rightRotation = Quaternion.Euler(0, coneAngle, 0);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftRotation * forward);
        Gizmos.DrawRay(transform.position, rightRotation * forward);
    }
}
