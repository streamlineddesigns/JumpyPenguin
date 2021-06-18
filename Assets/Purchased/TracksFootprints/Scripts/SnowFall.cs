using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFall : MonoBehaviour {

    //Script Used to restore the tessellation of the terrain leaving the terrain flat again, 
    //this script simulates falling snow on the ground making the terrain return to normal with snow

    public Shader groundFallShader;
    private Material _groundFallMat;
    private MeshRenderer _meshRenderer;
    [Range(0.0001f, 0.1f)]
    public float groundAmount;
    [Range(0f, 1f)]
    public float groundOpacity;

    //Use this for initialization
    void Start()
    {
        //Take the mesh render of the terrain
        _meshRenderer = GetComponent<MeshRenderer>();
        //Takes the shader and adds it to the terrain material
        _groundFallMat = new Material(groundFallShader);

    } 

    //Update is called once per frame
    void Update()
    {
        //Passes the value of the shader amount to the noise script to the variable that can be changed at run time
        _groundFallMat.SetFloat("_GroundAmount", groundAmount);
        //Passes the value of the shader opacity to the noise script to the variable that can be changed at run time
        _groundFallMat.SetFloat("_GroundOpacity", groundOpacity);
        //Get the material shader from the SplatMap terrain
        RenderTexture ground = (RenderTexture)_meshRenderer.material.GetTexture("_DownSplat");
        //Create a temporary render texture splatmap
        RenderTexture temp = RenderTexture.GetTemporary(ground.width, ground.height, 0, RenderTextureFormat.ARGBFloat);
        //Copies source texture into destination render texture with a shader, take the downTemp drawing and move to the teporary DownSplatMap in ground fall material
        Graphics.Blit(ground, temp, _groundFallMat);
        Graphics.Blit(temp, ground);
        //Passes the texture to SplatMap to restore the drawing made by SplatMap
        _meshRenderer.material.SetTexture("_DownSplat", ground);
        RenderTexture.ReleaseTemporary(temp);
    }
}
