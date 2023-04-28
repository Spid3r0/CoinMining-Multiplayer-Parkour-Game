using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomDoorPicker : MonoBehaviour
{
    private int openDoorNumber;
    private string DoorNumberS;
    public static GameObject Doors;
    Collider DoorCollider;
    void Start()
    {
        openDoorNumber = Random.Range(1, 6);
        DoorNumberS = "Door" + openDoorNumber.ToString();
        Doors = GameObject.Find(DoorNumberS);

        DoorCollider = Doors.GetComponent<Collider>();

        DoorCollider.enabled = false;
        Debug.Log("Collider for " + openDoorNumber + " is " + DoorCollider.enabled);
    }

}
