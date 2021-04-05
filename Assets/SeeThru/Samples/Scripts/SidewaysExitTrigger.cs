using SeeThru.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SeeThru.Source
{
    public class SidewaysExitTrigger : MonoBehaviour
    {
        public Vector3 RespawnPosition = new Vector3(5, 7, 5);
        private TargetFramingCamera _camera;

        private void Start()
        {
            _camera = FindObjectOfType<TargetFramingCamera>();
        }

        public void OnTriggerExit(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                _camera.Targets.Remove(other.gameObject);
                other.gameObject.transform.position = RespawnPosition;
                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}
