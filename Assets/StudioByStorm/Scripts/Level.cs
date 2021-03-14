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
        protected float despawnSmoothing = 1.5f;
        protected GameObject spawnedEnemy;
        public float bWhatLevlAmI;

        void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            //level pieces
            parts = new GameObject[gameObject.transform.childCount];
            
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                parts[i] = gameObject.transform.GetChild(i).gameObject;
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

            if (spawnedEnemy != null) {
                spawnedEnemy.SetActive(false);
            }

            bDespawning = false;
        }

        void OnEnable()
        {
            despawnPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + despawnOffset, gameObject.transform.position.z);

            if (GameController.Instance != null && GameController.Instance.isGameActive && ! (objEnemySpawnPoints == null || objEnemySpawnPoints.Length == 0)) {
                
                initEnemySpawnPoints();

                spawnedEnemy = EnemyPool.Singleton.getAvailableEnemy();
                if (spawnedEnemy != null) {
                    spawnedEnemy.SetActive(true);
                    spawnedEnemy.transform.position = vecEnemySpawnPoints[0];
                    spawnedEnemy.GetComponent<Enemy>().bWhatLevlAmI = bWhatLevlAmI;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            //despawning code
            if (bDespawning) {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, despawnPosition, despawnSmoothing * Time.deltaTime);
                if (gameObject.transform.position.y - thresholdDistance < despawnPosition.y) {
                    gameObject.SetActive(false);
                    //Debug.LogError("DESPAWNING!:  " + gameObject.transform.position);
                }
                return;
            }

            //if the players y position + the threshold distance is higher than the level y position, enable the level parts
            if (GameController.Instance.Player.transform.position.y + thresholdDistance > gameObject.transform.position.y) {
                for (int i = 0; i < gameObject.transform.childCount; i++) {
                    parts[i].GetComponent<BoxCollider>().enabled = true;
                }

                if (! bIsLevelDataUpdated) {
                    bIsLevelDataUpdated = true;
                    GameController.Instance.LevelController.incrementCurrentLevel();
                    GameController.Instance.LevelController.updateWaterTargetPosition(gameObject.transform.position);
                    GameController.Instance.LevelController.spawnLevelCheck();
                }

            //if the players y position + the threshold distance is lower than the level y position, disable the level parts
            } else if (GameController.Instance.Player.transform.position.y + thresholdDistance < gameObject.transform.position.y) {
                for (int i = 0; i < gameObject.transform.childCount; i++) {
                    parts[i].GetComponent<BoxCollider>().enabled = false;
                }
            }
        }


        void initEnemySpawnPoints()
        {
            if (! (objEnemySpawnPoints == null || objEnemySpawnPoints.Length == 0)) {
                vecEnemySpawnPoints = new Vector3[objEnemySpawnPoints.Length];
                for (int j = 0; j < objEnemySpawnPoints.Length; j++) {
                    //get point to world space
                    vecEnemySpawnPoints[j] = objEnemySpawnPoints[j].transform.TransformPoint(Vector3.zero);
                }   
            }
        }
    }
}
