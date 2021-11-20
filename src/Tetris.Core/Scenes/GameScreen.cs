using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tetris.Core.Entities;
using Tetris.Core.Enums;
using Tetris.Core.Graphics;
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
        private readonly TextureAtlas<BlockColor> _blockTextureAtlas;

        private readonly Matrix _matrix;

        private readonly BlockColor[] _tetris;

        private readonly Tetrinos _tetrinos;

        public GameScreen(Game game)
        {
            _content = new ContentManager(game.Services, "Content");
            _input = game.Input;
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);

            _blockTextureAtlas = new TextureAtlas<BlockColor>(_content.Load<Texture2D>("blocksTextureAtlas"));
            _blockTextureAtlas.AddRegion(BlockColor.Default, new Rectangle(0, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(BlockColor.Cyan, new Rectangle(UnitSize, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(BlockColor.Blue, new Rectangle(UnitSize * 2, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(BlockColor.Orange, new Rectangle(UnitSize * 3, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(BlockColor.Yellow, new Rectangle(UnitSize * 4, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(BlockColor.Green, new Rectangle(UnitSize * 5, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(BlockColor.Violet, new Rectangle(UnitSize * 6, 0, UnitSize, UnitSize));
            _blockTextureAtlas.AddRegion(BlockColor.Red, new Rectangle(UnitSize * 7, 0, UnitSize, UnitSize));

            _matrix =
                Matrix.CreateTranslation(0, 0, 0) *
                Matrix.CreateRotationZ(0) *
                Matrix.CreateScale(new Vector3(Game.Scale, Game.Scale, 1));

            _tetris = new BlockColor[GridWidth * GridHeight];

            _tetrinos = new Tetrinos(_input, _blockTextureAtlas, _spriteBatch, TetriminosType.Z, UnitSize);
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _matrix);

            var texture = _blockTextureAtlas.Texture();

            for (int i = 0, index = 0; i < GridHeight; i++)
            {
                for (var j = 0; j < GridWidth; j++)
                {
                    var sourceRectangle = _blockTextureAtlas.GetRegion(_tetris[index]);

                    _spriteBatch.Draw(
                        texture,
                        new Vector2(
                            j * UnitSize,
                            i * UnitSize),
                        sourceRectangle,
                        Color.White);

                    index++;
                }
            }

            _tetrinos.Draw(gameTime);

            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            _tetrinos.Update(gameTime);
        }

        public void Dispose()
        {
            _content.Dispose();
            _spriteBatch.Dispose();
        }
    }
}
