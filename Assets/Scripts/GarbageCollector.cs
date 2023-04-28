using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour
{
    [SerializeField] private Vector3 respawnLocation;
    private void OnTriggerEnter(Collider other)
    {
        //her oyuncunun vector3 bir respawn pointi olacak eðer deðerse checkpointe yeniden atanacak
        other.transform.position = respawnLocation;
        /*
        if (other.TryGetComponent<Player>(out Player player))
        {
            //her oyuncunun vector3 bir respawn pointi olacak eðer deðerse checkpointe yeniden atanacak
            other.transform.position = respawnLocation;
        } 
        */
    }
}
