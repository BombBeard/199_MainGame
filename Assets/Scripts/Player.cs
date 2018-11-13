using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {

    #region Editor Fields
    [SerializeField]
    private float scrollMod = .1f;
    [SerializeField]
    private float reachDist = 1.5f;

    public Transform hand;
    private bool isDropping = false;
    private Vector3 vel_1;
    public float rotSpeed = 1f;
    public Camera cameraFPV;
    #endregion


    GameObject selectedItem;
    float currentSelectedDist;

    [HideInInspector]
    public Item heldItem = null;
    [HideInInspector]
    public Item itemSec = null;

    private Ray ray;
    private RaycastHit hit;
    private Vector3 rayDist;
    private LayerMask layer;

    private string itemName = "Item";

    private void Start()
    {
        //cameraFPV = GetComponent<Camera>();
        layer = LayerMask.GetMask("Item");
    }

    
    void Update() {

        GetPlayerFocus();
        #region Player Controls

        #region Pickup/Place Item
        if (selectedItem != null && Input.GetButtonDown("Fire1"))
        {
            //
            if (heldItem == null)
            {
                //Debug.Log("selectedItem: " + selectedItem.name);
                Grab(selectedItem.GetComponentInParent<Transform>().gameObject);
                selectedItem.GetComponent<Item>().isHeld = true;
            }
        }
        else if (heldItem != null && Input.GetButtonDown("Fire1"))
        {
            if (heldItem != null)
            {
                isDropping = true;
                heldItem.isHeld = false;
                vel_1 = heldItem.transform.position;
                if (heldItem.isInVoid)
                {
                    StartCoroutine( Void.PlayItemEatenParticle(0, vel_1) );
                    heldItem.gameObject.SetActive(false);
                    /* Find what room the player is in (GameManager)
                     * Find what void is being sacrificed to based on that room (VoidManager)
                     * Use the void reference to a DoorController to increment the number of 
                     * items sacrificed.
                     */
                    Void tmpVoid = VoidManager.GetVoid(GameManager.instance.RoomPlayerIsIn());
                    if (tmpVoid.door == null)
                    {
                        Debug.LogWarning(tmpVoid.name + " has no door associated.");
                    }
                    else
                    {
                        tmpVoid.door.ItemSacrificied();
                    }

                    //TODO Add item to pool of eaten items
                }
            }
        }
        #endregion

        #region Rotate Input
        else if (Input.GetButtonDown("Fire2") && heldItem != null)
        {
            GetComponent<FirstPersonController>().canRotate = false;
            RotateObject();
        }

        else if (Input.GetButtonUp("Fire2") && heldItem != null)
        {

            GetComponent<FirstPersonController>().canRotate = true;
        }
        #endregion

        #region Scrollwheel input
        else if ( (Input.GetAxis("Mouse ScrollWheel")) > 0f && (heldItem != null))
        {
            heldItem.transform.localPosition = new Vector3( 0f, 0f, Mathf.Clamp((heldItem.transform.localPosition.z + Input.GetAxis("Mouse ScrollWheel")) + scrollMod, 0f,reachDist));
        }

        else if( (Input.GetAxis("Mouse ScrollWheel")) < 0f && (heldItem != null))
        {
            heldItem.transform.localPosition = new Vector3( 0f, 0f, Mathf.Clamp((heldItem.transform.localPosition.z + Input.GetAxis("Mouse ScrollWheel")) - scrollMod, 0f, reachDist));
        }

        #endregion

        #endregion
    }

    private void LateUpdate()
    {
        if (isDropping) { Release(); isDropping = false; }
    }

    

    private void GetPlayerFocus()
    {
        if (Physics.SphereCast(cameraFPV.transform.position, .25f, cameraFPV.transform.forward, out hit, reachDist, layer.value))
        {
            //case: no selected relic
            if (hit.collider.tag == "Item" && (selectedItem == null))
            {
                selectedItem = hit.collider.gameObject;
                currentSelectedDist = Vector3.Distance(gameObject.transform.position, selectedItem.transform.position);
            }

            //case: hit object is same object
            else if (hit.collider.tag == "Item" && hit.collider.gameObject == selectedItem.gameObject) { currentSelectedDist = Vector3.Distance(gameObject.transform.position, selectedItem.transform.position); }

            //case: selectedItem != null, we hit a new candidateItem
            //      selectedItem is closer
            else if (hit.collider.tag == "Item" && (currentSelectedDist) < (Vector3.Distance(gameObject.transform.position, hit.collider.transform.position)))
            { currentSelectedDist = Vector3.Distance(gameObject.transform.position, selectedItem.transform.position); }

            //case: hit relic closer than current 
            else
            {
                if (hit.collider.tag == "Item")
                {
                    selectedItem = hit.collider.gameObject;
                }
            }
        }
        else if (selectedItem != null)
        {
            //selectedItem.GetComponent<Outline>().enabled = false;
            selectedItem = null;
        }
    }


    public void Grab(GameObject item)
    {
        //new Vector3(  0f, ( (float)relic.GetComponent<Collider>().bounds.size.y /2f ),0f)
        item.gameObject.layer = 2; //ignore raycast
        item.gameObject.transform.SetParent(hand, false);
        item.gameObject.transform.localPosition = Vector3.zero;
        //item.gameObject.transform.localPosition = item.GetComponent<Collider>().bounds.center - item.transform.position;
        item.gameObject.transform.localRotation = Quaternion.identity;
        item.GetComponent<Rigidbody>().isKinematic = true;
        //TODO Add Spring to item interaction
        heldItem = item.GetComponent<Item>();

        return;
    }

    public Item Release()
    {
        
        Item droppedItem = null;

        //Calculate velocity just before 
        Vector3 vel_2 = heldItem.transform.position;

        //Debug.Log("heldItem: " + heldItem.name);
        droppedItem = heldItem;
        heldItem.gameObject.layer = 10; //enable raycast
        heldItem.transform.SetParent(null);
        //heldItem.GetComponent<Outline>().enabled = false;
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        //TODO Investigate why velocity suddenly stopped working
        droppedItem.GetComponent<Rigidbody>().AddForce( ((vel_2 - vel_1) / Time.deltaTime), ForceMode.VelocityChange);
        heldItem = null;

        return droppedItem;
        #region Swapping

        /* More for swapping items, which isn't supported yet.
         * 
        Item tmp = null;

        //Removing Item from hand
        if (heldItem != null)
        {
            tmp = heldItem;
            heldItem.gameObject.layer = 10; //enable raycast
            if (relicSec != null)
                heldItem = relicSec;
            else
                heldItem = null;
        }

        //relicPri.transform.SetParent(surface
        //update 

        return tmp;
        */

        #endregion
    }

    public void RotateObject()
    {
        //Gets the world vector space for cameras up vector 
        Vector3 relativeUp = cameraFPV.transform.TransformDirection(Vector3.up);
        //Gets world vector for space cameras right vector
        Vector3 relativeRight = cameraFPV.transform.TransformDirection(Vector3.right);

        //Turns relativeUp vector from world to objects local space
        //var objectRelativeUp: Vector3 = obj.transform.InverseTransformDirection(relativeUp);
        Vector3 objectRelativeUp = transform.InverseTransformDirection(relativeUp);
        //Turns relativeRight vector from world to object local space
        //var objectRelaviveRight: Vector3 = obj.transform.InverseTransformDirection(relativeRight);
        Vector3 objectRelativeRight = transform.InverseTransformDirection(relativeRight);

        //Calculate rotation
        //rotateBy = Quaternion.AngleAxis(-Input.GetAxis("Mouse X") / obj.transform.localScale.x * sensitivityX, objectRelativeUp * Quaternion.AngleAxis(Input.GetAxis("Mouse Y") / obj.transform.localScale.x * sensitivityY, objectRelaviveRight);
        Quaternion rotateBy = Quaternion.AngleAxis(-Input.GetAxis("Mouse X") / transform.localScale.x * rotSpeed, objectRelativeUp) * Quaternion.AngleAxis(Input.GetAxis("Mouse Y") / transform.localScale.x * rotSpeed, objectRelativeRight);
        //Finally rotate the object accordingly
        //obj.rigidbody.MoveRotation(obj.rigidbody.rotation * rotateBy);
        heldItem.transform.Rotate(rotateBy.eulerAngles);

    }
}
