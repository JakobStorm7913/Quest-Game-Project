using UnityEngine;
using System.Collections;

public class WitchCombatManager : MonoBehaviour
{
    public static WitchCombatManager Instance  { get; private set; }

    [Header("Fight Status")]
    public bool witchSlain { get; set; }
    public bool witchFrozen { get; set; }
    public bool entryCombatRunning { get; set; }
    public int requiredSpidersSlain { get; set; }
    public bool normalCombatRunning { get; set; }


    [Header("Script References")]
    [SerializeField] private WitchAttackScript attackScript;

     void Awake() {
        if (Instance == null) {
            
            Instance = this;
            witchSlain = false;
            witchFrozen = true;
            entryCombatRunning = false;
            requiredSpidersSlain = 999999999;
            normalCombatRunning = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        if (attackScript == null) 
        {
            attackScript = GameObject.Find("WitchMain").GetComponent<WitchAttackScript>();
            Debug.Log("AttackScript" + attackScript.ToString());
        }
    }

    public void StartCombat() {
        //INSERT DIALOGUE CODE / STORY

        witchFrozen = false;
        Debug.Log("Frozen = "+ witchFrozen.ToString());
        entryCombatRunning = true;
        requiredSpidersSlain = GameData.Instance.SpidersSlain + 2;
        Debug.Log("EntryRunning = " + entryCombatRunning.ToString());
        attackScript.SpawnInitialCombat();
    }

    public void EndCombat() {
        entryCombatRunning = false;
        Debug.Log("Frozen = " + witchFrozen.ToString());
        normalCombatRunning = false;
        Debug.Log("EntryRunning = " + entryCombatRunning.ToString());
        Destroy(GameObject.FindWithTag("Vines"));
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}