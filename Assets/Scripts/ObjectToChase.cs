using UnityEngine;

public class ObjectToChase : Entity_Enemy
{
    private Transform player;

    protected override void Awake()
    {
        base.Awake();

        player = FindAnyObjectByType<Player>().transform;
    }
       protected override void Update()
    {
        HandleFlip();
    }



    protected override void HandleFlip()
    {
        if (player == null)
            return;


        if (player.transform.position.x > transform.position.x && facingRight == false)
            Flip();
        else if (player.transform.position.x < transform.position.x && facingRight == true)
            Flip();
    }


}
