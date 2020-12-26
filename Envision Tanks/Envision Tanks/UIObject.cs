using Envision.Tanks.Math;
using System.Windows.Forms;

namespace Envision.Tanks
{
    //in theory I could also make this with parenting in mind, but it´s not necessary.
    public abstract class UIObject
    {
        public string name { get; protected set; }

        public Vector2 position { get; protected set; }
        public float width { get; protected set; }
        public float height { get; protected set; }



        public UIObject(Vector2 pos, float width, float height)
        {
            this.position = pos;
            this.width = width;
            this.height = height;

            UI.Getinstance()?.AddUIObject(this);
        }

        public abstract void Draw(PaintEventArgs e);

        public void Destroy()
        {
            UI.Getinstance()?.DeleteUIObejct(this);
        }
    }
}