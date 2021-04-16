using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts.FTUE
{
    public class Earth : MonoBehaviour
    {
        public Material EarthMaterial;
        protected Color initialEarthColor = new Color(0.02f, 0.54f, 0f);

        public Material AtmosphereMaterial;
        protected Color initialAtmosphereColor = new Color(0.09f, 0.0f, 1f, 0.15f);

        public void Start()
        {
            EarthMaterial.SetColor("_BaseColor", initialEarthColor);
            AtmosphereMaterial.SetColor("_BaseColor", initialAtmosphereColor);
        }
    }
}