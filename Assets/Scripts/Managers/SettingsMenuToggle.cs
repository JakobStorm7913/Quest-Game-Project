using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsMenuToggle : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject buttonSettings;
    [SerializeField] private InputAction escapeAction;
    [SerializeField] private bool escapeMenuShown = false;

    void Awake()
    {
        escapeAction = InputSystem.actions.FindAction("Escape");
    if (escapeAction != null)
    {
        escapeAction.Enable();
        escapeAction.performed += ctx => ShowAndHideSettings();
    }
    }
    public void ShowAndHideSettings()
    {
        if(!escapeMenuShown) {
        settingsPanel.SetActive(true);
        buttonSettings.SetActive(false);
        Time.timeScale = 0f;
        }
        if(escapeMenuShown)
        {
        settingsPanel.SetActive(false);
        buttonSettings.SetActive(true);
        Time.timeScale = 1f;
        }
    }
}
