using UnityEngine;

public class Entity_AnimationsEvents : MonoBehaviour
{

    private Entity_Enemy entity;

    private void Awake()

    {

        entity = GetComponentInParent<Entity_Enemy>();

    }

    public void DamageTargets() => entity.DamageTargets();

    


    private void DisableMovementAndJump()

    {

        entity.EnableMovement(false);
        
    }


    private void EnableMovementAndJump() 
    
    {

       entity.EnableMovement(true);

    }
  

}
