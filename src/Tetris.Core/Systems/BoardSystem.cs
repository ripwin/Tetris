using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Tetris.Core.Entities;
using Tetris.Core.Enums;
using Tetris.Core.Graphics;
using Tetris.Core.Scenes;
using Tetris.Core.Utils;

namespace Tetris.Core.Systems
{
    public sealed class BoardSystem : ISystem
    {
        private const float DownTimeMaxLag = 800;
        private const float MinXLag = 40;
        private const float MaxXLag = 200;
        private const float MaxYLag = 40;
        private const int PieceSize = 4;
        private const int NumberOfRotation = 4;

        private readonly Game _game;
        private readonly Input _input;
        private readonly TextureAtlas<TileColor> _blockTextureAtlas;
        private readonly SpriteBatch _spriteBatch;

        private readonly int _gridHeight;
        private readonly int _gridWidth;
        private readonly int _unitSize;

        private readonly TileColor[] _tetris;

        private float _currentDownLag;
        private float _currentXLag;
        private float _currentYLag;

        private readonly SpriteFont _font;
        private int _score;

        private ITetrinos _tetrinos;
        private ITetrinos _savedTetrinos;
        private bool isCurrentlySaved;

        private readonly ITetrinos[] _nextTetrinos;

        private enum Rotation
        { 
            Clockwise,
            CounterClockwise
        }

        public BoardSystem(
            Game game,
            Input input,
            TextureAtlas<TileColor> blockTextureAtlas,
            SpriteFont font,
            SpriteBatch spriteBatch,
            int gridHeight,
            int gridWidth,
            int unitSize)
        {
            _game = game;

            _input = input;
            _gridHeight = gridHeight;
            _gridWidth = gridWidth;
            _unitSize = unitSize;
            _blockTextureAtlas = blockTextureAtlas;
            _font = font;
            _spriteBatch = spriteBatch;

            _nextTetrinos = new ITetrinos[3];

            for (var i = 0; i < _nextTetrinos.Length; i++)
            {
                _nextTetrinos[i] = CreateNewTetrinos();
            }

            _tetrinos = GetNextTetrinos();
            _savedTetrinos = null;

            _tetris = new TileColor[_gridWidth * _gridHeight];
        }

