using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Cooldown_Example : MonoBehaviour

{
private SpriteRenderer sr;

    [SerializeField] private float redColorDuration = 1;


    public float curretTimeInGame;
    public float lastTimeWasDamaged;

    public float timer; 
    private void Awake()

    {
        sr = GetComponent<SpriteRenderer>();
    }



    private void Update()

    {

        curretTimeInGame = Time.time;

        if (curretTimeInGame > lastTimeWasDamaged + redColorDuration)

        {
            if (sr.color != Color.white)
                sr.color = Color.white;
        }
        

    }

    [ContextMenu("Update timer")]

    private void UpdateTimer() => timer = redColorDuration;

    public void TakeDamage()

    {
        sr.color = Color.red;
        lastTimeWasDamaged = Time.time;


    }


    private void TurnWhite()

    {
        sr.color = Color.white;
    }


}
