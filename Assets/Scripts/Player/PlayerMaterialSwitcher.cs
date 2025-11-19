using UnityEngine;
using System.Collections;

public class PlayerMaterialSwitcher : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private PhysicsMaterial2D normalMat;
    [SerializeField] private PhysicsMaterial2D lowFrictionMat;

    public void ApplyLowFriction(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(LowFrictionRoutine(duration));
    }

    private IEnumerator LowFrictionRoutine(float duration)
    {
        playerCollider.sharedMaterial = lowFrictionMat;
        yield return new WaitForSeconds(duration);
        playerCollider.sharedMaterial = normalMat;
    }
}
