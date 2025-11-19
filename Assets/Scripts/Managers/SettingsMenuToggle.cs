using UnityEngine;

public class SettingsMenuToggle : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
    }


    public void HideSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
