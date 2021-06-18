using UnityEngine;
using System.Collections;
 
public class ScrollingUVs_Particle : MonoBehaviour 
{
	public Vector2 startingUvOffset = new Vector2 (0f, 0f);
	public Vector2 targetUvOffset = new Vector2 (0f, 0f);
	public string textureName = "_MainTex";
	public float duration=2f;
	public float delay=0f;
	private Renderer rend;
	private Vector2 uvOffset;
	private float myTime=0f;
	void Awake() {
		rend = GetComponent<ParticleSystemRenderer>();
	}
	void OnEnable() {
		myTime = 0f;
		uvOffset = startingUvOffset;
		StartCoroutine(scroll());
	}
	IEnumerator scroll() {
		yield return new WaitForSeconds(delay);
		while(Vector2.Distance(uvOffset, targetUvOffset) > 0f) {
			myTime += Time.deltaTime/duration;
			uvOffset = Vector2.Lerp(startingUvOffset, targetUvOffset, myTime);
			rend.material.SetTextureOffset(textureName, uvOffset);
			yield return null;
		}
	}
}