using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimmerLight : MonoBehaviour {

    [Range(.01f, 5f)]
    public float frequency = 1f;
    [Range(.01f, 4f)] 
    public float intensity = 1f;
    GameObject player;
    Light light;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        updateArchLight(player);
	}

    void updateArchLight(GameObject player)
    {
        float distance = 0f;
        //find the distance between this object and the player.
        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        Debug.Log("Distance to Player: " + distance);
        //do this if further than 10M
        if (distance > 12)
        {
            //sine pulse

            light.intensity = Mathf.Sin(frequency * Time.time) * intensity;
        }
        //do this if further than 2M
        /*
        else if (distance == 12f)
        {
            float i = 1f;
            while(i > 0f)            
            {
                light.intensity
            }
        }
        */
        else if (distance >= 4 && distance < 12)
        {
            //growing intensity
            light.intensity = (distance / 12f) - .15f;
        }
        //do this if closer than 2M
        else
        {
            //dim flicker
            light.intensity = .2f;
        }
    }
}
