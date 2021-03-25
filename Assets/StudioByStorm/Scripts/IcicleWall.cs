using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class IcicleWall : MonoBehaviour
    {
        public List<GameObject> walls = new List<GameObject>();
        public GameObject iceTrack;
        protected float thresholdDistance = 1.1f;
        protected float initialWaitTimer = 0.8f;
        protected float currentWaitTimer;
        protected bool bCanEnable;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        void OnEnable()
        {
            currentWaitTimer = initialWaitTimer;
            
            //if (iceTrack != null) iceTrack.SetActive(false);

            if (GameController.Instance.isGameActive && (GameController.Instance.Player.transform.position.y + thresholdDistance > gameObject.transform.position.y)) {
                spawnIceTrack();
            } else {
                gameObject.SetActive(false);
            }
        }

        void OnDisable()
        {
            currentWaitTimer = initialWaitTimer;
            
            bCanEnable = false;
            
            //if (iceTrack != null) iceTrack.SetActive(false);

            for (int i = 0; i < walls.Count; i++) {
                walls[i].SetActive(false);
            }
        }

        protected void spawnIceTrack()
        {
            //iceTrack = ParticlePool.Singleton.getAvailableParticle(ParticlePool.ParticleType.IceTrack);
            //iceTrack.transform.SetParent(gameObject.transform);
            //iceTrack.transform.position = gameObject.transform.position;
            iceTrack.SetActive(true);
            bCanEnable = true;
        }

        void Update()
        {
            if (bCanEnable) {
                if (currentWaitTimer <= 0.0f) {
                    enableIcicles();
                } else {
                    currentWaitTimer -= Time.deltaTime;
                }
            }
        }

        protected void enableIcicles()
        {
            //if (iceTrack != null) iceTrack.SetActive(false);

            for (int i = 0; i < walls.Count; i++) {
                walls[i].SetActive(true);
            }
        }
    }
}