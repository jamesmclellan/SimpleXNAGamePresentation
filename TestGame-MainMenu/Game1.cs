using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Utilities;

namespace TestGame_MainMenu
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Timing
        double lastTenHertzUpdate = 0.0;
        double lastOneHertzUpdate = 0.0;

        // Textures
        Texture2D levelBackground;

        // Texture Location
        Vector2 levelBackgroundPosition = new Vector2(0.0f, 0.0f);

        // Spritefont Variables
        SpriteFont lostSaloon;
        SpriteFont nashville;

        // Spritefont Locations
        Vector2 mainTextLocation;
        Vector2[] menuTextLocation;

        // Menu Items
        string[] menuText = { "New Game", "Continue Game", "Medals", "Help" };

        // Sound
        Song menuSound;

        // List
        Utilities.LoopingList menuIndex;
        Utilities.DebounceItem menuIncrement;
        Utilities.DebounceItem menuDecrement;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            mainTextLocation = new Vector2(240.0f, 100.0f);
            menuTextLocation = new Vector2[4];
            for (int index = 0; index < 4; index++)
            {
                menuTextLocation[index] = new Vector2(380.0f, 200.0f + (50.0f * index));
            }
            menuIndex = new LoopingList(4);
            menuIncrement = new DebounceItem(menuIndex.Increment);
            menuDecrement = new DebounceItem(menuIndex.Decrement);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            levelBackground = Content.Load<Texture2D>("old_parchment");
            lostSaloon = Content.Load<SpriteFont>("LostSaloon");
            nashville = Content.Load<SpriteFont>("nashville");

            menuSound = Content.Load<Song>("April_Kisses");
            MediaPlayer.Play(menuSound);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (IsTenHertzUpdateFrame(gameTime))
            {
                PerformTenHertzTasks(gameTime);
            }

            if (IsOneHertzUpdateFrame(gameTime))
            {
                PerformOneHertzTasks(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // Begin sprite batch - to be done before drawing
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            spriteBatch.Draw(levelBackground, levelBackgroundPosition, null, Color.White, 0.0f,
                Vector2.Zero, 2.3f, SpriteEffects.None, 0.0f);

            spriteBatch.DrawString(lostSaloon, "THE LOST SALOON", mainTextLocation, Color.Black);

            Color fontColor = Color.Black;
            for (int index = 0; index < 4; index++)
            {
                // set the font color based on the selected menu item
                if (index == menuIndex.CurrentIndex)
                {
                    fontColor = Color.White;
                }
                else
                {
                    fontColor = Color.Black;
                }

                // draw the text
                Vector2 menuMeasurements = nashville.MeasureString(menuText[index]);
                Vector2 menuLocation = new Vector2(menuTextLocation[index].X - (menuMeasurements.X / 2.0f), menuTextLocation[index].Y);
                spriteBatch.DrawString(nashville, menuText[index], menuLocation, fontColor);
            }

            // End Sprite Batch - to be done after all drawing is complete, but before call to base.Draw()
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool IsTenHertzUpdateFrame(GameTime gameTime)
        {
            return ((gameTime.TotalGameTime.TotalSeconds - lastTenHertzUpdate) > 0.1);
        }

        private bool IsOneHertzUpdateFrame(GameTime gameTime)
        {
            return ((gameTime.TotalGameTime.TotalSeconds - lastOneHertzUpdate) > 1.0);
        }

        private void PerformTenHertzTasks(GameTime gameTime)
        {
            // reset the time starting point
            lastTenHertzUpdate = gameTime.TotalGameTime.TotalSeconds;

            UpdateInputs();
        }

        private void PerformOneHertzTasks(GameTime gameTime)
        {
            // reset the time starting point
            lastOneHertzUpdate = gameTime.TotalGameTime.TotalSeconds;
        }

        public virtual void UpdateInputs()
        {
            UpdateKeyboard();
            UpdateGamePad();
        }

        private void UpdateGamePad()
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);

            if (pad1.IsConnected)
            {
                menuDecrement.Update(pad1.DPad.Up == ButtonState.Pressed);
            }

            if (pad1.IsConnected)
            {
                menuIncrement.Update(pad1.DPad.Down == ButtonState.Pressed);
            }
        }

        private void UpdateKeyboard()
        {
            KeyboardState keys = Keyboard.GetState();

            menuDecrement.Update(keys.IsKeyDown(Keys.Up));

            menuIncrement.Update(keys.IsKeyDown(Keys.Down));
        }
    }
}
