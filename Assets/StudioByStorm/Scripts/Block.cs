using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class Block : MonoBehaviour
    {
        //The type of block it is when initialized
        public GroundObjectsController.BlockType InitialBlockType;
    
        /*
         * The current block type is a bit different.
         * Because GroundObjectsController can place blocks of different BlockTypes on this block.
         * So sometimes we want our CurrentBlockType to temporarily become the BlockType of the block that is being placed on it
         */
        protected GroundObjectsController.BlockType CurrentBlockType;

        //If the block is Overridden
        public bool bCurrentOverridden;

        public void OnEnable()
        {
            CurrentBlockType = InitialBlockType;
            bCurrentOverridden = false;
        }

        public void SetCurrentBlockType(GroundObjectsController.BlockType bt)
        {
            CurrentBlockType = bt;
            bCurrentOverridden = true;
        }

        public GroundObjectsController.BlockType GetCurrentBlockType()
        {
            return CurrentBlockType;
        }
    }
}