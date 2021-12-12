using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tetris.Core.Utils;

namespace Tetris.Core.Scenes
{
    public sealed class GameOverScreen : IScene
    {
        private readonly ContentManager _content;
        private readonly Game _game;
        private readonly Input _input;
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;

        private readonly Matrix _matrix;

        public GameOverScreen(Game game)
        {
            _content = new ContentManager(game.Services, "Content");
            _game = game;
            _input = game.Input;
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);

            _font = _content.Load<SpriteFont>("Score");

            _matrix =
                Matrix.CreateTranslation(0, 0, 0) *
                Matrix.CreateRotationZ(0) *
                Matrix.CreateScale(new Vector3(Game.Scale, Game.Scale, 1));
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _matrix);

            _spriteBatch.DrawString(_font, "GAME OVER", new Vector2(Game.Width / 2 - 50, Game.Height / 2 - 23), Color.White);
            _spriteBatch.DrawString(_font, "PRESS SPACE TO RESTART", new Vector2(Game.Width / 2 - 110, Game.Height / 2 + 25), Color.White);

            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            if (_input.IsKeyJustPressed(Keys.Space))
            {
                _game.ChangeScene(new GameScreen(_game));
            }
        }

        public void Dispose()
        {
            _content.Dispose();
            _spriteBatch.Dispose();
        }
    }
}
