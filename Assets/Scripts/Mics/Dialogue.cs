using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public static Dialogue Instance;

    private GameObject dialoguePanel;
    private Text dialogueText;

    [Header("Dialogue Settings")]
    public string[] dialogueLines;
    private int index;

    [SerializeField] private float wordSpeed;

    [Header("Trigger Settings")]
    public bool autoTrigger = false;
    public bool playerIsClose;
    public bool oneTimeOnly = false;
    private bool hasPlayed = false;

    void Start()
    {
        Instance = this;

        if (UI_Manager.Instance != null && UI_Manager.Instance.uiRoot != null)
        {
            Transform uiRoot = UI_Manager.Instance.uiRoot.transform;

            dialoguePanel = uiRoot.Find("DialoguePanel")?.gameObject;
            dialogueText = uiRoot.Find("DialoguePanel/DialogueText")?.GetComponent<Text>();
            
            Button nextButton = uiRoot.Find("DialoguePanel/ContinueButton")?.GetComponent<Button>();
            if (nextButton != null)
            {
                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(() =>
                {
                    if (Dialogue.Instance != null)
                        Dialogue.Instance.NextLine();
                });
            }

            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("UI_Manager hoặc uiRoot chưa spawn. Hãy đảm bảo UI_Manager được load trước.");
        }
    }

    void Update()
    {
        if(!autoTrigger && !hasPlayed && playerIsClose && Input.GetKeyDown(KeyCode.F))
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zezoText();
            }
            else
            {
                StartDialogue();
            }
        }
    }

    void StartDialogue()
    {
        if (oneTimeOnly && hasPlayed) return;

        Instance = this;
        dialoguePanel.SetActive(true);
        dialogueText.text = "";
        index = 0;
        hasPlayed = true;
        StartCoroutine(Typing());
    }

    private void zezoText()
    {
        if (dialogueText != null)
            dialogueText.text = "";

        index = 0;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (Instance == this)
            Instance = null;
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogueLines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        StopAllCoroutines();
        if (index < dialogueLines.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        { 
            zezoText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        { 
            playerIsClose = true;

            if (autoTrigger && !dialoguePanel.activeInHierarchy)
            {
                StartDialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            if (!oneTimeOnly) zezoText();
        }
    }
}
