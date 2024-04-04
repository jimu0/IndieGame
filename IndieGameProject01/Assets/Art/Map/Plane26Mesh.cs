using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane26Mesh : MonoBehaviour
{
    private Vector4 vec4 = new Vector4(0, 0, 0, 0);
    public List<float> param = new List<float>();
    private MeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //param.Clear();
        for (int i = 0; i < 676; i++)
        {
            param.Add(0);
        }
        setMeshParam();
        /*
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //Debug.Log(meshFilter.sharedMesh.vertexCount.ToString());
        //根据顶点index对应数据
        for (int i = 0; i < meshFilter.sharedMesh.vertexCount; i++)
        {
            param.Add(new Vector4(0, i*0.01f, 0, 0));
        }
        //param[1] = new Vector4(0, 0, 0, 0);
        //param[3] = new Vector4(0, 0, 1, 0);
        meshRenderer.sharedMaterial.SetVectorArray("param", param.ToArray());
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setMeshParam() 
    {
        //Debug.Log(param.Count);
        vec4.x = gameObject.transform.position.x;
        vec4.y = gameObject.transform.position.y;
        meshRenderer.sharedMaterial.SetFloatArray("param", param.ToArray());
        meshRenderer.sharedMaterial.SetVector("_UVPos", vec4);
    }
}
