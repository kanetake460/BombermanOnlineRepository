using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

public class Billboard : StrixBehaviour
{
    private Transform mainCameraTransform;

    private void Start()
    {
        if (isLocal == false) { return; }

        // カメラを取得
        mainCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if(isLocal == false) { return; }
        // カメラ方向を計算
        Vector3 lookDir = mainCameraTransform.position - transform.position;
        lookDir.y = 0f; // Y軸回転のみなので、Y成分を0に設定する

        // オブジェクトのY軸のみカメラの方向に向ける
        if (lookDir != Vector3.zero)
        {
            transform.forward = -lookDir.normalized;
        }
    }
}
