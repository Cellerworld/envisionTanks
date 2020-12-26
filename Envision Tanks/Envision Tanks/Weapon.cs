using Envision.Tanks.Math;
using System;

namespace Envision.Tanks
{
    public class Weapon
    {
        public struct WeaponStats
        {
            public int startAngle;
            public int maxAngle;
            public int minAngle;
            public int ammo;
            public Force force;
            public int dmg;
            public float mass;
        }
        public string name { get; private set; }
        public WeaponStats stats = new WeaponStats();
        private string visualFile;
        private Vector2 size;

        ImpactEffect effect;

        public Weapon(string name, string projectileVisualFile, Vector2 size, WeaponStats wStats, ImpactEffect effect = null)
        {
            this.name = name;
            this.visualFile = projectileVisualFile;
            this.size = size;
            this.stats = wStats;
            this.effect = effect;
        }

        public void Shoot(float power, Vector2 pos, int rotation, Action triggerNextGameState, string tag)
        {
            Vector2 projectileSpawnPos = pos + Vector2.RotationToVectorD(rotation) * this.size.X;
            Projectile p;
            stats.force.dir = Vector2.RotationToVectorD(rotation);
            Force pForce = new Force(stats.force.dir, stats.force.magnitude * power, stats.force.decline);
            p = new Projectile(projectileSpawnPos, rotation, visualFile, size, pForce, triggerNextGameState, stats.dmg, stats.mass, effect);
            p.tag = tag;
            stats.ammo--;
            //to keep the infinite ammo at -1 and avoid the unlikly case of someone shooting so often that the int starts to become positive again
            stats.ammo = System.Math.Max(stats.ammo, -1);
        }

    }
}