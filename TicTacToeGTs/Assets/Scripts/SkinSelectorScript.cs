using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelectorScript : MonoBehaviour
{
    public GameObject skin1;
    public GameObject skin2;
    public GameObject skin3;

    private GameObject currentSkin; 

    private void Start()
    {
        currentSkin = Instantiate(skin1, Vector3.zero, Quaternion.identity); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Destroy(currentSkin);
            currentSkin = Instantiate(skin1, Vector3.zero, Quaternion.identity); 
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Destroy(currentSkin);
            currentSkin = Instantiate(skin2, Vector3.zero, Quaternion.identity);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Destroy(currentSkin);
            currentSkin = Instantiate(skin3, Vector3.zero, Quaternion.identity); 
        }
    }
}
