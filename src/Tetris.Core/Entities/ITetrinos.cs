using Microsoft.Xna.Framework;
using Tetris.Core.Enums;

namespace Tetris.Core.Entities
{
    public interface ITetrinos
    {
        TileColor Color { get; }
        Vector2[] Piece { get; set; }
        Vector2 Positon { get; set; }
        int Rotation { get; set; }
        Vector2 RotationRelativePosition { get; }
        Vector2[,] Offsets =>
            new Vector2[,]
            {
                { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) },
                { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, 2), new Vector2(1, 2) },
                { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) },
                { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2) }
            };
    }
}
