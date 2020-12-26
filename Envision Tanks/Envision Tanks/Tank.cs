using Envision.Tanks.Math;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace Envision.Tanks
{
    public class Tank : GameObject
    {
        private Barrel barrel;
        private UIText hpText;
        private int hp = 100;
        public delegate void GameOverEvent(string winner);
        private GameOverEvent OnGameOver;

        public Tank(string name, string visualFile, Vector2 pos, Vector2 size, Barrel barrel, GameOverEvent onGameOver) : base(pos)
        {
            this.size = size;
            this.name = name;
            this.barrel = barrel;
            this.barrel.SetParent(this);
            this.visual = Image.FromFile(visualFile);
            this.hpText = new UIText("hp: " + hp, pos - Vector2.UnitY * 30 + size / 2, 5, 5, Color.Green);
            this.hpText.SetFont(new Font("Arial", 8));
            this.isStatic = true;
            this.AddComponent<Collider>();
            this.OnGameOver = onGameOver;
            XMLHelper.GetWeapons();
        }

        public override void FixedUpdate()
        {
            //make a class that recognizes key input?
            Keyboard.IsKeyDown(Key.Down);
            Keyboard.IsKeyDown(Key.Up);
            Keyboard.IsKeyDown(Key.Right);
            Keyboard.IsKeyDown(Key.Left);
            barrel.Aim();
            hpText.text = "hp: " + hp;
        }
        public override void GraphicsUpdate(object sender, PaintEventArgs e)
        {
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(position.X, position.Y);
            e.Graphics.DrawImage(visual, pivot.X, pivot.Y, size.X, size.Y);
        }

        public override void OnCollision(Collider sender)
        {
            if (sender.attachedObject is Projectile && sender.attachedObject.tag != barrel.tag)
            {
                TakeDamage((Projectile)sender.attachedObject);
            }
        }

        private void TakeDamage(Projectile p)
        {
            hp -= p.dmg;
            if (hp <= 0)
            {
                OnGameOver(p.tag);
                Die();
            }
        }

        private void Die()
        {
            barrel.Destroy();
            hpText.Destroy();
            this.Destroy();
        }
    }
}