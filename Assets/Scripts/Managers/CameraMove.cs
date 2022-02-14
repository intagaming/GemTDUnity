using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private float _minSize = 6, _maxSize = 19;
    private Vector3 _start;
    private Camera _camera;
    [SerializeField]
    private float _zoomChange = 4;
    private void Start()
    {
        _camera = GetComponent<Camera>();
    }
    void Update()
    {   
        if (Input.GetMouseButtonDown(1))
        {
            _start = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 direction = _start - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
        Zoom();
        
    }
    void Zoom()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            _camera.orthographicSize -= _zoomChange;
        }
        if(Input.mouseScrollDelta.y < 0)
        {
            _camera.orthographicSize += _zoomChange;
        }
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minSize, _maxSize);
    }

   

   
}
