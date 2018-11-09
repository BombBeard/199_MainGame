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

    public static GameManager instance = null;

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
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
