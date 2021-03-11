using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class LevelController : MonoBehaviour
    {
        protected int currentHighestLevel = 3;
        [SerializeField]
        protected int currentLevel = 0;
        protected int score = 0;
        public GameObject Water;
        protected Vector3 targetWaterPosition;
        protected float waterSmoothing = 0.125f;
        protected Vector3 thresholdDistance = new Vector3(0,2.5f,0);

        //Levels
        protected Vector3 highestLevelPosition = new Vector3(0,21,0);
        public GameObject LevelContainer;
        public float levelSpacing = 5.0f;

        // Start is called before the first frame update
        void Start()
        {
            targetWaterPosition = Water.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (! GameController.Instance.isGameActive) {
                return;
            }

            //use highest level position to set new target water position. TODO
            if (currentLevel > 0 && ((Water.transform.position + thresholdDistance).y < highestLevelPosition.y)) {
                Water.transform.position = Vector3.Lerp(Water.transform.position, highestLevelPosition, waterSmoothing * Time.deltaTime);
            }
        }

        public void incrementCurrentLevel()
        {
            currentLevel += 1;
            incrementScore();
        }

        protected void incrementScore()
        {
            score += 1;
            GameController.Instance.UserInterfaceController.updateScore(score);
        }

        public int getCurrentLevel()
        {
            return currentLevel;
        }

        public void updateWaterTargetPosition(Vector3 target)
        {
            targetWaterPosition.y = target.y;
        }

        public void spawnNewLevel()
        {
            currentHighestLevel += 1;
            highestLevelPosition.y += levelSpacing;

            GameObject level = LevelPool.Singleton.getAvailableLevel();
            level.transform.position = highestLevelPosition;
            level.SetActive(true);
        }

        public void spawnLevelCheck()
        {
            if (currentLevel + 2 >= currentHighestLevel) {
                spawnNewLevel();
            }
        }
    }

}