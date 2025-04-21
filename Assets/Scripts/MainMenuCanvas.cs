using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject mainButtonsSet;
    [SerializeField] private GameObject optionsSet;



    public void PlayButton() 
    {
        SceneManager.LoadScene(1);
    }

    public void OptionsManagement()
    {
        mainButtonsSet.SetActive(!mainButtonsSet.activeSelf);
        optionsSet.SetActive(!optionsSet.activeSelf);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
