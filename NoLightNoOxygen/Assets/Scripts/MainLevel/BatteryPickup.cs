using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public int restoreAmount = 100; // Energy restored

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player touched it
        Flashlight flashlight = other.GetComponentInChildren<Flashlight>();
        if (flashlight != null)
        {
            flashlight.RestoreEnergy(restoreAmount);
            Destroy(gameObject); // Remove the battery after use
        }
    }
}