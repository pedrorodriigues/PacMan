using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class enemy : MonoBehaviour
{
    public Transform Player;
    public NavMeshAgent naveMesh;
    void Start()
    {
        naveMesh = transform.GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        naveMesh.destination = Player.position;
    }
}