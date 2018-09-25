using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Cell houses information about the contents and 
 * metadata of each cell of a surface grid.
 */ 
public class Cell {
    public const float CELL_SIZE = 1f / SurfaceController.GRID_RESOLUTION;

    private Item item = null;
    public GameObject cellObject;
    public BoxCollider collider;

	// Use this for initialization
	void Awake () {
        cellObject = new GameObject();
        collider = new BoxCollider();
        collider.size = new Vector3(CELL_SIZE, CELL_SIZE, CELL_SIZE);
	}
	
    void SetItem(Item i) { if(i != null) item = i; }
    Item GetItem() { return item; }
}
