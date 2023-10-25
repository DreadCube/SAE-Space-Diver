using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{

    [SerializeField]
    private Color[] materialColors;


    [SerializeField]
    [Range(0f, 1f)]
    private float threshold = 0.9f;


    [SerializeField]
    private bool tailEnabled = true;

    [SerializeField]
    private Vector3 scale = new Vector3(1, 1, 1);

    [SerializeField]
    private Transform? rotateAroundTarget;


    // TODO: Hack for now to see actual changes in the freacking editor?
#if UNITY_EDITOR
    private void OnValidate() => UnityEditor.EditorApplication.delayCall += _OnValidate;

    private void _OnValidate()
    {
        UnityEditor.EditorApplication.delayCall -= _OnValidate;
        if (this == null) return;


        var childs = transform.childCount;

        for (var i = childs - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).gameObject.name == "Tail")
            {
                continue;
            }
            DestroyImmediate(transform.GetChild(i).gameObject);
        }


        Generate();
    }

#endif


    private void Start()
    {
        Generate();
    }

    void Generate()
    {

        transform.localScale = scale;

        MeshFilter meshFilter = GetComponent<MeshFilter>();

        Mesh mesh = meshFilter.sharedMesh;

        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        Material material = GetComponent<Renderer>().sharedMaterial;


        ParticleSystem tailParticles = GameObject.Find("Tail").GetComponent<ParticleSystem>();

        if (tailEnabled)
        {
            tailParticles.Play();
        }
        else
        {
            tailParticles.Stop();
        }


        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 position = transform.position + transform.rotation * vertices[i] + transform.rotation * (normals[i].normalized * 1f);


            if (Random.value > threshold)
            {

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                cube.transform.SetParent(this.transform);

                cube.transform.position = position;


                cube.transform.localScale = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                cube.GetComponent<Renderer>().sharedMaterial = material;
                cube.GetComponent<Renderer>().material.color = PickRandomMaterialColor();

            }
        }

        GetComponent<MeshRenderer>().enabled = false;

    }

    private Color PickRandomMaterialColor()
    {
        Debug.Log("COLOR");
        Debug.Log(materialColors.Length);
        if (materialColors.Length == 0) return Color.black;

        int index = Random.Range(0, materialColors.Length);

        return materialColors[index];
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * 20);

        if (rotateAroundTarget != null)
        {

            transform.RotateAround(rotateAroundTarget.position, Vector3.up, 50 * Time.deltaTime);
        }

        ParticleSystem tailParticles = GetComponent<ParticleSystem>();
        // tailParticles.transform.position = transform.position;
        //tailParticles.transform.RotateAround(rotateAroundTarget.position, Vector3.left, 50 * Time.deltaTime);
    }
}
