using UnityEngine;
using UnityEngine.InputSystem;   // new input system

public class PlayerActivateDialogue : MonoBehaviour
{
    [Header("Use settings")]
    [SerializeField] private float useRadius = 1.5f;
    [SerializeField] private Transform usePoint;
    [SerializeField] private LayerMask npcTargetLayer;

    [SerializeField] private GameObject targetNpc;

    //private InputAction interactAction;

   /* void Awake()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        interactAction.Enable();
        interactAction.performed += ctx => InteractWithNPC();
    }^*/

     private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            InteractWithNPC();
        }
    }

    public void InteractWithNPC()
    {
        // Check for an NPC in range (2D)
        Collider2D targetCollider = Physics2D.OverlapCircle(usePoint.position, useRadius, npcTargetLayer);
        if (targetCollider == null)
        {
            return;
        }

        targetNpc = targetCollider.gameObject;

        // Get NPC components
        DialogueFollower follower = targetNpc.GetComponent<DialogueFollower>();
        DialogueWriter   writer   = targetNpc.GetComponent<DialogueWriter>();

        if (follower != null)
        {
            follower.BeginFollowing(targetNpc.transform);  // ensure it follows the right target
            follower.ShowDialogueBox(true);                // show the UI
        }

        if (writer != null)
        {
            writer.StartDialogue();                        // start typewriter text
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (usePoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(usePoint.position, useRadius);
    }
}
