using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    private PlayerMovementScript playerAnimations;

    private void Awake()

    {

        playerAnimations = GetComponentInParent<PlayerMovementScript>();

    }

    public void DamageTargets() => playerAnimations.DamageTargets();


    private void DisableMovementAndJump()

    {

        playerAnimations.EnableMovement(false);
        
    }


    private void EnableMovementAndJump() 
    
    {

       playerAnimations.EnableMovement(true);

    }
  

}
