using UnityEngine;
using System.Collections;

public class TextureAnim : MonoBehaviour {
	public Vector2 scrollSpeed;
	public Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();
	}
	void Update() {
		Vector2 offset = Time.time * scrollSpeed;
		rend.material.SetTextureOffset("_MainTex", new Vector2(offset.x, offset.y));
	}
}