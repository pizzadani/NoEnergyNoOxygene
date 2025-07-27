using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public GameObject tutorialImage;
    public void QuitGame()
    {
        print("Quit");
        Application.Quit();
    }

    public void StrartGame()
    {
        SceneManager.LoadScene(1);

    }

    public void OpenTutorial()
    {
        if (tutorialImage != null)
        {
            tutorialImage.SetActive(true);
        }

    }

    public void HideTutorial()
    {
        if (tutorialImage != null)
        {
            tutorialImage.SetActive(false);
        }

    }

}
