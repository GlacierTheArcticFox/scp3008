using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NetworkLogic : NetworkBehaviour {

    [Header("Game")]
    public GameObject world;
    public GameObject movedObjects;
    public GameObject networkManager;
    public GameObject uiManager;
    public GameObject uiCamera;
    public GameObject shopMusicPlayer;
    
    [Header("UI")]
    public GameObject ipField;
    public GameObject nameField;
    public GameObject hostNameField;
    public GameObject joiningScreen;
    public GameObject disconnectedScreen;

    public string uname;

    public int worldSeed;
    string connectAddress = "127.0.0.1";

    public void StartClient() {
        uname = nameField.GetComponent<InputField>().text;
        if (ipField.GetComponent<InputField>().text != "") {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = ipField.GetComponent<InputField>().text;
        } else {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = "127.0.0.1";
        }
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer() {
        uname = hostNameField.GetComponent<InputField>().text;
        Debug.Log($"Started server...");
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoin;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientLeft;
        NetworkManager.Singleton.StartHost();
        worldSeed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(worldSeed);
        world.GetComponent<WorldLogic>().GenWorld();
        shopMusicPlayer.GetComponent<AudioSource>().mute = false;
        uiManager.GetComponent<UIManager>().inGame = true;
    }

    public override void OnNetworkDespawn() {
        Debug.Log("Lost connection to server");
        disconnectedScreen.SetActive(true);
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Disconnect(){
        NetworkManager.Singleton.Shutdown();
        ReloadScene();
    }

    public void OnClientLeft(ulong ClientId) {
        Debug.Log($"{ClientId} Joined the Game");
    }

    public void OnClientJoin(ulong ClientId) {
        Debug.Log($"{ClientId} Joined the Game");
        if (ClientId != 0 && IsServer){
            ClientRpcParams clientRpcParams = new ClientRpcParams {
                Send = new ClientRpcSendParams {
                    TargetClientIds = new ulong[]{ClientId}
                }
            };
            GenWorldClientRpc(worldSeed, clientRpcParams);
        }
    }

    [ClientRpc]
    void GenWorldClientRpc(int seed, ClientRpcParams clientRpcParams = default) {
        worldSeed = seed;
        Random.InitState(worldSeed);
        world.GetComponent<WorldLogic>().GenWorld();
        GetMovedObjectsServerRpc(clientRpcParams);
    }

    [ClientRpc]
    void MoveObjectClientRpc(int id, Vector3 position, Quaternion rotation, ClientRpcParams clientRpcParams = default) {
        GetGameObjectById(id).transform.parent = movedObjects.transform;
        GetGameObjectById(id).transform.position = position;
        GetGameObjectById(id).transform.rotation = rotation;
    }

    [ClientRpc]
    void ReadyClientRpc(int track, float time, ClientRpcParams clientRpcParams = default){
        joiningScreen.SetActive(false);
        uiManager.GetComponent<UIManager>().inGame = true;
        GameObject.Find("LocalPlayer").GetComponent<Player>().SetPlayerNameServerRpc(uname);  
        world.GetComponent<WorldLogic>().songIndex = track;
        shopMusicPlayer.GetComponent<AudioSource>().mute = false;
        shopMusicPlayer.GetComponent<AudioSource>().clip = world.GetComponent<WorldLogic>().songs[track];
        shopMusicPlayer.GetComponent<AudioSource>().time = time;
        shopMusicPlayer.GetComponent<AudioSource>().Play();
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveObjectServerRpc(int id, Vector3 position, Quaternion rotation){
        if (!world.GetComponent<WorldLogic>().movedObjects.Contains(GetGameObjectById(id))){
            world.GetComponent<WorldLogic>().movedObjects.Add(GetGameObjectById(id));
        }
        world.GetComponent<WorldLogic>().movedObjects[world.GetComponent<WorldLogic>().movedObjects.IndexOf(GetGameObjectById(id))].transform.position = position;
        world.GetComponent<WorldLogic>().movedObjects[world.GetComponent<WorldLogic>().movedObjects.IndexOf(GetGameObjectById(id))].transform.rotation = rotation;
        MoveObjectClientRpc(id, position, rotation);
    }

    [ServerRpc(RequireOwnership = false)]
    void GetMovedObjectsServerRpc(ClientRpcParams clientRpcParams){
        foreach (GameObject movedObject in world.GetComponent<WorldLogic>().movedObjects) {
            MoveObjectClientRpc(int.Parse(movedObject.name), movedObject.transform.position, movedObject.transform.rotation, clientRpcParams);
        }
        ReadyClientRpc(world.GetComponent<WorldLogic>().songIndex, shopMusicPlayer.GetComponent<AudioSource>().time, clientRpcParams);
    }

    GameObject GetGameObjectById(int id){
        return world.GetComponent<WorldLogic>().objects[id];
        /*for (int i = 0; i < world.GetComponent<WorldLogic>().objects.Count; i++) {
            if(world.GetComponent<WorldLogic>().objects[i].name == name) {
                return world.GetComponent<WorldLogic>().objects[i];
            }
        }
        return null;*/
    }

}
