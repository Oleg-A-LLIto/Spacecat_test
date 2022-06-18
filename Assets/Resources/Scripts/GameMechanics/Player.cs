using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Physics;

namespace GameEntities
{
    enum objectTypes
    {
        player,
        bullet,
        asteroid,
        enemy
    }

    public class Player : Entity
    {
        float enginePower;
        int maxAmmo;
        float cooldown;
        public Vector2 direction;
        float maxSpeed;
        public int ammo { get; private set; }
        public float cooldownProgress { get; private set; }
        float bulletRadius;
        public GameManager gm;

        override public int objectType()
        {
            return (int)objectTypes.player;
        }

        public Player(Vector2 coordinates, float power, int ammoCapacity, float cd, float bulletRadius, float size, float damping = 0.5f, float speedLimit = 50) : base(coordinates, size, damping)
        {
            enginePower = power;
            maxAmmo = ammoCapacity;
            ammo = maxAmmo;
            maxSpeed = speedLimit;
            cooldown = cd;
            cooldownProgress = cd;
        }

        public void Thrust(float deltaTime)
        {
            rigidbody.Accelerate(direction * deltaTime * enginePower);
            float speed = rigidbody.calculateAbsoluteSpeed();
            if (speed > maxSpeed)
            {
                rigidbody.Accelerate(-rigidbody.velocity * (speed - maxSpeed));
            }
        }

        public void Reload(float deltaTime)
        {
            if (ammo == maxAmmo)
                return;
            cooldownProgress -= deltaTime;
            if (cooldownProgress <= 0)
            {
                ammo++;
                cooldownProgress = cooldown;
            }
        }

        public Bullet Bullet()
        {
            return new Bullet(rigidbody.position, direction, rigidbody.velocity);
        }

        public bool Laser()
        {
            if (ammo > 0)
            {
                ammo--;
                List<Collider> toKill = PhysicsEngine.RayCast(rigidbody.position, direction);
                foreach(Collider c in toKill)
                {
                    if(c.attachedEntity.objectType() != (int)objectTypes.player)
                    {
                        c.attachedEntity.Die();
                    }
                }

                return true;
            }
            return false;
        }

        public float Speed()
        {
            return rigidbody.calculateAbsoluteSpeed();
        }

        override public void Die()
        {
            destroy = true;
            Delete();
            gm.entityDied(this);
        }
    }
}
