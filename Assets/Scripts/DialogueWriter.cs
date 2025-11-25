using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueWriter : MonoBehaviour
{
    private static DialogueWriter currentWriter = null;

    [Header("UI")]
    public TextMeshProUGUI textComponent;   // DialogueText (TMP)

    [Header("Dialogue")]
    [TextArea]
    public string dialogueText;             // NPC's line set in Inspector
    public float charDelay = 0.03f;
    public float autoHideDelay = 10f;      // time after finished before hiding
    public float textSize;

    private Coroutine typingCoroutine;
    private DialogueFollower follower;

    private void Awake()
    {
        // DialogueFollower is on the same NPC
        follower = GetComponent<DialogueFollower>();
    }

    public void StartDialogue()
    {
       if (textComponent == null) return;

    // If another DialogueWriter is typing, stop it cleanly
    if (currentWriter != null && currentWriter.typingCoroutine != null)
    {
        currentWriter.StopCoroutine(currentWriter.typingCoroutine);
        currentWriter.textComponent.text = "";
        currentWriter.typingCoroutine = null;
    }

    // This writer becomes the active one
    currentWriter = this;

    // Reset UI
    textComponent.text = "";

        textComponent.fontSize = textSize;
        typingCoroutine = StartCoroutine(TypeCoroutine());
    }

    private IEnumerator TypeCoroutine()
    {
        textComponent.text = "";
        //float timer = 0;
        SoundFXManager.Instance.PlayNPCTalkSFX();
        foreach (char c in dialogueText)
        {
            textComponent.text += c;
           // timer += Time.deltaTime;
            yield return new WaitForSeconds(charDelay);
            /*if (timer >= 4)
            {
                SoundFXManager.Instance.PlayNPCTalkSFX();
                timer = 0f;
            }*/
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
