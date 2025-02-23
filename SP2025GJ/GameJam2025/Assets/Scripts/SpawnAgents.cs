using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAgents : MonoBehaviour
{
    [SerializeField] List<GameObject> chairs = new List<GameObject>();
    private List<Vector3> spawnPositions = new List<Vector3>();

    [SerializeField] GameObject agentPrefab;
    List<GameObject> agents = new List<GameObject>();
    [SerializeField] float distanceFromSpawn = 0.2f;

    float timeStarted;

    // Start is called before the first frame update
    void Start()
    {
        if (chairs == null || chairs.Count == 0)
        {
            Debug.LogError("Chairs list is not assigned or empty in the Inspector!", this);
            return; // Prevent further execution
        }

        //populate spawn positions
        foreach (GameObject obj in chairs)
        {
            spawnPositions.Add(obj.transform.position);
        }

        foreach (Vector3 pos in spawnPositions)
        {
            SpawnAgent(pos);
        }

        timeStarted = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            if (Vector3.Distance(agents[i].transform.position, spawnPositions[i]) < distanceFromSpawn && Time.time - timeStarted > 3.0f)
            {
                agents[i].GetComponent<AgentBehavior>().SetState(AgentBehavior.AgentStates.Idle);
            }
        }
    }

    void SpawnAgent(Vector3 position)
    {
        GameObject newAgent = Instantiate(agentPrefab, position, Quaternion.identity);
        agents.Add(newAgent);
    }

    public List<GameObject> GetAgents() { return agents; }
}
