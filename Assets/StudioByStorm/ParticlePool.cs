using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class ParticlePool : MonoBehaviour
    {
        public static ParticlePool Singleton;
        public GameObject ParticleContainer;
        public enum ParticleType {
            EnemyDeath,
            PlayerJump,
            WaterSplash,
            IcicleWall,
            IceTrack,
            FallingIce,
            FlashingTarget,

        };
        //particles
        public GameObject EnemyDeathParticle;
        public GameObject PlayerJumpParticle;
        public GameObject WaterSplashParticle;
        public GameObject IcicleWallParticle;
        public GameObject IceTrackParticle;
        public GameObject FallingIceParticle;
        public GameObject FlashingTargetParticle;

        //pool
        protected List<GameObject> EnemyDeathParticlePool = new List<GameObject>();
        protected List<GameObject> PlayerJumpParticlePool = new List<GameObject>();
        protected List<GameObject> WaterSplashParticlePool = new List<GameObject>();
        protected List<GameObject> IcicleWallParticlePool = new List<GameObject>();
        protected List<GameObject> IceTrackParticlePool = new List<GameObject>();
        protected List<GameObject> FallingIceParticlePool = new List<GameObject>();
        protected List<GameObject> FlashingTargetParticlePool = new List<GameObject>();

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
            instantiateParticle(IcicleWallParticle, IcicleWallParticlePool);
            instantiateParticle(IceTrackParticle, IceTrackParticlePool);
            instantiateParticle(FallingIceParticle, FallingIceParticlePool);
            instantiateParticle(FlashingTargetParticle, FlashingTargetParticlePool);
        }

        protected void instantiateParticle(GameObject particleToAdd, List<GameObject> poolToAddTo)
        {
            GameObject particle = Instantiate(particleToAdd, ParticleContainer.transform);
            particle.SetActive(false);
            poolToAddTo.Add(particle);
        }



        public GameObject getAvailableParticle(ParticleType particleType)
        {
            GameObject particle = null;

            switch(particleType) {
                //Enemy Death
                case ParticleType.EnemyDeath :
                    particle = _getAvailableParticle(EnemyDeathParticlePool, ParticleType.EnemyDeath);
                    break;

                //Player Jump
                case ParticleType.PlayerJump :
                    particle = _getAvailableParticle(PlayerJumpParticlePool, ParticleType.PlayerJump);
                    break;

                //Water Splash
                case ParticleType.WaterSplash :
                    particle = _getAvailableParticle(WaterSplashParticlePool, ParticleType.WaterSplash);
                    break;

                //Ice Wall
                case ParticleType.IcicleWall :
                    particle = _getAvailableParticle(IcicleWallParticlePool, ParticleType.IcicleWall);
                    break;

                //Ice Track
                case ParticleType.IceTrack :
                    particle = _getAvailableParticle(IceTrackParticlePool, ParticleType.IceTrack);
                    break;

                //Falling Ice
                case ParticleType.FallingIce :
                    particle = _getAvailableParticle(FallingIceParticlePool, ParticleType.FallingIce);
                    break;

                //Flashing Target
                case ParticleType.FlashingTarget :
                    particle = _getAvailableParticle(FlashingTargetParticlePool, ParticleType.FlashingTarget);
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
                //Debug.LogError("INSTANTIATING NEW PARTICLE");
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
                    //Ice Wall
                    case ParticleType.IcicleWall :
                        instantiateParticle(IcicleWallParticle, IcicleWallParticlePool);
                        break;
                    //Ice Track
                    case ParticleType.IceTrack :
                        instantiateParticle(IceTrackParticle, IceTrackParticlePool);
                        break;
                    //Falling Ice
                    case ParticleType.FallingIce :
                        instantiateParticle(FallingIceParticle, FallingIceParticlePool);
                        break;
                    //Flashing Target
                    case ParticleType.FlashingTarget :
                        instantiateParticle(FlashingTargetParticle, FlashingTargetParticlePool);
                        break;
                }
                particle = pool[pool.Count - 1];
            }

            return particle;
        }

    }
}