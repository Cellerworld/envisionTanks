using Envision.Tanks.Math;

namespace Envision.Tanks
{
    //needs to rotate aswell
    public class Collider : GameComponent
    {
        //Typically this would be an abstract class an then I make box,circle, convey, concave collider
        //But for this task using only Boycollider is sufficient, so Collider will just be my Boxcollider in this case

        public Vector2 size;
        public GameObject attachedObject { get; private set; }


        public virtual Vector2 position
        {
            get
            {
                if (attachedObject == null)
                    return localPosition;
                else
                    return localPosition + attachedObject.position;
            }
            protected set
            {
                if (attachedObject == null)
                    localPosition = value;
                else
                    localPosition = value - attachedObject.position;
            }
        }
        public Vector2 localPosition;
        public Collider(Vector2 size, GameObject attachedGobj, Vector2 localPosition)
        {
            this.size = size;
            this.attachedObject = attachedGobj;
            this.localPosition = localPosition;
            CollisionSystem.instance.AddCollider(this);
        }

        //instead of the collider you could 
        public void OnCollision(Collider sender)
        {
            attachedObject?.OnCollision(sender);
        }

    }
}