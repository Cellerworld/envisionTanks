using Envision.Tanks.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Envision.Tanks
{
    public class CollisionSystem
    {
        private List<Collider> colliderList;
        private List<Collider> nonStaticColliderList;
        private List<Collider> removedCollider;

        private bool areColliderRemoved;

        public static CollisionSystem instance;

        public CollisionSystem()
        {
            if (instance != null)
                throw new Exception("There can not be more than one instance of the Collision System.");
            instance = this;
            colliderList = new List<Collider>();
            nonStaticColliderList = new List<Collider>();
            removedCollider = new List<Collider>();
        }

        public void AddCollider(Collider collider)
        {
            if (!colliderList.Contains(collider))
            {
                colliderList.Add(collider);
                if (!collider.attachedObject.isStatic)
                    nonStaticColliderList.Add(collider);
            }
        }

        public void RemoveCollider(Collider collider)
        {
            //if (colliderList.Contains(collider))
            //    colliderList.Remove(collider);
            //if (nonStaticColliderList.Contains(collider))
            //    nonStaticColliderList.Remove(collider);
            removedCollider.Add(collider);
            areColliderRemoved = true;
        }

        private Point RemoveCollider(int i, int k)
        {
            foreach (Collider collider in removedCollider)
            {
                if (colliderList.Contains(collider))
                {
                    colliderList.Remove(collider);
                    k--;
                }
                if (nonStaticColliderList.Contains(collider))
                {
                    nonStaticColliderList.Remove(collider);
                    i--;
                }
            }
            removedCollider.Clear();
            areColliderRemoved = false;
            return new Point(i, k);
        }

        //very simple Collision check, not good but works for this scenario
        public void CheckCollisions()
        {

            for (int i = 0; i < nonStaticColliderList.Count; i++)
            {
                for (int k = 0; k < colliderList.Count; k++)
                {
                    if (i < 0)
                        break;
                    if (colliderList[i] != colliderList[k])
                        if (IsColliding(nonStaticColliderList[i], colliderList[k]))
                        {
                            nonStaticColliderList[i].OnCollision(colliderList[k]);
                            colliderList[k].OnCollision(nonStaticColliderList[i]);
                            if (areColliderRemoved)
                            {
                                var modifiedIndexes = RemoveCollider(i, k);
                                i = modifiedIndexes.X;
                                k = modifiedIndexes.Y;
                            }
                        }
                }
            }
        }

        public void DrawAllColider(PaintEventArgs e)
        {
            for (int i = 0; i < colliderList.Count; i++)
            {
                e.Graphics.ResetTransform();
                float debugPenXPos = colliderList[i].position.X - colliderList[i].attachedObject.pivot.X;
                float debugPenYPos = colliderList[i].position.Y - colliderList[i].attachedObject.pivot.Y;
                e.Graphics.DrawRectangle(new Pen(Color.Yellow), debugPenXPos, debugPenYPos, colliderList[i].size.X, colliderList[i].size.Y);
            }
        }

        //actually this does not account for rotation. Change to sphere Collision instead?
        //->Collision is not too precise because I´m only using the image sizes for collider anyway. 
        private bool IsColliding(Collider a, Collider b)
        {
            bool isInXRange = a.position.X < b.position.X + b.size.X && a.position.X + a.size.X > b.position.X;
            bool isInYRange = a.position.Y < b.position.Y + b.size.Y && a.position.Y + a.size.Y > b.position.Y;

            return isInXRange && isInYRange;
        }
    }
}