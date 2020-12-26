using Envision.Tanks.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Envision.Tanks
{
    public partial class frmGame : Form
    {
        private Timer m_Timer;
        private DateTime m_LastUpdateTime;

        private Font m_Font;
        private SolidBrush m_FontBrush;

        public static frmGame gameInstance;
        private GameLogic gml;
        private List<GameObject> gameObjects;

        private UI gameUI;

        private CollisionSystem collisionSystem;

        public frmGame()
        {
            if (gameInstance != null)
            {
                //replace with specialized Excpetion "MultipleGameInstancesException"
                throw new Exception("There can not be more than one instance of the game.");
            }
            gameInstance = this;
            InitializeComponent();
            InitGameLogic();

            m_Font = new Font("Arial", 16);
            m_FontBrush = new SolidBrush(Color.White);

            m_Timer = new Timer();
            m_Timer.Interval = (int)(1f / 30f * 1000f);
            m_Timer.Enabled = true;
            m_Timer.Tick += OnNextFrame;

            this.DoubleBuffered = true;
        }

        private void InitGameLogic()
        {
            gameUI = new UI(Width, Height);
            collisionSystem = new CollisionSystem();
            gameObjects = new List<GameObject>();
            gml = new GameLogic(Width, Height);
        }

        public void StartGame()
        {
            m_Timer.Start();
        }

        public void AddGameObject(GameObject gObj)
        {
            //throw exception if object can´t be added
            if (!gameObjects.Contains(gObj))
                gameObjects.Add(gObj);
        }

        public void DeleteGameObejct(GameObject gObj)
        {
            //throw exception if object can´t be deleted
            if (gameObjects.Contains(gObj))
                gameObjects.Remove(gObj);
        }

        private void OnNextFrame(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            TimeSpan delta = now - m_LastUpdateTime;
            m_LastUpdateTime = now;

            // update logic objects here
            gml.FixedUpate(delta);
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].isActive && gameObjects[i].isEnabled)
                {
                    gameObjects[i].PhysicsUpdate();
                    gameObjects[i].FixedUpdate();
                }
            }
            collisionSystem.CheckCollisions();

            this.Invalidate(); // invalidate form to trigger repaint
        }

        private void frmGame_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            // render here

            // draw text
            UI.Getinstance()?.Update(sender, e);

            // draw filled rectangle
            e.Graphics.FillRectangle(new SolidBrush(Color.ForestGreen), 0, Height - 10, Width, 10);

            // draw image
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].isActive)
                    gameObjects[i].GraphicsUpdate(sender, e);
            }

            //collider debug
            //collisionSystem.DrawAllColider(e);

            // draw game area
            e.Graphics.ResetTransform();
            e.Graphics.DrawRectangle(new Pen(Color.Blue), 0, 50, Width - 1, Height - 50);
        }

    }
}
