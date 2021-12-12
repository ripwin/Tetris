using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tetris.Core.Enums;
using Tetris.Core.Graphics;
using Tetris.Core.Systems;
using Tetris.Core.Utils;

namespace Tetris.Core.Scenes
{
    public sealed class GameScreen : IScene
    {
        private const int UnitSize = 20;

        private const int GridHeight = 20;
        private const int GridWidth = 10;

        private readonly ContentManager _content;
        private readonly Input _input;
        private readonly SpriteBatch _spriteBatch;
        private readonly TextureAtlas<TileColor> _blockTextureAtlas;

        private readonly SpriteFont _font;

        private readonly Matrix _matrix;

        private readonly ISystem _boardSystem;

        public GameScreen(Game game)
        {
            _content = new ContentManager(game.Services, "Content");
            _input = game.Input;
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);

            _blockTextureAtlas = new TextureAtlas<TileColor>(_content.Load<Texture2D>("blocksTextureAtlas"));
            _blockTextureAtlas.AddRegion(TileColor.Default, new Rectangle(0, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(TileColor.Cyan, new Rectangle(UnitSize, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(TileColor.Blue, new Rectangle(UnitSize * 2, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(TileColor.Orange, new Rectangle(UnitSize * 3, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(TileColor.Yellow, new Rectangle(UnitSize * 4, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(TileColor.Green, new Rectangle(UnitSize * 5, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(TileColor.Violet, new Rectangle(UnitSize * 6, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(TileColor.Red, new Rectangle(UnitSize * 7, 0, UnitSize, UnitSize));

            _matrix =
                Matrix.CreateTranslation(0, 0, 0) *
                Matrix.CreateRotationZ(0) *
                Matrix.CreateScale(new Vector3(Game.Scale, Game.Scale, 1));

            _font = _content.Load<SpriteFont>("Score");

            _boardSystem = new BoardSystem(game, _input, _blockTextureAtlas, _font, _spriteBatch, GridHeight, GridWidth, UnitSize);
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _matrix);

            _boardSystem.Draw(gameTime);

            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            _boardSystem.Update(gameTime);
        }

        public void Dispose()
        {
            _content.Dispose();
            _spriteBatch.Dispose();
        }
    }
}
