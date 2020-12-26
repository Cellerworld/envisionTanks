using Envision.Tanks.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Envision.Tanks
{
    public abstract class GameObject
    {
        public string name { get; protected set; }
        public string tag = "";
        public bool isActive { get; protected set; }

        public bool isEnabled { get; set; }

        //in general I could just make a List of components, but I will address the two components I actually have directly in this case.
        //this also means that I limit my collider per object to one.
        public PhysicsComponent physicsCompenent { get; private set; }
        public List<Collider> collider { get; private set; }
        public bool isStatic { get; protected set; }
        public virtual Vector2 position
        {
            get
            {
                if (parent == null)
                    return localPosition;
                else
                    return localPosition + parent.position;
            }
            protected set
            {
                if (parent == null)
                    localPosition = value;
                else
                    localPosition = value - parent.position;
            }
        }
        public virtual Vector2 localPosition { get; protected set; } = Vector2.Zero;
        public Vector2 pivot { get; protected set; } = Vector2.Zero;
        public virtual int rotation { get; protected set; }
        public Vector2 size { get; protected set; }
        public virtual GameObject parent { get; protected set; }

        public virtual Image visual { get; protected set; }

        public GameObject(Vector2 pos, int rot = 0, GameObject parent = null)
        {
            isStatic = false;
            frmGame.gameInstance.AddGameObject(this);
            localPosition = pos;
            rotation = rot;
            this.parent = parent;
            isEnabled = true;
            isActive = true;
            collider = new List<Collider>();
        }

        public abstract void FixedUpdate();

        public void AddComponent<T>() where T : GameComponent
        {
            Type component = typeof(T);
            if (component.Name == "Collider")
            {
                AddCollider();
            }
            else if (component.Name == "PhysicsComponent")
            {
                AddPhysicsComponent();
            }
        }

        private void AddCollider()
        {
            this.collider.Add(new Collider(size, this, Vector2.Zero));
        }

        private void AddPhysicsComponent()
        {
            if (physicsCompenent == null)
                physicsCompenent = new PhysicsComponent(this);
        }

        public void PhysicsUpdate()
        {
            if (physicsCompenent != null)
            {
                physicsCompenent?.Update();
            }
        }

        public void Translate(Vector2 addPos)
        {
            position += addPos;
        }

        public abstract void GraphicsUpdate(object sender, PaintEventArgs e);

        public virtual void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }

        public virtual void SetParent(GameObject parent)
        {
            this.parent = parent;
        }

        public void Destroy()
        {
            if (collider.Count > 0)
            {
                for (int i = 0; i < collider.Count; i++)
                    CollisionSystem.instance.RemoveCollider(collider[i]);
                collider.Clear();
                collider = null;
            }
            frmGame.gameInstance.DeleteGameObejct(this);
        }

        public virtual void OnCollision(Collider sender) { }
    }
}