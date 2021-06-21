using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class Tree : MonoBehaviour
    {
        public Renderer treeTop;
        protected bool bIsInit = false;

        public void OnEnable()
        {
            if (bIsInit) {
                GameController.Instance.LevelController.GroundObjectsController.TreesController.SetTreeColor(treeTop);
            }
        }
        
        public void OnDisable()
        {
            if (! bIsInit) {
                bIsInit = true;
            }
        }
    }
}