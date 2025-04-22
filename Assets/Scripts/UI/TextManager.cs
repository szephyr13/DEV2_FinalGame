using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;


public class TextManager : MonoBehaviour
{
    private Queue<string> introduction;
    [SerializeField] private ConversationPart text;
    [SerializeField] private GameObject dialogUI;
    [SerializeField] private GameObject nextSquare;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private bool typingOver;
    [SerializeField] private GameObject generalManager;

    private Player player;



    public ConversationPart Text { get => text; set => text = value; }


    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    //lists text to write on screen and enqueues it. then starts showing
    public void StartDialogue()
    {
        Debug.Log("Text Manager executed by " + this.gameObject.name);
        dialogUI.SetActive(true);
        if (player) 
        {
            player.IsPaused = true;
        }
        
        introduction = new Queue<string>();
        typingOver = true;

        foreach (string sentence in text.sentence)
        {
            introduction.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    //gets the next sentence and starts coroutine to start typing it. 
    // if there are no more sentences, ends dialogue
    public void DisplayNextSentence()
    {
        nextSquare.SetActive(false);
        AudioManager.instance.PlaySFX("Click");
        if (introduction.Count == 0 && typingOver == true)
        {
            EndDialogue();
            return;
        }
        else
        {
            if (typingOver == true)
            {
                string sentence = introduction.Dequeue();
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
            }
        }
    }

    //types each letter in the sentence. when over, informs of it with a bool
    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            typingOver = false;
            dialogText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        typingOver = true;
        nextSquare.SetActive(true);
    }

    //when ending, stops coroutines and tells UIManager to get to the next screen
    private void EndDialogue()
    {
        dialogUI.SetActive(false);
        if (player) 
        {
            player.IsPaused = false;
        }
        
        StopAllCoroutines();
        dialogText.text = "";
        generalManager.GetComponent<UIGameManager>().DialogueEnd(this.gameObject);
    }

}
