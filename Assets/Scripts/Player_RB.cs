using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player_RB : MonoBehaviour {

    #region Editor Fields
    [SerializeField]
    private float scrollMod = .1f;
    public Vector3 scrollOffset;
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
    public Rigidbody heldItem_RB = null;

    private Ray ray;
    private RaycastHit hit;
    private Vector3 rayDist;
    private LayerMask layer;


    private string itemName = "Item";

    private void Start()
    {
        layer = LayerMask.GetMask("Item");
        scrollOffset = Vector3.zero;
    }

    
    void Update() {
        Vector3 moveVector = new Vector3();

        GetPlayerFocus();
        #region Player Controls

        #region Pickup/Place Item
        if (selectedItem != null && Input.GetButtonDown("Fire1"))
        {
            //Pick up Item
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
        else if ( ((Input.GetAxis("Mouse ScrollWheel")) >= 0f && (heldItem != null)))
        {
            float tmpF = 0f;
            //Start with existing offset. Take input and add to scrollMod.
            //Clamp the values between reasonable numbers.
            //Add existing offset to the result of the clamping.
            //set the new offset as the z offset
            tmpF = scrollOffset.z + Mathf.Clamp((Input.GetAxis("Mouse ScrollWheel") + scrollMod), .5f, reachDist);
            scrollOffset = Vector3.Scale(Vector3.one, new Vector3(0f, 0f, tmpF));
        }

        else if( (Input.GetAxis("Mouse ScrollWheel")) < 0f && (heldItem != null))
        {
            float tmpF = 0f;
            tmpF = scrollOffset.z + Mathf.Clamp((Input.GetAxis("Mouse ScrollWheel") - scrollMod), .5f, reachDist);
            scrollOffset = Vector3.Scale(Vector3.one, new Vector3(0f, 0f, tmpF));

        }

        #endregion

        #endregion
    }

    private void LateUpdate()
    {
        if (isDropping) { Release(); isDropping = false; }
        if(heldItem_RB != null)
        {
            //Hand position is used as offset
            //Item is parented to Hand
            //Rigidbody operations are all in worldspace
            //Find hand.transform.forward and multiply that by the offset
            //then add that to the starting point.

            heldItem_RB.position = heldItem_RB.position + (hand.transform.forward * scrollOffset.z);

            //heldItem_RB.MovePosition( heldItem_RB.position + ( (hand.transform.position + scrollOffset) - heldItem_RB.position) );
        }
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
            selectedItem = null;
        }
    }


    public void Grab(GameObject item)
    {
        heldItem = item.GetComponent<Item>();
        heldItem_RB = heldItem.GetComponent<Rigidbody>();

        heldItem.gameObject.layer = 2; //ignore raycast
        heldItem.gameObject.transform.SetParent(hand, false);
        heldItem_RB.MovePosition(hand.transform.position);
        heldItem_RB.MoveRotation(Quaternion.identity);
        heldItem_RB.useGravity = false;
        heldItem_RB.interpolation = RigidbodyInterpolation.Interpolate;
        heldItem_RB.useGravity = false;

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
        droppedItem.GetComponent<Rigidbody>().AddForce( ((vel_2 - vel_1) / Time.deltaTime), ForceMode.VelocityChange);

        heldItem_RB.collisionDetectionMode = CollisionDetectionMode.Discrete;
        heldItem_RB.useGravity = true;

        heldItem = null;
        heldItem_RB = null;

        scrollOffset = Vector3.zero;

        return droppedItem;
    }

    public void RotateObject()
    {
        //Gets the world vector space for cameras up vector 
        Vector3 relativeUp = cameraFPV.transform.TransformDirection(Vector3.up);
        //Gets world vector for space cameras right vector
        Vector3 relativeRight = cameraFPV.transform.TransformDirection(Vector3.right);

        //Turns relativeUp vector from world to objects local space
        Vector3 objectRelativeUp = transform.InverseTransformDirection(relativeUp);
        //Turns relativeRight vector from world to object local space
        Vector3 objectRelativeRight = transform.InverseTransformDirection(relativeRight);

        //Calculate rotation
        //rotateBy = Quaternion.AngleAxis(-Input.GetAxis("Mouse X") / obj.transform.localScale.x * sensitivityX, objectRelativeUp * Quaternion.AngleAxis(Input.GetAxis("Mouse Y") / obj.transform.localScale.x * sensitivityY, objectRelaviveRight);
        Quaternion rotateBy = Quaternion.AngleAxis(-Input.GetAxis("Mouse X") / transform.localScale.x * rotSpeed, objectRelativeUp) * Quaternion.AngleAxis(Input.GetAxis("Mouse Y") / transform.localScale.x * rotSpeed, objectRelativeRight);
        //Finally rotate the object accordingly
        heldItem.GetComponent<Rigidbody>().useGravity = true;
        heldItem.GetComponent<Rigidbody>().MoveRotation(rotateBy);

    }
}
