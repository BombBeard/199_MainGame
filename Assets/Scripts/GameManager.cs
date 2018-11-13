using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GameManager is responsible for:
 *   Monitoring the state of the player
 *   Monitoring the position of the player
 *   Reacting to the player's behavior
 *   Unlocking paths
 *   Activating visual effects based on player
 */

public class GameManager : MonoBehaviour {

    //Used by RoomTrigger
    public enum ROOM_LIST
    {
        HUB,
        BASEMENT,
        DAUGHTER,
        SON,
        EX,
        LOVER,
        NONE
    };
    [System.Serializable]
    public struct roomChecker
    {
        public ROOM_LIST room;
        public bool b;
    };
    int numRooms = 7;

    Dictionary<ROOM_LIST, bool> RoomStatus;

    public static GameManager instance = null;

    [Header("Player is in...")]
    public roomChecker[] rooms = new roomChecker[3];


    private void Awake()
    {
        #region Singelton Pattern
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        #endregion

        RoomStatus = new Dictionary<ROOM_LIST, bool>();
        RoomStatus.Add(ROOM_LIST.HUB, false);
        RoomStatus.Add(ROOM_LIST.BASEMENT, false); //starting room-- assures that a state isn't missed.
        RoomStatus.Add(ROOM_LIST.DAUGHTER, false);
        RoomStatus.Add(ROOM_LIST.SON, false);
        RoomStatus.Add(ROOM_LIST.EX, false);
        RoomStatus.Add(ROOM_LIST.LOVER, false);
        RoomStatus.Add(ROOM_LIST.NONE, false);

    }
    private void Start()
    {
        rooms[0].room = ROOM_LIST.HUB;
        rooms[0].b = false;
        rooms[1].room = ROOM_LIST.BASEMENT;
        rooms[1].b = false;
        rooms[2].room = ROOM_LIST.DAUGHTER;
        rooms[2].b = false;
    }

    public void UpdateRoomState(ROOM_LIST _room, bool _b)
    {
        RoomStatus[_room] = _b;
        rooms[(int)_room].b = _b;
        if (_b)
        {
            VoidManager.instance.ActivateVoid(_room);
        }
        else
        {
            VoidManager.instance.DeactivateVoid(_room);
        }
    }

    public ROOM_LIST RoomPlayerIsIn()
    {
        //determine which room the player is in
        //TODO better algorithm for determining which room player is in
        for (int i = 0; i < RoomStatus.Count; i++)
        {
            if (RoomStatus[(GameManager.ROOM_LIST)i])
            {
                return (GameManager.ROOM_LIST)i;
            }
        }
        return ROOM_LIST.NONE;
    }

}
