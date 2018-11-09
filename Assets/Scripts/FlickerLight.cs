using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour {

    [Range(0f, 100f)]
    public float flickerRate = 5f;
    public float intensity = 1f;
    public int seed = 1;
    public float flickerPeriod = 0f;
    private float timePassed = 0.0f;

    private void Start()
    {
        Random.InitState(seed);
    }
    // Update is called once per frame
    void Update () {
        timePassed += Time.deltaTime;
        if (timePassed > flickerPeriod)
        {
            GetComponent<Light>().intensity = intensity + ( flickerRate * Time.deltaTime * Random.value);
            timePassed = 0f;
            //timeRandom = Random.value;
        }

	}
}
