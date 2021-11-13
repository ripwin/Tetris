using Microsoft.Xna.Framework.Input;

namespace Tetris.Core.Utils
{
    public class Input
    {
        private KeyboardState _previousKeyboardState;
        private KeyboardState _currentKeyboardState;

        public Input()
        {
        }

        public bool IsKeyUp(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyJustPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        }

        public void Update(KeyboardState state)
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = state;
        }
    }
}
