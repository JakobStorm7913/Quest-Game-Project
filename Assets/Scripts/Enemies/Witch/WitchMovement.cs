using UnityEngine;
using System.Collections.Generic;

public class WitchMovement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float speed = 1f;

    [Header ("Animations")]
    [SerializeField] private Animator witchAnimator;
    private SpriteRenderer sr;

    [Header ("Movement Path")]
    [SerializeField] private List<Transform> patrolNodes;
    [SerializeField] private float reachDistance = 0.1f;
    [SerializeField] private int currentNode = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!WitchCombatManager.Instance.witchFrozen) {
        if (patrolNodes == null || patrolNodes.Count == 0) return;

        Transform targetNode = patrolNodes[currentNode];

        Vector3 oldPos = transform.position;

        // Move toward the current node
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetNode.position,
            speed * Time.deltaTime
        );

        //Code to dermine if witch is moving left or right and flip sprite.
        float dir = transform.position.x - oldPos.x;
        if (dir > 0.01f)
            sr.flipX = false; // moving right
        else if (dir < -0.01f)
            sr.flipX = true;  // moving left

        // Check if reached node
        float distance = Vector3.Distance(transform.position, targetNode.position);
        if (distance <= reachDistance)
        {
            // Move to the next node (loop)
            currentNode = (currentNode + 1) % patrolNodes.Count;
        }
        }
    }
}
