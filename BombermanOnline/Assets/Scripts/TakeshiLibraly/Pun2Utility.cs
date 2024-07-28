using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pun2Utility
{
    public static void ChangeSceneToAll(PhotonView photonView, string sceneName)
    {
        if(SceneManager.GetSceneByName(sceneName) == null)
            Debug.Log("その名前のシーンは存在しません。");
        if (photonView == null)
            Debug.Log("フォトンビューがNullです");
        
        photonView.RPC("ChangeScene",RpcTarget.All,sceneName);
    }


    public static void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName, new RoomOptions(), TypedLobby.Default);
    }

    public static void JoinOrCreateRoom(string roomName)
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("既につながってます");
            return;
        }
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(), TypedLobby.Default);
    }

}
