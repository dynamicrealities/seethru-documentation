using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RTSAgent : MonoBehaviour
{
    public float WaitTime = 2f;
    public Transform DropoffPoint;
    public Transform ResourcePoint;

    private NavMeshAgent _agent;
    private NavMeshPath _path;
    private Transform _target;
    private float _elapsed = 0f;
    private bool _isWaiting = false;

    [SerializeField]
    [Tooltip("Whether the agent successfully generated a path or not.")]
    private bool success = false;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _path = new NavMeshPath();
        _target = ResourcePoint;
        _agent.destination = _target.position;
    }

    // Update is called once per frame
    void Update()
    {
        success = _agent.CalculatePath(_target.position, _path);
        if (_isWaiting == false)
        {
            if (Vector3.Distance(_agent.transform.position, _agent.destination) < 1f)
            {
                if (_target == ResourcePoint)
                {
                    _isWaiting = true;
                    _agent.isStopped = true;
                }
                else if (_target == ResourcePoint && _isWaiting == false)
                {
                    _target = DropoffPoint;
                    _isWaiting = false;
                    _agent.SetDestination(_target.position);
                }
                else if (_target == DropoffPoint)
                {
                    _target = ResourcePoint;
                    _agent.SetDestination(_target.position);
                }
            }
        }
        else
        {
            if (_elapsed < WaitTime)
            {
                _elapsed += Time.deltaTime;
            }
            else
            {
                _target = DropoffPoint;
                _elapsed = 0f;
                _isWaiting = false;
                _agent.isStopped = false;
                _agent.SetDestination(_target.position);
            }
        }
    }
}
