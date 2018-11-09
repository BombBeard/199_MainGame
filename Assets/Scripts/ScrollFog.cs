using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollFog : MonoBehaviour {

    [Range(-.05f, .05f)]
    public float ScrollX = 0.05f;
    [Range(-.05f, .05f)]
    public float ScrollY = 0.05f;

    private float prevOffsetX;
    private float prevOffsetY;

	// Update is called once per frame
	void Update () {

    float OffsetX = Time.time * ScrollX;
    float OffsetY = Time.time * ScrollY;
    GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(
    Mathf.Sin(OffsetX),
    Mathf.Sin(OffsetY));
/*
        if (Time.time % 2.0f > 1.0f)
        {
            GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(
                Mathf.Sin(OffsetX), 
                Mathf.Sin(OffsetY));
        }
        else if(Time.time % 2.0f < 1.0f)
        {
            GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(
                prevOffsetX = -OffsetX,
                prevOffsetY = -OffsetY);
        }
        */
    }
}
