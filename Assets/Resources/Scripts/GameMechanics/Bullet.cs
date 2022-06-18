using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Physics;

namespace GameEntities
{
    public class Bullet : Entity
    {
        static float bulletSpeed = 10;
        float lifetime = 2;

        override public int objectType()
        {
            return (int)objectTypes.bullet;
        }

        public Bullet(Vector2 coordinates, Vector2 dir, Vector2 initialSpeed, float size = 0.1f, float damping = 0) : base(coordinates, size, damping)
        {
            rigidbody.Accelerate(dir * bulletSpeed + initialSpeed);
        }

        public void Update(float deltaTime)
        {
            lifetime -= deltaTime;
            if (lifetime < 0)
            {
                Die();
                return;
            }
        }
    }
}