using UnityEngine;

public class FearCone : MonoBehaviour
{
    public GameObject ConeForGame;
    public KeyCode activateKey = KeyCode.Space;
    public float shrinkSpeed = 2f;          // shrink
    public Vector3 initialScale = new Vector3(1, 1, 1);  // Reset normal size
    public float minScaleThreshold = 0.1f;  // Below this disable the cone

    private bool ConeON = false;

    void Start()
    {
        ConeForGame.SetActive(false);
    }

    void Update()
    {
        // Trigger the cone with a key
        if (Input.GetKeyDown(activateKey))
        {
            ActivateCone();
        }

        // Shrink the cone while active
        if (ConeON && ConeForGame.activeSelf)
        {
            ConeForGame.transform.localScale = Vector3.Lerp(
                ConeForGame.transform.localScale,
                Vector3.zero,
                shrinkSpeed * Time.deltaTime
            );

            // If it's small enough, turn it off
            if (ConeForGame.transform.localScale.magnitude < minScaleThreshold)
            {
                ConeForGame.SetActive(false);
                ConeON = false;
            }
        }
    }

    void ActivateCone()
    {
        ConeForGame.SetActive(true);
        ConeForGame.transform.localScale = initialScale;
        ConeON = true;
    }
}