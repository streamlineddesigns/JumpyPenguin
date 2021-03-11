using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        protected bool bEnteredWater;
        protected bool isDead;
        protected Animator playerAnim;
        public GameObject dizzyEffect;

        void Start()
        {
            playerAnim = GetComponent<Animator>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "H2O" && !bEnteredWater) {
                bEnteredWater = true;
                GameController.Instance.GameOver();
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