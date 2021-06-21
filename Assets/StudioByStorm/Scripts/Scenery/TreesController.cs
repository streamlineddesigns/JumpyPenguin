using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{

    public class TreesController : MonoBehaviour
    {
        protected int indexToUse = 0;
        public List<Material> treeMaterials = new List<Material>();

        public void SetTreeColor(Renderer treeTop)
        {
            if (indexToUse > treeMaterials.Count - 1) {
                indexToUse = 0;
            }

            treeTop.material = treeMaterials[indexToUse];

            indexToUse++;
        }
    }
}