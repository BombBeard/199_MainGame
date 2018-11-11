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
    public new string name;
    [SerializeField]
    public RectTransform descriptionPanel;
    public string desc;

    [Header("Helpers")]
    [SerializeField]
    GameObject player;
    Item parentItem;
    [HideInInspector] public Animator animator;

	// Use this for initialization
	void Start () {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
        parentItem = gameObject.GetComponentInParent<Item>();
        Debug.Log(parentItem.attributes.name);
        namePanel.GetComponent<Text>().text = parentItem.attributes.name;
        name = parentItem.name;
        descriptionPanel.GetComponent<Text>().text = parentItem.attributes.description;
        desc = parentItem.description;
        animator = GetComponent<Animator>();
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
