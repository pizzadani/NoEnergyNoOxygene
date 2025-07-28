using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameTrigger : MonoBehaviour
{
    private bool playerInTrigger = false;

    void Update()
    {
        if(playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(2);

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            
        }
        else
        {
            playerInTrigger= false;
        }
    }
}
