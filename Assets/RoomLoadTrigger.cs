using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class RoomLoadTrigger : MonoBehaviour {
    BoxCollider trigger;
    public string roomToLoad;


	// Use this for initialization
	void Start () {
        trigger = GetComponent<BoxCollider>();
        trigger.isTrigger = true;
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //load given scene
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(roomToLoad);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
