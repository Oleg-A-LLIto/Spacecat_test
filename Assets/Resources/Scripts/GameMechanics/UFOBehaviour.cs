using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEntities;

public class UFOBehaviour : MonoBehaviour
{
    public UFO ufo;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ufo.Update(Time.deltaTime);
        transform.position = UnityToNumerics.NtoU(ufo.getPosition());
        if (ufo.destroy)
        {
            Destroy(gameObject);
        }
    }
}
