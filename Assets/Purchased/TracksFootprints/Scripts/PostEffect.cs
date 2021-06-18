using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect : MonoBehaviour {

    //Script used to put on the camera that goes below the terrain to capture the objects that are on top, this script adds the shader and makes the objects brighter for the SplatMap to be more visible

    Camera AttachedCamera;
    public Shader Post_Outline;
    public Shader DrawSimple;
    public Camera TempCam;
    Material Post_Mat;

    //Use this for initialization
    void Start()
    {

        //Takes the camera component from the ground camera
        AttachedCamera = GetComponent<Camera>();
        //Adds to the material the shader that changes the material of the object in contact with the ground
        Post_Mat = new Material(Post_Outline);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Set up a temporary camera
        TempCam.CopyFrom(AttachedCamera);
        TempCam.clearFlags = CameraClearFlags.Color;
        TempCam.backgroundColor = Color.black;

        //Make the temporary rendertexture
        RenderTexture TempRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);

        //Put it to video memory
        TempRT.Create();

        //Set the camera's target texture when rendering
        TempCam.targetTexture = TempRT;

        //Render all objects this camera can render, but with our custom shader.
        TempCam.RenderWithShader(DrawSimple, "");

        //Copy the temporary RT to the final image
        Graphics.Blit(TempRT, destination, Post_Mat);

        //Release the temporary RT
        TempRT.Release();
    }

}
