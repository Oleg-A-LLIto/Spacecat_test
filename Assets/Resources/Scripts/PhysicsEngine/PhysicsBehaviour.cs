using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Physics;

public class PhysicsBehaviour : MonoBehaviour
{

    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        PhysicsEngine.Update(Time.deltaTime);
    }
}
