using UnityEngine;
public static class ExplosionForce
{
    // Extension method for Rigidbody2D
    public static void AddExplosionForce2D(
        this Rigidbody2D rb,
        Vector2 explosionOrigin,
        float explosionForce,
        float explosionRadius)
        {

        if (rb == null)
        {
            Debug.LogWarning("ExplosionForce: rb is null!");
            return;
        }
        // Direction from explosion to the rigidbody
        Vector2 direction = (Vector2)rb.position - explosionOrigin;
        float distance = direction.magnitude;

        Debug.Log($"ExplosionForce on {rb.name} | distance = {distance}, radius = {explosionRadius}");

        // Outside radius? No force
        if (distance > explosionRadius) {
            Debug.Log("Target outside explosion radius, no force applied.");
            return;
        }
        // 1 at center, 0 at edge
        float forceFalloff = 1f - (distance / explosionRadius);

        GameObject player = GameObject.FindWithTag("Player");

        var switcher = player.GetComponent<PlayerMaterialSwitcher>();
        if (switcher != null)
        {
            switcher.ApplyLowFriction(0.2f); // 0.2 seconds of “slippery”
}
        
        //Blast force
        rb.AddForce(direction.normalized * explosionForce * forceFalloff, ForceMode2D.Impulse);
        Vector2 blastDir = (direction.normalized + Vector2.up * 0.5f).normalized;
        rb.AddForce(blastDir * explosionForce * forceFalloff, ForceMode2D.Impulse);
        Debug.Log($"Applied force: {explosionForce * forceFalloff}, new velocity: {rb.linearVelocity}");
    }
}

