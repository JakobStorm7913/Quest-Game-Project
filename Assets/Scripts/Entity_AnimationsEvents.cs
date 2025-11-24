using UnityEngine;

public class Entity_AnimationsEvents : MonoBehaviour
{

    private Entity_Enemy entity_Enemy;

    private void Awake()

    {

        entity_Enemy = GetComponentInParent<Entity_Enemy>();

    }

    public void DamageTargets() => entity_Enemy.DamageTargets();



    private void DisableMovementAndJump()

    {

        entity_Enemy.EnableMovement(false);
        
    }


    private void EnableMovementAndJump() 
    
    {

       entity_Enemy.EnableMovement(true);

    }
  

}
