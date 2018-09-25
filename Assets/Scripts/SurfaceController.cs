using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SurfaceController : MonoBehaviour {

    public const int GRID_RESOLUTION = 2; //Assumes a unit of 1 meter
    public List<Item> items;

    public MeshFilter meshFilter;
    public new BoxCollider collider;
    public Rigidbody rb;

    private Vector3 surfaceDimensions;


    // Use this for initialization
    void Awake () {
        #region MeshFilter Init
        //Set up associated mesh
        if (GetComponent<MeshFilter>() == null)
        {
            meshFilter = new MeshFilter();
            //set mesh to default mesh
            Debug.Log("filter is Null");
        }
        else
        {
            meshFilter = GetComponent<MeshFilter>();
        }
        //TODO create scriptable object with mesh to supply
        //and materials to apply

        #endregion

        #region BoxCollider & Rigidbody Init
        //Setup collider, rigidbody (for good measure) and 
        //determine the size of the collider using the mesh.

        if (gameObject.GetComponent<BoxCollider>() == null)
            collider = gameObject.AddComponent<BoxCollider>();
        else collider = gameObject.GetComponent<BoxCollider>();

        collider.size = meshFilter.sharedMesh.bounds.size;

        if (GetComponent<Rigidbody>() == null) rb = gameObject.AddComponent<Rigidbody>();
        else rb = gameObject.GetComponent<Rigidbody>();

        #endregion

        surfaceDimensions = collider.size;

        UpdateGrid();
    }


    // Update is called once per frame
    void Update () {
		
	}

    void UpdateGrid()
    {
        //define surface size
        //Divide surface into a grid
            //from 0,0 to sizeX,sizeZ; stepping every .5f
        for(float i = 0f; i < surfaceDimensions.x; i = i + (1 / GRID_RESOLUTION))
        {
            Debug.Log("i = " + i);
        }
        //populate surface grid with Cells.
    }
}
