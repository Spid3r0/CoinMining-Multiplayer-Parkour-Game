using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class endTimer : MonoBehaviourPunCallbacks
{
    private float timeLeft;
    [SerializeField]
    public float timeDuration;
    [SerializeField]
    private float startTimeDuration;
    [SerializeField]
    private GameObject TimeisUp;
    
    private GameObject EntranceGate;
    [SerializeField]
    private TextMeshProUGUI firstMinute, secondMinute, separator, firstSecond, secondSecond;
    private int numberOfPlayer=0;

    private bool started = false;

 
    public PickUp pickUp;

    void Start()
    {
       

        pickUp = gameObject.GetComponentInParent<PickUp>();
        timeLeft = timeDuration;
        numberOfPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        pickUp.SetDifficulty();
        if (!PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SyncTime", RpcTarget.All, timeLeft);
        }

    }

    void Update()
    {

        if (numberOfPlayer != PhotonNetwork.CurrentRoom.PlayerCount)
        {
            pickUp.SetDifficulty();
            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SyncTime", RpcTarget.All, timeLeft);
            }
            numberOfPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        }


        if (!photonView.IsMine)
        {
            Destroy(gameObject);
        }

        if (startTimeDuration > 0)
        {
            startTimeDuration -= Time.deltaTime;
           
            UpdateTimeDisplay(startTimeDuration);
        }
        else
        {//
            if (GameObject.Find("EntranceBlocker") == null)
            {
                
            }
            else
            {
                EntranceGate = GameObject.Find("EntranceBlocker");
                EntranceGate.SetActive(false);
            }
            

            if (timeLeft > 0 )
            {
                //
                timeLeft -= Time.deltaTime;
                UpdateTimeDisplay(timeLeft);
            }
            else
            {
                timeLeft = 0;
                UpdateTimeDisplay(timeLeft);
                TimeisUp.SetActive(true);
            }
        }
    }


    [PunRPC]
    void SyncTime(float time)
    {
        started = true;
        timeLeft = time - startTimeDuration;
    }



    private void UpdateTimeDisplay(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime = string.Format("{00:00}{1:00}", minutes, seconds);
        firstMinute.text = currentTime[0].ToString();
        secondMinute.text = currentTime[1].ToString();
        firstSecond.text = currentTime[2].ToString();
        secondSecond.text = currentTime[3].ToString();
    }
}
