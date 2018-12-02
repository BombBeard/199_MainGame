using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

    public GameManager.roomChecker room;
    [SerializeField]
    Light[] lights;

    public void DisableLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.SetActive(false);
        }
    }
    public void EnableLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.SetActive(true);
        }
    }

}
