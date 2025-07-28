using UnityEngine;

public class OxygenStation : MonoBehaviour
{
    public int oxygenPerFrame = 1; // Amount of oxygen restored each frame the player is in the zone

    private void OnTriggerStay(Collider other)
    {
        // Check if the player is in the zone
        CharakterController player = other.GetComponent<CharakterController>();
        if (player != null)
        {
            // Increase oxygen, clamp at 100
            player.oxygen = Mathf.Min(player.oxygen + oxygenPerFrame, 100);
        }
    }
}