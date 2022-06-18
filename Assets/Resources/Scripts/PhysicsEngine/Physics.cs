using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace Physics
{
    public class PhysicsEngine{
        public static Vector2 WorldStart { get; private set; } = new Vector2(-9, -5);
        public static Vector2 WorldEnd { get; private set; } = new Vector2(9,5);
        public static HashSet<Collider> CollisionPool { get; private set; } = new HashSet<Collider>();
        public static HashSet<Body> BodyPool { get; private set; } = new HashSet<Body>();
        public static float Epsilon = 0.000005f;
        public static float tpOffset = 0.1f;

        public static void SetWorldSize (float left, float top, float right, float bottom)
        {
            WorldStart = new Vector2(left, top);
            WorldEnd = new Vector2(right, bottom);
        }

        public static void AddToCollisionPool(Collider c)
        {
            CollisionPool.Add(c);
        }

        public static void RemoveFromCollisionPool(Collider c)
        {
            CollisionPool.Remove(c);
        }

        public static void AddToBodyPool(Body b)
        {
            BodyPool.Add(b);
        }

        public static void RemoveFromBodyPool(Body b)
        {
            BodyPool.Remove(b);
        }

        public static void Update(float deltaTime)
        {
            foreach (Body b in BodyPool)
            {
                b.Step(deltaTime);
            }
        }

        public static float Vec2Magnitude(Vector2 v)
        {
            return (float)System.Math.Sqrt((v.X * v.X) + (v.Y * v.Y));
        }

        public static Vector2 Normalize(Vector2 v)
        {
            return (v / Vec2Magnitude(v));
        }

        public static Vector2 projectOnRay(Vector2 start, Vector2 dir, Vector2 point)
        {
            float t = dir.Y / dir.X;
            float t2 = t * t;
            float b = start.Y - (t * start.X);

            float X = (t * point.Y + point.X - t * b) / (t2 + 1);
            float Y = (t2 * point.Y + t * point.X + b) / (t2 + 1);

            if(System.Math.Sign(X - start.X) == System.Math.Sign(dir.X))
            {
                if (System.Math.Sign(Y - start.Y) == System.Math.Sign(dir.Y))
                {
                    return new Vector2(X, Y);
                }
            }
            return Vector2.Zero;
        }

        public static List<Collider> RayCast(Vector2 start, Vector2 dir)
        {
            List<Collider> touched = new List<Collider>();
            foreach (Collider c in CollisionPool)
            {
                Vector2 point = projectOnRay(start, dir, c.attachedEntity.getPosition());
                if (point == Vector2.Zero)
                    continue;
                if(Vec2Magnitude(c.attachedEntity.getPosition() - point) < c.radius)
                {
                    touched.Add(c);
                }
            }
            return touched;
        }
    }

    public class Collider
    {
        public float radius { get; private set; } //all colliders are approximated as circles
        public Entity attachedEntity { get; private set; }

        public Collider(float r, Entity e)
        {
            radius = r;
            attachedEntity = e;
            PhysicsEngine.AddToCollisionPool(this);
        }

        public List<Entity> CheckForCollisions()
        {
            List<Entity> e = new List<Entity>();
            foreach(Collider c in PhysicsEngine.CollisionPool)
            {
                float dist = PhysicsEngine.Vec2Magnitude(attachedEntity.getPosition() - c.attachedEntity.getPosition());
                if(dist <= (radius + c.radius))
                {
                    e.Add(c.attachedEntity);
                }
            }
            return e;
        }
    }

    public class Body
    {
        public Vector2 position { get; private set; }
        public Vector2 velocity { get; private set; }
        float damping;

        public Body(Vector2 coordinates, float damp = 0)
        {
            position = coordinates;
            damping = damp;
            PhysicsEngine.AddToBodyPool(this);
        }

        public void Step(float deltaTime)
        {
            position += velocity * deltaTime;
            velocity *= 1 - (damping * deltaTime);
            position = new Vector2(mirrorCoordinate(position.X, PhysicsEngine.WorldStart.X, PhysicsEngine.WorldEnd.X), 
                mirrorCoordinate(position.Y, PhysicsEngine.WorldStart.Y, PhysicsEngine.WorldEnd.Y));
        }

        public float calculateAbsoluteSpeed()
        {
            return PhysicsEngine.Vec2Magnitude(velocity);
        }

        private float mirrorCoordinate(float x, float min, float max)
        {
            if (x > max + PhysicsEngine.tpOffset)
            {
                return (max - x) + min;
            }
            else
            {
                if (x < min - PhysicsEngine.tpOffset)
                {
                    return (min - x) + max;
                }
            }
            return x;
        }

        public void Accelerate(Vector2 acceleration)
        {
            velocity += acceleration;
        }

    }

    public abstract class Entity
    {
        protected Body rigidbody;
        protected Collider collisionModel;
        public bool destroy { get; protected set; } = false;

        public Entity(Vector2 coordinates, float size, float damping = 0)
        {
            collisionModel = new Collider(size, this);
            rigidbody = new Body(coordinates, damping);
        }

        public List<Entity> CheckForCollisions()
        {
            return collisionModel.CheckForCollisions();
        }

        public abstract int objectType();

        public Vector2 getPosition()
        {
            return rigidbody.position;
        }

        virtual public void Die()
        {
            destroy = true;
            Delete();
        }

        protected void Delete()
        {
            PhysicsEngine.RemoveFromBodyPool(rigidbody);
            PhysicsEngine.RemoveFromCollisionPool(collisionModel);
        }
    }
}
