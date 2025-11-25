using UnityEngine;

public class HealthBarFollower : MonoBehaviour
{
    [Header("Target to follow (Witch/Boss)")]
    [SerializeField] private Transform targetWorld;

    [Header("UI")]
    [SerializeField] private RectTransform canvasRectTransform;   // root Canvas
    [SerializeField] private Vector2 offset = new Vector2(0f, 80f);

    private RectTransform _rect;
    private Canvas _canvas;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();

        // Find canvas if not assigned
        if (canvasRectTransform == null)
        {
            _canvas = GetComponentInParent<Canvas>();
            if (_canvas != null)
            {
                canvasRectTransform = _canvas.GetComponent<RectTransform>();
            }
            else
            {
                Debug.LogError("HealthBarFollower: No Canvas found in parents!");
            }
        }
        else
        {
            _canvas = canvasRectTransform.GetComponent<Canvas>();
        }

        // Optional: start hidden
        // ShowHealthBar(false);
    }

    private void LateUpdate()
    {
        if (targetWorld == null || _rect == null || canvasRectTransform == null)
            return;

        MoveUI();
    }

    // ===== PUBLIC API =====

    /// <summary>
    /// Old name you used earlier. Safe to keep.
    /// </summary>
    public void BeginFollowing(Transform target)
    {
        SetTarget(target);
        ShowHealthBar(true); // make sure it's visible when following starts
    }

    /// <summary>
    /// Show/hide the health bar.
    /// </summary>
    public void ShowHealthBar(bool show)
    {
        if (_rect != null)
        {
            _rect.gameObject.SetActive(show);
        }
    }

    /// <summary>
    /// Newer generic name. Also safe to call directly.
    /// </summary>
    public void SetTarget(Transform target)
    {
        targetWorld = target;
        MoveUI();
    }

    // ===== INTERNAL =====

    private void MoveUI()
    {
        // 1. World -> Screen
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetWorld.position);

        // 2. Screen -> Canvas local position
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            screenPos,
            (_canvas != null && _canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                ? null
                : Camera.main,
            out localPoint
        );

        // 3. Apply to bar + offset
        _rect.anchoredPosition = localPoint + offset;
    }
}
