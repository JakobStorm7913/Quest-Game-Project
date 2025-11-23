using UnityEngine;

public class DialogueFollower : MonoBehaviour
{
    [Header("World target (NPC)")]
    public Transform targetWorld;               // usually this NPC's transform

    [Header("UI References")]
    public RectTransform canvasRectTransform;   // Canvas (Canvas)
    public RectTransform dialogueRectTransform; // DialogueBox (Panel)
    public Vector2 offset;                      // e.g. (0, 50) to be above the head

    private void Awake()
    {
        // Ensure the dialogue box is hidden at the start
        if (dialogueRectTransform != null)
        {
            dialogueRectTransform.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (targetWorld != null &&
            dialogueRectTransform != null &&
            dialogueRectTransform.gameObject.activeSelf)
        {
            MoveUI();
        }
    }

    public void BeginFollowing(Transform target)
    {
        targetWorld = target;
        MoveUI(); // snap once immediately
    }

    public void ShowDialogueBox(bool show)
    {
        if (dialogueRectTransform != null)
        {
            dialogueRectTransform.gameObject.SetActive(show);
        }
    }

    private void MoveUI()
    {
        if (targetWorld == null || canvasRectTransform == null || dialogueRectTransform == null)
            return;

        // 1. World -> Screen
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetWorld.position);

        // 2. Screen -> Canvas local position
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            screenPos,
            canvasRectTransform.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : Camera.main,
            out localPoint
        );

        // 3. Apply to dialogue box + offset
        dialogueRectTransform.anchoredPosition = localPoint + offset;
    }
}
