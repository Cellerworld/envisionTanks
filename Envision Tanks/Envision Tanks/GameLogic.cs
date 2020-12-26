using Envision.Tanks.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Envision.Tanks
{
    public class GameLogic
    {
        private int width;
        private int height;
        private enum GameState
        {
            P1TURN,
            P2TURN,
            WAITIMPACT,
            GAMEOVER
        };
        private List<GameState> gameSequence;
        private int stateIndex;
        private GameState gameState;
        private Action nextState;

        private Tank tank1;
        private Tank tank2;
        private Vector2 tanksize = new Vector2(50, 50);

        private Vector2 UIItemSize = new Vector2(150, 25);

        private UIText activePlayer;

        private UIText roundTimer;
        private const double roundTime = 30;
        private double currentTime;

        private bool gameOver = false;

        public GameLogic(int width, int height)
        {
            nextState = NextState;
            this.width = width;
            this.height = height;
            Terrain terrain = new Terrain(width, height, tanksize);

            Barrel b1 = new Barrel("Resources\\green_cannon.png", new Vector2(23, 30), -30, nextState, "p1");
            tank1 = new Tank("Green", "Resources\\green_tank.png", terrain.GetTankSpawnPos(true), tanksize, b1, OnGameOver);

            Barrel b2 = new Barrel("Resources\\red_cannon.png", new Vector2(23, 30), -150, nextState, "p2");
            tank2 = new Tank("Green", "Resources\\red_tank.png", terrain.GetTankSpawnPos(false), tanksize, b2, OnGameOver);

            currentTime = roundTime;
            roundTimer = new UIText(currentTime.ToString(), new Vector2(width / 2, UIItemSize.Y), UIItemSize.X, UIItemSize.Y, Color.Red);


            gameSequence = new List<GameState>()
            {
                GameState.P1TURN,
                GameState.WAITIMPACT,
                GameState.P2TURN,
                GameState.WAITIMPACT
            };
            gameState = gameSequence.First();
            activePlayer = new UIText(gameState.ToString(), new Vector2(width / 2, 0), UIItemSize.X, UIItemSize.Y, Color.Blue);
            stateIndex = -1;
            NextState();
        }

        public void FixedUpate(TimeSpan deltaTime)
        {
            if (!gameOver)
                ExecuteState((double)deltaTime.Milliseconds / 1000f);
        }

        private void NextState()
        {
            stateIndex++;
            stateIndex %= gameSequence.Count;
            gameState = gameSequence[stateIndex];
            PrepareState();
        }

        //If it gets more complex than this change to an abstract state system
        private void PrepareState()
        {
            switch (gameState)
            {
                case GameState.P1TURN:
                    tank1.isEnabled = (true);
                    tank2.isEnabled = (false);
                    break;
                case GameState.P2TURN:
                    tank1.isEnabled = (false);
                    tank2.isEnabled = (true);
                    break;
                case GameState.WAITIMPACT:
                    tank1.isEnabled = (false);
                    tank2.isEnabled = (false);
                    currentTime = roundTime;
                    break;
            }
            activePlayer.text = gameState.ToString();
        }

        private void ExecuteState(double deltaTime)
        {
            if (gameState != GameState.WAITIMPACT)
            {
                currentTime -= deltaTime;
                roundTimer.text = ("Time left: " + (int)(currentTime));
                if (currentTime < 0.1f)
                {
                    nextState();
                    nextState();
                }
            }
        }

        private void OnGameOver(string winner)
        {
            tank1.isEnabled = (false);
            tank2.isEnabled = (false);
            gameOver = true;
            UIText u = new UIText(winner + " wins", new Vector2(UI.GameWidth / 2, UI.GameHeight / 2), 50, 50, Color.Gold);
            u.SetFont(new Font("Aerial", 32));
        }
    }
}
