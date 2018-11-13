using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Void
 *   A static object which persists across scene transitions.
 *   This is the only object of its kind in the game, though 
 *   it changes appearances based on context and through 
 *   what the player feeds it.
 */

    //TODO Design method for determining which room player is entering.

[RequireComponent(typeof(SphereCollider))]
public class Void : MonoBehaviour
{
    public GameManager.ROOM_LIST roomType;
    [Header("Controls")]
    [SerializeField]
    float interactionRadius = 1.75f;
    [Header("Void Particle System Components")]
    // TODO Store varied versions of each component
    // TODO Create ENUM to index easily
    // eg: Void.ROOM_TYPE.BASEMENT == 0
    [SerializeField]
    ParticleSystem vortex;
    [SerializeField]
    ParticleSystem ring;
    [SerializeField]
    ParticleSystem center;
    [SerializeField]
    GameObject itemEatenPrefab;
    [Header("Components")]
    [SerializeField]
    SphereCollider interactionCollider;

    [SerializeField]
    public DoorController door;


    static ParticleSystem itemEatenParticles;

    // Use this for initialization
    void Start()
    {
        if (interactionCollider == null)
        {
            interactionCollider = GetComponent<SphereCollider>();
            interactionCollider.isTrigger = true;
            interactionCollider.radius = interactionRadius;
        }
        itemEatenParticles = Instantiate(itemEatenPrefab, Vector3.zero, Quaternion.Euler(new Vector3(-90, 0, 0))).GetComponent<ParticleSystem>();
        itemEatenParticles.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        Item itemTMP;
        if (itemTMP = other.GetComponent<Item>())
        {
            itemTMP.isInVoid = true;
            //TODO update Particle systems to ItemNearby state
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Item itemTMP;
        if (itemTMP = other.GetComponent<Item>())
        {
            itemTMP.isInVoid = false;
            //TODO update Particle Systems to !ItemNearby state
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO determine how void updates over time
    }

    /* Plays the given particle effect at the given location
     */
    public static IEnumerator PlayItemEatenParticle(int famMember, Vector3 spawnPos)
    {
        Debug.Log("ADD PARTICLES FOR ITEM EATEN");
        itemEatenParticles.Stop();
        Debug.Log("trying to spawn at:" + spawnPos);
        itemEatenParticles.gameObject.transform.position = spawnPos;
        itemEatenParticles.Play();

        

        yield return null;
    }

    public void PlayVoid()
    {
        vortex.Play();
        ring.Play();
        center.Play();
    }
    public void StopVoid()
    {
        vortex.Stop();
        ring.Stop();
        center.Stop();
    }
}
