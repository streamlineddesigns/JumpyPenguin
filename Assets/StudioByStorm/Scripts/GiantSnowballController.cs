using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class GiantSnowballController : MonoBehaviour
    {
        protected CharacterController controller;
        protected float Speed = 3.0f;

        public float playerWidth = 3.0f;
        protected float RightBounds;
        protected float LeftBounds;
        protected Vector3 targetRightPos;
        protected Vector3 targetLeftPos;

        protected float lastSpawnPosition = 0;//0=left; 1=right;
        protected float horizontalMovement = 0.0f;
        protected float moveLeft = -1.0f;
        protected float moveRight = 1.0f;
        protected float walkSpeed = 3.0f;
        public bool canPlayerMove;

        //Grounding
        protected float gravityValue = -9.81f;
        protected float distToGround = 1.0f;
        public bool groundedPlayer;
        protected float rayCastOffset = 0.5f;

        public enum State {
            Walk,
            Fall,
        };
        public State FSM;

        //fall timer
        protected float initialFallTimer = 1.0f;
        protected float currentFallTimer;

        public void Awake()
        {
            RightBounds = Screen.width;
            LeftBounds = 0;
            canPlayerMove = true;
        }

        public void Start()
        {
            controller = GetComponent<CharacterController>();
            horizontalMovement = moveLeft;
            lastSpawnPosition = 1;
        }

        public void OnEnable()
        {
            currentFallTimer = initialFallTimer;
        }

        public void Update()
        {
            BoundsChecking();
            StateChangeListener();

            if (canPlayerMove) {
                //walk
                if (FSM == State.Walk) {
                    Vector3 move = new Vector3(horizontalMovement, 0, 0);
                    controller.Move(move * Time.deltaTime * walkSpeed);
                //Fall
                } else if (FSM == State.Fall) {
                    Vector3 move = new Vector3(horizontalMovement, (gravityValue / 1.5f), 0);
                    controller.Move(move * Time.deltaTime * walkSpeed);
                }  
            }
            
        } 

        protected void StateChangeListener()
        {
            //check grounding
            groundedPlayer = IsGrounded();

            //falling
            if (!groundedPlayer && canPlayerMove) {
                
                if (currentFallTimer <= 0.0f) {
                    if (FSM != State.Fall) {
                        FSM = State.Fall;
                        currentFallTimer = initialFallTimer;
                    }
                } else {
                    if (FSM != State.Fall) {
                        currentFallTimer -= Time.deltaTime;
                    }
                }

            //Moving
            } else {
                if (FSM != State.Walk) {
                    FSM = State.Walk;
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

        protected bool outRightBounds()
        {
            targetRightPos = new Vector3(transform.position.x - playerWidth - 2.0f, transform.position.y, transform.position.z);
            bool outta = (GameController.Instance.Camera.WorldToScreenPoint(targetRightPos).x > RightBounds);
            if (outta) {
                currentFallTimer = initialFallTimer;
            }
            return outta;
        }

        protected bool outLeftBounds()
        {
            targetLeftPos = new Vector3(transform.position.x + playerWidth, transform.position.y, transform.position.z);
            bool outta = (GameController.Instance.Camera.WorldToScreenPoint(targetLeftPos).x < LeftBounds);
            if (outta) {
                currentFallTimer = initialFallTimer;
            }
            return outta;
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
        }
    }
}