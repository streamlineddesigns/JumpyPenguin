using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class LevelPool : MonoBehaviour
    {
        public static LevelPool Singleton;
        public List<GameObject> ObjectsToPool = new List<GameObject>();
        public List<GameObject> Pool = new List<GameObject>();
        private System.Random rng = new System.Random();  

        void Awake()
        {
            if (LevelPool.Singleton == null) {
                LevelPool.Singleton = this;
            }
        }

        void InitializePool()
        {
            for(int i = 0; i < ObjectsToPool.Count; i++) {
                instantiateLevel(i);
            }

            Shuffle(Pool);
        }

        protected void instantiateLevel(int i)
        {
            GameObject level = Instantiate(ObjectsToPool[i], GameController.Instance.LevelController.LevelContainer.transform);
            level.SetActive(false);
            Pool.Add(level);
        }  

        // Start is called before the first frame update
        void Start()
        {
            InitializePool();
        }

        public GameObject getAvailableLevel()
        {
            int index = 0;
            GameObject level = null;
            
            while(index < Pool.Count) {
                if (Pool[index].active == false) {
                    level = Pool[index];
                    break; 
                }
                index++;
            }

            if (level == null) {
                //instantiate some new new and add to pool
                int randomIndex = Random.Range(0, ObjectsToPool.Count);
                instantiateLevel(randomIndex);
                level = Pool[Pool.Count - 1];
            }

            return level;
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