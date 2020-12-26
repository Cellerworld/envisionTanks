using Envision.Tanks.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace Envision.Tanks
{
    public class Barrel : GameObject
    {
        private const int rotationSpeed = 3;

        private Action triggerNextGameState;

        private bool changingWeapon = false;
        private bool isLoading = false;
        private float power;
        private float powerIncrease = 0.05f;

        private List<Weapon> weapons;
        private int selectedWeaponIndex;
        private Color selectedWeaponColor;
        private Color unselectedWeaponColor;
        private List<UIFramedText> weaponSelection;
        private UIText weaponAngle;

        public Barrel(string visualFile, Vector2 pos, int rot, Action onShot, string tag) : base(pos, rot)
        {
            size = new Vector2(32, 4);
            this.tag = tag;
            this.visual = Image.FromFile(visualFile);
            this.triggerNextGameState = onShot;
            power = 1;

            weaponSelection = new List<UIFramedText>();
            weapons = XMLHelper.GetWeapons();
            selectedWeaponIndex = 0;
            selectedWeaponColor = Color.Gold;
            unselectedWeaponColor = Color.Gray;

            int uiHeight = 0;
            for (int i = 0; i < weapons.Count; i++)
            {
                if (tag == "p2")
                {
                    weapons[i].stats.startAngle = (180 - weapons[i].stats.startAngle);
                    weaponSelection.Add(new UIFramedText(weapons[i].name + ": " + weapons[i].stats.ammo, new Vector2(UI.GameWidth / 6 * 5, uiHeight), UI.GameWidth / 8, 14, Color.Red, unselectedWeaponColor));
                }
                else
                {
                    weaponSelection.Add(new UIFramedText(weapons[i].name + ": " + weapons[i].stats.ammo, new Vector2(UI.GameWidth / 6 * 2, uiHeight), UI.GameWidth / 8, 14, Color.Green, unselectedWeaponColor));
                }
                weaponSelection[i].SetFont(new Font("Aerial", 10));
                uiHeight += 15;
            }
            weaponSelection[selectedWeaponIndex].ChangeFrameColor(selectedWeaponColor);
            this.rotation = weapons[selectedWeaponIndex].stats.startAngle;
            weaponAngle = new UIText("Angle: " + rotation, new Vector2(weaponSelection[0].position.X - weaponSelection[0].width, 15), 0, 0, Color.Blue);
        }

        public override void FixedUpdate()
        {

        }

        public void ChangeWeapon(bool nextWeapon)
        {
            weaponSelection[selectedWeaponIndex].ChangeFrameColor(unselectedWeaponColor);
            if (nextWeapon)
                selectedWeaponIndex++;
            else
                selectedWeaponIndex--;

            selectedWeaponIndex %= weapons.Count;
            weaponSelection[selectedWeaponIndex].ChangeFrameColor(selectedWeaponColor);
            this.rotation = weapons[selectedWeaponIndex].stats.startAngle;
        }

        public override void GraphicsUpdate(object sender, PaintEventArgs e)
        {
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(position.X, position.Y);
            e.Graphics.RotateTransform(rotation);
            e.Graphics.DrawImage(visual, pivot.X, pivot.Y, size.X, size.Y);
            if (isLoading)
            {
                e.Graphics.ResetTransform();
                e.Graphics.FillRectangle(new SolidBrush(Color.White), position.X - size.X / 2, position.Y - 40, size.X * (power - 1), 6);
            }
        }

        //make my own input system that already has the 3 key states?
        public void Aim()
        {
            if (Keyboard.IsKeyDown(Key.Down))
            {
                if (rotation < weapons[selectedWeaponIndex].stats.startAngle + weapons[selectedWeaponIndex].stats.maxAngle)
                    rotation += rotationSpeed;
            }
            else if (Keyboard.IsKeyDown(Key.Up))
            {
                if (rotation > weapons[selectedWeaponIndex].stats.startAngle + weapons[selectedWeaponIndex].stats.minAngle)
                    rotation -= rotationSpeed;
            }
            else if (!isLoading && Keyboard.IsKeyDown(Key.Right))
            {
                changingWeapon = true;
            }
            else if (changingWeapon && Keyboard.IsKeyUp(Key.Right))
            {
                ChangeWeapon(true);
                changingWeapon = false;
            }
            if (Keyboard.IsKeyDown(Key.Space))
            {
                if (weapons[selectedWeaponIndex].stats.ammo != 0)
                    Load();
            }
            else if (isLoading && Keyboard.IsKeyUp(Key.Space))
            {
                Shoot();
            }
            weaponAngle.text = "Angle: " + rotation;
        }

        private void Load()
        {
            isLoading = true;
            power += powerIncrease;
            if (power > 2 || power < 1)
            {
                powerIncrease *= -1;
            }
        }

        public void Shoot()
        {
            triggerNextGameState();
            Vector2 projectileSpawnPos = this.position + Vector2.RotationToVectorD(this.rotation) * this.size.X;
            Projectile p;
            weapons[selectedWeaponIndex].Shoot(power, projectileSpawnPos, rotation, triggerNextGameState, this.tag);
            weaponSelection[selectedWeaponIndex].text = weapons[selectedWeaponIndex].name + ": " + weapons[selectedWeaponIndex].stats.ammo;
            isLoading = false;
        }
    }
}