using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance  { get; private set; }

    [Header("Player Stats")]
    public float PlayerMaxHealth { get; set; }
    public float PlayerHealth { get; set; }
    public float PlayerAttackDamage { get; set; }
    
    [Header("Other")]
    public bool GameRunning { get; set; }
    public int SpidersSlain { get; set; }



    void Awake() {
        if (Instance == null) {
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayerMaxHealth = 100;
            PlayerHealth = 100;
            PlayerAttackDamage = 5;
            SpidersSlain = 0;
            GameRunning = false;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}