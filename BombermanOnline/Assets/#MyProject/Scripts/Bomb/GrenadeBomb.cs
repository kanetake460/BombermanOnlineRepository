using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// Άe
// PoisonBombEHealBombΙgp
public class GrenadeBomb : BombBase
{
    // ===CxgΦ================================================

    private void Update()
    {
        BombTimer(PredictionFire, Fire);
        Fly();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLocal == false) return;
        
    }

    // ===Ο====================================================
    [Header("p[^[")]
    [SerializeField] float m_throwSpeed;
    
    private Vector3 _throwDirection;


    // ===Φ====================================================
    
    /// <summary>
    /// eπ°ι
    /// </summary>
    /// <param name="coord">ΐW</param>
    /// <param name="dir">ό«</param>
    public void Throw(Coord coord,Vector3 dir)
    {
        Put(coord,1);
        _throwDirection = dir;
    }


    /// <summary>
    /// eͺςρΕ’­
    /// </summary>
    private void Fly()
    {
        rb.velocity = _throwDirection * m_throwSpeed;
    }

    private void Stop()
    {
        rb.velocity = Vector3.zero;
    }


    /// <summary>
    /// ΡΚeΜ­Ξ
    /// </summary>
    private void Fire()
    {
        // eΜΚu
        map.ActivePredictLandmark(Coord, false);
        
        PlayExplosionEffect(Coord);
        map.BreakStone(Coord);

        Coord fowardCoord = Coord + new Coord((int)_throwDirection.x, (int)_throwDirection.z);
        PlayExplosionEffect(fowardCoord);
        map.BreakStone(fowardCoord);

        isHeld = true;
        CallInActive();

        AudioManager.PlayOneShot("­", 0.3f);
    }


    /// <summary>
    /// eρANeBuiRPCj
    /// </summary>
    public void CallInActive() { RpcToAll(nameof(InActive)); }
    [StrixRpc]
    private void InActive()
    {
        gameObj.SetActive(false);
    }
}
