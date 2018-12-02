using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour {

    //States
    public bool isLocked = true;
    public bool isOpen = false;
    public bool isInteractable = false;
    public bool isProppedOpen = false;


    //Editor Controls
    public GameManager.ROOM_LIST roomType;
    [Range(1f, 5f)]
    [SerializeField]
    float interactionRadius = 2.5f;
    [Range(.1f, 5f)]
    public float openSpeed=.1f;
    [Range(.1f, 5f)]
    public float closeSpeed=.1f;
    [SerializeField]
    public int numItemsSoldToUnlock = 2;
    public int numItemsSold = 0;

    //Misc
    Animator animator;
    MeshFilter meshFilter;
    ParticleSystem unlockSystem;

    GameObject player;

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
        player = GameManager.instance.GetPlayerObject();
	}

    void Update() {
        if (isProppedOpen)
        {
            isLocked = false;
        }

        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= interactionRadius)
        {
            isInteractable = true;
        }
        else
        {
            isInteractable = false;
        }
        if (!isLocked && !isOpen && isInteractable && Input.GetButtonDown("Interact"))
        {
            isOpen = true;
            animator.SetBool("isOpen", isOpen);
        }
        else if (!isLocked && isOpen && isInteractable && Input.GetButtonDown("Interact"))
        {
            isOpen = false;
            animator.SetBool("isOpen", isOpen);
        }
        else if (isLocked && !isInteractable)
        {
            isOpen = false;
            animator.SetBool("isOpen", isOpen);
        }
    }

    public void ItemSacrificied()
    {
        numItemsSold++;
        if(numItemsSold >= numItemsSoldToUnlock)
        {
            isLocked = false;
        }
    }

}
