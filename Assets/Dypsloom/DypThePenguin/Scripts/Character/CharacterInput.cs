/// ---------------------------------------------
/// Dyp The Penguin Character | Dypsloom
/// Copyright (c) Dyplsoom. All Rights Reserved.
/// https://www.dypsloom.com
/// ---------------------------------------------

namespace Dypsloom.DypThePenguin.Scripts.Character
{
    using Dypsloom.DypThePenguin.Scripts.Items;
    using UnityEngine;
    using StudioByStorm.Scripts;
    using Lean.Gui;

    /// <summary>
    /// The character Input.
    /// </summary>
    public class CharacterInput : ICharacterInput
    {
        protected Character m_Character;
        protected float horizontalInput;
        protected float horizontalMovementSlideTime = (1.0f / 60.0f) / 3.5f;
        protected float verticalInput;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="character"></param>
        public CharacterInput(Character character)
        {
            m_Character = character;
        }

        public float Horizontal => getHorizontalInput();//Input.GetAxisRaw("Horizontal");
        public float Vertical => Input.GetAxisRaw("Vertical");
        public bool Jump => getJumpInput();//Input.GetButtonDown("Jump");
        public bool Interact => (Input.GetKeyDown(KeyCode.E) ||Input.GetButtonDown("Fire2"));

        /// <summary>
        /// The input to use an item action.
        /// </summary>
        /// <param name="usableItemObject">The usable ItemObject.</param>
        /// <param name="actionIndex">The action index.</param>
        /// <returns>True if the input is valid.</returns>
        public bool UseEquippedItemInput(IUsableItem usableItemObject, int actionIndex)
        {
            if (usableItemObject == null || usableItemObject.Item == null) { return false; }

            if (actionIndex == 0) {
                return Input.GetButtonDown("Fire1");
            }

            return true;
        }

        /// <summary>
        /// Use the item hot bar button.
        /// </summary>
        /// <param name="slotIndex">The hot bar index</param>
        /// <returns>True if the item should be used.</returns>
        public bool UseItemHotbarInput(int slotIndex)
        {
            return !Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1 + slotIndex);
        }

        /// <summary>
        /// Drop the item in the hot bar slot specified.
        /// </summary>
        /// <param name="slotIndex">The slot index.</param>
        /// <returns>True if the item should be dropped.</returns>
        public bool DropItemHotbarInput(int slotIndex)
        {
            return Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1 + slotIndex);
        }


        public float getHorizontalInput() 
        {
            if (! GameController.Instance.isGameActive) {
                return 0.0f;
            }

            if (MobileInput.Singleton.LeanJoyStick.ScaledValue.x != 0.0f) {

                if (MobileInput.Singleton.SwipeLeft) {
                    horizontalInput = -1.0f;
                    playWalkSound();
                } else if (MobileInput.Singleton.SwipeRight) {
                    horizontalInput = 1.0f;
                    playWalkSound();
                }

            } else {
                //if previously sliding
                if (horizontalInput != 0.0f && m_Character.IsGrounded) {

                    //if previously sliding left -1
                    if (horizontalInput < 0.0f) {
                        if (horizontalInput != 0.0f) {
                            if (horizontalInput == -1) {
                                playSlideSound();
                            }
                            horizontalInput += horizontalMovementSlideTime;
                        }

                    //if previously sliding right +1
                    } else if (horizontalInput > 0.0f) {
                        if (horizontalInput != 0.0f) {
                            if (horizontalInput == 1) {
                                playSlideSound();
                            }
                            horizontalInput -= horizontalMovementSlideTime;
                        }
                    }

                }
            }

            if (! m_Character.IsGrounded) {
                stopGroundSounds();
            }

            if ((horizontalInput > 0.0f && horizontalInput < 0.1f) || (horizontalInput < 0.0f && horizontalInput > -0.1f)) {
                horizontalInput = 0.0f;
            }

            return horizontalInput;
        }

        public bool getJumpInput() 
        {
            if (! GameController.Instance.isGameActive) {
                return false;
            }

            if (MobileInput.Singleton.SwipeUp) {
                AudioController.Singleton.PlayJumpSound();
            }
            return MobileInput.Singleton.SwipeUp;
            //return MobileInput.Singleton.JumpButtonPressed;
        }

        protected void playSlideSound()
        {
            if (m_Character.IsGrounded) {
                AudioController.Singleton.StopSnowWalkingSound();
                AudioController.Singleton.PlaySlideSound();
            } else {
                stopGroundSounds();
            }
        }

        protected void playWalkSound()
        {
            if (m_Character.IsGrounded) {
                AudioController.Singleton.StopSlideSound();
                AudioController.Singleton.PlaySnowWalkingSound();
            } else {
                stopGroundSounds();
            }
        }

        protected void stopGroundSounds()
        {
            AudioController.Singleton.StopSlideSound();
            AudioController.Singleton.StopSnowWalkingSound();
        }
    }
}