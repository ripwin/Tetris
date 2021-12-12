using Microsoft.Xna.Framework;

namespace Tetris.Core.Systems
{
    public interface ISystem
    {
        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
    }
}
