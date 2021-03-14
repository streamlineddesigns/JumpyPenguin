#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class EnableSelectedGameObject : MonoBehaviour {
	public SceneView.OnSceneFunc s;
	public Transform prevSelect;
	public int count;

	public void PlayParticles() {
		ParticleSystem pss = null;
		if(prevSelect) pss = prevSelect.GetComponent<ParticleSystem>();
		if (!pss) return;
		count++;
		if (count == 5 && pss.isStopped) {
			pss.Clear();
			pss.Play();
			count = 0;
		}
	}

	private void Update() {
		PlayParticles();
	}
}

[CustomEditor(typeof(EnableSelectedGameObject))]
[CanEditMultipleObjects]
public class MinigolfTrackEditor : Editor {
	EnableSelectedGameObject t;
	
	public void OnScene(SceneView sceneview) {
		if (Selection.activeTransform == null) return;
		Transform s = Selection.activeTransform;
		if (s == null) return;
		if (s == t.prevSelect) return;
		if (s.parent == null) return;
		if (s.parent.parent == null) return;
		if (s.parent.parent.parent == null) return;
		if (s.parent.parent.parent != t.transform) return;
		if (t.prevSelect != null) {
			t.prevSelect.gameObject.SetActive(false);
			s.gameObject.SetActive(true);
			ParticleSystem pss = s.GetComponent<ParticleSystem>();
			if (pss) {
				pss.time -= pss.time;
				t.count = 0;
				pss.Stop();
			}
			SceneView.RepaintAll();
		}
		t.prevSelect = s;
		}

	public void Awake() {
		t = (target as EnableSelectedGameObject);
		if (t.s != null) return;
		SceneView.onSceneGUIDelegate -= t.s;
		t.s = SceneView.onSceneGUIDelegate += OnScene;
	}
}

public static class MyMenuCommands {
	[MenuItem("Window/Play Particles &a")]
	static void FirstCommand() {
		Transform e = Selection.activeTransform;
		ParticleSystem pss;
		pss =	e.parent.GetComponent<ParticleSystem>();
		if(!pss) pss = e.GetComponent<ParticleSystem>();
		if (!pss) return;
		pss.Stop(); 
		pss.Clear();
		ParticleSystem[] p;
		p = pss.transform.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem ps in p) {
			ps.randomSeed = (uint)Random.Range(1f, 10000f);
		}
		pss.Play();
	}
}
#endif
