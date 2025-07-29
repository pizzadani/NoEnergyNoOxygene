using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameTrigger : MonoBehaviour
{
    private bool playerInTrigger = false;
    public int nextSceneIndex;

    void Update()
    {
        if(playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            ChangeScene();

        }
        if (playerInTrigger)
        {
            Debug.Log("inTrigger");
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


    public void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneIndex);
    }
}
