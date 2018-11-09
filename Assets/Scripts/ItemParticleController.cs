using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class ItemParticleController : MonoBehaviour {

    [Header("Controls")]
    [SerializeField] public float detectionRadius = 2.4f;

    [Header("Components")]
    public GameObject itemApproachParticles;
    private ParticleSystem approachParticles;
    public Mesh EmitterShapeMesh;
    public SphereCollider detectorCollider;

	// Use this for initialization
	void Start () {
        approachParticles = Instantiate(itemApproachParticles, gameObject.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        approachParticles.transform.SetParent(gameObject.transform);
        approachParticles.Stop();
        if (EmitterShapeMesh == null) {
            ParticleSystem.ShapeModule newShape = approachParticles.shape;
            newShape.mesh = gameObject.GetComponentInChildren<MeshFilter>().mesh;
        }
        else
        {
            ParticleSystem.ShapeModule newShape = approachParticles.shape;
            newShape.mesh = EmitterShapeMesh;
        }
        detectorCollider = GetComponent<SphereCollider>();
        detectorCollider.radius = detectionRadius;
        detectorCollider.isTrigger = true;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            //approachParticles.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //approachParticles.Stop();
        }
    }
    // Update is called once per frame
    void Update ()
    {
    }

    public void PlayParticles()
    {
        Debug.Log(gameObject.name + "Should Play Particles");
        if(!approachParticles.isPlaying)
            approachParticles.Play();

    }
    public void StopParticles()
    {
        Debug.Log(gameObject.name + "Should be stopped");
        if(approachParticles.isPlaying)
            approachParticles.Stop();

    }
}
