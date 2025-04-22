using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIGameManager : MonoBehaviour
{
    [SerializeField] private TextManager blackScreen; 
    [SerializeField] private GameObject pauseMenuGO;
    [SerializeField] private UnityEngine.UI.Button pauseMainButton;
    [SerializeField] private GameObject youLostScreen;
    [SerializeField] private UnityEngine.UI.Button lostMainButton;
    [SerializeField] private GameObject youWonScreen;
    [SerializeField] private UnityEngine.UI.Button wonMainButton;
    [SerializeField] private Player player;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private Slider consciousness;
    private float timeLeft;
    private bool timerStart;
    private bool intro;
    private bool lost;
    


    void Start()
    {
        blackScreen.StartDialogue();
        intro = true;
        lost = false;
        player = FindAnyObjectByType<Player>();
        timeLeft = 120f;
    }

    void Update()
    {
        if(timerStart)
        {
            timeLeft -= Time.deltaTime;
            consciousness.value = timeLeft;
        }

        if (timeLeft <= 0 && lost == false)
        {
            YouLost();
        }
    }

    private void YouLost()
    {
        Time.timeScale = 0f;
        player.IsPaused = true;
        AudioManager.instance.PlayBGM("Lost");
        youLostScreen.SetActive(true);
        lostMainButton.Select();
        lost = true;
    }

    public void YouWon()
    {
        player.IsPaused = true;
        Time.timeScale = 0f;
        AudioManager.instance.PlayBGM("Menu");
        AudioManager.instance.PlaySFX("YouWon");
        youWonScreen.SetActive(true);
        wonMainButton.Select();
    }

    public void Interaction() 
    {
        if (intro) 
        {
            blackScreen.DisplayNextSentence();
        }
    }

    public void DialogueEnd(GameObject whoEnded) 
    {
        if (whoEnded.name == "BlackBackground")
        {
            intro = false;
            whoEnded.SetActive(false);
            AudioManager.instance.PlayBGM("Game");
            timerStart = true;
        } 
        if (whoEnded.TryGetComponent<Door>(out Door door))
        {
            player.IsPaused = false;
            player.NearDoor = null;
            player.NearLockedDoor = false;
            player.NearUnlockableDoor = false;
            player.NearUnlockedDoor = false;
        }
        if (whoEnded.name == "Key")
        {
            player.HasKey = true;
            AudioManager.instance.PlaySFX("GotKey");
            Destroy(whoEnded);
        }
    }

    //ENTER ON PAUSE MENU
    public void PauseMenu()
    {
        pauseMenuGO.SetActive(true);
        Time.timeScale = 0f;
        player.IsPaused = true;
        pauseMainButton.Select();
    }
    public void Unpause() 
    {
        pauseMenuGO.SetActive(false);
        Time.timeScale = 1f;
        player.IsPaused = false;
    }


    //BUTTONS
    public void MainMenu() 
    {
        AudioManager.instance.PlaySFX("Play");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        AudioManager.instance.PlayBGM("Menu");
        
    }

    public void Restart() 
    {
        AudioManager.instance.PlaySFX("Click");
        AudioManager.instance.StopMusic();
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void SetVolumeBGM(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetVolumeSFX(float volume)
    {
        sfxSource.volume = volume;
    }
     public void HoverSound()
    {
        AudioManager.instance.PlaySFX("Hover");
    }
        
}
