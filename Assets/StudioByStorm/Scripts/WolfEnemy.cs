using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class WolfEnemy : Enemy
    {
        private System.Random rng = new System.Random();  

        //movement
        protected CharacterController controller;
        protected float moveUp = 2.0f;
        protected float waterOffset = 2.5f;
        public bool canPlayerMove;

        //Speed
        protected Vector3 playerVelocity;
        protected float runSpeed = 12.0f;
        protected float walkSpeed = 3.0f;
        protected float swimSpeed = 3.0f;

        //Grounding
        protected float gravityValue = -9.81f;
        protected float distToGround = 1.0f;
        public bool groundedPlayer;
        protected float rayCastOffset = 0.5f;

        //State & Animation
        public GameObject Magic;
        public List<string> attackTriggerStrings = new List<string>();
        public Animator anim;
        protected bool bAlive;
        protected bool bAttacking;
        protected bool bSwimming;
        public enum State {
            Walk,
            Run,
            Fall,
            Swim,
            Attacking,
            Dead,
        };
        public State FSM;

        //fall timer
        protected float initialFallTimer = 0.2f;
        protected float currentFallTimer;

        //Bounds checking
        public float playerWidth = 3.0f;
        protected float RightBounds;
        protected float LeftBounds;
        protected Vector3 targetRightPos;
        protected Vector3 targetLeftPos;

        //other
        protected Collider boxCollider;

        void Awake()
        {
            boxCollider = GetComponent<Collider>();
            RightBounds = Screen.width;
            LeftBounds = 0;
            canPlayerMove = true;
        }

        void OnEnable()
        {
            StopAllCoroutines();
            canEnableCheck();
            boxCollider.enabled = true;
            Magic.SetActive(false);
            currentFallTimer = initialFallTimer;
            bAttacking = false;
            bSwimming = false;
            bAlive = true;
            FSM = State.Walk;
            resetDespawnTimer();
        }

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>();
            Shuffle(attackTriggerStrings);
            FSM = State.Walk;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !bSwimming) {
                if (other.gameObject.transform.position.y - 1.75f > transform.position.y) {
                    Die();
                } else {
                    bAttacking = true;
                }
            } else if (other.tag == "H2O") {
                bSwimming = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (FSM != State.Swim) {
                BoundsChecking();
                StateChangeListener();
            } else {
                if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "C_Swim") {
                    anim.SetTrigger("Swim");
                }
            }

            if (canPlayerMove && !bAttacking) {
                //walk
                if (FSM == State.Walk) {
                    Vector3 move = new Vector3(horizontalMovement, 0, 0);
                    controller.Move(move * Time.deltaTime * walkSpeed);
                //Run
                } else if (FSM == State.Run) {
                    Vector3 move = new Vector3(horizontalMovement, 0, 0);
                    controller.Move(move * Time.deltaTime * runSpeed);
                //Swim
                } else if (FSM == State.Swim) {
                    Vector3 move;
                    if (transform.position.y + waterOffset < GameController.Instance.LevelController.Water.transform.position.y) {
                        move = new Vector3(horizontalMovement, moveUp, 0);
                    } else {
                        move = new Vector3(horizontalMovement, 0, 0);
                    }
                    controller.Move(move * Time.deltaTime * swimSpeed);
                //Fall
                } else if (FSM == State.Fall) {
                    Vector3 move = new Vector3(horizontalMovement, (gravityValue / 1.5f), 0);
                    controller.Move(move * Time.deltaTime * walkSpeed);
                }  
            }      
        }

        protected void BoundsChecking()
        {
            if (lastSpawnPosition == 0 && outRightBounds()) {

                canPlayerMove = false;
                Vector3 targetLeftPos = new Vector3(- (transform.position.x + 2.0f), transform.position.y, transform.position.z);
                transform.position = targetLeftPos;

            } else if (lastSpawnPosition == 1 && outLeftBounds()) {

                canPlayerMove = false;
                Vector3 targetRightPos = new Vector3(Mathf.Abs(transform.position.x + 2.0f), transform.position.y, transform.position.z);
                transform.position = targetRightPos;

            } else {

                canPlayerMove = true;

            }
        }

        protected void StateChangeListener()
        {
            //check grounding
            groundedPlayer = IsGrounded();

            //dead
            if (! bAlive) {
                if (FSM != State.Dead) {
                    FSM = State.Dead;
                }
            //attacking
            } else if (bAttacking) {
                if (FSM != State.Attacking) {
                    FSM = State.Attacking;
                    boxCollider.enabled = false;
                    anim.SetTrigger(attackTriggerStrings[0]);
                }
            //swimming
            } else if (bSwimming) {
                if (FSM != State.Swim) {
                    FSM = State.Swim;
                    anim.SetTrigger("Swim");
                    Magic.SetActive(false);
                }
            //falling
            } else if ((!groundedPlayer && !bSwimming) && canPlayerMove && GameController.Instance.LevelController.getCurrentLevel() >= bWhatLevlAmI) {
                
                if (currentFallTimer <= 0.0f) {
                    if (FSM != State.Fall) {
                        FSM = State.Fall;
                        anim.SetTrigger("Fall");
                        Magic.SetActive(false);
                        currentFallTimer = initialFallTimer;
                    }
                } else {
                    currentFallTimer -= Time.deltaTime;
                }

            //running
            } else if (GameController.Instance.LevelController.getCurrentLevel() >= bWhatLevlAmI && (groundedPlayer && !bSwimming)) {
                if (FSM != State.Run) {
                    FSM = State.Run;
                    anim.SetTrigger("Run");
                    Magic.SetActive(true);
                }
            //walking
            } else {
                if (FSM != State.Walk) {
                    FSM = State.Walk;
                    anim.SetTrigger("Walk");
                }
            }
        }

        protected bool IsGrounded()
        {
            bool rayCastFront;
            bool rayCastCenter;

            if (lastSpawnPosition == 0) {
                rayCastCenter = Physics.Raycast(new Vector3(transform.position.x + rayCastOffset, transform.position.y, transform.position.z), Vector3.down, distToGround);//rayCastRight;
                rayCastFront = Physics.Raycast(new Vector3(transform.position.x + rayCastOffset * 2.0f, transform.position.y, transform.position.z), Vector3.down, distToGround);//rayCastRight;
            } else {
                rayCastCenter = Physics.Raycast(new Vector3(transform.position.x - rayCastOffset, transform.position.y, transform.position.z), Vector3.down, distToGround);//rayCastLeft;
                rayCastFront = Physics.Raycast(new Vector3(transform.position.x - rayCastOffset * 2.0f, transform.position.y, transform.position.z), Vector3.down, distToGround);//rayCastLeft;
            }

            if (!rayCastFront && !rayCastCenter) {
                return false;
            }

            return true;




            /*BEST YET
            bool rayCastRight = Physics.Raycast(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Vector3.down, distToGround);
            //bool rayCastAtPosition = Physics.Raycast(transform.position, Vector3.down, distToGround);
            bool rayCastLeft = Physics.Raycast(new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Vector3.down, distToGround);

            bool targetRaycast;
            if (lastSpawnPosition == 0) {
                targetRaycast = rayCastRight;
            } else {
                targetRaycast = rayCastLeft;
            }

            return targetRaycast;
            */
        }

        protected void Die()
        {
            if (bAlive) {
                bAlive = false;
                GameObject deathParticle = ParticlePool.Singleton.getAvailableParticle(ParticlePool.ParticleType.EnemyDeath);
                deathParticle.transform.position = GameController.Instance.Player.transform.position;
                deathParticle.SetActive(true);
                StartCoroutine(DelayDeath());
                AudioController.Singleton.PlayEnemyDeathSound();
            }
        }

        IEnumerator DelayDeath()
        {
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
        }

        /*Detects if enemy moves off the right side of the screen*/
        protected bool outRightBounds()
        {
            targetRightPos = new Vector3(transform.position.x - playerWidth - 2.0f, transform.position.y, transform.position.z);
            bool outta = (GameController.Instance.Camera.WorldToScreenPoint(targetRightPos).x > RightBounds);
            if (outta) {
                currentFallTimer = initialFallTimer;
            }
            return outta;
        }

        /*Detects if enemy moves off the left side of the screen*/
        protected bool outLeftBounds()
        {
            targetLeftPos = new Vector3(transform.position.x + playerWidth, transform.position.y, transform.position.z);
            bool outta = (GameController.Instance.Camera.WorldToScreenPoint(targetLeftPos).x < LeftBounds);
            if (outta) {
                currentFallTimer = initialFallTimer;
            }
            return outta;
        }

        override protected bool bCanDespawn()
        {
            if (outLeftBounds() || outRightBounds()) {
                return true;
            }

            return false;
        }

        public void Shuffle(List<string> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                string value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }

    }
}