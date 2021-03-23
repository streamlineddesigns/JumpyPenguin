using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class EnemyPool : MonoBehaviour
    {
        public static EnemyPool Singleton;
        public List<GameObject> ObjectsToPool = new List<GameObject>();
        public List<GameObject> Pool = new List<GameObject>();
        public float lastSpawnPosition = 0;//0=left; 1=right;
        private System.Random rng = new System.Random(); 

        void Awake()
        {
            if (EnemyPool.Singleton == null) {
                EnemyPool.Singleton = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            InitializePool();
        }

        void InitializePool()
        {
            for(int i = 0; i < ObjectsToPool.Count; i++) {
                instantiateEnemy(i);
            }
        }

        protected void instantiateEnemy(int i)
        {
            GameObject enemy = Instantiate(ObjectsToPool[i], GameController.Instance.LevelController.LevelContainer.transform);
            enemy.SetActive(false);
            Pool.Add(enemy);
        } 

        public GameObject getAvailableEnemy()
        {
            Shuffle(Pool);
            
            int index = 0;
            GameObject enemy = null;
            
            while(index < Pool.Count) {
                if (Pool[index].active == false) {
                    enemy = Pool[index];
                    break; 
                }
                index++;
            }

            if (enemy == null) {
                //its okay to be null
            }

            if (lastSpawnPosition == 0) {
                lastSpawnPosition = 1;
            } else {
                lastSpawnPosition = 0;
            }

            return enemy;
        }

        public void Shuffle(List<GameObject> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                GameObject value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }

    }
}