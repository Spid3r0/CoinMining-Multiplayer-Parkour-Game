using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviourPunCallbacks
{
    public PhotonView playerPrefab;
    public GameObject planeArea;
    public PhotonView coinsPreFab;
    

    public GameObject[] spawnLocation;


    public float secondBetweenSpawns =4;
    float nextSpawnTime;
    
    float moveAreaX ;
    float moveAreaZ;
    Vector3 center ;
    float targetCoordsX ;
    float targetCoordsZ;
    public Vector2 SpawnSizeMinMax;

    
    // Start is called before the first frame update
    void Start()
    {


        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
        

        moveAreaX = planeArea.GetComponent<Renderer>().bounds.size.x / 2;
        moveAreaZ = planeArea.GetComponent<Renderer>().bounds.size.z / 2;
        center = planeArea.GetComponent<Renderer>().bounds.center;

        

    }
    public override void OnConnectedToMaster()
    {
        
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinRandomOrCreateRoom();
        
    }
    public override void OnCreatedRoom()
    {
       

    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Run the code you want to execute on the master client
            Debug.Log("I am the master client!");
            blockSpawnToPosition();
        }


        Debug.Log("Joined a room succesfully!");
       PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        
    }




    void blockSpawnToPosition()
    {


        foreach (GameObject locObj in spawnLocation )
        {

            Vector3 spawnPosition = locObj.transform.position;
            //Debug.Log(spawnPosition);
            
            randomBlockSpawn(spawnPosition);


        }


    }


    void randomBlockSpawn(Vector3 spawnPos)
    {

        
            
            GameObject differentSizedCoin = PhotonNetwork.Instantiate(coinsPreFab.name, spawnPos, Quaternion.identity);

            float Spawnsize = Random.Range(SpawnSizeMinMax.x, SpawnSizeMinMax.y);
            differentSizedCoin.transform.localScale += Vector3.up * Spawnsize;
            Spawnsize = Random.Range(SpawnSizeMinMax.x, SpawnSizeMinMax.y);
            differentSizedCoin.transform.localScale += Vector3.forward * Spawnsize;
            Spawnsize = Random.Range(SpawnSizeMinMax.x, SpawnSizeMinMax.y);
            differentSizedCoin.transform.localScale += Vector3.right * Spawnsize;
        

    }



    // Update is called once per frame
    void Update()
    {


        //if (Time.time > nextSpawnTime)
        //{
        //    targetCoordsX = center.x + Random.Range(-moveAreaX * scale, moveAreaX * scale);
        //    targetCoordsZ = center.z + Random.Range(-moveAreaZ * scale, moveAreaZ * scale);
        //    nextSpawnTime = Time.time + secondBetweenSpawns;
        //    Vector3 spawnPosition = new Vector3(targetCoordsX, 1, targetCoordsZ);


        //    GameObject differentSizedCoin = PhotonNetwork.Instantiate(coinsPreFab.name, spawnPosition, Quaternion.identity);

        //    float Spawnsize = Random.Range(SpawnSizeMinMax.x, SpawnSizeMinMax.y);
        //    differentSizedCoin.transform.localScale += Vector3.up * Spawnsize;
        //    Spawnsize = Random.Range(SpawnSizeMinMax.x, SpawnSizeMinMax.y);
        //    differentSizedCoin.transform.localScale += Vector3.forward * Spawnsize;
        //    Spawnsize = Random.Range(SpawnSizeMinMax.x, SpawnSizeMinMax.y);
        //    differentSizedCoin.transform.localScale += Vector3.right * Spawnsize;
        //    //branch deneme
        //    ////
        //}
       
        
    }
}
