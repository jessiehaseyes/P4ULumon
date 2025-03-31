using UnityEngine;

public class HansAnimationController : MonoBehaviour
{
    public Animator animator;
 

    private Vector2 _input;
    private Vector3 _moveDirection;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float speed = 5f;
    private float _currentVelocity;
    private CharacterController _characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

    
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        HandleMovement();
        HandleRotation();
    }

    private void HandleInput()
    {
        // Get input for movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _input = new Vector2(horizontal, vertical).normalized;
        
        // Calculate move direction based on input
        _moveDirection = new Vector3(_input.x, 0f, _input.y);
    }



    private void HandleMovement()
    {
        if (_input.sqrMagnitude == 0) return;
        
 
        Vector3 movement = _moveDirection * Time.deltaTime;
        
        // Use CharacterController for movement if available, otherwise move the transform directly
        if (_characterController != null)
        {
            _characterController.Move(movement);
        }
        else
        {
            transform.position += movement;
        }
    }

    private void HandleRotation()
    {
        if (_input.sqrMagnitude == 0) return;
        
        var targetAngle = Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
}