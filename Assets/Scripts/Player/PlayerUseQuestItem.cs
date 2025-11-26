using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using Unity.Cinemachine;

public class PlayerUseQuestItem : MonoBehaviour
{

    [Header("Hitbox")]
    //[SerializeField] private Collider2D attackHitbox;

     [Header("Attack details")]
    [SerializeField] protected float useRadius; // Den radius der må være
    [SerializeField] protected Transform usePoint; // Hvor detection sker
    [SerializeField] protected LayerMask doorTargetLayer; // Hvad den skal registere
    [SerializeField] protected GameObject newDoor;
    [SerializeField] float doorXPosition = 119.87f;
    [SerializeField] float doorYPosition = 34.06f;
    [SerializeField] protected LayerMask wellTargetLayer;
    [SerializeField] private GameObject targetTriggerZone;

    [Header("Curse fog effects")]
    [SerializeField] private GameObject villageCurseFog;
    [SerializeField] private CurseFogRemover fog;
    [SerializeField] private GameObject redFogPrefab2;

    [Header("SoundFX")]
    [SerializeField] private AudioClip doorOpenSFX;

    [Header("Camera")]
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera leftSideCam;
    [SerializeField] private CinemachineCamera rightSideCam;
    [SerializeField] private CinemachineCamera fullVillageCam;

    [Header("Camera Zones to disable")]
    [SerializeField] private RightSideVillageBorder rightSideZone;
    [SerializeField] private LeftSideVillageBorder leftSideZone;

    void Awake()
    {
        newDoor = Resources.Load<GameObject>("Prefabs/WitchDoorOpen");
        doorOpenSFX = Resources.Load<AudioClip>("SoundFX/DoorOpenSFX");
        villageCurseFog = GameObject.Find("VillageCurseFog");
        fog = GameObject.Find("VillageCurseFog").GetComponent<CurseFogRemover>();
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
    Collider2D[] hits = Physics2D.OverlapCircleAll(usePoint.position, useRadius);

    foreach (var hit in hits)
    {
        if (hit.CompareTag("WitchDoor"))
        {
            ChangeDoorPrefab();
            Debug.Log("ChangeDoorMethod Called");
            return true;
        }
    }

    return false;
}

   /* public bool UseWitchKey()
    {
    bool keyWasUsed = false;
    
    // Don’t filter by layer here
    Collider2D targetCollider = Physics2D.OverlapCircle(usePoint.position, useRadius);

    if (targetCollider != null && targetCollider.CompareTag("WitchDoor"))
    {
        ChangeDoorPrefab();
        Debug.Log("ChangeDoorMethod Called");
        keyWasUsed = true;
    }

    return keyWasUsed;
    } */


    public bool UseCure()
    {
        bool antidoteWasUsed = false;
        Collider2D targetCollider = Physics2D.OverlapCircle(usePoint.position, useRadius, wellTargetLayer);
        
            if (targetCollider != null)
            {
                targetTriggerZone = targetCollider.gameObject;
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
        playerCam.Priority = 0;
        leftSideCam.Priority = 0;
        rightSideCam.Priority = 10;
        fullVillageCam.Priority = 0;
        leftSideZone.DisableZone();
        rightSideZone.DisableZone();
        StartCoroutine(StartCurseLiftedCo());
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
    if (oldDoor == null)
        {
        Debug.LogWarning("Door not found");
        return;
        }

    Vector3 doorPos = new Vector3(doorXPosition, doorYPosition, oldDoor.transform.position.z);
    Destroy(oldDoor);
    SoundFXManager.Instance.PlaySoundFX(doorOpenSFX, transform);
    Instantiate(newDoor, doorPos, Quaternion.identity);
    }


    IEnumerator StartCurseLiftedCo()
    {   yield return new WaitForSeconds(3);
        SoundFXManager.Instance.StartVillageSavedMusic();
        yield return new WaitForSeconds(14);
        playerCam.Priority = 2;
        leftSideCam.Priority = 0;
        rightSideCam.Priority = 1;
        fullVillageCam.Priority = 10;
        yield return new WaitForSeconds(1);
        fog.StartFade();

        yield return new WaitForSeconds(6);
        leftSideZone.EnableZone();
        rightSideZone.EnableZone();
    }
}
