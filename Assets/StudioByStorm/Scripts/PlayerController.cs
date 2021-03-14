using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        protected bool bEnteredWater;
        protected bool bAttacked;
        protected bool isDead;
        protected Animator playerAnim;
        public GameObject dizzyEffect;
        protected bool bSlayingEnemy;

        void Start()
        {
            playerAnim = GetComponent<Animator>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "H2O" && !bEnteredWater) {
                
                bEnteredWater = true;
                WaterEffect();
                GameController.Instance.GameOver(GameController.DeathType.Water);
                GameController.Instance.UserInterfaceController.FrostCamera();

            } else if (other.tag == "Enemy" && !bAttacked) {
                //player got killed by enemy
                if (other.gameObject.transform.position.y >= transform.position.y - 1.75f) {
                    bAttacked = true;
                    GameController.Instance.GameOver(GameController.DeathType.Enemy);
                //player killed enemy
                } else {
                    if (! bSlayingEnemy) {
                        StartCoroutine(SlayEnemy());
                    }
                }
            }
        }

        void Update()
        {
            if (! GameController.Instance.isGameActive && GameController.Instance.isGameOver) {
                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler(0,180,0), 0.5f);
                if (gameObject.transform.rotation == Quaternion.Euler(0,180,0) && !isDead) {
                    Die();
                }
            }
        }

        IEnumerator SlayEnemy()
        {
            bSlayingEnemy = true;
            //make player bounce off enemy
            JumpOverride();

            //Give player extra score for killing enemy
            GameController.Instance.LevelController.incrementScore();
            yield return new WaitForSeconds(0.25f);
            GameController.Instance.LevelController.incrementScore();
            yield return new WaitForSeconds(0.25f);
            GameController.Instance.LevelController.incrementScore();
            yield return new WaitForSeconds(0.25f);
            GameController.Instance.LevelController.incrementScore();
            yield return new WaitForSeconds(0.25f);
            GameController.Instance.LevelController.incrementScore();
            bSlayingEnemy = false;
        }

        protected void JumpOverride()
        {
            GameController.Instance.bPlayerJumpOverride = true;
        }

        void WaterEffect()
        {
            GameObject waterSplash = ParticlePool.Singleton.getAvailableParticle(ParticlePool.ParticleType.WaterSplash);
            Vector3 targetPosition = new Vector3();
            targetPosition.x = GameController.Instance.Player.transform.position.x;
            targetPosition.y = GameController.Instance.Player.transform.position.y + 2.0f;
            targetPosition.z = GameController.Instance.Player.transform.position.z;
            waterSplash.transform.position = targetPosition;
            waterSplash.SetActive(true);
        }

        void Die()
        {
            isDead = true;
            playerAnim.SetBool("Die", true);
            ShowDizzyEffect();
        }

        public void ShowDizzyEffect()
        {
            dizzyEffect.SetActive(true);
        }

        public void HideDizzyEffect()
        {
            dizzyEffect.SetActive(false);
        }
    }
}