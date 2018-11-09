using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ItemUI : MonoBehaviour {

    [Header("Controls")]
    [SerializeField]
    bool isShown = true;

    [Header("Components")]
    [SerializeField]
    GameObject itemUIPrefab;
    [SerializeField]
    Canvas baseCanvas;
    [SerializeField]
    RectTransform namePanel;
    [SerializeField]
    RectTransform descriptionPanel;

    [Header("Helpers")]
    [SerializeField]
    GameObject player;

	// Use this for initialization
	void Start () {
        if(baseCanvas == null)
            baseCanvas = GetComponent<Canvas>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("MainCamera").gameObject;

        //Instantiate canvas prefab
        Vector3 tmpPos = gameObject.GetComponentInParent<BoxCollider>().bounds.size;
        baseCanvas = Instantiate(itemUIPrefab, gameObject.transform.position, Quaternion.identity).GetComponent<Canvas>();

        //Assign references to gameObjects


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
