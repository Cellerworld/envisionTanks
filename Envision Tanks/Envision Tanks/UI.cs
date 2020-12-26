using Envision.Tanks.Math;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Envision.Tanks
{
    //UI as Gameobject is weird, isolate it and have it always be rendered last?
    public class UI
    {
        public static float GameWidth { get; private set; }
        public static float GameHeight { get; private set; }

        private List<UIObject> uiObjects;
        private static UI gameUI;

        public static UI Getinstance()
        {
            return gameUI;
        }

        private static void Setinstance(UI value)
        {
            gameUI = value;
        }

        public UI(int gameWidth, int gameHeight)
        {
            if (Getinstance() != null)
            {
                return;
            }
            Setinstance(this);
            GameWidth = gameWidth;
            GameHeight = gameHeight;
            uiObjects = new List<UIObject>();
        }

        public void AddUIObject(UIObject uiObj)
        {
            //throw exception if object can´t be added
            uiObjects.Add(uiObj);
        }

        public void DeleteUIObejct(UIObject uiObj)
        {
            //throw exception if object can´t be deleted
            uiObjects.Remove(uiObj);
        }

        public void Update(object sender, PaintEventArgs e)
        {
            foreach (UIObject uiObj in uiObjects)
            {
                uiObj.Draw(e);
            }
        }

        ~UI()
        {
            Setinstance(null);
        }
    }
}