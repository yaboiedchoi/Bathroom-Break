using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringTest : MonoBehaviour
{
    [SerializeField] SpringJoint joint;
    [SerializeField] Rigidbody rb;
    [SerializeField] Rigidbody rb2;

    // Start is called before the first frame update
    void Start()
    {
        joint.anchor = rb.position;
        joint.connectedBody = rb2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
