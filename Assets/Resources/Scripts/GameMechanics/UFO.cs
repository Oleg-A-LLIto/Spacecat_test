using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Physics;

namespace GameEntities
{
    public class UFO : Entity
    {
        Player player;
        float maxSpeed;
        float enginePower;
        GameManager gm;

        override public int objectType()
        {
            return (int)objectTypes.enemy;
        }

        public UFO(Vector2 coordinates, Player p, float size, GameManager gameManager, float power = 3, float damping = 0.5f, float speedLimit = 6) : base(coordinates, size, damping)
        {
            enginePower = power;
            maxSpeed = speedLimit;
            player = p;
            gm = gameManager;
        }

        public void Update(float deltaTime)
        {
            rigidbody.Accelerate(PhysicsEngine.Normalize(player.getPosition() - rigidbody.position));
            float speed = rigidbody.calculateAbsoluteSpeed();
            if (speed > maxSpeed)
            {
                rigidbody.Accelerate(-rigidbody.velocity * (speed - maxSpeed));
            }
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
            Delete();
            gm.entityDied(this);
        }
    }
}