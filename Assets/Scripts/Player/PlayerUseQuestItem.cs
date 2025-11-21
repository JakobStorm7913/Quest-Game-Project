using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUseQuestItem : MonoBehaviour
{

    [Header("Hitbox")]
    [SerializeField] private Collider2D attackHitbox;

     [Header("Attack details")]
    [SerializeField] protected float useRadius; // Den radius der må være
    [SerializeField] protected Transform usePoint; // Hvor detection sker
    [SerializeField] protected LayerMask doorTargetLayer; // Hvad den skal registere
    [SerializeField] protected GameObject newDoor;
    [SerializeField] protected LayerMask wellTargetLayer;

    [Header("SoundFX")]
    [SerializeField] private AudioClip doorOpenSFX;

    void Awake()
    {
        newDoor = Resources.Load<GameObject>("Prefabs/openDoor");
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
        Vector3 doorPosition = oldDoor.transform.position;
        Destroy(oldDoor);
        SoundFXManager.Instance.PlaySoundFX(doorOpenSFX, transform, 2f);
        GameObject openDoor = Instantiate(newDoor, doorPosition, Quaternion.identity);
    }
}
