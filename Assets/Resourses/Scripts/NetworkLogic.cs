using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;


public class NetworkLogic : NetworkBehaviour {

    public GameObject world;
    public GameObject movedObjects;
    public GameObject networkManager;

    public int worldSeed;
    string connectAddress = "127.0.0.1";

    //public NetworkVariable<int> worldSeed = new NetworkVariable<int>();
    
    public void SetIp(string ip) {
        connectAddress = ip;
    }

    public void StartClient() {
        NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = connectAddress;
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer() {
        Debug.Log($"Started server...");
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoin;
        NetworkManager.Singleton.StartHost();
        worldSeed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(worldSeed);
        world.GetComponent<WorldLogic>().GenWorld();
        //worldSeed.Value = Random.seed;
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
    void MoveObjectClientRpc(string id, Vector3 position, Quaternion rotation, ClientRpcParams clientRpcParams = default) {
        GetGameObjectByName(id).transform.parent = movedObjects.transform;
        GetGameObjectByName(id).transform.position = position;
        GetGameObjectByName(id).transform.rotation = rotation;
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveObjectServerRpc(string id, Vector3 position, Quaternion rotation){
        if (!world.GetComponent<WorldLogic>().movedObjects.Contains(GetGameObjectByName(id))){
            world.GetComponent<WorldLogic>().movedObjects.Add(GetGameObjectByName(id));
        }
        world.GetComponent<WorldLogic>().movedObjects[world.GetComponent<WorldLogic>().movedObjects.IndexOf(GetGameObjectByName(id))].transform.position = position;
        world.GetComponent<WorldLogic>().movedObjects[world.GetComponent<WorldLogic>().movedObjects.IndexOf(GetGameObjectByName(id))].transform.rotation = rotation;
        MoveObjectClientRpc(id, position, rotation);
    }

    [ServerRpc(RequireOwnership = false)]
    void GetMovedObjectsServerRpc(ClientRpcParams clientRpcParams){
        foreach (GameObject movedObject in world.GetComponent<WorldLogic>().movedObjects) {
            Debug.Log(movedObject);
            MoveObjectClientRpc(movedObject.name, movedObject.transform.position, movedObject.transform.rotation, clientRpcParams);
        }
    }

    GameObject GetGameObjectByName(string name){
        return GameObject.Find(name);
        /*for (int i = 0; i < world.GetComponent<WorldLogic>().objects.Count; i++) {
            if(world.GetComponent<WorldLogic>().objects[i].name == name) {
                return world.GetComponent<WorldLogic>().objects[i];
            }
        }
        return null;*/
    }

}
