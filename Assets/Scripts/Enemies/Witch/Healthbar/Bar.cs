using UnityEngine;

public class Bar : MonoBehaviour
{
    
    //Referencer til at styre healthbaren for witchen
    public WitchHealthManager witch; //Referer til Heksens health script
    public Transform fill; //Skrumpe bar FillTop
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
        //Opdatere healthbaren
        float percent = (float)witch.currentHealth / witch.maxHealth;
        fill.localScale = new Vector3(percent, 1f, 1f);

    }
}
