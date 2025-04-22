using System.Diagnostics.Tracing;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Door : MonoBehaviour
{
    private Animator anim;
    private TextManager interactionText;



    void Start()
    {
        anim = GetComponent<Animator>();
        interactionText = GetComponent<TextManager>();
    }
    public void OpenDoor()
    {
        anim.enabled = true;
        
        if(this.gameObject.name == "Door_A")
        {
            anim.Play("MetDoor@Opening");
        } else 
        {
            anim.Play("Door@Opening");
        }
        
        Destroy(this.gameObject.GetComponent<BoxCollider>());
    }

    public void NeedsKey()
    {
        interactionText.Text.sentence[0] = "I need a key to open this door.";
        interactionText.StartDialogue();
    }

    public void DoesntOpen()
    {
        interactionText.Text.sentence[0] = "This door won't open.";
        interactionText.StartDialogue();
    }

}
