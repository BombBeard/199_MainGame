/* Supplies information about various relics as
 * static data. Only used during Init or Reset
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemAttributes", menuName = "ItemAttributes", order = 0)]
public class ItemAttributes : ScriptableObject {

    [Range(.001f, 2f)]
    public float growth = 1f;
    [Range(.001f, 2f)]
    public float modifier = 1f;
    [Range(1f, 1000f)]
    public float value = 1;
    [TextArea]
    public string description = "";

    public Mesh mesh;

}
