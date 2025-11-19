using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if(GameData.Instance == null) {
            GameObject dataObj = new GameObject("GameData");
            dataObj.AddComponent<GameData>();
        }
        StartGame();
    }

    public void StartGame() {
        GameData.Instance.GameRunning = true;
        SoundFXManager.Instance.PlayMainMusic();
    }

    public void EndGame() {
        GameData.Instance.GameRunning = false;
    }

    void Update() {
        
    }
}
