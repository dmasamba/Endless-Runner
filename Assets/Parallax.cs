using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    private GameObject cam;

    public float parallaxEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");

        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x) * (1 - parallaxEffect);  //how far we moved relatively to camera
        
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + length) //move background to the right
        {
            startPos = startPos + length;
        }
        else if (temp < startPos - length) // Not necessary for this game. Use if want to move background to the lelft
        {
            startPos = startPos - length;
        }
    }
}
