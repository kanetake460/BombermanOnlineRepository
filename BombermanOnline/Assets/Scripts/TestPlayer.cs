using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviourPunCallbacks
{
    void Update()
    {
        if (photonView.IsMine == false) return;

        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.forward * 0.1f;
        if(Input.GetKey(KeyCode.S))
            transform.position += Vector3.back * 0.1f;
        if(Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * 0.1f;
        if(Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * 0.1f;
    }
}
