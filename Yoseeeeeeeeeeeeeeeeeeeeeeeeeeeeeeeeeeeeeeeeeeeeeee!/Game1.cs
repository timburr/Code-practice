using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Yoseeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee_
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //camera
        Vector3 CamTarget;
        Vector3 CamPosition;
        Matrix ProjectionMatrix;
        Matrix ViewMatrix;
        Matrix WorldMatrix;

        // basic effect for rendering
        BasicEffect BaseEffect;

        // geometric info
        VertexPositionColor[] TriangleVertices;
        VertexBuffer VertexBuff;

        //orbit
        bool Orbit = false;
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

            //setup for camera
            CamTarget = new Vector3(0f, 0f, 0f);
            CamPosition = new Vector3(0f, 0f, -100f);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45f),
                GraphicsDevice.DisplayMode.AspectRatio,
                1f, 1000f);
            ViewMatrix = Matrix.CreateLookAt(CamPosition, CamTarget,
                new Vector3(0f, 1f, 0f)); // Y-as up
            WorldMatrix = Matrix.CreateWorld(CamTarget, Vector3.Forward, Vector3.Up);
            // basiceffect
            BaseEffect = new BasicEffect(GraphicsDevice);
            BaseEffect.Alpha = 1f;

            // for seeing colors of the vertices
            BaseEffect.VertexColorEnabled = true;

            // lightning requires normal info which VertexPositionColor doesn't have
            //so we have to make a custom definition for it
            BaseEffect.LightingEnabled = false;

            //geometry
            TriangleVertices = new VertexPositionColor[3];
            TriangleVertices[0] = new VertexPositionColor(new Vector3(0, 20, 0), Color.Red);
            TriangleVertices[1] = new VertexPositionColor(new Vector3(-20, -20, 0), Color.Green);
            TriangleVertices[2] = new VertexPositionColor(new Vector3(20, -20, 0), Color.Blue);

            //vertical buffer
            VertexBuff = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            VertexBuff.SetData(TriangleVertices);
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
           
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var KState = Keyboard.GetState();
            if (KState.IsKeyDown(Keys.A))
            {
                CamPosition.X -= 1f;
                CamTarget.X -= 1f;
            }
            if (KState.IsKeyDown(Keys.D))
            {
                CamPosition.X += 1f;
                CamTarget.X += 1f;
            }
            if (KState.IsKeyDown(Keys.W))
            {
                CamPosition.Y -= 1f;
                CamTarget.Y -= 1f;
            }
            if (KState.IsKeyDown(Keys.S))
            {
                CamPosition.Y += 1f;
                CamTarget.Y += 1f;
            }
            if (KState.IsKeyDown(Keys.R))
            {
                CamPosition.Z += 1f;
            }
            if (KState.IsKeyDown(Keys.F))
            {
                CamPosition.Z -= 1f;
            }
            if (KState.IsKeyDown(Keys.Space))
            {
                Orbit = !Orbit;
            }
            if (Orbit)
            {
                Matrix RotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                CamPosition = Vector3.Transform(CamPosition, RotationMatrix);
            }
            ViewMatrix = Matrix.CreateLookAt(CamPosition, CamTarget, Vector3.Up);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            BaseEffect.Projection = ProjectionMatrix;
            BaseEffect.View = ViewMatrix;
            BaseEffect.World = WorldMatrix;
            GraphicsDevice.Clear(Color.HotPink);
            GraphicsDevice.SetVertexBuffer(VertexBuff);

            //turn off culling so we see both sides of our triangles
            RasterizerState RasterState = new RasterizerState();
            RasterState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = RasterState;
            foreach (EffectPass Pass in BaseEffect.CurrentTechnique.Passes)
            {
                Pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }
            

            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
