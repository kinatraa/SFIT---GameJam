using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 _mousePosition;
    private Vector2 _direction;
    private float _mouseDistance;
    public float speed;
    
    void Update()
    {
        MoveToMousePosition();
    }

    private void MoveToMousePosition()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseDistance = Vector2.Distance(transform.position, _mousePosition);
        if (_mouseDistance > 0.1f)
        {
            _direction = _mousePosition - transform.position;
            _direction.Normalize();
            transform.Translate(_direction * Time.deltaTime * speed);
        }
    }
}
