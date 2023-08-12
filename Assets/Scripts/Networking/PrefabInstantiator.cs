using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PrefabInstantiator : NetworkBehaviour
{
    private void Awake() {
        
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<NetworkObject>().Spawn();
        Debug.Log("Instantiated prefab");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
