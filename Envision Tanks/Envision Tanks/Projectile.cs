using Envision.Tanks.Math;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace Envision.Tanks
{
    public class Projectile : GameObject
    {
        private Action triggerNextGameState;

        public int dmg { get; private set; }
        private ImpactEffect effect;

        public Projectile(Vector2 pos, int rotation, string visualFile, Vector2 size, Force force, Action triggerNextGameState, int dmg, float mass = 1, ImpactEffect effect = null) : base(pos)
        {
            pivot = new Vector2(size.X / 2, size.Y / 2);
            visual = Image.FromFile(visualFile);
            this.rotation = rotation;
            this.size = size;
            this.dmg = dmg;
            this.triggerNextGameState = triggerNextGameState;
            AddComponent<PhysicsComponent>();
            physicsCompenent.SetMass(mass);
            physicsCompenent.AddForce(force);
            this.AddComponent<Collider>();
            int a = collider.Count;
            this.effect = effect;
        }

        public override void FixedUpdate()
        {
            this.rotation = (int)physicsCompenent.currentTrajectory.GetRotation();
            if (IsOutOFBounds())
            {
                triggerNextGameState();
                this.Destroy();
            }
        }

        private bool IsOutOFBounds()
        {
            bool flewTooFar = this.position.X < 0 || this.position.X > frmGame.gameInstance.Width;
            bool isBelowGround = this.position.Y > frmGame.gameInstance.Height;
            return flewTooFar || isBelowGround;
        }

        public override void GraphicsUpdate(object sender, PaintEventArgs e)
        {
            e.Graphics.ResetTransform();
            if (Keyboard.IsKeyDown(Key.Left))
            {
                e.Graphics.TranslateTransform(500, 500);
                e.Graphics.DrawImage(Image.FromFile("Resources\\green_tank.png"), 0, 0, 50, 50);
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                e.Graphics.TranslateTransform(500, 500);
                e.Graphics.DrawImage(Image.FromFile("Resources\\red_tank.png"), 0, 0, 50, 50);
            }

            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(position.X, position.Y);
            e.Graphics.RotateTransform(rotation);
            e.Graphics.DrawImage(visual, -pivot.X, -pivot.Y, size.X, size.Y);
        }

        public override void OnCollision(Collider sender)
        {
            if (sender.attachedObject.tag != this.tag)
            {
                triggerNextGameState();
                if (effect != null)
                {
                    effect.TriggerEffect(this);
                }
                this.Destroy();
            }

        }
    }
}