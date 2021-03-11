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

        // Start is called before the first frame update
        void Start()
        {
            parts = new GameObject[gameObject.transform.childCount];
            
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                parts[i] = gameObject.transform.GetChild(i).gameObject;
            }
        }

        // Update is called once per frame
        void Update()
        {
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
    }
}
