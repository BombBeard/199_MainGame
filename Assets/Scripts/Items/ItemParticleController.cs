using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemParticleController : MonoBehaviour {

    [Header("Controls")]
    [SerializeField] public float detectionRadius = 2.4f;

    [Header("Components")]
    public GameObject itemApproachParticles;
    private ParticleSystem approachParticles;
    public Mesh EmitterShapeMesh;

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
	}

    public void PlayApproach()
    {
        if(!approachParticles.isPlaying)
            approachParticles.Play();
    }
    public void StopApproach()
    {
        if(approachParticles.isPlaying)
            approachParticles.Stop();
    }
}
