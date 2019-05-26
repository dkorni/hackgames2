using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonControl :  Photon.PunBehaviour
{   
    public Animator AnimatorController;
    public MouseController MsControllerVertical;
    public MouseController MsControllerHorizontal;

    public Camera PersonCamera;

    public float Speed = 6.0f;
    public float JumpSpeed = 8.0f;
    public float Gravity = 20.0f;
    public float Damage = 100;
    public float Hp = 100;

    private Text HPtext;

    private CharacterController _characterController;

    private Vector3 _moveDirection = Vector3.zero;

    private PhotonView _photonView;

    // Start is called before the first frame update
    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _characterController = GetComponent<CharacterController>();
        CheckIfPhotonPersonIsMine();
        HPtext = GameObject.Find("HPText").GetComponent<Text>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_photonView.isMine)
        {
        }
        else
        {
            HPtext.text = $"Health {Hp.ToString()}";
            Movement();
        }
    }

    private void CheckIfPhotonPersonIsMine()
    {
        if (!_photonView.isMine)
        {
            MsControllerVertical.enabled = false;
            MsControllerHorizontal.enabled = false;
            PersonCamera.enabled = false;
            this.enabled = false;
        }
    }

    private void Movement()
    {
        var mouseY = Input.GetAxis("Mouse Y");

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        if (_characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            _moveDirection *= Speed;


            if (Input.GetButton("Jump"))
            {
                _moveDirection.y = JumpSpeed;
            }
        }

        AnimatorController.SetFloat("X", x);
        AnimatorController.SetFloat("Y", y);

        AnimatorController.SetFloat("Mouse Y", MsControllerVertical.rotationY);

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        _moveDirection.y -= Gravity * Time.deltaTime;


        // Move the controller
        _characterController.Move(_moveDirection * Time.deltaTime);
        transform.Translate(new Vector3(x, 0, y) * Speed);
    }

    [PunRPC]
    public void SetDamage(float damage) {
        Hp -= damage;
        if(Hp < 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }


}

