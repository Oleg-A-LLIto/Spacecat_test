using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Physics;

namespace GameEntities
{
    public class Asteroid : Entity
    {
        static float asteroidSpeed = 1;
        public int grade { get; private set; }
        public List<Asteroid> segments = null;
        GameManager gm;

        override public int objectType()
        {
            return (int)objectTypes.asteroid;
        }

        public Asteroid(Vector2 coordinates, Vector2 dir, Vector2 initialSpeed, float size, GameManager gameManager, int g = 3, float damping = 0) : base(coordinates, size, damping)
        {
            rigidbody.Accelerate(dir * asteroidSpeed + initialSpeed);
            segments = null;
            grade = g;
            gm = gameManager;
        }

        public void Update(float deltaTime)
        {
            foreach (Entity other in collisionModel.CheckForCollisions())
            {
                if (other.objectType() == (int)objectTypes.bullet)
                {
                    Die();
                    other.Die();
                    break;
                }
                if (other.objectType() == (int)objectTypes.player)
                {
                    other.Die();
                    break;
                }
            }
        }

        override public void Die()
        {
            destroy = true;
            if (grade > 1)
            {
                System.Random rng = new System.Random();
                segments = new List<Asteroid>();
                for (int i = 0; i < 4; i++)
                {
                    int randomAngle = rng.Next(360);
                    Vector2 randomDir = new Vector2((float)System.Math.Cos(randomAngle), (float)System.Math.Sin(randomAngle));
                    segments.Add(new Asteroid(rigidbody.position, randomDir, rigidbody.velocity, collisionModel.radius / 2, gm, grade - 1));
                }
            }
            gm.entityDied(this);
            Delete();
        }
    }
}