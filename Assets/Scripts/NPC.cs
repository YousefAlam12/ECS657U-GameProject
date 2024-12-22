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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            StartInteraction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            EndInteraction();
        }
    }

    void StartInteraction()
    {
        if (isInteracting || dialogueCanvasPrefab == null || dialogueLines.Length == 0) return;

        dialogueCanvasInstance = Instantiate(dialogueCanvasPrefab);
        dialogueText = dialogueCanvasInstance.GetComponentInChildren<TextMeshProUGUI>();
        currentLineIndex = 0;
        isInteracting = true;
        ShowNextLine();
    }

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

    IEnumerator RevealText(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);
        ShowNextLine();
    }

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
