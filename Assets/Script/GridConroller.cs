using UnityEngine;

public class GridConroller : MonoBehaviour
{
    [SerializeField] private float speed;
    private SpriteRenderer _spriteRenderer;
    private float _leftBound;
    private float _rightBound;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        _rightBound = (_spriteRenderer.bounds.max.x - cameraHalfWidth) / 2;
        
    }
    // Update is called once per frame
    void Update()
    {
       var targetX = transform.position.x;
        Debug.Log(_rightBound);
       if(targetX >= -_rightBound - 3f)
            transform.Translate(Vector2.left  * (speed * Time.deltaTime));
        
      
    }
}
