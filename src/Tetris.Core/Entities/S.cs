using Microsoft.Xna.Framework;
using Tetris.Core.Enums;

namespace Tetris.Core.Entities
{
    public sealed class S : ITetrinos
    {
        public S()
        {
            Piece = new Vector2[]
            {
                new Vector2(1, 0),
                new Vector2(2, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
            };
        }

        public TileColor Color => TileColor.Green;

        public Vector2[] Piece { get; set; }

        public Vector2 Positon { get; set; }

        public int Rotation { get; set; }

        public Vector2 RotationRelativePosition => Piece[3];
    }
}
