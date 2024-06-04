using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// �����e
// PoisonBomb�EHealBomb�Ɏg�p
public class GrenadeBomb : BombBase
{
    // ===�C�x���g�֐�================================================
    private void Update()
    {
        if (isLocal == false) return;
    }

    // ===�ϐ�====================================================
    [Header("�p�����[�^�[")]
    [SerializeField] float m_throwSpeed;
    
    private Vector3 _throwDirection;


    // ===�֐�====================================================
    
    /// <summary>
    /// ���e�𓊂��鏈��
    /// </summary>
    /// <param name="coord">���W</param>
    /// <param name="dir">����</param>
    private void Throw(Coord coord,Vector3 dir)
    {
        Put(coord,1);
        _throwDirection = dir;
    }


    /// <summary>
    /// ���e�����ł�������
    /// </summary>
    private void Fly()
    {

    }


    /// <summary>
    /// �ђʔ��e�̔���
    /// </summary>
    private void Fire()
    {
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
        Debug.Log("�C���A�N�e�B�u");
        gameObj.SetActive(false);
    }
}