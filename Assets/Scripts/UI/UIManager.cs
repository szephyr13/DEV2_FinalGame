using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainButtonsSet;
    [SerializeField] private GameObject optionsSet;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;


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

