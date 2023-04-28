using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    public float amplitude = 0.5f; //how much the coin goes up and down
    public float frequency = 1f; //how much time it takes to complete a loop

    //Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    private void Start()
    {
        // Store the starting position of the object
        posOffset = transform.position;
    }

    private void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
    }
}
