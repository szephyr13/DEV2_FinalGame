using Unity.AppUI.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainButtonsSet;
    [SerializeField] private GameObject optionsSet;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private UnityEngine.UI.Button menuPrimarySelection;
    [SerializeField] private UnityEngine.UI.Button optionsPrimarySelection; 


    //BUTTON INTERACTION
    public void PlayButton() 
    {
        AudioManager.instance.PlaySFX("Play");
        SceneManager.LoadScene(1);
        AudioManager.instance.StopMusic();
    }

    public void OptionsManagement()
    {
        AudioManager.instance.PlaySFX("Click");
        mainButtonsSet.SetActive(!mainButtonsSet.activeSelf);
        optionsSet.SetActive(!optionsSet.activeSelf);
        if (mainButtonsSet.activeSelf)
        {
            menuPrimarySelection.Select();
        } else 
        {
            optionsPrimarySelection.Select();
        }
    }

    public void ExitButton()
    {
        AudioManager.instance.PlaySFX("Click");
        Application.Quit();
    }


    //SOUND FEEDBACK MANAGEMENT
    public void HoverSound()
    {
        AudioManager.instance.PlaySFX("Hover");
    }

    public void SetVolumeBGM(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetVolumeSFX(float volume)
    {
        sfxSource.volume = volume;
    }
}

