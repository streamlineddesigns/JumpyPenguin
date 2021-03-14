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

            return enemy;
        }

    }
}