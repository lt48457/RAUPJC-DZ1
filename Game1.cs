using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;
using Zadatak3;

namespace Game1
{
    public abstract class Sprite : IPhysicalObject2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Texture2D Texture { get; set; }
        protected Sprite(int width, int height, float x = 0, float y = 0)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }
        /// <summary >
        /// Base draw method
        /// </ summary >
        public virtual void DrawSpriteOnScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0,
            Width, Height), Color.White);
        }
    }

    /// <summary >
    /// Game background representation
    /// </ summary >
    public class Background : Sprite
    {
        public Background(int width, int height) : base(width, height)
        {
        }
    }
    /// <summary >
    /// Game ball object representation
    /// </ summary >
    public class Ball : Sprite
    {
        /// <summary >
        /// Defines current ball speed in time .
        /// </ summary >
        public float Speed { get; set; }
        public float BumpSpeedIncreaseFactor { get; set; }
        /// <summary >
        /// Defines ball direction .
        /// Valid values ( -1 , -1) , (1 ,1) , (1 , -1) , ( -1 ,1).
        /// Using Vector2 to simplify game calculation . Potentially
        /// dangerous because vector 2 can swallow other values as well .
        /// OPTIONAL TODO : create your own , more suitable type
        /// </ summary >
        public Vector2 Direction { get; set; }
        public Ball(int size, float speed, float
        defaultBallBumpSpeedIncreaseFactor) : base(size, size)
        {
            Speed = speed;
            BumpSpeedIncreaseFactor = defaultBallBumpSpeedIncreaseFactor;
            // Initial direction
            Direction = new Vector2(1, 1);
        }
    }
    /// <summary >
    /// Represents player paddle .
    /// </ summary >
    public class Paddle : Sprite
    {
        /// <summary >
        /// Current paddle speed in time
        /// </ summary >
        public float Speed { get; set; }
        public Paddle(int width, int height, float initialSpeed) : base(width,
        height)
        {
            Speed = initialSpeed;
        }
        /// <summary >
        /// Overriding draw method . Masking paddle texture with black color .
        /// </ summary >
        public override void DrawSpriteOnScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0,
            Width, Height), Color.GhostWhite);
        }
    }
    
        /// <summary>
        /// This is the main type for your game.
        /// </summary>
        /// 

        public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<Wall> Walls { get; set; }
        public List<Wall> Goals { get; set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 500
            };
            Content.RootDirectory = " Content ";
        }

        /// <summary >
        /// Bottom paddle object
        /// </ summary >
        public Paddle PaddleBottom { get; private set; }
        /// <summary >
        /// Top paddle object
        /// </ summary >
        public Paddle PaddleTop { get; private set; }
        /// <summary >
        /// Ball object
        /// </ summary >
        public Ball Ball { get; private set; }
        /// <summary >
        /// Background image
        /// </ summary >
        public Background Background { get; private set; }
        /// <summary >
        /// Sound when ball hits an obstacle .
        /// SoundEffect is a type defined in Monogame framework
        /// </ summary >
        public SoundEffect HitSound { get; private set; }
        /// <summary >
        /// Background music . Song is a type defined in Monogame framework
        /// </ summary >
        public Song Music { get; private set; }
        /// <summary >
        /// Generic list that holds Sprites that should be drawn on screen
        /// </ summary >
        private IGenericList<Sprite> SpritesForDrawList = new GenericList<Sprite>();


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Screen bounds details . Use this information to set up game objects positions.
            var screenBounds = GraphicsDevice.Viewport.Bounds;
            PaddleBottom = new Paddle(GameConstants.PaddleDefaultWidth,
            GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);
            PaddleBottom.X = 0;
            PaddleBottom.Y = 0;
            PaddleTop = new Paddle(GameConstants.PaddleDefaultWidth,
            GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);
            PaddleTop.X = 0;
            PaddleTop.Y = graphics.PreferredBackBufferHeight;
            Ball = new Ball(GameConstants.DefaultBallSize,
            GameConstants.DefaultInitialBallSpeed,
            GameConstants.DefaultBallBumpSpeedIncreaseFactor)
            {
                X = graphics.PreferredBackBufferWidth/2,
                Y = graphics.PreferredBackBufferHeight/2
            };
            Background = new Background(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);          

            // Add our game objects to the sprites that should be drawn collection .. you ’ll see why in a second
            SpritesForDrawList.Add(Background);
            SpritesForDrawList.Add(PaddleBottom);
            SpritesForDrawList.Add(PaddleTop);
            SpritesForDrawList.Add(Ball);

            Walls = new List<Wall>()
            {
            // try with 100 for default wall size !
            new Wall (- GameConstants.WallDefaultSize ,0,
            GameConstants.WallDefaultSize , screenBounds . Height ),
            new Wall ( screenBounds .Right ,0, GameConstants.WallDefaultSize ,
            screenBounds . Height ),
            };
            Goals = new List<Wall>()
            {
            new Wall (0, screenBounds .Height , screenBounds .Width ,
            GameConstants . WallDefaultSize ),
            new Wall ( screenBounds .Top ,- GameConstants . WallDefaultSize ,
            screenBounds .Width , GameConstants . WallDefaultSize ),
            };
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Initialize new SpriteBatch object which will be used to draw textures .
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Set textures
            Texture2D paddleTexture = Content.Load<Texture2D>(" paddle ");
            PaddleBottom.Texture = paddleTexture;
            PaddleTop.Texture = paddleTexture;
            Ball.Texture = Content.Load<Texture2D>(" ball ");
            Background.Texture = Content.Load<Texture2D>(" background ");
            // Load sounds
            // Start background music
            HitSound = Content.Load<SoundEffect>(" hit");
            Music = Content.Load<Song>(" music ");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Music);
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

            var touchState = Keyboard.GetState();
            if(touchState.IsKeyDown(Keys.Left))
            {
                PaddleBottom.X = PaddleBottom.X - (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            if (touchState.IsKeyDown(Keys.Right))
            {
                PaddleBottom.X = PaddleBottom.X + (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            PaddleBottom.X = MathHelper.Clamp(PaddleBottom.X, bounds.Left, bounds.Right - PaddleBottom.Width);

            var ballPositionChange = Ball.Direction * (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Ball.Speed);
            Ball.X += ballPositionChange.X;
            Ball.Y += ballPositionChange.Y;

            // Ball - side walls
            if (Walls.Any(w => CollisionDetector.Overlaps(Ball, w)))
            {
                Ball.Direction.X = -Ball.Direction.X;
                Ball.Speed = Ball.Speed * Ball.BumpSpeedIncreaseFactor;
            }
            // Ball - winning walls
            if (Goals.Any(w => CollisionDetector.Overlaps(Ball, w)))
            {
                Ball.X = bounds.Center.ToVector2().X;
                Ball.Y = bounds.Center.ToVector2().Y;
                Ball.Speed = GameConstants.DefaultInitialBallSpeed;
                HitSound.Play();
            }
            // Paddle - ball collision
            if (CollisionDetector.Overlaps(Ball, PaddleTop) && Ball.Direction.Y < 0
            || (CollisionDetector.Overlaps(Ball, PaddleBottom) && Ball.Direction.Y > 0))
            {
                Ball.Direction.Y = -Ball.Direction.Y;
                Ball.Speed *= Ball.BumpSpeedIncreaseFactor;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            // Start drawing .
            spriteBatch.Begin();
            for (int i = 0; i < SpritesForDrawList.Count; i++)
            {
                SpritesForDrawList.GetElement(i).DrawSpriteOnScreen(spriteBatch);
            }
            // End drawing .
            // Send all gathered details to the graphic card in one batch .
            spriteBatch.End();
            base.Draw(gameTime);
        }
    
    }

    public class GameConstants
    {
        public const float PaddleDefaulSpeed = 0.9f;
        public const int PaddleDefaultWidth = 200;
        public const int PaddleDefaulHeight = 20;
        public const float DefaultInitialBallSpeed = 0.4f;
        public const float DefaultBallBumpSpeedIncreaseFactor = 1.05f;
        public const int DefaultBallSize = 40;
    }

    public interface IPhysicalObject2D
    {
        float X { get; set; }
        float Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }
    }

    public class CollisionDetector
    {
        public static bool Overlaps(IPhysicalObject2D a, IPhysicalObject2D b)
        {
            // return true if overlaps , false otherwise ...
            
        }
    }

    public class Wall : IPhysicalObject2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Wall(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

}
