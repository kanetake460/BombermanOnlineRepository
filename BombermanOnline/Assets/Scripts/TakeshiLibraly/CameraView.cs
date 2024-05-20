using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraView : Base
{
    void Start()
    {
        parent = new GameObject("MainCameraPrent");
        Trafo.parent = parent.transform;
    }

    void Update()
    {
        parent.transform.position = playerTransform.position;
        parent.transform.localRotation = playerTransform.localRotation;
        CameraViewport();
    }

    [SerializeField] Transform playerTransform;
    GameObject parent;


    /// <summary>
    /// カメラの角度制限をします(上下)
    /// </summary>
    /// <param name="q">制限したいquoternion</param>
    /// <param name="minX">下の角度制限</param>
    /// <param name="maxX">上の角度制限</param>
    /// <returns>角度</returns>
    private Quaternion ClampRotation(Quaternion q, float minX, float maxX)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }


    ///<summary>カメラの視点移動関数(上下の視点移動)</summary>>
    ///<param name="Xsensityvity"<pragma>視点移動スピード</pragma>
    ///<param name="minX"<pragma>下の角度制限</pragma>
    ///<param name="maxX"<pragma>上の角度制限</pragma>
    public void CameraViewport(float Xsensityvity = 3f, float minX = -90f, float maxX = 90f)
    {
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;       // マウスの座標代入
        transform.localRotation *= Quaternion.Euler(-yRot, 0, 0);     // 角度代入

        transform.localRotation = ClampRotation(transform.localRotation, minX, maxX);           // 角度制限
    }
}
