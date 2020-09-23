using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    public float speedS = 0.1f;
    public float maxVisiblity = 0.5f; 

    private Material material;
    private float changingSpeed = 0;

    private void Start()
    {
        this.material = GetComponent<MeshRenderer>().material;

        Color newColor = this.material.color;

        newColor.a = 0;

        this.material.color = newColor;
    }

    private void FixedUpdate()
    {
        Debug.Log(this.material.color.a); 
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
}
