using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using TMPro;

public class PickUp : MonoBehaviourPunCallbacks
{
    public float collectedValue = 0;



    public Image[] uiblockHolder;
    public GameObject collectedUI;
    public int coins;
    public int playerTank;
    float startTime = 0;

    [SerializeField]
    float cooldown;

    public float blockCollectedSize;
    bool firstSpin = true;
    bool playerWon = false;
    
    public float transactionFee = 0;
    
    [SerializeField]
    private TextMeshProUGUI transactionFeeTracker, diffTracker, earnedAmount;
        
    public float difficultyboundaryX;
    public float difficultyboundaryY;
    public float difficulty;

    public GameObject SHAUI;
    public GameObject CorrectHash;
    public GameObject FailedHash;
    public GameObject Hashing;

    [SerializeField]
    private TextMeshProUGUI HashText;



    private endTimer xendTimer;

    // Start is called before the first frame update
    void Start()
    {


        xendTimer =  gameObject.GetComponentInChildren<endTimer>();
        // difficulty = (int)Random.Range(difficultyboundaryX, difficultyboundaryY);
        
        diffTracker.text = "Diffculty: "+difficulty.ToString();
        
        if (!photonView.IsMine)
        {
            //int viewID = collectedUI.gameObject.GetComponent<PhotonView>().ViewID;

          
              Destroy(collectedUI);
           
       

            //Destroy(collectedUI);
        }
       
       // someFunc();
    }


    public void SetDifficulty()
    {
       // Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        int diff = PhotonNetwork.CurrentRoom.PlayerCount;


      
            if (diff == 1)
            {
                difficulty = (int)Random.Range(1, 5);
                xendTimer.timeDuration = 120;
                difficultyboundaryX = 1;
                difficultyboundaryY = 5;
            }
            else if (diff == 2)
            {
                difficulty = (int)Random.Range(5, 10);
                xendTimer.timeDuration = 120;
                difficultyboundaryX = 5;
                difficultyboundaryY = 10;
            }
            else if (diff == 3)
            {
                difficulty = (int)Random.Range(10, 20);
                xendTimer.timeDuration = 300;
                difficultyboundaryX = 10;
                difficultyboundaryY = 20;
            }
            else
            {
                difficulty = (int)Random.Range(15, 20);
                xendTimer.timeDuration = 600;
                difficultyboundaryX = 15;
                difficultyboundaryY = 20;
            }
        
        

        Debug.Log("Diff:"+difficulty+"Time:" + xendTimer.timeDuration );
        //diffTracker.text = difficulty.ToString();

        if (!PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SyncDiff", RpcTarget.All, difficulty);
           // photonView.RPC("SyncDiff", RpcTarget.MasterClient, difficulty);
        }

    }

    [PunRPC]
    void SyncDiff(float diff)
    {
        
        difficulty = diff ;
        diffTracker.text = "Diffculty: " + difficulty.ToString();
    }


