using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TapToStopGame;

public class TapToStopGame : MonoBehaviour
{
    [System.Serializable]
    public class MovingBar
    {
        public RectTransform barTransform;
        public float speed = 200f;
        public float direction = 1f;
        public bool isMoving = true;
        //[System.NonSerialized]
        public Image image;
    }
    public float resetTimer = 1.5f;
    public int successCount = 0;
    public int buttonPressed = 0;
    public MovingBar[] bars;
    public RectTransform targetZone;
    public Button stopButton;
    public Text resultText;

    private float leftLimit, rightLimit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stopButton.onClick.AddListener(StopMovement);
        resultText.text = "";

        StartCoroutine(InitBounds());
    }

    IEnumerator InitBounds()
    {
        yield return new WaitForEndOfFrame();
         
        //Calculate bounds(assuming all bars share the same parent size)
        float parentWidth = bars[0].barTransform.parent.GetComponent<RectTransform>().rect.width;
        float barWidth = bars[0].barTransform.rect.width;

        leftLimit = -parentWidth / 2 + barWidth / 2;
        rightLimit = parentWidth / 2 - barWidth / 2;

        //Cache Image components
        foreach (var bar in bars)
        {
            bar.image = bar.barTransform.GetComponent<Image>();

            //Ramdomize starting position
            float randomX = Random.Range(leftLimit, rightLimit);
            Vector3 pos = bar.barTransform.localPosition;
            bar.barTransform.localPosition = new Vector3(randomX, pos.y, pos.z);
        }
    }

    // Update is called once per frame
    void Update()
    {

        

        foreach (var bar in bars)
        {
            
            if (!bar.isMoving) continue;


            //move bar
            Vector3 pos = bar.barTransform.localPosition;
            pos.x += bar.speed * bar.direction * Time.deltaTime;


            if (pos.x > rightLimit || pos.x < leftLimit)
            {
                bar.direction *= -1;
                pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);

            }

            bar.barTransform.localPosition = pos;

            //Check if inside target zone
            float zoneLeft = targetZone.localPosition.x - targetZone.rect.width / 2;
            float zoneRight = targetZone.localPosition.x + targetZone.rect.width / 2;


            if(pos.x - bar.barTransform.rect.width / 2 >= zoneLeft && pos.x + bar.barTransform.rect.width / 2 <= zoneRight)
            {
                bar.image.color = Color.yellow;
            }
            else
            {
                bar.image.color = Color.grey;
            }
        }
    }

    void StopMovement()
    {
        buttonPressed++;
        for (int i = 0; i < bars.Length ; i++)
        {
            float barX = bars[i].barTransform.localPosition.x;
            float zoneLeft = targetZone.localPosition.x - targetZone.rect.width / 2;
            float zoneRight = targetZone.localPosition.x + targetZone.rect.width / 2;

            if (bars[i].isMoving)
            {
                bars[i].isMoving = false;

                if (barX - bars[i].barTransform.rect.width / 2 >= zoneLeft && barX + bars[i].barTransform.rect.width / 2 <= zoneRight)
            {
                successCount++;
            }
                else
                {
                    resultText.text = "Falsch";
                    resultText.color = Color.red;
                    ResetGame();
                }

                break;
            }
            
        }

        //Evaluate results;
        if (buttonPressed == bars.Length)
        {
            if (successCount == bars.Length)
            {
                resultText.text = "Perfect";
                resultText.color = Color.green;
                StartCoroutine(OneSecondTimer());
                SceneManager.LoadScene(5);

            }
        }
    }

    IEnumerator ResetGameWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetGame();
    }

    IEnumerator OneSecondTimer()
    {
        yield return new WaitForSeconds(1f);
    }

    void ResetGame()
    {
        successCount = 0;
        buttonPressed = 0;
        resultText.text = "";

        foreach (var bar in bars)
        {
            bar.isMoving = true;
            bar.direction = 1f;
            bar.barTransform.localPosition = new Vector3(Random.Range(leftLimit, rightLimit), bar.barTransform.localPosition.y, bar.barTransform.localPosition.z);
            bar.image.color = Color.grey;
        }
    }
}
