using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUpdater : MonoBehaviour
{
    [SerializeField] private NavMeshSurface mesh;

    private void Start()
    {
        StartCoroutine(NavMeshHeartbeat());
    }

    private IEnumerator NavMeshHeartbeat()
    {
        while (true)
        {
            mesh.BuildNavMesh();

            yield return new WaitForSeconds(5);
        }
    }
}
