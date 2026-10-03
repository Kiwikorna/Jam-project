using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarDrive : MonoBehaviour
{
    public enum State
    {
        Driving,
        DrivingForward,
        DrivingBackward,
        DrivingLeft,
        DrivingRight,
        Jumping,
        Landing,
    }
    [SerializeField] private  float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float acceleration;
    [SerializeField] private float forwardAcceleration;
    [SerializeField] private float forwardDeceleration;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject buttonStart;
    [SerializeField] private GameObject endText;

    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider2D;
    private InputAction _moveAction;
    private InputAction _jumpAction;   
    private State _state;
    private Vector2 _targetVelocity;
    private float _startJumpPosition;
    private bool _isCollision;
    private void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _moveAction.performed += Move;
        _moveAction.canceled += StopMove;
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _jumpAction.performed += Jump;
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        _state = State.Jumping;
        _boxCollider2D.enabled = false;
        _startJumpPosition = _rb.position.y;
        _rb.linearVelocity = new Vector2(_rb.linearVelocityX, 0f);
        _rb.gravityScale = 1.0f;
        _rb.AddForce(new Vector2(speed,jumpForce), ForceMode2D.Impulse);
    }

    private void StopMove(InputAction.CallbackContext obj)
    {
        if (_state != State.Jumping)
        {
            _state = State.Driving;
        }
    }

    private void Move(InputAction.CallbackContext carInput)
    {
        if (_state == State.Jumping)
            return;
        HandleStateMove(carInput.ReadValue<Vector2>());

    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Debug.Log(_state);
        /*if (_state == State.Jumping && _rb.linearVelocity.y < 0)
        {
            _state = State.Landing;
        }*/
       
        
        
        if(_state != State.Jumping)
        {
            _targetVelocity = HandleDrive();
        }
        else
        {
            var isGrounded = _startJumpPosition > _rb.position.y;
            if (isGrounded && _state == State.Jumping)
            {
                _rb.position = new Vector2(_rb.position.x, _startJumpPosition);
                _boxCollider2D.enabled = true;
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, 0f);
                _rb.gravityScale = 0f;
                if(_moveAction.ReadValue<Vector2>() == Vector2.zero)
                    _state = State.Driving;
                else
                {
                    HandleStateMove(_moveAction.ReadValue<Vector2>());
                }
            }
        }
        
    }
    private void FixedUpdate()
    {
 
        if(_state != State.Jumping)
        {
            _rb.linearVelocity = Vector2.MoveTowards(_rb.linearVelocity,_targetVelocity,acceleration * Time.fixedDeltaTime);
        }
        
    }
    
    private Vector2 HandleDrive()
    {
        switch (_state)
        {
            case State.DrivingForward:
                return Vector2.right * (speed * forwardAcceleration);
            case State.DrivingBackward:
                return Vector2.left * (speed * forwardDeceleration);
            case State.DrivingLeft:
                return new Vector2(speed, speed * forwardAcceleration);
            case State.DrivingRight:
                    return new Vector2(speed, -speed  * forwardAcceleration);
            default:
                return Vector2.right * speed;
            
        }
       
    }

    private void HandleStateMove(Vector2 direction)
    {
        if (direction.y > 0)
        {
            _state = State.DrivingLeft;
            return;
        }

        if (direction.y < 0)
        {
            _state = State.DrivingRight;
            return;
        }

        if (direction.x > 0)
        {
            _state = State.DrivingForward;
            return;
        }

        if (direction.x < 0)
        {
            _state = State.DrivingBackward;
        }
    }
    public void SetСCollisionNormal(Vector2 normal)
    {
        if (normal != Vector2.zero && _state != State.Jumping)
        {
            var dot = Vector2.Dot(_moveAction.ReadValue<Vector2>(), normal);
            if (dot > 0)
            {
                _state = State.Driving;
            }
        }
    }

    private void OnDestroy()
    {
        menu.SetActive(true);
        buttonStart.SetActive(false);
        endText.SetActive(true);
        Time.timeScale = 0;
    }
}
