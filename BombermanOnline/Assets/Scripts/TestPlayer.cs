using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using SoftGear.Strix.Unity.Runtime;
using System.Linq;

public class TestPlayer : StrixBehaviour
{
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fps = new FPS(null,rb,gameObject,null);
    }

    private void Start()
    {

    }


    void Update()
    {
        if(isLocal == false)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
        }
        fps.AddForceLocomotion();
        fps.PlayerViewport();
    }

    FPS fps;
    Rigidbody rb;
    [SerializeField] GameObject mainCamera;
    [SerializeField] Vector3 cameraPos;
}
