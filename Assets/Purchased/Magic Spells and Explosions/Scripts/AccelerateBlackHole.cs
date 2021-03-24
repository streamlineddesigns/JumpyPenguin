using UnityEngine;
using System.Collections;

public class AccelerateBlackHole : MonoBehaviour
{

    public float Speed = 0.01f;
    public MagicalFX.FX_Rotation[] Quads;
    public Vector2 scaleRange = new Vector2(0.1f, 3.0f);
    public float scaleTime = 3.0f;
	public float destroyTime = 1.0f;

    private bool bStartScale = false;
    private float startScaleTime = 0.0f;
	private float startTime = 0.0f;

    // Use this for initialization
    void Start ()
    {
        ScaleAnim();
		startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Quads[0].Speed = new Vector3(Quads[0].Speed.x + Speed, Quads[0].Speed.y + Speed, Quads[0].Speed.z + Speed);
        Quads[1].Speed = new Vector3(Quads[1].Speed.x + Speed, Quads[1].Speed.y + Speed, Quads[1].Speed.z + Speed);
        Quads[2].Speed = new Vector3(Quads[2].Speed.x + Speed, Quads[2].Speed.y + Speed, Quads[2].Speed.z + Speed);

		if (Time.time - startTime >= destroyTime)
			Destroy(gameObject);
    }

    void ScaleAnim()
    {
        if (!bStartScale)
        {
            bStartScale = true;
            startScaleTime = Time.time;
            transform.localScale = new Vector3(scaleRange.x, scaleRange.x, scaleRange.x);
        }

        float scaleParam = scaleRange.x + (scaleRange.y - scaleRange.x) * (Time.time - startScaleTime) / scaleTime;
        transform.localScale = new Vector3(scaleParam, scaleParam, scaleParam);

        if (scaleParam < scaleRange.y)
            Invoke("ScaleAnim", Time.fixedDeltaTime);
    }
}
