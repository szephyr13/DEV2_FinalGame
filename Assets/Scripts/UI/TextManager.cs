using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class TextManager : MonoBehaviour
{
    private Queue<string> introduction;
    [SerializeField] private Text text;
    [SerializeField] private TextMeshProUGUI intro;
    [SerializeField] private bool typingOver;
    [SerializeField] private GameObject generalManager;

    public Text Text { get => text; set => text = value; }

    //lists text to write on screen and enqueues it. then starts showing
    public void StartDialogue()
    {
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
        AudioManager.instance.PlaySFX("UISelect");
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
        intro.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            typingOver = false;
            intro.text += letter;
            yield return new WaitForSeconds(0f);
        }
        typingOver = true;
    }

    //when ending, stops coroutines and tells UIManager to get to the next screen
    private void EndDialogue()
    {
        StopAllCoroutines();
        intro.text = "";
        //generalManager.GetComponent<UIManager>().NextScreen();
    }

}
