using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour {

    //States
    public bool isLocked = true;
    public bool isOpen = false;
    

    //Editor Controls
    [Range(.1f, 5f)]
    public float openSpeed=.1f;
    [Range(.1f, 5f)]
    public float closeSpeed=.1f;

    //Misc
    Animator animator;
    MeshFilter meshFilter;
    ParticleSystem unlockSystem;
    SphereCollider sphereCollider;


	// Use this for initialization
	void Start () {
        if ((animator = GetComponent<Animator>() ) == null)
        {
            animator = new Animator();
        }
		if((meshFilter = GetComponent<MeshFilter>() ) == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }
        if((unlockSystem = GetComponent<ParticleSystem>() ) == null)
        {
            unlockSystem = new ParticleSystem();
        }
        if((sphereCollider = GetComponent<SphereCollider>()) == null)
        {
            sphereCollider = new SphereCollider();
        }

        sphereCollider.isTrigger = true;
        sphereCollider.radius = 1.4f;


	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Item>() != null) isLocked = false;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Item>() != null) isLocked = true;
    }

    // Update is called once per frame
    void Update () {
        if (isLocked == false && isOpen == false && Input.GetButtonDown("Interact"))
        {
            isOpen = true;
            animator.SetBool("isOpen", isOpen);
        }
        else if (isLocked == false && isOpen == true && Input.GetButtonDown("Interact"))
        {
            isOpen = false;
            animator.SetBool("isOpen", isOpen);
        }


    }

}
