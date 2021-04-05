using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace SeeThru.Samples
{
    public class OrbitingAgentManager : MonoBehaviour
    {
        [Header("General")]
        public NavMeshAgent agent;
        [Tooltip("A list of locations for where the Agent should go.")]
        public List<Transform> Locations = new List<Transform>();
        [Tooltip("How long the AI should wait, before it moves towards its end destination.")]
        public float WaitTime = 0.1f;
        [Header("Rotation")]
        [Tooltip("Whether the agent should rotate towards its steering target before it moves towards it or not.")]
        public bool RotateBeforeMove = false;
        [Tooltip("How many degrees of a difference between the agent and its Steering Target rotation before it can start moving. Only used if RotateBeforeMove is true.")]
        public float AngleThreshold = 1f;
        [Tooltip("How fast the agent spins when rotating. Only used if RotateBeforeMove is true.")]
        public float rotateSpeed = 300f;

        /// <summary>
        /// The path that the Agent generates.
        /// </summary>
        private NavMeshPath path;
       

        [Header("Debug - Readonly")]
        [SerializeField]
        [Tooltip("Whether the agent successfully generated a path or not.")]
        private bool success = false;
        [SerializeField]
        [Tooltip("The angle between the agent and where it needs to face before moving. Only used if RotateBeforeMove is true.")]
        private float turnAngle = 0f;
        [SerializeField]
        [Tooltip("Whether the agent is currently waiting or not.")]
        private bool isWaiting;
        [SerializeField]
        [Tooltip("How long the agent has waited.")]
        private float elapsedWaitingTime = 0f;
        [SerializeField]
        [Tooltip("Indicates at what index in the Locations array the bot is going for next.")]
        private int locationIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            path = new NavMeshPath();
            locationIndex = 0;
            agent.destination = Locations[locationIndex].transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            success = agent.CalculatePath(Locations[locationIndex].position, path);

            if (Vector3.Distance(agent.transform.position, agent.destination) < 0.25f)
            {
                if (locationIndex + 1 == Locations.Count)
                {
                    locationIndex = 0;
                }
                else
                {
                    locationIndex += 1;
                }
                agent.SetDestination(Locations[locationIndex].position);
                isWaiting = true;
                elapsedWaitingTime = 0f;
                agent.isStopped = true;
            }

            if (isWaiting == false)
            {
                EvaluateAgentRotation();
            }
            else
            {
                if (elapsedWaitingTime < WaitTime)
                {
                    elapsedWaitingTime += Time.deltaTime;
                }
                else
                {
                    isWaiting = false;
                    agent.isStopped = false;
                }

            }
        }

        private void EvaluateAgentRotation()
        {
            if (RotateBeforeMove == true)
            {
                turnAngle = Vector3.SignedAngle(agent.transform.forward, agent.steeringTarget - agent.transform.position, agent.transform.up);
                if (turnAngle < AngleThreshold && success == true)
                {
                    agent.isStopped = false;
                    agent.Move(Vector3.zero);
                }
                else
                {
                    agent.isStopped = true;
                    Quaternion difference = Quaternion.Euler(0, turnAngle, 0);
                    transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, difference * agent.transform.rotation, rotateSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (success == true)
                {
                    agent.Move(Vector3.zero);
                }
            }
        }
    }
}