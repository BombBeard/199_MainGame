using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoomTrigger : MonoBehaviour {
    //Used by GameManager to load assets
    public GameManager.ROOM_LIST roomType;
    BoxCollider boxCollider;
    int stupidShittyCounterToFixCompoundColliderExits = 0;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stupidShittyCounterToFixCompoundColliderExits++;
            if(stupidShittyCounterToFixCompoundColliderExits > 0)
            {
                GameManager.instance.UpdateRoomState(roomType, true);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            stupidShittyCounterToFixCompoundColliderExits--;
            if (stupidShittyCounterToFixCompoundColliderExits <= 0)
            {
                GameManager.instance.UpdateRoomState(roomType, false);
            }
    }
}
