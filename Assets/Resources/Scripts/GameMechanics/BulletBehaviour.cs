using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEntities;

public class BulletBehaviour : MonoBehaviour
{
    public Bullet bullet;

    private void Start()
    {
        
    }

    void Update()
    {
        transform.position = UnityToNumerics.NtoU(bullet.getPosition());
        bullet.Update(Time.deltaTime);
        if (bullet.destroy)
        {
            Destroy(gameObject);
        }
    }

}
