using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarManager : MonoBehaviour
{
    [SerializeField]
    private HealthBar _heathBarPrefab;
    public HealthBar HeathBarPrefab { get => _heathBarPrefab; }

    private Dictionary<BaseEnemy, HealthBar> _tracking;

    [SerializeField]
    private Canvas _healthBarCanvas;

    private Camera _camera;

    private static EnemyHealthBarManager _instance;
    public static EnemyHealthBarManager Instance { get => _instance; }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _tracking = new Dictionary<BaseEnemy, HealthBar>();
        _camera = Camera.main;
    }

    public void UpdateHealthBar(BaseEnemy enemy, bool isFinished)
    {
        if (!_tracking.ContainsKey(enemy))
        {
            var newHealthBar = Instantiate(_heathBarPrefab, _healthBarCanvas.transform);
            newHealthBar.SetMaxHealth(enemy.ScriptableEnemy.hp);
            _tracking.Add(enemy, newHealthBar);
        }
        var healthBar = _tracking[enemy];

        if (enemy.Health <= 0)
        {
            Destroy(healthBar.gameObject);
            _tracking.Remove(enemy);
            return;
        }

        if (isFinished)
        {
            Destroy(healthBar.gameObject);
            return;
        }


        // Update the health bar

        // Set HealthBar position
        RectTransform canvasRect = _healthBarCanvas.GetComponent<RectTransform>();
        Vector2 viewportPosition = _camera.WorldToViewportPoint(enemy.transform.position + new Vector3(0, 0.7f));
        Vector2 screenPosition = new Vector2(
        (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
        (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
        healthBar.GetComponent<RectTransform>().anchoredPosition = screenPosition;

        // Set health
        healthBar.SetHealth(enemy.Health);
    }

}
