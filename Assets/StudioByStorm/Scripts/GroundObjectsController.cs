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
            Logs,
            Water,
            WaterFall,
            Snowman,
        };

        [Tooltip("Add any newly created scenery BlockType's please. These get placed ontop of the ground itself. ie tree, flower, etc")]
        public List<BlockType> SceneryBlockTypes = new List<BlockType>();

        [Tooltip("Add BlockType's who's pool size should be dynamic & able to grow on the fly instead of being fixed")]
        public List<BlockType> DynamicallyPooledBlockTypes = new List<BlockType>();

        //Keeps associations between BlockTypes and their prefabs
        protected Dictionary<BlockType, GameObject> BlockTypePrefabs = new Dictionary<BlockType, GameObject>();

        public GameObject blocksContainer;
        protected int blocksToPoolCount = 5;

        [Tooltip("Add Block prefabs to this list to have pools created for it's BlockType")]
        public List<GameObject> blocksToPool = new List<GameObject>();

        protected Dictionary<BlockType, List<Block>> blocksRegistry = new Dictionary<BlockType, List<Block>>();

        protected System.Random rando = new System.Random();

        //For scenery
        protected bool bLastScenerySpawnedInFront = false;
        protected float sceneryOffset = 3.0f;
        public TreesController TreesController; 

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

                        AddToBlockTypePrefabs(blockType, blocksToPool[i]);
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

            //if the block registry has a key/value pair for the supplied BlockType
            if (blocksRegistry.ContainsKey(bt)) {
                
                //check to see if there's an available Block of type BlockType
                while(index < blocksRegistry[bt].Count) {
                    if (! blocksRegistry[bt][index].gameObject.activeSelf) {
                        block = blocksRegistry[bt][index];
                        break; 
                    }
                    index++;
                }

                //if there wasn't an available Block of type BlockType, && it's a dynamically pooled block type
                if (block == null && DynamicallyPooledBlockTypes.Contains(bt)) {

                    //create a new block and add it to its corresponding pool so we have an available block
                    block = Instantiate(GetPrefabByBlockType(bt), blocksContainer.transform).GetComponent<Block>();
                    block.gameObject.SetActive(false);
                    blocksRegistry[bt].Add(block);

                    Debug.LogError("Instantiating DynamicallyPooledBlockType: " + bt);
                }

            }

            return block;
        }

        /*
         * Adds blocktype/prefab associations to BlockTypePrefabs
         * @param BlockType bt : the key used in BlockTypePrefabs
         * @param GameObject prefab : the value to bt
         */
        protected void AddToBlockTypePrefabs(BlockType bt, GameObject prefab)
        {
            //makes sure to only add association one time
            if (! BlockTypePrefabs.ContainsKey(bt)) {
                BlockTypePrefabs.Add(bt, prefab);
            }
        }

        /*
         * Used to retrieve prefabs by BlockType from BlockTypePrefabs dictionary
         * @param BlockType bt : the key used in BlockTypePrefabs
         * @return GameObject : the value to the key supplied
         */
        protected GameObject GetPrefabByBlockType(BlockType bt)
        {
            GameObject prefab = null;
            if (BlockTypePrefabs.ContainsKey(bt)) {
                prefab = BlockTypePrefabs[bt];
            }
            return prefab;
        }

        /*
         * Places special blocks in a level & called by levels themselves
         * @param List<Block> BlocksWithinBounds : a level's Blocks that are within the screens viewport
         */
        public IEnumerator DetermineGroundObjects(Level level)
        {
            yield return new WaitForSeconds(0.1f);

            //Get a random Block from the list for tops
            int blockIndex = rando.Next(level.BlocksWithinBounds.Count);

            //Get another random Block from list for ground block
            int sceneryIndex = rando.Next(level.BlocksWithinBounds.Count);
            
            //safety check so we aren't accessing non existent indexs
            if (level.BlocksWithinBounds.Count > 0) {
                //blocks to Queue up. Need these so they can be deactivated properly
                List<Block> blocksToQueue = new List<Block>();

                //Set's the block's top's
                Block b = level.BlocksWithinBounds[blockIndex];
                Block blockTop = SetBlockTop(b);
                //Enqueue blockTop
                if (blockTop != null) {
                    blocksToQueue.Add(blockTop);
                }

                //Set the scenery ground block
                Block s = level.BlocksWithinBounds[sceneryIndex];
                Block sceneryGroundBlock = SetSceneryGroundBlock(s);
                if (sceneryGroundBlock != null) {
                    //Enqueue sceneryGroundBlock
                    blocksToQueue.Add(sceneryGroundBlock);

                    //Set the scenery on the ground block
                    Block sceneryBlock = SetSceneryOnGround(sceneryGroundBlock);
                    //Enqueue sceneryBlock
                    if (sceneryBlock != null) {
                        blocksToQueue.Add(sceneryBlock);
                    }

                }

                //add blocks to the levels block queue
                level.EnqueueBlocks(blocksToQueue);
            }
            
        }

        /*
         * add the proper Top to the supplied block
         * @param Block b : the block that we're adding a top to
         * @return Block : the block top, if any
         */
        public Block SetBlockTop(Block b)
        {
            Block BlockTop = null;

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
                /*case BlockType.Grass :
                    newBlockType = BlockType.MovingGrass;
                    break;*/

            }

            //if a corresponding newBlockType was found
            if (newBlockType != BlockType.Default) {

                //get the new BlockTop to use
                BlockTop = GetBlockByType(newBlockType);

                if (BlockTop != null) {
                    //put the BlockTop in scene
                    Vector3 targetTopPosition = b.gameObject.transform.position;
                    BlockTop.gameObject.transform.SetParent(b.gameObject.transform);
                    BlockTop.gameObject.transform.position = targetTopPosition;
                    BlockTop.gameObject.SetActive(true);
                    //##$$!!FIX
                    //only override blocktype if the new blocktype meets certain criteria 
                    if (newBlockType != BlockType.UndersideIcicle && newBlockType != BlockType.MovingGrass) {
                        b.SetCurrentBlockType(newBlockType);
                    }
                    //Debug.LogError("Placing: " + newBlockType);
                } else {
                    //Debug.Log("couldn't set the new top. This means they're either all in use, or the blocktype of the request block hasn't been added to the registry yet");
                }
            }

            return BlockTop;
        }

        /*
         * add a ground scenery block to the supplied block
         * @param Block s : the block that we're adding another ground scenery block to
         * @return Block : the scenery block, if any
         */
        public Block SetSceneryGroundBlock(Block s)
        {
            //get the new SceneryBlock to use
            Block SceneryGroundBlock = GetBlockByType(s.InitialBlockType);

            //if a corresponding blocktype was found
            if (SceneryGroundBlock != null) {

                //put the block in the scene
                Vector3 targetPos = s.gameObject.transform.position;
                if (bLastScenerySpawnedInFront) {
                    bLastScenerySpawnedInFront = false;
                    targetPos.z += sceneryOffset;
                } else {
                    bLastScenerySpawnedInFront = true;
                    targetPos.z -= sceneryOffset;
                }
                
                SceneryGroundBlock.gameObject.transform.SetParent(s.gameObject.transform);
                SceneryGroundBlock.gameObject.transform.position = targetPos;
                SceneryGroundBlock.gameObject.SetActive(true);
                //Debug.LogError("Placing scenery");
            }

            return SceneryGroundBlock;
        }

        /*
         * Add actual scenery to a scenery ground block
         * @param Block SceneryGroundBlock : the ground block that we're placing scenery onto
         * @return Block : the scenery block that we just placed
         */
        protected Block SetSceneryOnGround(Block SceneryGroundBlock)
        {
            //get random index from SceneryBlockTypes list
            int randomIndex = rando.Next(SceneryBlockTypes.Count); 
            //get a random BlockType from SceneryBlockTypes using that index
            BlockType bt = SceneryBlockTypes[randomIndex];
            //get a sceneryBlock from it's corresponding pool using that random BlockType
            Block SceneryBlock = GetBlockByType(bt);
            //if there's an available SceneryBlock of type bt, add it to the scene
            if (SceneryBlock != null && SceneryGroundBlock != null) {
                Vector3 targetPos = SceneryGroundBlock.gameObject.transform.position;
                SceneryBlock.gameObject.transform.SetParent(SceneryGroundBlock.gameObject.transform);
                SceneryBlock.gameObject.transform.position = targetPos;
                SceneryBlock.gameObject.SetActive(true);
                //Debug.LogError("We found a match for on top of the ground: " + bt);
            }
            return SceneryBlock;
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