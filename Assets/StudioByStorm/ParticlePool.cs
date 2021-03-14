using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class ParticlePool : MonoBehaviour
    {
        public static ParticlePool Singleton;
        public enum ParticleType {
            EnemyDeath,
            PlayerJump,
            WaterSplash,
        };
        //particles
        public GameObject EnemyDeathParticle;
        public GameObject PlayerJumpParticle;
        public GameObject WaterSplashParticle;

        //pool
        protected List<GameObject> EnemyDeathParticlePool = new List<GameObject>();
        protected List<GameObject> PlayerJumpParticlePool = new List<GameObject>();
        protected List<GameObject> WaterSplashParticlePool = new List<GameObject>();

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
            instantiateParticle(EnemyDeathParticle, EnemyDeathParticlePool);
            instantiateParticle(PlayerJumpParticle, PlayerJumpParticlePool);
            instantiateParticle(WaterSplashParticle, WaterSplashParticlePool);
        }

        protected void instantiateParticle(GameObject particleToAdd, List<GameObject> poolToAddTo)
        {
            GameObject particle = Instantiate(particleToAdd, GameController.Instance.LevelController.LevelContainer.transform);
            particle.SetActive(false);
            poolToAddTo.Add(particle);
        }



        public GameObject getAvailableParticle(ParticleType particleType)
        {
            GameObject particle = null;

            switch(particleType) {
                //Enemy Death
                case ParticleType.EnemyDeath :
                    particle = _getAvailableParticle(EnemyDeathParticlePool, particleType);
                    break;

                //Player Jump
                case ParticleType.PlayerJump :
                    particle = _getAvailableParticle(PlayerJumpParticlePool, particleType);
                    break;

                //Water Splash
                case ParticleType.WaterSplash :
                    particle = _getAvailableParticle(WaterSplashParticlePool, particleType);
                    break;
            }

            return particle;
        }

        protected GameObject _getAvailableParticle(List<GameObject> pool, ParticleType particleType)
        {
            int index = 0;
            GameObject particle = null;
            
            while(index < pool.Count) {
                if (pool[index].active == false) {
                    particle = pool[index];
                    break; 
                }
                index++;
            }

            if (particle == null) {
                Debug.LogError("PARTICLE INSTANTIATING");
                //instantiate some new new and add to pool
                switch(particleType) {
                    //Enemy Death
                    case ParticleType.EnemyDeath :
                        instantiateParticle(EnemyDeathParticle, EnemyDeathParticlePool);
                        break;
                    //Player Jump
                    case ParticleType.PlayerJump :
                        instantiateParticle(PlayerJumpParticle, PlayerJumpParticlePool);
                        break;
                    //Water Splash
                    case ParticleType.WaterSplash :
                        instantiateParticle(WaterSplashParticle, WaterSplashParticlePool);
                        break;
                }
                particle = pool[pool.Count - 1];
            }

            return particle;
        }

    }
}