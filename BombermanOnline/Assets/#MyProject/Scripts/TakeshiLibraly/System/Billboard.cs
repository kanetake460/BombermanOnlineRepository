using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

public class Billboard : StrixBehaviour
{
    private Transform mainCameraTransform;

    private void Start()
    {
        if (isLocal == false) { return; }

        // �J�������擾
        mainCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if(isLocal == false) { return; }
        // �J�����������v�Z
        Vector3 lookDir = mainCameraTransform.position - transform.position;
        lookDir.y = 0f; // Y����]�݂̂Ȃ̂ŁAY������0�ɐݒ肷��

        // �I�u�W�F�N�g��Y���̂݃J�����̕����Ɍ�����
        if (lookDir != Vector3.zero)
        {
            transform.forward = -lookDir.normalized;
        }
    }
}
