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
            if (GameController.Instance.LevelController.getCurrentLevel() <= 1) {
                gameObject.SetActive(false);
            }
        }
    }
}