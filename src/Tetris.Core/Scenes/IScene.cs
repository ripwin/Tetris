using Microsoft.Xna.Framework;
using System;

namespace Tetris.Core.Scenes
{
    public interface IScene : IDisposable
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
