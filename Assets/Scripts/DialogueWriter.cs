using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueWriter : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;   // DialogueText (TMP)

    [Header("Dialogue")]
    [TextArea]
    public string dialogueText;             // NPC's line set in Inspector
    public float charDelay = 0.03f;
    public float autoHideDelay = 10f;      // time after finished before hiding

    private Coroutine typingCoroutine;
    private DialogueFollower follower;

    private void Awake()
    {
        // DialogueFollower is on the same NPC
        follower = GetComponent<DialogueFollower>();
    }

    public void StartDialogue()
    {
        if (textComponent == null)
        {
            Debug.LogWarning("DialogueWriter: No TextMeshProUGUI assigned on " + name);
            return;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeCoroutine());
    }

    private IEnumerator TypeCoroutine()
    {
        textComponent.text = "";

        foreach (char c in dialogueText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        // done typing, wait a bit
        yield return new WaitForSeconds(autoHideDelay);

        // hide the dialogue box via follower
        if (follower != null)
        {
            follower.ShowDialogueBox(false);
        }

        typingCoroutine = null;
    }

    public void SkipToEnd()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        textComponent.text = dialogueText;

        // optional: immediately hide after skipping
        if (follower != null)
        {
            follower.ShowDialogueBox(false);
        }
    }
}
