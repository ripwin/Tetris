using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Tetris.Core.Scenes;
using Tetris.Core.Utils;

namespace Tetris.Core
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public const int SCALE = 2;

        private readonly GraphicsDeviceManager _graphics;
        private IScene _scene;

        public Input Input { get; }

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Input = new Input();
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 640 * SCALE;
            _graphics.PreferredBackBufferHeight = 360 * SCALE;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _scene = new GameScreen(this);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update(Keyboard.GetState());

            if (Input.IsKeyDown(Keys.Escape))
                Exit();

            _scene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _scene.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void ChangeScene(IScene scene)
        {
            _scene.Dispose();

            _scene = scene;
        }
    }
}
