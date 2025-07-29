using UnityEngine;
using UnityEngine.UI;

public class BarUIAnimation : MonoBehaviour
{
    public CharakterController player;
    public Image barImage;
    public float animationSpeed = 5f;


    void Update()
    {
        if (player == null || barImage == null)
            return;

        // Get target fill amount from PlayerStats
        float targetFill = player.GetOxygenePercent();
        barImage.fillAmount = Mathf.Lerp(barImage.fillAmount, targetFill, animationSpeed * Time.deltaTime);



    }
}
