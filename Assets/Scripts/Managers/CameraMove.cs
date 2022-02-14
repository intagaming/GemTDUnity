using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private float _minSize = 6, _maxSize = 19;
    private Vector3 _start;
    private void Start()
    {
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
            Debug.Log(direction);
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }
    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, _minSize,_maxSize);
    }

   
}
