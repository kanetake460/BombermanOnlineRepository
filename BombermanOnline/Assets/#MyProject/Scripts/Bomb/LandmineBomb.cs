using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// �����e
// PoisonBomb�EHealBomb�Ɏg�p
public class LandmineBomb : BombBase
{
    // ===�C�x���g�֐�================================================
    private void Update()
    {
        if (isLocal == false) return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isLocal == false) return;
        if (collision.gameObject.CompareTag("Player"))
            Fire();

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
