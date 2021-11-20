using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tetris.Core.Enums;
using Tetris.Core.Graphics;
using Tetris.Core.Utils;

namespace Tetris.Core.Entities
{
    public class Tetrinos
    {
        private const float MinXLag = 40;
        private const float MaxXLag = 200;
        private const float MaxYLag = 40;

        private readonly BlockColor[] _blocks;
        private readonly Input _input;
        private readonly TextureAtlas<BlockColor> _blockTextureAtlas;
        private readonly SpriteBatch _spriteBatch;
        private readonly int _unitSize;

        private Vector2 _position;
        private int _size;
        private float _currentXLag;
        private float _currentYLag;

        public Tetrinos(
            Input input, 
            TextureAtlas<BlockColor> blockTextureAtlas, 
            SpriteBatch spriteBatch, 
            TetriminosType type,
            int unitSize)
        {
            _unitSize = unitSize;

            _input = input;
            _blockTextureAtlas = blockTextureAtlas;
            _spriteBatch = spriteBatch;

            switch (type)
            {
                case TetriminosType.I:
                    _size = 4;

                    _blocks = new BlockColor[_size * _size];
                    _blocks[4] = BlockColor.Cyan;
                    _blocks[5] = BlockColor.Cyan;
                    _blocks[6] = BlockColor.Cyan;
                    _blocks[7] = BlockColor.Cyan;
                    break;

                case TetriminosType.J:
                    _size = 3;

                    _blocks = new BlockColor[_size * _size];
                    _blocks[0] = BlockColor.Blue;
                    _blocks[3] = BlockColor.Blue;
                    _blocks[4] = BlockColor.Blue;
                    _blocks[5] = BlockColor.Blue;
                    break;

                case TetriminosType.L:
                    _size = 3;

                    _blocks = new BlockColor[_size * _size];
                    _blocks[2] = BlockColor.Orange;
                    _blocks[3] = BlockColor.Orange;
                    _blocks[4] = BlockColor.Orange;
                    _blocks[5] = BlockColor.Orange;
                    break;

                case TetriminosType.O:
                    _size = 2;

                    _blocks = new BlockColor[_size * _size];
                    _blocks[0] = BlockColor.Yellow;
                    _blocks[1] = BlockColor.Yellow;
                    _blocks[2] = BlockColor.Yellow;
                    _blocks[3] = BlockColor.Yellow;
                    break;

                case TetriminosType.S:
                    _size = 3;

                    _blocks = new BlockColor[_size * _size];
                    _blocks[1] = BlockColor.Green;
                    _blocks[2] = BlockColor.Green;
                    _blocks[3] = BlockColor.Green;
                    _blocks[4] = BlockColor.Green;
                    break;

                case TetriminosType.T:
                    _size = 3;

                    _blocks = new BlockColor[_size * _size];
                    _blocks[1] = BlockColor.Violet;
                    _blocks[3] = BlockColor.Violet;
                    _blocks[4] = BlockColor.Violet;
                    _blocks[5] = BlockColor.Violet;
                    break;

                case TetriminosType.Z:
                    _size = 3;

                    _blocks = new BlockColor[_size * _size];
                    _blocks[0] = BlockColor.Red;
                    _blocks[1] = BlockColor.Red;
                    _blocks[4] = BlockColor.Red;
                    _blocks[5] = BlockColor.Red;
                    break;
            }
        }

        public void Draw(GameTime _)
        {
            var texture = _blockTextureAtlas.Texture();

            for (int i = 0, index = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    if (_blocks[index] == BlockColor.Default)
                    {
                        index++;
                        continue;
                    }

                    var sourceRectangle = _blockTextureAtlas.GetRegion(_blocks[index]);

                    _spriteBatch.Draw(
                        texture,
                        new Vector2(
                            _position.X + j * _unitSize,
                            _position.Y + i * _unitSize),
                        sourceRectangle,
                        Color.White);

                    index++;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_input.IsKeyJustPressed(Keys.Left))
            {
                _position += new Vector2(-_unitSize, 0);
                _currentXLag = MaxXLag;
            }
            else if (_input.IsKeyDown(Keys.Left))
            {
                if (_currentXLag < 0)
                {
                    _position += new Vector2(-_unitSize, 0);
                    _currentXLag = MinXLag;
                }
            }

            if (_input.IsKeyJustPressed(Keys.Right))
            {
                _position += new Vector2(_unitSize, 0);
                _currentXLag = MaxXLag;
            }
            else if (_input.IsKeyDown(Keys.Right))
            {
                if (_currentXLag < 0)
                {
                    _position += new Vector2(_unitSize, 0);
                    _currentXLag = MinXLag;
                }
            }

            _currentXLag -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_input.IsKeyDown(Keys.Up))
            {

            }
            else if (_input.IsKeyDown(Keys.Down))
            {
                if (_currentYLag < 0)
                { 
                    _position += new Vector2(0, _unitSize);
                    _currentYLag = MaxYLag;
                }
            }

            _currentYLag -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }
}
