using UnityEngine;

public class PlayerBar : MonoBehaviour
{
    //Referencer til at styre healthbaren for witchen
    public Transform fill; //Skrumpe bar FillTop
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
        //Opdatere healthbaren
        float percent = GameData.Instance.PlayerHealth / GameData.Instance.PlayerMaxHealth;
        fill.localScale = new Vector3(percent, 1f, 1f);

    }
}
