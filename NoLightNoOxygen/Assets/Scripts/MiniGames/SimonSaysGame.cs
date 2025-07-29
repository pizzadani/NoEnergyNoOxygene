using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimonSaysGame : MonoBehaviour
{
    public GameObject simonSaysPanel;
    public GameObject wrongButtonPressed;
    public GameObject[] doors;
    public Button[] colorButtons;
    public float delayBetweenFlashes = 0.6f;
    public float buttonFalshDuration = 0.4f;
    public int endRound = 5;
    public GameObject taskCompletedObject;

    private List<int> sequence = new List<int>();
    private bool isPlayerTurn = false;
    private int playerIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void StartNewGame()
    {
        sequence.Clear();
        wrongButtonPressed.SetActive(false);
        AddToSequence();
    }

    void AddToSequence()
    {
        int next = Random.Range(0, colorButtons.Length);
        sequence.Add(next);
        StartCoroutine(PlaySequence());
    }
    //Starts PlaySequence and sets player turn to flase, after sets playerindex to 0 and allows player to interact again
    IEnumerator PlaySequence()
    {
        isPlayerTurn = false;

        for (int i = 0; i < sequence.Count; i++)
        {
            int index = sequence[i];
            yield return StartCoroutine(FlashButton(colorButtons[index]));
            yield return new WaitForSeconds(delayBetweenFlashes);
        }

        playerIndex = 0;
        isPlayerTurn = true;
    }

    IEnumerator FlashButton(Button btn)
    {
        Color originalColor = btn.image.color;
        btn.image.color = Color.white;
        yield return new WaitForSeconds(buttonFalshDuration);
        btn.image.color = originalColor;
    }
    //Cheks if player can press Buttons
    public void OnColorButtonPressed(int index)
    {
        if (!isPlayerTurn) return;

        //If Player completes sequence en round and start new round
        if (index == sequence[playerIndex])
        {
            playerIndex++;

            if (playerIndex >= sequence.Count)
            {
                //Player won Round
                isPlayerTurn = false;
                StartCoroutine(NextRoundAfterDelay());
            }
        }
        else
        {
            //On Wrong buttonpress 
            Debug.Log("Wrong button! Restarting...");
            wrongButtonPressed.SetActive(true);
            StartCoroutine(SecondsTimer());
        }
    }

    //After Waiting starts new Round only if sequnce < endRound
    IEnumerator NextRoundAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (sequence.Count == endRound)
        {
            taskCompletedObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(3);

        }
        else
        {
            AddToSequence();
        }
    }
    IEnumerator SecondsTimer()
    {
        yield return new WaitForSeconds(2f);
        StartNewGame();
    }
}
