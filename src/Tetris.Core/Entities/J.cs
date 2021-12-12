using Microsoft.Xna.Framework;
using Tetris.Core.Enums;

namespace Tetris.Core.Entities
{
    public sealed class J : ITetrinos
    {
        public J()
        {
            Piece = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(2, 1),
            };
        }

        public TileColor Color => TileColor.Blue;

        public Vector2[] Piece { get; set; }

        public Vector2 Positon { get; set; }

        public int Rotation { get; set; }

        public Vector2 RotationRelativePosition => Piece[2];
    }
}
