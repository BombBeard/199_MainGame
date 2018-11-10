using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

    [Header("Controls")]
    [SerializeField]
    bool isShown = true;

    [Header("Components")]
    [SerializeField]
    public RectTransform namePanel;
    [SerializeField]
    public RectTransform descriptionPanel;

    [Header("Helpers")]
    [SerializeField]
    GameObject player;

	// Use this for initialization
	void Start () {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
    }

    // Update is called once per frame
    void Update () {
        if (isShown)
        {
            gameObject.transform.LookAt(player.transform);
            //TODO include subtle animations here
        }
	}
}