    public void OnTriggerEnter(Collider Col) {

        if (photonView.IsMine)
        {
            if (Col.gameObject.tag == "Coin" && uiblockHolder[19].GetComponent<Slider>().value != 10)
            {
                int viewID = Col.gameObject.GetComponent<PhotonView>().ViewID;
                Debug.Log("Coin Collected!");
                blockCalculation(Col.gameObject);
                blockValueCalculation(Col.gameObject);
                someFunc();
                coins++;
                //Destroy(Col.gameObject);



                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(Col.gameObject);
                }
                else
                {

                    viewID = Col.GetComponent<PhotonView>().ViewID;
                    this.photonView.RPC("DestroyHelpOfMaster", RpcTarget.MasterClient, viewID);

                }



            }
        }
   




    }

    [PunRPC]
    void DestroyHelpOfMaster(int viewID)
    {
        PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
        
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {

            if (photonView.IsMine)
            {
                collectedUI.SetActive(!collectedUI.activeSelf);
            }  
          
        }



        if (Input.GetKeyDown(KeyCode.B))
        {

            if (photonView.IsMine) { 
            blockInventoryCaculation();
            if (blockCollectedSize >= 200 )
            {
                spinWheel();
            }
            blockCollectedSize = 0;
            }
        }

        if (photonView.IsMine)
        {
            if (playerWon == false)
            {
                if (Time.time > startTime + (cooldown - 1.5))
                {
                    Hashing.SetActive(false);
                    FailedHash.SetActive(true);
                }

                if (Time.time > startTime + cooldown)
                {
                    FailedHash.SetActive(false);
                }
            }
            else
            {
                Hashing.SetActive(false);
                CorrectHash.SetActive(true);
            }
        }
    }

    void blockInventoryCaculation()
    {
        
        for (int i=0; i< 20;i++)
        {
           //Debug.Log(uiblockHolder[i].GetComponent<Slider>().value.ToString());
          
             blockCollectedSize += uiblockHolder[i].GetComponent<Slider>().value;
        }

        Debug.Log(blockCollectedSize);

    }

    void spinWheel()
    {
        string guessHashedData;
        string dataHashable;
        string hiddenHashedData;
        SHAUI.SetActive(true);
        Debug.Log(cooldown);
        if(playerWon == false) {
            if (Time.time > startTime + cooldown)
            {
                Hashing.SetActive(true);
                FailedHash.SetActive(false);
                startTime = Time.time;
                cooldown = 4;
                if (firstSpin)
                {
                    dataHashable = difficulty.ToString() + collectedValue.ToString();
                    hiddenHashedData = Hash(dataHashable);
                    
                    Debug.Log("Hidden data :" + hiddenHashedData);
                }


                Debug.Log(difficulty);


                int guessedDif = (int)Random.Range(difficultyboundaryX, difficultyboundaryY);


                dataHashable = guessedDif.ToString() + collectedValue.ToString();

                guessHashedData = Hash(dataHashable);
                
                Debug.Log("Guess data :" + guessHashedData);

                if (guessedDif == difficulty)
                {
                    playerWon = true;
                    for (int i = 0; i < difficulty; i++)
                    {
                        guessHashedData = "0" + guessHashedData;
                    }
                    HashText.text = guessHashedData;
                    earnedAmount.text = "You Won! Earned " + transactionFee.ToString() + " Amount of Bitcoin";
                    Debug.Log("YOU WON");
                }
            }
        } 
        
    }

    string Hash(string data)
    {
        byte[] textToBytes = Encoding.UTF8.GetBytes(data);
        SHA256 nSHA256 = SHA256.Create();
        byte[] hashValue = nSHA256.ComputeHash(textToBytes);

        return HexStringFromHash(hashValue);
    }


    string HexStringFromHash(byte[] hash)
    {
        string temp = "";
        foreach (byte b in hash)
            temp += b.ToString("x2");

        return temp;
    }



    void blockValueCalculation(GameObject block)
    {


        float blockY = block.transform.localScale.y;
        float blockZ = block.transform.localScale.z;
        float value =  blockY * blockZ;
        Debug.Log((int)(value * 100));
        collectedValue += (int)(value * 100);
        
        transactionFee += block.transform.localScale.y * 0.01f;
        transactionFeeTracker.text = transactionFee.ToString();
    }

    void blockCalculation(GameObject block) {


        float blockX = block.transform.localScale.x;
        float blockY = block.transform.localScale.y;
        float blockZ = block.transform.localScale.z;
        float size = blockX * blockY * blockZ;
        Debug.Log(blockX);
        Debug.Log(blockY);
        Debug.Log(blockZ);
        Debug.Log("size: " + ((int)(size * 100)));
        playerTank = ((int)(size * 100));
    }

    void someFunc()
    {
      
        while (playerTank != 0 && uiblockHolder[19].GetComponent<Slider>().value != 10) 
        {

            for (int i = 0; i < 20; i++)
            {
                if (uiblockHolder[i].GetComponent<Slider>().value == 10)
                {
                    continue;
                }
                int temp = ((int)uiblockHolder[i].GetComponent<Slider>().value);
                int space = 10 - temp;
                if (space > playerTank)
                {
                    uiblockHolder[i].GetComponent<Slider>().value += playerTank;
                    playerTank -= playerTank;
                }
                else
                {
                    uiblockHolder[i].GetComponent<Slider>().value += space;
                    playerTank -= space;
                }

            }


        }


    }



}
