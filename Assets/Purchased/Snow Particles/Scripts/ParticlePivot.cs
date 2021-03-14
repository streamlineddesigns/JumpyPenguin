#if UNITY_EDITOR
#pragma warning disable 0618
using UnityEngine;
using UnityEditor;

public class ParticlePivot : MonoBehaviour {
	public Vector3 pivot;
}


[CustomEditor(typeof(ParticlePivot))]
public class ParticlePivotEditor : Editor {
	private ParticlePivot tar;
	private ParticleSystemRenderer psr;

	private void OnEnable() {
		tar = target as ParticlePivot;
		psr = tar.GetComponent<ParticleSystemRenderer>();
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		if (GUI.changed) {
			psr.pivot = tar.pivot;
		}
	}
}
#endif