using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();

        AudioManager.PlayBGM("���r�[BGM",1f);
    }
}
