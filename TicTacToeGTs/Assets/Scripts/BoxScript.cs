using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [HideInInspector]
    int i, j, k; 

    public float speedS = 0.1f;
    public float maxVisiblity = 0.5f;

    public GameObject redPointPrefab;
    public GameObject bluePointPrefab;

    [HideInInspector]
    public BoxType boxType = BoxType.None;

    private GameObject skin; 

    private Material material;
    private float changingSpeed = 0;

    public void SetIndex(int i, int j, int k)
    {
        this.i = i;
        this.j = j;
        this.k = k; 
    }

    private void Start()
    {
        this.material = GetComponent<MeshRenderer>().material;

        Color newColor = this.material.color;

        newColor.a = 0;

        this.material.color = newColor;

        this.boxType = BoxType.None;
    }

    private void FixedUpdate()
    {
        Color newColor = material.color;

        newColor.a += changingSpeed;
        if (newColor.a >= maxVisiblity)
        {
            newColor.a = maxVisiblity;
            changingSpeed = 0; 
        } 
        else if (newColor.a <= 0)
        {
            newColor.a = 0;
            changingSpeed = 0; 
        }

        material.color = newColor; 
    }

    public void StartGlowing()
    {
        changingSpeed = speedS; 
    } 

    public void StartFading()
    {
        changingSpeed = -speedS; 
    } 

    public void MakePlayerMove()
    {
        if (boxType == BoxType.None)
        {
            boxType = BoxType.Blue; 
        } 
        SpawnSkin(); 
    }

    public void MakeComputerMove()
    {
        if (boxType == BoxType.None)
        {
            boxType = BoxType.Red;
        }
        SpawnSkin();
    }

    public void SpawnSkin()
    {
        if (boxType == BoxType.Blue)
        {
            skin = Instantiate(bluePointPrefab, Vector3.zero, Quaternion.identity);
            skin.transform.parent = transform;
            skin.transform.localPosition = Vector3.zero;
        } 
        else if (boxType == BoxType.Red)
        {
            skin = Instantiate(redPointPrefab, Vector3.zero, Quaternion.identity);
            skin.transform.parent = transform;
            skin.transform.localPosition = Vector3.zero;
        }
    }

    public void Clear()
    {
        boxType = BoxType.None;
        Destroy(skin); 
    } 
}
