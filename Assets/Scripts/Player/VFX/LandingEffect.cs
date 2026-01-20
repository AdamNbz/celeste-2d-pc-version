using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Player.VFX
{
    public class LandingEffect : MonoBehaviour
    {
        [SerializeField] GameObject landingEffectPrefab;
        GameObject effectInstance;
        public void SpawnLandingEffect(Vector2 position)
        {
            if (landingEffectPrefab != null && position != null)
            {
                Vector2 spawnPos = position;
                effectInstance = Instantiate(landingEffectPrefab, spawnPos, Quaternion.identity, transform);
            }

            DestroyEffect();
           
        }

        public void DestroyEffect()
        {
            if (effectInstance != null)
            {
                Destroy(effectInstance, 3f);
            }
        }
    }
}
