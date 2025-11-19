using UnityEngine;

public class TestAttackBehavior : MonoBehaviour
{
     [SerializeField] private GameObject player;
    Vector2 direction;
    [SerializeField] private float speed;
    [SerializeField] private float health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        direction = player.transform.position - transform.position;
    
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