        public void Draw(GameTime _)
        {
            var texture = _blockTextureAtlas.Texture();

            for (int i = 0, index = 0; i < _gridHeight; i++)
            {
                for (var j = 0; j < _gridWidth; j++)
                {
                    var sourceRectangle = _blockTextureAtlas.GetRegion(_tetris[index]);

                    _spriteBatch.Draw(
                        texture,
                        new Vector2(
                            j * _unitSize,
                            i * _unitSize),
                        sourceRectangle,
                        Color.White);

                    index++;
                }
            }

            for (var i = 0; i < PieceSize; i++)
            {
                var sourceRectangle = _blockTextureAtlas.GetRegion(_tetrinos.Color);

                _spriteBatch.Draw(
                    texture,
                    new Vector2(
                        _tetrinos.Positon.X + _tetrinos.Piece[i].X * _unitSize,
                        _tetrinos.Positon.Y + _tetrinos.Piece[i].Y * _unitSize),
                    sourceRectangle,
                    Color.White);
            }


            for (var i = 0; i < _nextTetrinos.Length; i++)
            {
                var position = new Vector2(320, 40 + i * 100);

                for (var j = 0; j < PieceSize; j++)
                {
                    var sourceRectangle = _blockTextureAtlas.GetRegion(_nextTetrinos[i].Color);

                    _spriteBatch.Draw(
                        texture,
                        new Vector2(
                            position.X + _nextTetrinos[i].Piece[j].X * _unitSize,
                            position.Y + _nextTetrinos[i].Piece[j].Y * _unitSize),
                        sourceRectangle,
                        Color.White);
                }
            }

            if (_savedTetrinos is not null)
            {
                var position = new Vector2(420, 40);

                for (var i = 0; i < PieceSize; i++)
                {
                    var sourceRectangle = _blockTextureAtlas.GetRegion(_savedTetrinos.Color);

                    _spriteBatch.Draw(
                        texture,
                        new Vector2(
                            position.X + _savedTetrinos.Piece[i].X * _unitSize,
                            position.Y + _savedTetrinos.Piece[i].Y * _unitSize),
                        sourceRectangle,
                        Color.White);
                }
            }

            _spriteBatch.DrawString(_font, "Score: " + _score, new Vector2(210, 0), Color.White);
            _spriteBatch.DrawString(_font, "Next", new Vector2(320, 0), Color.White);
            _spriteBatch.DrawString(_font, "Save", new Vector2(420, 0), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            var isMovingHorizontal = false;
            var previousPosition = _tetrinos.Positon;

            if (_input.IsKeyJustPressed(Keys.Left))
            {
                _tetrinos.Positon += new Vector2(-_unitSize, 0);
                _currentXLag = MaxXLag;
                isMovingHorizontal = true;
            }
            else if (_input.IsKeyDown(Keys.Left))
            {
                if (_currentXLag < 0)
                {
                    _tetrinos.Positon += new Vector2(-_unitSize, 0);
                    _currentXLag = MinXLag;
                    isMovingHorizontal = true;
                }
            }

            if (_input.IsKeyJustPressed(Keys.Right))
            {
                _tetrinos.Positon += new Vector2(_unitSize, 0);
                _currentXLag = MaxXLag;
                isMovingHorizontal = true;
            }
            else if (_input.IsKeyDown(Keys.Right))
            {
                if (_currentXLag < 0)
                {
                    _tetrinos.Positon += new Vector2(_unitSize, 0);
                    _currentXLag = MinXLag;
                    isMovingHorizontal = true;
                }
            }

            _currentXLag -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_input.IsKeyJustPressed(Keys.Space))
            {
                do
                {
                    _tetrinos.Positon -= new Vector2(0, -_unitSize);
                } while (!IsColliding(_tetrinos.Piece));
            }
            else if (_input.IsKeyDown(Keys.Down))
            {
                if (_currentYLag < 0)
                {
                    _tetrinos.Positon += new Vector2(0, _unitSize);
                    _currentYLag = MaxYLag;
                }
            }

            _currentYLag -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Rotate CounterClockwise
            if (_input.IsKeyJustPressed(Keys.Y))
            {
                RotatePiece(Rotation.CounterClockwise);
            }
            // Rotate clockwise
            else if (_input.IsKeyJustPressed(Keys.X) || _input.IsKeyJustPressed(Keys.Up))
            {
                RotatePiece(Rotation.Clockwise);
            }

            if (_input.IsKeyJustPressed(Keys.C) && !isCurrentlySaved)
            {
                isCurrentlySaved = true;

                var temp = (ITetrinos)Activator.CreateInstance(_tetrinos.GetType());
  
                if (_savedTetrinos is null)
                {
                    _tetrinos = GetNextTetrinos();
                }
                else
                {
                    _tetrinos = _savedTetrinos;
                    _tetrinos.Positon = new Vector2(3 * _unitSize, 0);
                    _tetrinos.Rotation = 0;
                }

                _savedTetrinos = temp;
            }

            if (_currentDownLag < 0 && !IsColliding(_tetrinos.Piece))
            {
                _tetrinos.Positon += new Vector2(0, _unitSize);
                _currentDownLag = DownTimeMaxLag;
            }

            _currentDownLag -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Collision on X axis 
            if (isMovingHorizontal && IsColliding(_tetrinos.Piece))
            {
                _tetrinos.Positon = new Vector2(previousPosition.X, _tetrinos.Positon.Y);
            }

            // Collision on Y axis
            if (IsColliding(_tetrinos.Piece))
            {
                isCurrentlySaved = false;

                // Move the piece up
                _tetrinos.Positon -= new Vector2(0, _unitSize);

                // Update the grid
                foreach (var tile in _tetrinos.Piece)
                {
                    _tetris[(int)(tile.X * _unitSize + _tetrinos.Positon.X) / _unitSize + (int)(tile.Y * _unitSize + _tetrinos.Positon.Y) / _unitSize * _gridWidth] = _tetrinos.Color;
                }

                // Check for complete lines
                var nbCompletedLine = 0;
                for (int row = _gridHeight - 1; row > 0; row--)
                {
                    var lineCompleted = true;

                    for (var col = 0; col < _gridWidth; col++)
                    {
                        if (_tetris[row * _gridWidth + col] == TileColor.Default)
                        {
                            lineCompleted = false;
                        }

                        _tetris[(row + nbCompletedLine) * _gridWidth + col] = _tetris[row * _gridWidth + col];
                    }

                    if (lineCompleted)
                    {
                        nbCompletedLine++;
                    }
                }

                if (nbCompletedLine > 0)
                {
                    _score += nbCompletedLine switch
                    {
                        1 => 40,
                        2 => 100,
                        3 => 300,
                        4 => 1200,
                        _ => throw new ArgumentException("Number of completed line should be between 1 and 4.")
                    };
                }

                // New piece + collision for end game
                _tetrinos = GetNextTetrinos();

                // Check if game over
                if (IsColliding(_tetrinos.Piece))
                {
                    _game.ChangeScene(new GameOverScreen(_game));
                }
            }
        }

