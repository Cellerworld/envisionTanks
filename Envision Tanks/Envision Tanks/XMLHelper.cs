using Envision.Tanks.Math;
using System.Collections.Generic;
using System.Xml;

namespace Envision.Tanks
{
    public static class XMLHelper
    {
        public static List<Weapon> GetWeapons()
        {
            List<Weapon> weapons = new List<Weapon>();

            XmlDocument doc = new XmlDocument();
            doc.Load("Resources\\Weapons.xml");
            XmlNode root = doc.DocumentElement;
            XmlNodeList weaponNodes = root.ChildNodes;
            for (int i = 0; i < weaponNodes.Count; i++)
            {
                string name = weaponNodes[i].ChildNodes[0].InnerText;
                string visualFile = weaponNodes[i].ChildNodes[1].InnerText;
                int size;
                int.TryParse(weaponNodes[i].ChildNodes[2].InnerText, out size);

                XmlNodeList weaponStatslist = weaponNodes[i].ChildNodes[3].ChildNodes;
                Weapon.WeaponStats stats = new Weapon.WeaponStats();
                float magnitude;
                float decline;
                int.TryParse(weaponStatslist[0].InnerText, out stats.maxAngle);
                int.TryParse(weaponStatslist[1].InnerText, out stats.minAngle);
                int.TryParse(weaponStatslist[2].InnerText, out stats.ammo);

                float.TryParse(weaponStatslist[3].ChildNodes[1].InnerText, out magnitude);
                float.TryParse(weaponStatslist[3].ChildNodes[2].InnerText, out decline);
                stats.force = new Force(Vector2.UnitX, magnitude, decline);

                int.TryParse(weaponStatslist[4].InnerText, out stats.dmg);
                float.TryParse(weaponStatslist[5].InnerText, out stats.mass);
                int.TryParse(weaponStatslist[6].InnerText, out stats.startAngle);

                ImpactEffect effect = getEffect(weaponNodes[i].ChildNodes[4].InnerText);

                Weapon weapon = new Weapon(name, visualFile, new Vector2(size, size), stats, effect);
                weapons.Add(weapon);
            }
            return weapons;
        }

        private static ImpactEffect getEffect(string effect)
        {
            if (effect == "faketank")
                return new FakeTankEffect();
            else
                return null;
        }
    }
}