using UnityEngine;
using UnityEngine.UIElements;

public class parkourTransform : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 targetPos1;
    [SerializeField] private Vector3 targetPos2;
    int PositionFlag;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PositionFlag == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos1, _speed * Time.deltaTime);

            if(transform.position == targetPos1)
            {
                PositionFlag = 0;
            }

        }
        else if (PositionFlag == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos2, _speed * Time.deltaTime);

            if (transform.position == targetPos2)
            {
                PositionFlag = 1;
            }
        }

    }
}
