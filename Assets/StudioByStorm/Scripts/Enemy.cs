using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public abstract class Enemy : MonoBehaviour
    {
        public float bWhatLevlAmI;
        protected float initialDespawnTimer = 10.0f;
        public float currentDespawnTimer;
        protected float lastSpawnPosition = 0;//0=left; 1=right;

        //movement
        protected float horizontalMovement = 0.0f;
        protected float moveLeft = -1.0f;
        protected float moveRight = 1.0f;

        public void Despawn()
        {
            if (gameObject.activeSelf) {
                StartCoroutine(DespawnCoroutine());
            }
        }

        protected IEnumerator DespawnCoroutine()
        {
            yield return new WaitUntil(()=> bCanDespawn() || bDespawnTimerOver());
            gameObject.SetActive(false);
        }

        protected abstract bool bCanDespawn();

        protected bool bDespawnTimerOver()
        {
            if (currentDespawnTimer <= 0.0f) {
                return true;
            } else {
                currentDespawnTimer -= Time.deltaTime;
            }

            return false;
        }

        protected void resetDespawnTimer()
        {
            currentDespawnTimer = initialDespawnTimer;
        }

        protected void canEnableCheck()
        {
            //can't enable
            if (GameController.Instance.LevelController.getCurrentLevel() <= 5) {
                gameObject.SetActive(false);
            }
        }

        public void initFromLevel(Vector3 leftPos, Vector3 rightPos, float whatLevel)
        {
            //set level
            bWhatLevlAmI = whatLevel;

            //Set to right position, if previously was left
            if (EnemyPool.Singleton.lastSpawnPosition == 0) {
                //set position
                lastSpawnPosition = 1;
                transform.position = rightPos;
                //set rotation
                Quaternion targetEuler = transform.rotation;
                targetEuler.y = 0;
                transform.rotation = targetEuler;
                //Set to right movement
                horizontalMovement = moveLeft;

            //Set to left position, if previously was right
            } else {
                //set position
                lastSpawnPosition = 0; 
                transform.position = leftPos;
                //set rotation
                Quaternion targetEuler = transform.rotation;
                targetEuler.y = 180;
                transform.rotation = targetEuler;
                //Set to right movement
                horizontalMovement = moveRight;
            }

            //set to active
            gameObject.SetActive(true);
        }
    }
}