using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class ParticlePool : MonoBehaviour
    {
        public static ParticlePool Singleton;
        public List<GameObject> EnemyDeathParticle = new List<GameObject>();
        public List<GameObject> EnemyDeathParticlePool = new List<GameObject>();

        void Awake()
        {
            if (ParticlePool.Singleton == null) {
                ParticlePool.Singleton = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            InitializePool();
        }

        void InitializePool()
        {
            for(int i = 0; i < EnemyDeathParticle.Count; i++) {
                instantiateParticle(i);
            }
        }

        protected void instantiateParticle(int i)
        {
            GameObject particle = Instantiate(EnemyDeathParticle[i], GameController.Instance.LevelController.LevelContainer.transform);
            particle.SetActive(false);
            EnemyDeathParticlePool.Add(particle);
        } 

        public GameObject getAvailableParticle()
        {
            int index = 0;
            GameObject particle = null;
            
            while(index < EnemyDeathParticlePool.Count) {
                if (EnemyDeathParticlePool[index].active == false) {
                    particle = EnemyDeathParticlePool[index];
                    break; 
                }
                index++;
            }

            if (particle == null) {
                //its okay to be null
            }

            return particle;
        }

    }
}