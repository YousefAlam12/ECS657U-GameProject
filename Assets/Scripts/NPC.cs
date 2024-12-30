using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject dialogueCanvasPrefab;
    public string[] dialogueLines;
    private GameObject dialogueCanvasInstance;
    private TextMeshProUGUI dialogueText;
    private bool playerInRange;
    private bool isInteracting;
    private int currentLineIndex;
    private Coroutine textRevealCoroutine;

    // Trigger interaction when the player enters collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            StartInteraction();
        }
    }

    // End interaction when the player exits the collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            EndInteraction();
        }
    }

    // Starts the dialogue displaying the text on screen
    void StartInteraction()
    {
        if (isInteracting || dialogueCanvasPrefab == null || dialogueLines.Length == 0) return;

        dialogueCanvasInstance = Instantiate(dialogueCanvasPrefab);
        dialogueText = dialogueCanvasInstance.GetComponentInChildren<TextMeshProUGUI>();
        currentLineIndex = 0;
        isInteracting = true;
        ShowNextLine();
    }

    // Shows the next line of dialogue
    void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            if (textRevealCoroutine != null)
            {
                StopCoroutine(textRevealCoroutine);
            }

            textRevealCoroutine = StartCoroutine(RevealText(dialogueLines[currentLineIndex]));
            currentLineIndex++;
        }
        else
        {
            EndInteraction();
        }
    }

    // Reveals text one char at a time
    IEnumerator RevealText(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.7f);
        ShowNextLine();
    }

    // Ends dialogue and removes the text from screen
    void EndInteraction()
    {
        if (dialogueCanvasInstance != null)
        {
            Destroy(dialogueCanvasInstance);
        }
        isInteracting = false;
        if (textRevealCoroutine != null)
        {
            StopCoroutine(textRevealCoroutine);
        }
    }
}
