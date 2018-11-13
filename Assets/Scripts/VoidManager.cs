using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidManager : MonoBehaviour {

    public delegate void PlayerPosition(GameObject gameObject);
    public PlayerPosition trackPlayer;

    [System.Serializable]
    public struct VoidCollection
    {
        public GameManager.ROOM_LIST Tag;
        public Void v;
    };

    public static VoidManager instance = null;
    
    [SerializeField] //Inspector Hack
    public VoidCollection[] voidCollection; 

    //Convienient Dictionary
    public static Dictionary<GameManager.ROOM_LIST, Void> voids; 

    private void Awake()
    {
        #region Singleton
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
        
        #region Fill Out Dictionary of Voids
        voids = new Dictionary<GameManager.ROOM_LIST, Void>();
        for (int i = 0; i < voidCollection.Length; i++)
        {
            voids.Add(voidCollection[i].Tag, voidCollection[i].v);
        }
        #endregion

        foreach (KeyValuePair<GameManager.ROOM_LIST, Void> v in voids)
        {
            v.Value.StopVoid();
        }

    }

    public void ActivateVoid(GameManager.ROOM_LIST room)
    {
        voids[room].PlayVoid();
    }
    public void DeactivateVoid(GameManager.ROOM_LIST room)
    {
        voids[room].StopVoid();
    }

    public static Void GetVoid(GameManager.ROOM_LIST room)
    {
        return voids[room];
    }
    


}
