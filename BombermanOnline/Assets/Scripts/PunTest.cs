using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PunTest : MonoBehaviourPunCallbacks
{

    void Start()
    {
        //旧バージョンでは引数必須でしたが、PUN2では不要です。
        PhotonNetwork.ConnectUsingSettings();
    }

    void OnGUI()
    {
        //ログインの状態を画面上に出力
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("既につながってます");
                return;
            }
                PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(photonView == null)
            {
                Debug.Log("ふぉとん");
            }
            Pun2Utility.ChangeSceneToAll(photonView,"GameScene");
        }
    }

    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    //ルームに入室前に呼び出される
    public override void OnConnectedToMaster()
    {
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        Debug.Log("入室前");
    }

    //ルームに入室後に呼び出される
    public override void OnJoinedRoom()
    {
        Debug.Log("入室後");
        //Playerを生成する座量をランダムに決める
        var position = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f));

        //Resourcesフォルダから"Player"を探してきてそれを生成
        //PhotonNetwork.Instantiate("Player", position, Quaternion.identity);
    }


}

