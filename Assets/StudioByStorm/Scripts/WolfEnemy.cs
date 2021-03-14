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
        protected float moveLeft = -1.0f;
        protected float moveRight = 1.0f;
        public bool canPlayerMove;

        //Speed
        protected Vector3 playerVelocity;
        protected float runSpeed = 10.0f;
        protected float walkSpeed = 3.0f;
        protected float swimSpeed = 3.0f;

        //Grounding
        protected float gravityValue = -9.81f;
        protected float distToGround = 1.0f;
        public bool groundedPlayer;

        //State & Animation
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
        protected float playerWidth = 3.0f;
        protected float RightBounds;
        protected float LeftBounds;
        protected Vector3 targetRightPos;
        protected Vector3 targetLeftPos;

        void Awake()
        {
            RightBounds = Screen.width;
            LeftBounds = 0;
            canPlayerMove = true;
        }

        void OnEnable()
        {
            currentFallTimer = initialFallTimer;
            bAttacking = false;
            bSwimming = false;
            bAlive = true;
            FSM = State.Walk;
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
                if (other.gameObject.transform.position.y - 1.5f > transform.position.y) {
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
            if (!bSwimming) {
                BoundsChecking();
            }

            StateChangeListener();

            groundedPlayer = IsGrounded();
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            if (canPlayerMove && !bAttacking) {
                //walk
                if (FSM == State.Walk) {
                    Vector3 move = new Vector3(moveRight, 0, 0);
                    controller.Move(move * Time.deltaTime * walkSpeed);
                //Run
                } else if (FSM == State.Run) {
                    Vector3 move = new Vector3(moveRight, 0, 0);
                    controller.Move(move * Time.deltaTime * runSpeed);
                //Swim
                } else if (FSM == State.Swim) {
                    Vector3 move;
                    if (transform.position.y < GameController.Instance.LevelController.Water.transform.position.y) {
                        float offset = (Mathf.Abs(GameController.Instance.LevelController.Water.transform.position.y - transform.position.y)) / 5.0f;
                        move = new Vector3(moveRight, offset, 0);
                    } else {
                        move = new Vector3(moveRight, 0, 0);
                    }
                    controller.Move(move * Time.deltaTime * swimSpeed);
                } 
            }

            //Fall
            if (FSM == State.Fall) {
                playerVelocity.y += (gravityValue * 2.0f) * Time.deltaTime;
                controller.Move(playerVelocity * Time.deltaTime);
            }        
        }

        protected void BoundsChecking()
        {
            if (outRightBounds()) {

                canPlayerMove = false;
                Vector3 targetLeftPos = new Vector3(- transform.position.x - (playerWidth / 2), transform.position.y, transform.position.z);
                transform.position = targetLeftPos;

            } else if (outLeftBounds()) {

                canPlayerMove = false;
                Vector3 targetRightPos = new Vector3(Mathf.Abs(transform.position.x) - (playerWidth / 2), transform.position.y, transform.position.z);
                transform.position = targetRightPos;

            } else {

                canPlayerMove = true;

            }
        }

        protected void StateChangeListener()
        {
            //dead
            if (! bAlive) {
                if (FSM != State.Dead) {
                    FSM = State.Dead;
                    //Bring in particle system
                    gameObject.SetActive(false);
                }
            //attacking
            } else if (bAttacking) {
                if (FSM != State.Attacking) {
                    FSM = State.Attacking;
                    anim.SetTrigger(attackTriggerStrings[0]);
                }
            //swimming
            } else if (bSwimming) {
                if (FSM != State.Swim) {
                    FSM = State.Swim;
                    anim.SetTrigger("Swim");
                }
            //falling
            } else if (! groundedPlayer && canPlayerMove && GameController.Instance.LevelController.getCurrentLevel() >= bWhatLevlAmI) {
                
                if (currentFallTimer <= 0.0f) {
                    if (FSM != State.Fall) {
                        FSM = State.Fall;
                        anim.SetTrigger("Fall");
                    }
                } else {
                    currentFallTimer -= Time.deltaTime;
                }

            //running
            } else if (GameController.Instance.LevelController.getCurrentLevel() >= bWhatLevlAmI) {
                if (FSM != State.Run) {
                    FSM = State.Run;
                    anim.SetTrigger("Run");
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
            return Physics.Raycast(transform.position, Vector3.down, distToGround);
        }

        protected void Die()
        {
            bAlive = false;
        }

        /*Detects if enemy moves off the right side of the screen*/
        protected bool outRightBounds()
        {
            targetRightPos = new Vector3(transform.position.x - playerWidth * 2, transform.position.y, transform.position.z);
            bool outta = (GameController.Instance.Camera.WorldToScreenPoint(targetRightPos).x > RightBounds);
            if (outta) {
                currentFallTimer = initialFallTimer;
            }
            return outta;
        }

        /*Detects if enemy moves off the left side of the screen*/
        protected bool outLeftBounds()
        {
            targetLeftPos = new Vector3(transform.position.x + playerWidth * 2, transform.position.y, transform.position.z);
            bool outta = (GameController.Instance.Camera.WorldToScreenPoint(targetLeftPos).x < LeftBounds);
            if (outta) {
                currentFallTimer = initialFallTimer;
            }
            return outta;
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