using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring3D : MonoBehaviour
{
    [SerializeField] private float angularFrequency;
    [SerializeField] private float dampingRatio;

    public Vector3 Position { get { return new Vector3(x.Position, y.Position, z.Position); } }

    private Spring x;
    private Spring y;
    private Spring z;

    // Start is called before the first frame update
    void Start()
    {
        x = new Spring(angularFrequency, dampingRatio, 0, true);
        y = new Spring(angularFrequency, dampingRatio, 0, true);
        z = new Spring(angularFrequency, dampingRatio, 0, true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSprings();
    }

    void SetRestPosition(Vector3 resPos)
    {
        x.RestPosition = resPos.x;
        y.RestPosition = resPos.y;
        z.RestPosition = resPos.z;
    }

    void UpdateSprings()
    {
        x.Update();
        y.Update();
        z.Update();
    }
}
