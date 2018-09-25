using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {
    private static Vector3[] childDirections =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f),
    };

    private Material[] materials;

    [Range(0, 360)]
    public float maxTwist;
    [Range(0,360)]
    public float maxRotationSpeed;
    private float rotationSpeed;

    public int maxDepth;
    private int depth;
    [Range(0,1)]
    public float spawnProbability;

    public Mesh mesh;
    public Material material;
    public Color baseColor;
    [Range(0f,1f)]
    public float saturation;
    [Range(0, 1)]
    public float maxSpecular;

    public float childScale;

	// Use this for initialization
	void Start () {
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);
        if (materials == null)
        {
            InitializeMaterials();
        }
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = materials[depth];
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
	}

    private void Initialize(Fractal parent, int index)
    {
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwist = parent.maxTwist;
        mesh = parent.mesh;
        materials = parent.materials;
        baseColor = parent.baseColor;
        saturation = parent.saturation;
        maxSpecular = parent.maxSpecular;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        spawnProbability = parent.spawnProbability;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[index] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[index];
    }

    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;
            materials[i] = new Material(material);
            materials[i].color = 
                Color.Lerp(new Color(saturation, saturation, saturation) 
                - baseColor, baseColor, t);
            materials[i].SetFloat(Shader.PropertyToID("_Glossiness"), 
                Random.Range(0, maxSpecular));
        }
    }

    private IEnumerator CreateChildren()
    {
        for (int i = 0; i < childDirections.Length; i++)
        {
            if (Random.value < spawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal Child").
                        AddComponent<Fractal>().Initialize(this, i);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
