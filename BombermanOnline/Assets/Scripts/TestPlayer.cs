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
            Instantiate(test,transform.position, Quaternion.identity);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            bool active = !test2.activeSelf;
            test2.SetActive(active);
        }
        fps.AddForceLocomotion();
        fps.PlayerViewport();
    }

    FPS fps;
    Rigidbody rb;
    [SerializeField] GameObject mainCamera;
    [SerializeField] Vector3 cameraPos;
    [SerializeField] GameObject test;
    [SerializeField] GameObject test2;
}
