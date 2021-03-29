using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformableSnow : MonoBehaviour
{
    protected Mesh deformingMesh;
	protected Vector3[] originalVertices;
    protected Vector3[] displacedVertices;
    protected Vector3[] vertexVelocities;

    // Start is called before the first frame update
    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}

        vertexVelocities = new Vector3[originalVertices.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}