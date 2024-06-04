using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpesialBomb : MonoBehaviour
{
    [Header("ŠeŽíƒ{ƒ€")]
    public GrenadeBomb grenadeBomb;
    public LandmineBomb landmineBomb;
    public NormalBomb RemoteBomb;
    public CrossBomb crossBomb;
    public NormalBomb persistentBomb;
    public MegatonBomb megatonBomb;
    public PierceBomb pierceBomb;
    public NormalBomb transparentBomb;
    public IceBomb iceBomb;
    public FogBomb poisonBomb;
    public FogBomb healBomb;

    public enum BombType
    {
        GrenadeBomb,
        LandmineBomb,
        RemoteBomb,
        CrossBomb,
        PersistentBomb,
        TransparentBomb,
        IceBomb,
        MegatonBomb,
        PierceBomb,
        HealBomb,
        PoisonBomb,
    };

    public void Put(BombType type)
    {

    }



}
