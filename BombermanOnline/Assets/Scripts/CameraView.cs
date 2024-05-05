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
    /// �J�����̊p�x���������܂�(�㉺)
    /// </summary>
    /// <param name="q">����������quoternion</param>
    /// <param name="minX">���̊p�x����</param>
    /// <param name="maxX">��̊p�x����</param>
    /// <returns>�p�x</returns>
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


    ///<summary>�J�����̎��_�ړ��֐�(�㉺�̎��_�ړ�)</summary>>
    ///<param name="Xsensityvity"<pragma>���_�ړ��X�s�[�h</pragma>
    ///<param name="minX"<pragma>���̊p�x����</pragma>
    ///<param name="maxX"<pragma>��̊p�x����</pragma>
    public void CameraViewport(float Xsensityvity = 3f, float minX = -90f, float maxX = 90f)
    {
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;       // �}�E�X�̍��W���
        transform.localRotation *= Quaternion.Euler(-yRot, 0, 0);     // �p�x���

        transform.localRotation = ClampRotation(transform.localRotation, minX, maxX);           // �p�x����
    }
}
