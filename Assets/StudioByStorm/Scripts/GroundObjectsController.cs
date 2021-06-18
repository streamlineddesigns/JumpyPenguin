using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class GroundObjectsController : MonoBehaviour
    {
        //Rule of thumb: Block types generally have different sounds when walked on, and each may have its own behavior
        public enum BlockType {
            Default,
            Snow,
            SnowDeformable,//Top
            SnowBreakable,
            Ice,
            UndersideIcicle,//Top
            Rock,
            Wood,
            Dirt,
            Mud,//Top
            Grass,
            MovingGrass,//Top
            Flowers,//Top & Scenery
            Oil,//Top
            Tree,
            TreeStump,
        };

        public GameObject blocksContainer;
        protected int blocksToPoolCount = 5;
        public List<GameObject> blocksToPool = new List<GameObject>();
        protected Dictionary<BlockType, List<Block>> blocksRegistry = new Dictionary<BlockType, List<Block>>();

        protected System.Random rando = new System.Random();

        //For scenery
        protected bool bLastScenerySpawnedInFront = false;
        protected float sceneryOffset = 3.0f;

        public void Awake()
        {
            GenerateRegistry();
        }

        /*
         * iterate over the blocks to pool and add them to the registry by their BlockType
         */
        public void GenerateRegistry()
        {
            if (blocksToPool != null) {

                for (int i = 0; i < blocksToPool.Count; i++) {
                    
                    //create empty list of blocks
                    List<Block> blockList = new List<Block>();
                    BlockType blockType = BlockType.Default;

                    //instantiate & add blocks to that list
                    for (int j = 0; j < blocksToPoolCount; j++) {
                        Block block = Instantiate(blocksToPool[i], blocksContainer.transform).GetComponent<Block>();
                        blockType = block.InitialBlockType;
                        block.gameObject.SetActive(false);
                        blockList.Add(block);
                    }

                    //Add that list to the block registry by it's blocktype
                    if (! blocksRegistry.ContainsKey(blockType)) {
                        blocksRegistry.Add(blockType, blockList);
                    } else {
                        Debug.LogError("All ready registered BlockType: " + blockType + " Double check the BlockType of any newly added blocks");
                    }
                }
            }
        }

        /*
         * Retrieve a Block from the BlocksRegistry by supplying a BlockType
         * @param Block.BlockType bt : the key to the registry
         * @return Block : an available block of type bt; if any
         */
        public Block GetBlockByType(BlockType bt)
        {
            int index = 0;
            Block block = null;

            if (blocksRegistry.ContainsKey(bt)) {
                
                while(index < blocksRegistry[bt].Count) {
                    if (blocksRegistry[bt][index].gameObject.active == false) {
                        block = blocksRegistry[bt][index];
                        break; 
                    }
                    index++;
                }

            }

            return block;
        }

        //NOTE: Levels can end up with multiple Ground Objects for scenery, etc.
        //This is unintentional and could only be fixed by de-parenting blocks from the blocks we're placing them on OnDisable of the Level

        /*
         * Places special blocks in a level & called by levels themselves
         * @param List<Block> BlocksWithinBounds : a level's Blocks that are within the screens viewport
         */
        public IEnumerator DetermineGroundObjects(List<Block> BlocksWithinBounds)
        {
            yield return new WaitForSeconds(0.1f);

            //Get a random Block from the list for tops
            int blockIndex = rando.Next(BlocksWithinBounds.Count);

            //Get another random Block from list for scenery
            int sceneryIndex = rando.Next(BlocksWithinBounds.Count);
            
            //safety check so we aren't accessing non existent indexs
            if (BlocksWithinBounds.Count > 0) {
                //Set's the block's top's
                Block b = BlocksWithinBounds[blockIndex];
                SetBlockTop(b);

                //Set the scenery
                Block s = BlocksWithinBounds[sceneryIndex];
                SetScenery(s);
            }
            
        }

        /*
         * add the proper Top to the supplied block
         * @param Block b : the block that we're adding a top to
         */
        public void SetBlockTop(Block b)
        {
            //The new blocktype that will override b's blocktype
            BlockType newBlockType = BlockType.Default;

            //detect the blocktype of b and determine a corresponding newBlockType
            switch(b.InitialBlockType) {

                case BlockType.Snow :
                    newBlockType = BlockType.SnowDeformable;//Tested
                    break;
                case BlockType.Ice :
                    newBlockType = BlockType.UndersideIcicle;//Tested
                    break;
                case BlockType.Dirt :
                    newBlockType = BlockType.Mud;
                    break;
                case BlockType.Grass :
                    newBlockType = BlockType.MovingGrass;
                    break;

            }

            //if a corresponding newBlockType was found
            if (newBlockType != BlockType.Default) {

                //get the new BlockTop to use
                Block BlockTop = GetBlockByType(newBlockType);

                if (BlockTop != null) {
                    //put the BlockTop in scene
                    Vector3 targetTopPosition = b.gameObject.transform.position;
                    BlockTop.gameObject.transform.SetParent(b.gameObject.transform);
                    BlockTop.gameObject.transform.position = targetTopPosition;
                    BlockTop.gameObject.SetActive(true);
                    b.SetCurrentBlockType(newBlockType);
                    //Debug.LogError("Placing: " + newBlockType);
                } else {
                    //Debug.Log("couldn't set the new top. This means they're either all in use, or the blocktype of the request block hasn't been added to the registry yet");
                }
            }
        }

        public void SetScenery(Block s)
        {
            //get the new SceneryBlock to use
            Block SceneryBlock = GetBlockByType(s.InitialBlockType);

            //if a corresponding blocktype was found
            if (SceneryBlock != null) {

                //put the block in the scene
                Vector3 targetPos = s.gameObject.transform.position;
                if (bLastScenerySpawnedInFront) {
                    bLastScenerySpawnedInFront = false;
                    targetPos.z += sceneryOffset;
                } else {
                    bLastScenerySpawnedInFront = true;
                    targetPos.z -= sceneryOffset;
                }
                
                SceneryBlock.gameObject.transform.SetParent(s.gameObject.transform);
                SceneryBlock.gameObject.transform.position = targetPos;
                SceneryBlock.gameObject.SetActive(true);
                //Debug.LogError("Placing scenery");
            }
        }
    }
}


///Oil will be placed using a seperate script.
///Level OnEnable passes BlocksWithinBounds
//Ice => Icicles underside
//Snow => Snow Top
//Grass => Grass Top
//Dirt => Mud Top
        /*
         * [Block Obstacles]
         * Snow Top
         * Grass
         * Flowers
         * Water
         * Oil
         * Underside Icicles
         */
        
        /*
         * [Scenery (Not on path)]
         * Trees
         * Stumps
         * Grass
         * Flowers
         * Waterfalls
         */

        /*
         * [Level Obstacles]
         * Wolf
         * Giant Snowballs
         * Target Icicles
         */