        private ITetrinos CreateNewTetrinos()
            => new ITetrinos[] { new I(), new J(), new L(), new O(), new S(), new T(), new Z() }[new Random().Next(7)];

        private ITetrinos GetNextTetrinos()
        {
            var tetrinos = _nextTetrinos[0];

            _nextTetrinos[0] = _nextTetrinos[1];
            _nextTetrinos[1] = _nextTetrinos[2];
            _nextTetrinos[2] = CreateNewTetrinos();

            tetrinos.Positon = new Vector2(3 * _unitSize, 0);
            tetrinos.Rotation = 0;

            return tetrinos;
        }

        private void RotatePiece(Rotation rotation)
        {
            var rotationRelativePosition = _tetrinos.RotationRelativePosition;
            var piece = new Vector2[PieceSize];

            var previousRotation = _tetrinos.Rotation;
            var nextRotation = (_tetrinos.Rotation + (rotation == Rotation.Clockwise ? 1 : -1) + NumberOfRotation) % NumberOfRotation;

            var pieceColliding = true;

            for (var o = 0; o < _tetrinos.Offsets.GetLength(1) && pieceColliding; o++)
            {
                // Get offset
                var previousOffset = _tetrinos.Offsets[previousRotation, o];
                var nextOffset = _tetrinos.Offsets[nextRotation, o];

                var offset = previousOffset - nextOffset;
                piece = new Vector2[PieceSize]; // TODO : Optimise, make the rotation and check for offset after

                for (var i = 0; i < PieceSize; i++)
                {

                    var relativePositionToRotationTile = rotationRelativePosition - _tetrinos.Piece[i];

                    // Multiply by rotation matrix
                    Vector2 newRelativePositionToRotationTile;

                    if (rotation == Rotation.Clockwise)
                    {
                        newRelativePositionToRotationTile = new Vector2(
                            0 * relativePositionToRotationTile.X + 1 * relativePositionToRotationTile.Y,
                            -1 * relativePositionToRotationTile.X + 0 * relativePositionToRotationTile.Y);
                    }
                    else
                    {
                        newRelativePositionToRotationTile = new Vector2(
                            0 * relativePositionToRotationTile.X + -1 * relativePositionToRotationTile.Y,
                            1 * relativePositionToRotationTile.X + 0 * relativePositionToRotationTile.Y);
                    }


                    var newPosition = rotationRelativePosition + newRelativePositionToRotationTile;

                    piece[i] = new Vector2(newPosition.X + (int)offset.X, newPosition.Y - (int)offset.Y);
                }

                pieceColliding = IsColliding(piece);
            }

            if (!pieceColliding)
            {
                _tetrinos.Piece = piece;
                _tetrinos.Rotation = nextRotation;
            }
        }

        private bool IsColliding(Vector2[] piece)
        {
            var minBounds = new Vector2(0, 0);
            var maxBounds = new Vector2(_gridWidth * _unitSize, _gridHeight * _unitSize);

            foreach (var tile in piece)
            {
                if (tile.X * _unitSize + _tetrinos.Positon.X < minBounds.X || 
                    tile.Y * _unitSize + _tetrinos.Positon.Y < minBounds.Y ||
                    tile.X * _unitSize + _tetrinos.Positon.X >= maxBounds.X ||
                    tile.Y * _unitSize + _tetrinos.Positon.Y >= maxBounds.Y)
                {
                    return true;
                }

                if (_tetris[(int)(tile.X * _unitSize + _tetrinos.Positon.X) / _unitSize + (int)(tile.Y * _unitSize + _tetrinos.Positon.Y) / _unitSize * _gridWidth] != TileColor.Default)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
