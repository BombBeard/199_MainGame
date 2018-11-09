using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class SurfaceController : MonoBehaviour {

    public const int GRID_RESOLUTION = 2; //Assumes a unit of 1 meter
    public const float CEILING_HEIGHT = 2f;
    public List<Cell> cells = new List<Cell>();

    

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
        rb.isKinematic = true;

        #endregion

        surfaceDimensions = collider.size;

        UpdateGrid();
    }

    void UpdateGrid()
    {
        //define surface size
        //Divide surface into a grid
        //from 0,0 to sizeX,sizeZ; stepping every .5f
        for (float y = surfaceDimensions.y + .25f; y <= (CEILING_HEIGHT); y += .5f)
        {
            GameObject layer = new GameObject("Layer_" + (y - .25f).ToString());
            layer.transform.SetParent(transform);
            layer.transform.transform.localPosition = new Vector3(0, y, 0);
            for (float i = 0f; i < surfaceDimensions.x; i += .5f)
            {
                for (float j = 0f; j < surfaceDimensions.z; j += .5f)
                {
                    string cellName = "Cell_" + ((i + 1) * (j + 1) - 1).ToString();
                    Cell tmp = gameObject.AddComponent<Cell>();
                    if (tmp.cellObject == null)
                        Debug.Log("Cell: is null");
                    tmp.cellObject.transform.SetParent(layer.transform);
                    tmp.cellObject.transform.localPosition = new Vector3(i + .25f, 0, j - .75f);
                    cells.Add(tmp);

                }
            }
        }

        //populate surface grid with Cells.
    }

    void PlaceItem(Item item)
    {
        
    }
}
