using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonControl : MonoBehaviour
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

    public Vector3 TargetPosition;
    public Quaternion TargetRotation;

    private CharacterController _characterController;

    private Vector3 _moveDirection = Vector3.zero;

    private PhotonView _photonView;

    // Start is called before the first frame update
    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _characterController = GetComponent<CharacterController>();
        CheckIfPhotonPersonIsMine();

    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPhotonPersonIsMine();
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

        SyncMove();
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

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void SyncMove()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.5f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 720f * Time.deltaTime);
    }
}

