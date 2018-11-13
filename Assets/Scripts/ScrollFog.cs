using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollFog : MonoBehaviour {

    [Range(-1f, 1f)]
    public float ScrollX = 0f;
    [Range(-1f, 1f)]
    public float ScrollY = 0f;
    public bool SineScroll = false;

    private float prevOffsetX;
    private float prevOffsetY;
    Vector2 mainTexOffsetPrev;

    // Update is called once per frame
    private void Start()
    {
        mainTexOffsetPrev = GetComponent<MeshRenderer>().sharedMaterial.mainTextureOffset;

    }
    void Update () {

        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * ScrollY;
        if (SineScroll) {
            GetComponent<MeshRenderer>().sharedMaterial.mainTextureOffset = new Vector2(
            Mathf.Sin(OffsetX),
            Mathf.Sin(OffsetY));
        }
        else
        {
            GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
        }
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
