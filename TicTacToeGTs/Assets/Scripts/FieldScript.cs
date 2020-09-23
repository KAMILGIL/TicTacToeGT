using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    public GameObject boxPrefab;

    [HideInInspector]
    public GameObject[,,] ar = new GameObject[3, 3, 3];

    private Vector3 delta = Vector3.back + Vector3.down + Vector3.left; 

    private Vector3 IndexToCoordinates(int i, int j, int k)
    {
        return new Vector3(k, i, j) + delta;
    }

    public void StartHidingBoxes()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    ar[i, j, k].GetComponent<BoxScript>().StartFading();
                }
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    ar[i, j, k] = Instantiate(boxPrefab, IndexToCoordinates(i, j, k), Quaternion.identity); 
                }
            }
        }
    }
}
