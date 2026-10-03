using System;
using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Rigidbody2D _rb;
    public Action<GameObject> destroyAction;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        _rb.AddForce(Vector2.left * speed,ForceMode2D.Impulse);
    }

    private void OnDestroy()
    {
        destroyAction.Invoke(gameObject);
    }
}
