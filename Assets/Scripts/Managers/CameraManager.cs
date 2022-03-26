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
  private UITester _uiTest;
  [SerializeField]
  private float leftLimit;
  [SerializeField]
  private float rightLimit;
  [SerializeField]
  private float bottomLimit;
  [SerializeField]
  private float topLimit;

  private void Start()
  {
    _camera = GetComponent<Camera>();
    _uiTest = UITester.Instance;
  }

  void Update()
  {
    var isOverUI = _uiTest.IsPointerOverUIElement();

    if (Input.touchCount == 2) // Pinching
    {
      Touch touchZero = Input.GetTouch(0);
      Touch touchOne = Input.GetTouch(1);

      Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
      Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

      float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
      float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

      float difference = currentMagnitude - prevMagnitude;

      Zoom(difference * 0.01f);
    }
    else
    {
      // Dragging
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
    }

    // Zoom
    if (!isOverUI)
    {
      Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }
  }

  private void Zoom(float increment)
  {
    Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, _minSize, _maxSize);
  }
}
