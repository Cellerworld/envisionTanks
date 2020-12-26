using System.Drawing;
using System.Windows.Forms;
using Envision.Tanks.Math;

namespace Envision.Tanks
{
    public class FakeTank : GameObject
    {
        //maybe implement hp for tree so taht it has more meaning
        int hp = 50;

        public FakeTank(Vector2 pos, string tag, int rot = 0, GameObject parent = null) : base(pos, rot, parent)
        {
            //this.isStatic = true;
            this.tag = tag;
            size = new Vector2(50, 50);
            if (tag == "p1")
            {
                visual = Image.FromFile("Resources\\green_tank.png");
            }
            else
            {
                visual = Image.FromFile("Resources\\red_tank.png");
            }

            AddComponent<Collider>();
        }

        public override void FixedUpdate()
        {
        }

        public override void GraphicsUpdate(object sender, PaintEventArgs e)
        {
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(position.X, position.Y);
            e.Graphics.DrawImage(visual, pivot.X, pivot.Y, size.X, size.Y);
        }

        public override void OnCollision(Collider sender)
        {
            if (sender.attachedObject is Projectile && sender.attachedObject.tag != this.tag)
            {
                this.Destroy();
            }
            if (sender.attachedObject.tag == "terrain")
            {
                while (this.position.Y + this.size.Y >= sender.position.Y)
                {
                    this.localPosition = new Vector2(localPosition.X, localPosition.Y - Vector2.UnitY.Y);
                    //this.localPosition = localPosition - Vector2.UnitY * 10;
                }

            }
        }
    }
}