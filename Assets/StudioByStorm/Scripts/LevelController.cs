using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class LevelController : MonoBehaviour
    {
        protected int currentHighestLevel = 3;
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
        protected Queue<Level> levelQueue = new Queue<Level>();

        //Weather
        public GameObject Snow;
        protected float snowOffset = 30.0f;

        // Start is called before the first frame update
        void Start()
        {
            targetWaterPosition = Water.transform.position;
            UpdateSnowPosition();
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

        public void incrementScore()
        {
            score += 1;
            GameController.Instance.UserInterfaceController.updateScore(score);
        }

        public int getCurrentLevel()
        {
            return currentLevel;
        }

        public int getCurrentScore()
        {
            return score;
        }

        public void updateWaterTargetPosition(Vector3 target)
        {
            targetWaterPosition.y = target.y;
        }

        public void spawnNewLevel()
        {
            currentHighestLevel += 1;
            highestLevelPosition.y += levelSpacing;

            UpdateSnowPosition();

            GameObject level = LevelPool.Singleton.getAvailableLevel();
            level.transform.position = highestLevelPosition;
            level.GetComponent<Level>().bWhatLevlAmI = currentHighestLevel;
            level.SetActive(true);

            levelQueue.Enqueue(level.GetComponent<Level>());
            despawnLevelCheck();
        }

        public void spawnLevelCheck()
        {
            if (currentLevel + 3 >= currentHighestLevel) {
                spawnNewLevel();
            }
        }

        protected void despawnLevelCheck()
        {
            if (levelQueue.Count > 7) {
                Level level = levelQueue.Dequeue();
                level.Despawn();
            }
        }

        protected void UpdateSnowPosition()
        {
            Snow.transform.position = new Vector3(Snow.transform.position.x, highestLevelPosition.y + snowOffset, Snow.transform.position.z);
        }
    }

}