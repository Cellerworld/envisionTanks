using Envision.Tanks.Math;
using System;
using System.Collections.Generic;

namespace Envision.Tanks
{
    public class PhysicsComponent : GameComponent
    {
        public Vector2 currentTrajectory { get; private set; }

        private const float gravity = 10f;
        private static float wind = 0.7f;
        private static Vector2 windDirection = Vector2.UnitX;

        GameObject assignedObject;
        //increasing mass leads to more influence through gravity and less influence through wind and custom forces
        public float mass { get; private set; }
        private List<Force> customForces;

        public PhysicsComponent(GameObject gObj, float mass = 1)
        {
            assignedObject = gObj;
            customForces = new List<Force>();
            this.mass = mass;
        }

        //for simplicity this only simple forces with a multiplicative decline
        public void AddForce(Force force)
        {
            customForces.Add(force);
        }

        public void Update()
        {
            Vector2 gravityForce = (Vector2.UnitY * gravity * mass);
            Vector2 windForce = Vector2.Zero; //(windDirection * wind / mass); -> out of time
            Vector2 customForce = Vector2.Zero;
            for (int i = 0; i < customForces.Count; i++)
            {
                customForce += customForces[i].UseForce();
                if (customForces[i].magnitude < 1)
                {
                    customForces.RemoveAt(i);
                }
            }
            customForce /= mass;
            currentTrajectory = gravityForce + windForce + customForce;
            assignedObject.Translate(currentTrajectory);
        }

        public void SetMass(float mass)
        {
            this.mass = mass;
        }
    }
}