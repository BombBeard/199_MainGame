using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ItemParticleController))]
public class Item : MonoBehaviour
{

    //private string baseRelicAttributePath = "Assets/Relics/RelicAttributes/Relic_Base.asset";

    [Header("Dependent Objects")]
    public ItemAttributes attributes;
    public ItemParticleController itemParticleController;
    public ItemUI itemPreview;
    private Animator itemPreviewAnimator;
    public ItemUI itemDescription;
    private Animator itemDescriptionAnimator;

    [Header("Controls")]
    public bool isInVoid = false;

    public new string name = "";
    public string description = "";

    //Displayed in the UI
    public float value;

    //Used in calculations only
    private float modifier;

    Rigidbody rb;
    public BoxCollider itemCollider;
    private LayerMask layer = 10;
    [HideInInspector] public bool isHeld = false;

    private MeshFilter meshFilter; //TODO consider basing measurements off supplied mesh from ItemAttributes
    private MeshRenderer meshRenderer;

    //Static Player Reference
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        gameObject.tag = "Item";
        gameObject.layer = layer;

        name = attributes.name;
        description = attributes.description;
        value = attributes.value;

        if (GetComponent<Rigidbody>() != null)
            rb = GetComponent<Rigidbody>();
        else
            rb = gameObject.AddComponent<Rigidbody>();
        itemCollider = GetComponent<BoxCollider>();
        itemCollider.size = attributes.mesh.bounds.size;
        
    }

    private void Update()
    {
        if (transform.position.y < -10f)
        {
            transform.position = FindObjectOfType<Player>().transform.position + new Vector3(0, .5f, 0) ;
        }
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= itemParticleController.detectionRadius)
        {
            if (!isHeld)
            {
                itemPreview.PlayFade();
                itemParticleController.PlayApproach();
            }
            else
            {
                itemPreview.StopFade();
                itemParticleController.StopApproach();
            }
        }
        else
        {
            itemPreview.StopFade();
            itemParticleController.StopApproach();
        }
    }



}