using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUseQuestItem : MonoBehaviour
{

    [Header("Hitbox")]
    //[SerializeField] private Collider2D attackHitbox;

     [Header("Attack details")]
    [SerializeField] protected float useRadius; // Den radius der må være
    [SerializeField] protected Transform usePoint; // Hvor detection sker
    [SerializeField] protected LayerMask doorTargetLayer; // Hvad den skal registere
    [SerializeField] protected GameObject newDoor;
    [SerializeField] float doorXPosition = 119.7196f;
    [SerializeField] float doorYPosition = 33.88123f;
    [SerializeField] protected LayerMask wellTargetLayer;
    [SerializeField] private GameObject targetTriggerZone;

    [Header("SoundFX")]
    [SerializeField] private AudioClip doorOpenSFX;

    void Awake()
    {
        newDoor = Resources.Load<GameObject>("Prefabs/WitchDoorOpen");
        doorOpenSFX = Resources.Load<AudioClip>("SoundFX/DoorOpenSFX");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool UseWitchKey()
    {
        bool keyWasUsed = false;
        Collider2D targetCollider = Physics2D.OverlapCircle(usePoint.position, useRadius, doorTargetLayer);
            if (targetCollider != null)
            {
                ChangeDoorPrefab();
                keyWasUsed = true;
            }
        return keyWasUsed;
    }

    public bool UseCure()
    {
        bool antidoteWasUsed = false;
        Collider2D targetCollider = Physics2D.OverlapCircle(usePoint.position, useRadius, wellTargetLayer);
            if (targetCollider != null)
            {
                DialogueFollower follower = targetTriggerZone.GetComponent<DialogueFollower>();
                DialogueWriter   writer   = targetTriggerZone.GetComponent<DialogueWriter>();

        if (follower != null)
        {
            follower.BeginFollowing(targetTriggerZone.transform);  // ensure it follows the right target
            follower.ShowDialogueBox(true);                // show the UI
        }

        if (writer != null)
        {
            writer.StartDialogue();                        // start typewriter text
        }
                // Code for what happens when everything is saved
                // Maybe:
                // Dissable player movement
                // Change CameraCinema
                // update scene with no scene, flowers bloom etc etc
                // play happy music
                antidoteWasUsed = true;
            }
        return antidoteWasUsed;
    }
    

    void ChangeDoorPrefab()
    {
        GameObject oldDoor = GameObject.FindWithTag("WitchDoor");
        Vector3 doorPos = new Vector3(doorXPosition, doorYPosition, oldDoor.transform.position.z);
        Destroy(oldDoor);
        SoundFXManager.Instance.PlaySoundFX(doorOpenSFX, transform);
        GameObject NewDoor = Instantiate(newDoor, doorPos, Quaternion.identity);
    }
}
