using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Cell houses information about the contents and 
 * metadata of each cell of a surface grid.
 */ 
public class Cell : MonoBehaviour{
    public const float CELL_SIZE = 1f / SurfaceController.GRID_RESOLUTION;

    public List<Item> items = new List<Item>();
    public bool itemLocked = false;
    public GameObject cellObject;
    public BoxCollider cellCollider;

    // Use this for initialization
    /*
	public Cell(string name) {
        cellObject = new GameObject(name);
        cellObject.layer = 13;
        cellObject.tag = "Cells";
        cellCollider = cellObject.AddComponent<BoxCollider>();
        cellCollider.size = new Vector3(CELL_SIZE, CELL_SIZE, CELL_SIZE);
        cellCollider.isTrigger = true;
	}
    */

    private void Awake()
    {
        cellObject = new GameObject(name);
        cellObject.layer = 13;
        cellObject.tag = "Cells";

        cellCollider = cellObject.AddComponent<BoxCollider>();
        cellCollider.size = new Vector3(CELL_SIZE, CELL_SIZE, CELL_SIZE);
        cellCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log("TRIGGERED");
        //Check to make sure collision is with an Item
        if (other.GetComponent<Item>() != null)
        {
            //Debug.Log(other.name + "FOUND");
            if (!items.Contains(other.GetComponent<Item>()))
            {
                Item tmp = other.GetComponent<Item>();
                AddItem(tmp);
                tmp.cellsInteracting.Add(gameObject.GetComponent<Cell>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Item>() != null)
        {
            if (items.Contains(other.GetComponent<Item>()))
            {
                Item tmp = other.GetComponent<Item>();
                items.Remove(tmp);
                tmp.cellsInteracting.Remove(gameObject.GetComponent<Cell>());
            }
        }
    }

    /*  Supplying null represents an object being removed.  */
    public void AddItem(Item i, bool locked = false) {
        items.Add(i);
        itemLocked = locked;
        //update "Surface State"-- Call the Surface, add this item.
    }
    public List<Item> GetItems() { return items; }

    /*  When an object collides: 
     *    Ask if an Item. If so
     *      Store Cell as currently interacting
     *  
     *  When an object exits collision
     *      Ask if Item: if so
     *          Remove self from list of currently interacting.
     */
}
