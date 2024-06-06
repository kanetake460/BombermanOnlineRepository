using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// �����e
// PoisonBomb�EHealBomb�Ɏg�p
public class GrenadeBomb : BombBase
{
    // ===�C�x���g�֐�================================================

    private void Update()
    {
        Fly();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLocal == false) return;
        Fire();
    }

    // ===�ϐ�====================================================
    [Header("�p�����[�^�[")]
    [SerializeField] float m_throwSpeed;
    
    private Vector3 _throwDirection;
    private Coord _instanceCoord;


    // ===�֐�====================================================
    
    /// <summary>
    /// ���e�𓊂��鏈��
    /// </summary>
    /// <param name="coord">���W</param>
    /// <param name="dir">����</param>
    public void Throw(Coord coord,Vector3 dir)
    {
        Put(coord,1);
        _instanceCoord = coord;
        _throwDirection = dir;
    }


    /// <summary>
    /// ���e�����ł�������
    /// </summary>
    private void Fly()
    {
        rb.velocity = _throwDirection * m_throwSpeed;
    }


    /// <summary>
    /// �ђʔ��e�̔���
    /// </summary>
    private void Fire()
    {
        // �����A���݂̍��W���A���������W�Ȃ�A�������Ȃ�
        if (_instanceCoord == Coord) return;
        // ���e�̈ʒu
        map.ActivePredictLandmark(Coord, false);
        PlayExplosionEffect(Coord);
        map.BreakStone(Coord);

        isHeld = true;
        CallInActive();

        AudioManager.PlayOneShot("����", 0.3f);
    }


    /// <summary>
    /// ���e��A�N�e�B�u�iRPC�j
    /// </summary>
    public void CallInActive() { RpcToAll(nameof(InActive)); }
    [StrixRpc]
    private void InActive()
    {
        gameObj.SetActive(false);
    }
}
