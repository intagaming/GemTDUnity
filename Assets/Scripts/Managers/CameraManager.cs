using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
  [SerializeField]
  private float _minSize = 6, _maxSize = 19;
  private Vector3? _start;
  private Camera _camera;
  [SerializeField]
  private float _zoomChange = 4;
  [SerializeField]
  private float _smoothness = 8;
  private UITester _uiTest;

  private void Start()
  {
    _camera = GetComponent<Camera>();
    _uiTest = UITester.Instance;
  }

  void Update()
  {
    var isOverUI = _uiTest.IsPointerOverUIElement();
    if (Input.GetMouseButtonDown(0) && !isOverUI)
    {
      _start = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    if (_start != null)
    {
      if (Input.GetMouseButton(0))
      {
        Vector3 direction = (Vector3)_start - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Camera.main.transform.position += direction;
      }
      else
      {
        _start = null;
      }
    }

    // Zoom
    if (!isOverUI)
    {
      if (Input.mouseScrollDelta.y > 0)
      {
        _camera.orthographicSize -= _zoomChange * Time.deltaTime * _smoothness;
      }
      if (Input.mouseScrollDelta.y < 0)
      {
        _camera.orthographicSize += _zoomChange * Time.deltaTime * _smoothness;
      }
      _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minSize, _maxSize);
    }
  }
}
