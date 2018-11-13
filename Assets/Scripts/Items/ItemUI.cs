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
    [HideInInspector] public Animator anim;

    private void Awake()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
        parentItem = gameObject.GetComponentInParent<Item>();
        Debug.Log(parentItem.attributes.name);
        namePanel.GetComponent<Text>().text = parentItem.attributes.name;
        anim = GetComponent<Animator>();
        Debug.Log("anim = " +anim);
        
    }



    // Use this for initialization
    void Start () {
        name = parentItem.name;
        descriptionPanel.GetComponent<Text>().text = parentItem.attributes.description;
        desc = parentItem.description;
    }

    // Update is called once per frame
    void Update () {
        if (isShown)
        {
            gameObject.transform.LookAt(player.transform);
            //TODO include subtle animations here
        }
	}

    //Assumes you check if animation is playing already.
    public void PlayFade()
    {
        if(!anim.GetBool("isShown"))
            anim.SetBool("isShown", true);
    }
    public void StopFade()
    {
        if (anim.GetBool("isShown"))
            anim.SetBool("isShown", false);
    }

}
