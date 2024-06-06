using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// �����e
// PoisonBomb�EHealBomb�Ɏg�p
public class LandmineBomb : BombBase
{
    // ===�C�x���g�֐�================================================
    private void Update()
    {
        _safeCount--;
    }


    private void OnTriggerStay(Collider other)
    {
        if (isLocal == false) return;

        if (other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag("Explosion"))
            Fire();
    }

    // ===�ϐ�====================================================
    private int _safeCount = 0;
    [SerializeField] int m_safeMaxCount = 120;

    // ===�֐�====================================================
    public void Put(Coord coord,int exploLevel)
    {
        base.Put(coord,exploLevel);
        _safeCount = m_safeMaxCount;
    }


    /// <summary>
    /// �ђʔ��e�̔���
    /// </summary>
    private void Fire()
    {
        // �����A�������Ȃ��J�E���g��0���傫���Ȃ甚�����Ȃ�
        if (_safeCount > 0) return;
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
