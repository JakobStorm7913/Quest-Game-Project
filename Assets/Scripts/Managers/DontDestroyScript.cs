using UnityEngine;

public class DontDestroyScript : MonoBehaviour
{
    private static DontDestroyScript instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}