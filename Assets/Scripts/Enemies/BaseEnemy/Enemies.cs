using System;
using UnityEngine;

public class Enemy : Entity_Enemy
{

    private bool playerDetected;

     [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 2f; // Kode til moveSpeed

    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }


    protected override void HandleAttack()
    {
        if (playerDetected)
            anim.SetTrigger("attack");
    }


    protected override void HandleMovement()
    {

        if (canMove)
            rb.linearVelocity = new Vector2(facingDir * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }


    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);

    }

    protected override void Die()
    {
        base.Die();
        UI.instance.AddKillCount();
    }

}

