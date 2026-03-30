using GGG.Tool;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(EnemyHealthController))]
public class AutoEnemyHealthBar : MonoBehaviour
{
    [Header("Bar Settings")]
    public float width = 100f;
    public float height = 16f;
    public float extraHeight = 0.3f;
    public bool alwaysFaceCamera = true;
    public bool hideWhenFull = false;
    public bool hideWhenDead = false;
    public bool debugHPLog = false;

    [Header("Colors")]
    public Color backgroundColor = new Color(0f, 0f, 0f, 0.65f);
    public Color fillColor = new Color(0.2f, 1f, 0.2f, 1f);

    private EnemyHealthController _health;
    private CharacterController _characterController;

    private GameObject _barRoot;
    private RectTransform _fillRect;

    private float _lastHp = -9999f;
    private float _lastMaxHp = -9999f;

    private void Awake()
    {
        _health = GetComponent<EnemyHealthController>();
        _characterController = GetComponent<CharacterController>();

        CreateBar();
        UpdateBarPosition();
        UpdateBarValue(true);
    }

    private void LateUpdate()
    {
        if (_health == null || _health.HealthCurrentData == null)
            return;
        

        UpdateBarPosition();
        UpdateBarValue(false);

        if (alwaysFaceCamera && ObjectsManager.MainInstance.MainCamera != null && _barRoot != null)
        {
            _barRoot.transform.forward = ObjectsManager.MainInstance.MainCamera.forward;
        }
    }

    private void CreateBar()
    {
        Transform old = transform.Find("AutoHPBar");
        if (old != null)
        {
            _barRoot = old.gameObject;
            _fillRect = _barRoot.transform.Find("BG/Fill")?.GetComponent<RectTransform>();
            return;
        }

        _barRoot = new GameObject("AutoHPBar");
        _barRoot.transform.SetParent(transform, false);
        _barRoot.layer = gameObject.layer;
        _barRoot.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        Canvas canvas = _barRoot.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 100;

        _barRoot.AddComponent<CanvasScaler>();
        _barRoot.AddComponent<GraphicRaycaster>();

        RectTransform canvasRect = _barRoot.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(width, height);

        // BG
        GameObject bg = new GameObject("BG");
        bg.transform.SetParent(_barRoot.transform, false);
        bg.layer = gameObject.layer;

        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = backgroundColor;

        RectTransform bgRect = bg.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0.5f, 0.5f);
        bgRect.anchorMax = new Vector2(0.5f, 0.5f);
        bgRect.pivot = new Vector2(0.5f, 0.5f);
        bgRect.sizeDelta = new Vector2(width, height);
        bgRect.anchoredPosition = Vector2.zero;

        // Fill
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(bg.transform, false);
        fill.layer = gameObject.layer;

        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = fillColor;

        _fillRect = fill.GetComponent<RectTransform>();
        _fillRect.anchorMin = new Vector2(0f, 0f);
        _fillRect.anchorMax = new Vector2(1f, 1f);
        _fillRect.pivot = new Vector2(0f, 0.5f);
        _fillRect.offsetMin = new Vector2(2f, 2f);
        _fillRect.offsetMax = new Vector2(-2f, -2f);
        _fillRect.localScale = Vector3.one;
    }

    private void UpdateBarPosition()
    {
        if (_barRoot == null) return;

        Vector3 localPos;

        if (_characterController != null)
        {
            localPos = _characterController.center;
            localPos.y += (_characterController.height * 0.5f) + extraHeight;
        }
        else
        {
            localPos = Vector3.up * 2f;
        }

        _barRoot.transform.localPosition = localPos;
    }

    private void UpdateBarValue(bool forceLog)
    {
        if (_fillRect == null) return;
        if (_health == null || _health.HealthCurrentData == null) return;

        float currentHp = _health.HealthCurrentData.CurrentHP;
        float maxHp = _health.HealthCurrentData.MaxHP;

        float percent = 0f;
        if (maxHp > 0f)
            percent = Mathf.Clamp01(currentHp / maxHp);

        // 直接缩放宽度，不用 fillAmount
        _fillRect.localScale = new Vector3(percent, 1f, 1f);

        if (hideWhenDead && currentHp <= 0f)
        {
            _barRoot.SetActive(false);
            return;
        }

        if (hideWhenFull && currentHp >= maxHp)
        {
            _barRoot.SetActive(false);
            return;
        }

        _barRoot.SetActive(true);

        if (debugHPLog && (forceLog || !Mathf.Approximately(currentHp, _lastHp) || !Mathf.Approximately(maxHp, _lastMaxHp)))
        {
            DevelopmentToos.WTF($"{name} HP => {currentHp} / {maxHp}   percent={percent}");
            _lastHp = currentHp;
            _lastMaxHp = maxHp;
        }
    }
}