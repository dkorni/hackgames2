using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonControl : MonoBehaviour
{
    
    public Animator AnimatorController;

    public float Speed = 6.0f;
    public float JumpSpeed = 8.0f;
    public float Gravity = 20.0f;
    public float Damage = 100;
    public float Hp = 100;

    private CharacterController _characterController;

    private Vector3 _moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            _moveDirection *= Speed;

            if (Input.GetButton("Jump"))
            {
                _moveDirection.y = JumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        _moveDirection.y -= Gravity * Time.deltaTime;

        // Move the controller
        _characterController.Move(_moveDirection * Time.deltaTime);
    }
}
