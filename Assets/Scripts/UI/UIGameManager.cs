using UnityEngine;

public class UIGameManager : MonoBehaviour
{
    [SerializeField] private TextManager blackScreen; 
    private bool intro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blackScreen.StartDialogue();
        intro = true;
    }

    public void Interaction() 
    {
        Debug.Log("Interaction detected by UIGM");
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
        }
    }
}
