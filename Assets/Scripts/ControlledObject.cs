using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledObject : MonoBehaviour
{
    private Vector3 initialPos;
    [Range(0,3)]
    public int objectType;

    private void OnEnable()
    {
        initialPos = transform.position;
        objectType = Random.Range(0, 3);
        Setup(objectType);
    }

    public void RestetPosition()
    {
        transform.position = initialPos;
    }

    private void Setup(int colorCode)
    {
        Material mt = GetComponentInChildren<MeshRenderer>().material;
        switch (colorCode)
        {
            case 0:
                mt.color = Color.red;
                break;
            case 1:
                mt.color = Color.white;
                break;
            case 2:
                mt.color = Color.blue;
                break;
            case 3:
                mt.color = Color.yellow;
                break;
            default:
                mt.color = Color.grey;
                break;
        }
    }
}
