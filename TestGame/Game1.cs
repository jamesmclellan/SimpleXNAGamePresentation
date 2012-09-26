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



namespace TestGame
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

        // Texture Variables
        protected Texture2D[] hero;
        protected Texture2D[] villain;
        protected Texture2D levelBackground;
        
        // Animation Variables
        Vector2 levelBackgroundPosition = new Vector2(0.0f, 0.0f);

        bool drawHero = true;
        Vector2 heroPosition = new Vector2(30.0f, 440.0f);
        bool isHeroRunning = false;
        bool drawAltHero = false;
        Rectangle heroRectangle;

        bool drawVillain = true;
        Vector2 villainPosition = new Vector2(330.0f, 460.0f);
        bool isVillainRunning = false;
        bool drawAltVillain = false;
        Rectangle villainRectangle;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            levelBackground = Content.Load<Texture2D>("desert-background");
            hero = new Texture2D[2];
            hero[0] = Content.Load<Texture2D>("mario");
            hero[1] = Content.Load<Texture2D>("mario-alt");
            villain = new Texture2D[2];
            villain[0] = Content.Load<Texture2D>("goomba");
            villain[1] = Content.Load<Texture2D>("goomba-alt");

            heroRectangle = new Rectangle((int)heroPosition.X, (int)heroPosition.Y, hero[0].Width, 
                hero[0].Height);
            villainRectangle = new Rectangle((int)villainPosition.X, (int)villainPosition.Y, 
                villain[0].Width, villain[0].Height);

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
            UpdateInputs();
            UpdateSprite(graphics, gameTime);

            // Perform collision detection
            heroRectangle.X = (int) heroPosition.X;
            heroRectangle.Y = (int)heroPosition.Y;
            villainRectangle.X = (int)villainPosition.X;
            villainRectangle.Y = (int)villainPosition.Y;

            if(heroRectangle.Intersects(villainRectangle))
            {
                drawVillain = false;
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

            if (IsTenHertzUpdateFrame(gameTime))
            {
                PerformTenHertzTasks(gameTime);
            }

            if (drawHero)
            {
                if (drawAltHero)
                {
                    spriteBatch.Draw(hero[1], heroPosition, Color.White);
                }
                else
                {
                    spriteBatch.Draw(hero[0], heroPosition, Color.White);
                }
            }

            if (drawVillain)
            {
                if (drawAltVillain)
                {
                    spriteBatch.Draw(villain[1], villainPosition, Color.White);
                }
                else
                {
                    spriteBatch.Draw(villain[0], villainPosition, Color.White);
                }
            }

            // End Sprite Batch - to be done after all drawing is complete, but before call to base.Draw()
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool IsTenHertzUpdateFrame(GameTime gameTime)
        {
            return ((gameTime.TotalGameTime.TotalSeconds - lastTenHertzUpdate) > 0.1);
        }

        private void PerformTenHertzTasks(GameTime gameTime)
        {
            // reset the time starting point
            lastTenHertzUpdate = gameTime.TotalGameTime.TotalSeconds;

            // set or lower the hero running animation flag
            if (isHeroRunning)
            {
                drawAltHero = !drawAltHero;
            }
            else
            {
                drawAltHero = false;
            }
        }

        public virtual void UpdateInputs()
        {
            isHeroRunning = false;

            UpdateKeyboard();
            UpdateGamePad();
        }

        public virtual void UpdateSprite(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            float MaxX = ((float)graphics.GraphicsDevice.Viewport.Width) / 2.0f;
            float MinX = ((float)graphics.GraphicsDevice.Viewport.Width) / 10.0f;

            if (heroPosition.X > MaxX)
            {
                // leave the hero in place, move the texture instead
                float difference = heroPosition.X - MaxX;
                heroPosition.X = MaxX;
                levelBackgroundPosition.X -= difference;
                // allow the hero to run to the end of the screen (but not past it)
                if (levelBackgroundPosition.X < -levelBackground.Width)
                {
                    levelBackgroundPosition.X = -levelBackground.Width;
                }
            }
            if (heroPosition.X < MinX)
            {
                float difference = MinX - heroPosition.X;
                heroPosition.X = MinX;
                levelBackgroundPosition.X += difference;
                if (levelBackgroundPosition.X > 0.0)
                {
                    levelBackgroundPosition.X = 0.0f;
                }
            }
        }

        private void UpdateGamePad()
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);

            if (pad1.IsConnected && (pad1.DPad.Left == ButtonState.Pressed))
            {
                heroPosition.X -= 2.0f;
                isHeroRunning = true;
            }
            if (pad1.IsConnected && (pad1.DPad.Right == ButtonState.Pressed))
            {
                heroPosition.X += 2.0f;
                isHeroRunning = true;
            }
            /*
            if (pad1.IsConnected && (pad1.DPad.Up == ButtonState.Pressed))
            {
                heroPosition.Y -= 2.0f;
                isHeroRunning = true;
            }
            if (pad1.IsConnected && (pad1.DPad.Down == ButtonState.Pressed))
            {
                heroPosition.Y += 2.0f;
                isHeroRunning = true;
            }
             */
        }

        private void UpdateKeyboard()
        {
            KeyboardState keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.Left))
            {
                heroPosition.X -= 2.0f;
                isHeroRunning = true;
            }
            if (keys.IsKeyDown(Keys.Right))
            {
                heroPosition.X += 2.0f;
                isHeroRunning = true;
            }
            /*
            if (keys.IsKeyDown(Keys.Up))
            {
                heroPosition.Y -= 2.0f;
                isHeroRunning = true;
            }
            if (keys.IsKeyDown(Keys.Down))
            {
                heroPosition.Y += 2.0f;
                isHeroRunning = true;
            }
             */
        }

    }
}
