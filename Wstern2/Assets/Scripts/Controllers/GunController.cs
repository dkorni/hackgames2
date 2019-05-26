using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public float Damage;
    public PhotonView View;
    public Camera Camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (View.isMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 1000))
                {
                    Transform objectHit = hit.transform;

                    if(objectHit.tag == "Player")
                    {
                        objectHit.GetComponent<PhotonView>().RPC("SetDamage", PhotonTargets.AllBuffered, Damage);
                       
                    }
                    Debug.Log(objectHit.name);
                }
            }
        }
    }
}
