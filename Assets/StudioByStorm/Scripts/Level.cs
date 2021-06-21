using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class Level : MonoBehaviour
    {
        public GameObject[] parts;
        protected float thresholdDistance = 1.1f;
        protected bool bIsLevelDataUpdated;
        public GameObject[] objEnemySpawnPoints;
        protected Vector3[] vecEnemySpawnPoints;
        public bool bDespawning;
        protected float despawnOffset = -10.0f;
        protected Vector3 despawnPosition;
        protected Vector3 iceWallDespawnPosition;
        protected float despawnSmoothing = 1.5f;
        protected GameObject spawnedEnemy;
        protected GameObject spawnedIceWallParticle;
        protected bool bHasIceWall;
        protected bool bIceWallEnabled;
        public float bWhatLevlAmI;
        protected bool isInitialized = false;

        public bool bCanHaveIce;
        protected float lastEnemySpawned;


        //used to determine if parts of the level are fully on screen or not
        public List<Block> BlocksWithinBounds = new List<Block>();
        protected float partWidth = 2.0f;
        protected float RightBounds;
        protected float LeftBounds;
        protected Vector3 targetRightPos;
        protected Vector3 targetLeftPos;
        protected Queue<Block> QueuedBlocks = new Queue<Block>();


        protected bool bGroundObjectsSwitch = false;

        void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            RightBounds = Screen.width;
            LeftBounds = 0;

            //level pieces
            parts = new GameObject[gameObject.transform.childCount];
            
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                parts[i] = gameObject.transform.GetChild(i).gameObject;

                if(isPartInLeftBounds(parts[i]) && isPartInRightBounds(parts[i])) {
                    BlocksWithinBounds.Add(parts[i].GetComponent<Block>());
                }
            }
        }

        public void Despawn()
        {
            bDespawning = true;
        }

        void OnDisable()
        {
            bIsLevelDataUpdated = false;
            for(int l = 0; l < parts.Length; l++){
                parts[l].SetActive(true);
            }

            if (isInitialized && spawnedEnemy != null && spawnedEnemy.activeSelf) {
                spawnedEnemy.GetComponent<Enemy>().Despawn();
            }

            bDespawning = false;

            isInitialized = true;

            /*if (bHasIceWall && bIceWallEnabled) {
                spawnedIceWallParticle.SetActive(false);
            }*/
            
            resetIceWall();

            bGroundObjectsSwitch = false;

            DespawnQueuedBlocks();
        }

        void OnEnable()
        {
            despawnPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + despawnOffset, gameObject.transform.position.z);

            if (isInitialized && GameController.Instance != null && GameController.Instance.isGameActive && ! (objEnemySpawnPoints == null || objEnemySpawnPoints.Length == 0)) {
                SpawnEnemy();
            }

            if (!bGroundObjectsSwitch && isInitialized && GameController.Instance != null && GameController.Instance.isGameActive) {
                bGroundObjectsSwitch = true;
                StartCoroutine(GameController.Instance.LevelController.GroundObjectsController.DetermineGroundObjects(this));
            }
        }

        // Update is called once per frame
        void Update()
        {
            //despawning code
            if (bDespawning) {

                //platform lerp position
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, despawnPosition, despawnSmoothing * Time.deltaTime);
                if (gameObject.transform.position.y - thresholdDistance < despawnPosition.y) {
                    gameObject.SetActive(false);
                }

                //ice lerp position
                /*if (bHasIceWall && bIceWallEnabled) {
                    spawnedIceWallParticle.transform.position = Vector3.Lerp(spawnedIceWallParticle.transform.position, iceWallDespawnPosition, despawnSmoothing * Time.deltaTime);
                    if (spawnedIceWallParticle.transform.position.y - thresholdDistance < iceWallDespawnPosition.y) {
                        spawnedIceWallParticle.SetActive(false);
                        resetIceWall();
                    }
                }*/

                return;
            }

            //if the players y position + the threshold distance is higher than the level y position, enable the level parts
            if (! GameController.Instance.isGameOver && GameController.Instance.Player.transform.position.y + thresholdDistance > gameObject.transform.position.y) {
                for (int i = 0; i < parts.Length; i++) {
                    parts[i].GetComponent<BoxCollider>().enabled = true;
                }

                if (bHasIceWall && !bIceWallEnabled) {
                    EnableIceWall();
                }

                if (! bIsLevelDataUpdated) {
                    bIsLevelDataUpdated = true;
                    GameController.Instance.LevelController.incrementCurrentLevel();
                    GameController.Instance.LevelController.updateWaterTargetPosition(gameObject.transform.position);
                    GameController.Instance.LevelController.spawnLevelCheck();
                }

            //if the players y position + the threshold distance is lower than the level y position, disable the level parts
            } else if (GameController.Instance.Player.transform.position.y + thresholdDistance < gameObject.transform.position.y) {
                for (int i = 0; i < parts.Length; i++) {
                    parts[i].GetComponent<BoxCollider>().enabled = false;
                }
            }
        }

        protected void SpawnEnemy()
        {
            if (lastEnemySpawned == 1 || !bCanHaveIce){
                lastEnemySpawned = 0;

                spawnedEnemy = EnemyPool.Singleton.getAvailableEnemy();
                if (spawnedEnemy != null) {
                    spawnedEnemy.GetComponent<Enemy>().initFromLevel(objEnemySpawnPoints[0].transform.position, objEnemySpawnPoints[1].transform.position, bWhatLevlAmI);
                }
                

            } else if (lastEnemySpawned == 0) {
                lastEnemySpawned = 1;

                if (bCanHaveIce) {
                    StartCoroutine(SpawnIceWall());
                }
            }
        }

        protected IEnumerator SpawnIceWall()
        {
            yield return new WaitForSeconds(0.25f);
            spawnedIceWallParticle = ParticlePool.Singleton.getAvailableParticle(ParticlePool.ParticleType.IcicleWall);
            //if the ice wall isn't active or being used
            if (! spawnedIceWallParticle.activeSelf && spawnedIceWallParticle != null) {
                spawnedIceWallParticle.transform.SetParent(gameObject.transform);
                spawnedIceWallParticle.transform.localPosition = new Vector3(0,0,0);
                iceWallDespawnPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + despawnOffset, gameObject.transform.position.z);
                bHasIceWall = true;
            } else {
                resetIceWall();
            }
        }

        protected void EnableIceWall()
        {
            if (! spawnedIceWallParticle.activeSelf) {
                bIceWallEnabled = true;
                spawnedIceWallParticle.SetActive(true);
            }
        }

        protected void resetIceWall()
        {
            bHasIceWall = false;
            spawnedIceWallParticle = null;
            bIceWallEnabled = false;
        }

        //not being used anymore
        protected void initEnemySpawnPoints()
        {
            if (! (objEnemySpawnPoints == null || objEnemySpawnPoints.Length == 0)) {
                vecEnemySpawnPoints = new Vector3[objEnemySpawnPoints.Length];
                for (int j = 0; j < objEnemySpawnPoints.Length; j++) {
                    //get point to world space
                    vecEnemySpawnPoints[j] = objEnemySpawnPoints[j].transform.TransformPoint(Vector3.zero);
                }   
            }
        }

        protected void DespawnQueuedBlocks()
        {
            if (QueuedBlocks.Count <= 0) {
                return;
            }

            while(QueuedBlocks.Count > 0)
            {
                Block b = QueuedBlocks.Dequeue();
                if (b != null) b.gameObject.SetActive(false);
            }
        }

        public void EnqueueBlocks(List<Block> blocks)
        {
            for(int i = 0; i < blocks.Count; i++) {
                QueuedBlocks.Enqueue(blocks[i]);
            }
        }

        protected bool isPartInRightBounds(GameObject part)
        {
            targetRightPos = new Vector3(part.transform.position.x - partWidth + 0.4f, part.transform.position.y, part.transform.position.z);
            return (GameController.Instance.Camera.WorldToScreenPoint(targetRightPos).x < RightBounds);
        }

        protected bool isPartInLeftBounds(GameObject part)
        {
            targetLeftPos = new Vector3(part.transform.position.x + partWidth - 0.4f, part.transform.position.y, part.transform.position.z);
            return (GameController.Instance.Camera.WorldToScreenPoint(targetLeftPos).x > LeftBounds);
        }
    }
}
