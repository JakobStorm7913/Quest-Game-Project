using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueWriter : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    public List<string> lines = new List<string>();   // Add your NPC lines here in the Inspector
    public float charDelay = 0.03f;                   // Time between characters

    [Header("Input")]
    public KeyCode advanceKey = KeyCode.Space;        // Key to skip / go to next line

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip typeSound;
    [Range(0, 1)] public float typeSoundVolume = 0.7f;
    public int soundEveryNCharacters = 2;             // Play sound every N visible characters

    [Header("Options")]
    public bool autoStart = true;                     // Start typing on Start()
    public bool loopDialogue = false;                 // Loop after the last line

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (textComponent == null)
        {
            Debug.LogError("DialogueTypewriter: No TextMeshProUGUI assigned!");
            enabled = false;
            return;
        }

        textComponent.text = "";

        if (autoStart && lines.Count > 0)
        {
            StartTypingCurrentLine();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(advanceKey))
        {
            if (isTyping)
            {
                // Skip to end of current line
                CompleteCurrentLineInstant();
            }
            else
            {
                // Go to next line
                GoToNextLine();
            }
        }
    }

    // --- Public API if you want to trigger from other scripts ---
    public void StartDialogue(List<string> newLines)
    {
        lines = newLines;
        currentLineIndex = 0;
        StartTypingCurrentLine();
    }

    public void StartDialogue(string singleLine)
    {
        lines = new List<string> { singleLine };
        currentLineIndex = 0;
        StartTypingCurrentLine();
    }

    // --- Internal logic ---

    private void StartTypingCurrentLine()
    {
        if (lines.Count == 0) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(lines[currentLineIndex]));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;

        // Set full text so TMP can handle rich text tags correctly
        textComponent.text = line;
        textComponent.ForceMeshUpdate();

        int totalVisibleCharacters = textComponent.textInfo.characterCount;
        int visibleCount = 0;
        int soundCounter = 0;

        textComponent.maxVisibleCharacters = 0;

        while (visibleCount < totalVisibleCharacters)
        {
            visibleCount++;
            textComponent.maxVisibleCharacters = visibleCount;

            // Optional: “fast-forward” when holding the key
            float delay = Input.GetKey(advanceKey) ? charDelay * 0.2f : charDelay;

            // Play sound every N characters (and ignore whitespace)
            if (audioSource != null && typeSound != null)
            {
                var charInfo = textComponent.textInfo.characterInfo[visibleCount - 1];
                char c = charInfo.character;

                if (!char.IsWhiteSpace(c))
                {
                    soundCounter++;
                    if (soundCounter >= soundEveryNCharacters)
                    {
                        audioSource.PlayOneShot(typeSound, typeSoundVolume);
                        soundCounter = 0;
                    }
                }
            }

            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private void CompleteCurrentLineInstant()
    {
        if (!isTyping) return;

        // Stop the coroutine and reveal all characters
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        textComponent.ForceMeshUpdate();
        textComponent.maxVisibleCharacters = textComponent.textInfo.characterCount;

        isTyping = false;
        typingCoroutine = null;
    }

    private void GoToNextLine()
    {
        if (lines.Count == 0) return;

        currentLineIndex++;

        if (currentLineIndex >= lines.Count)
        {
            if (loopDialogue)
            {
                currentLineIndex = 0;
                StartTypingCurrentLine();
            }
            else
            {
                // End of dialogue – you can hide box or trigger event here
                Debug.Log("Dialogue finished.");
            }
        }
        else
        {
            StartTypingCurrentLine();
        }
    }
}
