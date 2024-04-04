using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldPos2shaderUV : MonoBehaviour
{
    private Vector4 vec4 = new Vector4(0, 0, 0, 0);
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        vec4.x = gameObject.transform.position.x;
        vec4.y = gameObject.transform.position.z;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1)) 

        setMeshParam();
    }

    public void setMeshParam()
    {
        vec4.x = gameObject.transform.position.x;
        vec4.y = gameObject.transform.position.z;
        spriteRenderer.sharedMaterial.SetVector("_UVPos", vec4);
    }
}
