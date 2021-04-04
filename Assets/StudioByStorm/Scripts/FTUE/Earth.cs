using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts.FTUE
{
    public class Earth : MonoBehaviour
    {
        public Material EarthMaterial;
        protected Color initialEarthColor = new Color(0.02f, 0.54f, 0f);

        public void Start()
        {
            EarthMaterial.SetColor("_BaseColor", initialEarthColor);
        }
    }
}