using SeeThru.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SeeThru.Samples
{
    public class TopdownManager : MonoBehaviour
    {
        public TargetFramingCamera TargetCamera;
        public float SpawnDelay = 5f;
        public List<GameObject> objectsToSpawn;

        private int visibleObjects = 0;
        private float elapasedTime = 0f;

        private void Start()
        {
            foreach (GameObject go in objectsToSpawn)
            {
                go.SetActive(false);
            }
        }

        private void Update()
        {
            if (elapasedTime < SpawnDelay)
            {
                elapasedTime += Time.deltaTime;
            }
            else
            {
                if (visibleObjects == objectsToSpawn.Count)
                {
                    visibleObjects = 0;
                    foreach (GameObject go in objectsToSpawn)
                    {
                        go.SetActive(false);
                    }
                    TargetCamera.Targets = new List<GameObject>() { FindObjectOfType<TopdownPlayerController>().gameObject };
                    elapasedTime = 0f;
                }
                else
                {
                    objectsToSpawn[visibleObjects].SetActive(true);
                    TargetCamera.Targets.Add(objectsToSpawn[visibleObjects]);
                    visibleObjects += 1;
                    elapasedTime = 0f;
                }
            }
        }
    }
}
