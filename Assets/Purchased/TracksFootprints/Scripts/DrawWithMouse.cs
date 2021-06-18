using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour {

    //Script to draw in the sand with the mouse, it creates two SplatMaps one to sink and another to raise the sand

    public Camera mainCamera;
    public Shader downDrawShader;
    public Shader upDrawShader;
    private RenderTexture _upSplatmap;
    private RenderTexture _downSplatmap;
    private Material _groundMaterial, _downDrawMaterial, _upDrawMaterial;
    private RaycastHit _hit;
    [Range(1,500)]
    public float downBrushSize;
    [Range(0, 1)]
    public float downBrushStrength;
    [Range(1, 500)]
    public float upBrushSize;
    [Range(0, 1)]
    public float upBrushStrength;

    //Use this for initialization
    public void Start()
    {
        //Starts the shader material for the down sand
        _downDrawMaterial = new Material(downDrawShader);
        //Passing the red color on the material to draw the down sand
        _downDrawMaterial.SetVector("_Color", Color.red);
        //Starts the shader material for the up sand
        _upDrawMaterial = new Material(upDrawShader);
        //Passing the gray color on the material to draw the up sand
        _upDrawMaterial.SetVector("_Color", Color.gray);
        //Get the material shader from the terrain
        _groundMaterial = GetComponent<MeshRenderer>().material;
        //Creating a render texture to draw the hole in the sand
        _upSplatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        //adding the render texture created in the terrain material shader in the material's UpSplaMap variable
        _groundMaterial.SetTexture("_UpSplat", _upSplatmap);
        //Creating a render texture to draw the hole in the sand
        _downSplatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        //adding the render texture created in the terrain material shader  in the material's DownSplaMap variable
        _groundMaterial.SetTexture("_DownSplat", _downSplatmap);
    }

    //Update is called once per frame
    public void Update() {
        //If the left mouse button is pressed
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //If the ray cast of the object touched the ground with the Sand layer
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out _hit))
            {   //Passes = Paases value to terrain shader material
                //Passes the coordinate value from where the object collides with the GroundShader material, this is done because of the groundhit, the touch of the object's raycast to draw the hole in the sand material
                _downDrawMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                //Passes the value of the strength that the brush will make when drawing on the render texture DownSplatMap
                _downDrawMaterial.SetFloat("_Strength", downBrushStrength);
                //Passes the value of the size that the brush will make when drawing on the render texture DownSplatMap
                _downDrawMaterial.SetFloat("_Size", downBrushSize);
                //Passes the coordinate value from where the object collides with the GroundShader material, this is done because of the groundhit, the touch of the object's raycast draw the rising material in the sand
                _upDrawMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                //Passes the value of the strength that the brush will make when drawing on the render texture UpSplatMap
                _upDrawMaterial.SetFloat("_Strength", upBrushStrength);
                //Passes the value of the size that the brush will make when drawing on the render texture UpSplatMap
                _upDrawMaterial.SetFloat("_Size", upBrushSize);
                //Draws on the Render texture where the object's raycast collides with the terrain, everything that is red will create a hole in the ground
                RenderTexture downTemp = RenderTexture.GetTemporary(_downSplatmap.width, _downSplatmap.height, 0, RenderTextureFormat.ARGBFloat);
                //Draw on the Render texture where the radius of the object collides with the terrain, everything that is gray will increase in size creating a mound of sand on the ground
                RenderTexture upTemp = RenderTexture.GetTemporary(_upSplatmap.width, _upSplatmap.height, 0, RenderTextureFormat.ARGBFloat);
                //Copies source texture into destination render texture with a shader, take the downTemp drawing and move to the DownSplatMap
                Graphics.Blit(_downSplatmap, downTemp);
                Graphics.Blit(downTemp, _downSplatmap, _downDrawMaterial);
                RenderTexture.ReleaseTemporary(downTemp);
                //Copies source texture into destination render texture with a shader, take the upTemp drawing and move to the UpSplatMap
                Graphics.Blit(_upSplatmap, upTemp);
                Graphics.Blit(upTemp, _upSplatmap, _upDrawMaterial);
                RenderTexture.ReleaseTemporary(upTemp);
            }
        }
	}


    public void OnGUI()
    {

        //Shows the Splatmaps on the scene screen
        GUI.DrawTexture(new Rect(0, 0, 100, 100), _downSplatmap, ScaleMode.ScaleToFit, false, 1);
        GUI.DrawTexture(new Rect(0, 100, 100, 100), _upSplatmap, ScaleMode.ScaleToFit, false, 1);
    }
   
}
