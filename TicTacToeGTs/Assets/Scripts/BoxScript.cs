using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    private Material material;
    private float changingSpeed = 0; 

    private void Start()
    {
        this.material = GetComponent<MeshRenderer>().material; 
    }

    private void Update()
    {
        Color newColor = material.color;

        newColor.a += changingSpeed * Time.deltaTime;
        if (newColor.a >= 256)
        {
            newColor.a = 256;
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
        changingSpeed = 0.1f; 
    } 

    public void StartFading()
    {
        changingSpeed = -0.1f; 
    } 
}
