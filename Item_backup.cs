using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Item: MonoBehaviour {

    //private string baseRelicAttributePath = "Assets/Relics/RelicAttributes/Relic_Base.asset";

    public ItemAttributes attributes;

    public bool isInVoid = false;

    public new string name = "";
    public string description = "";
    public float durability;
    public ItemAttributes.ITEM_SIZE size;

    //Displayed in the UI
    public float value;
    public float growth;

    //Used in calculations only
    private float modifier;
    private float stackModifier;

    Rigidbody rb;
    public BoxCollider relicCollider;
    private LayerMask layer = 10;

    private MeshFilter meshFilter; //TODO consider basing measurements off supplied mesh from ItemAttributes
    private MeshRenderer meshRenderer;
    Color oldColor;
    //public Outline outline;

    public List<Cell> cellsInteracting = new List<Cell>();
    public int numCellsNeeded = 0;


    private void Start()
    {
        gameObject.tag = "Item";

        gameObject.layer = layer;

        //if (attributes == null)
           //attributes = (RelicAttributes)AssetDatabase.LoadAssetAtPath(baseRelicAttributePath, typeof(RelicAttributes));
   
        //name = attributes.name;
        description = attributes.description;
        size = attributes.relicSize;
 
        value = attributes.value;
        growth = attributes.growth;

        meshFilter = GetComponent<MeshFilter>();

        if (meshFilter.mesh == null)
        {
            meshFilter = GetComponentInChildren<MeshFilter>();
        }


        meshRenderer = GetComponent<MeshRenderer>();
        oldColor = meshRenderer.material.color;

        {//Determine how many cells are needed to support this object.
            Vector3 tmp = meshFilter.sharedMesh.bounds.size;
            numCellsNeeded = (int)((tmp.x * 2) * (tmp.z * 2) * (tmp.y * 2));
        }

        //meshFilter.mesh = attributes.mesh;

        if (GetComponent<Rigidbody>() != null)
            rb = GetComponent<Rigidbody>();
        else
            rb = gameObject.AddComponent<Rigidbody>();
        relicCollider = GetComponent<BoxCollider>();
        InitBoxCollider(relicCollider);

    }

    private void Update()
    {
        if(transform.position.y < -10f)
        {
            transform.position = new Vector3(0f, 8f, 0f);
        }
        if (cellsInteracting.Count > 0)
        {
            meshRenderer.material.color = Color.yellow;
        }
        else
            meshRenderer.material.color = oldColor;
    }

    private void InitBoxCollider(BoxCollider collider)
    {
        float localScaleK_X = 1;
        float localScaleK_Y = 1;
        float localScaleK_Z = 1;
        if(collider.transform.localScale.x < 1)
            localScaleK_X = (1 / collider.transform.localScale.x);
        if (collider.transform.localScale.y < 1)
            localScaleK_Y = (1 / collider.transform.localScale.y);
        if (collider.transform.localScale.z < 1)
            localScaleK_Z = (1 / collider.transform.localScale.z);
        float SMALL_X = .5f * localScaleK_X;
        float SMALL_Y = .5f * localScaleK_Y;
        float SMALL_Z = .5f * localScaleK_Z;
        float LARGE_X = 1f * localScaleK_X;
        float LARGE_Y = 1f * localScaleK_Y;
        float LARGE_Z = 1f * localScaleK_Z;

        float meshHeight = GetComponent<MeshFilter>().sharedMesh.bounds.size.y;
        Debug.Log(gameObject.name + " meshHeight = " + meshHeight);
        collider.center = (Vector3.zero);

        switch (collider.GetComponent<Item>().size)
        {
            case ItemAttributes.ITEM_SIZE.XSMALL:
                collider.size = new Vector3(SMALL_X, SMALL_Y, SMALL_Z);
               break;
            case ItemAttributes.ITEM_SIZE.SMALL:
                collider.size = new Vector3(SMALL_X, LARGE_Y, SMALL_Z);
                break;
            case ItemAttributes.ITEM_SIZE.MEDIUM:
                if(collider.GetComponent<MeshFilter>().mesh.bounds.size.x > collider.GetComponent<MeshFilter>().mesh.bounds.size.z)
                    collider.size = new Vector3(LARGE_X, LARGE_Y, SMALL_Z);
                else
                    collider.size = new Vector3(SMALL_X, LARGE_Y, LARGE_Z);
                break;
            case ItemAttributes.ITEM_SIZE.LARGE:
                 collider.size = new Vector3(LARGE_X, LARGE_Y, LARGE_Z);
               break;
            case ItemAttributes.ITEM_SIZE.XLARGE:
                 collider.size = new Vector3(LARGE_X, LARGE_Y, LARGE_Z);
               break;
            default:
                break;
        }
    }
    
    /*private void InitOutline()
    {
        outline.enabled = false;
        outline.OutlineWidth = 7.5f;
        outline.OutlineColor = Color.yellow;
    }
    */
    

    #region Metrics

    //Called from parent RelicStack when stack is updated
    //public void SetStackModifier(float stackMod) { stackModifier = stackMod; }

    //Intended to be called every turn transition
    /*
    public float CalculateGrowth()
    {
        float result;
        result = value * modifier;
        result *= stackModifier;
        result -= value;

        return 0f;//TODO update return value
    }

    public Color GetGrowthColor()
    {

        return new Color(.5f, .5f, .5f);//TODO Update return value
    }
    */
    #endregion
}
