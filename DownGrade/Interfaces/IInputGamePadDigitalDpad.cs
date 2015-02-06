using DownGrade;

namespace DownGrade.Framework
{
    public interface IInputGamePadDigitalDpad
    {
        void ButtonDpadDownPressed(InputController.ButtonStates buttonStates);
        void ButtonDpadUpPressed(InputController.ButtonStates buttonStates);
        void ButtonDpadLeftPressed(InputController.ButtonStates buttonStates);
        void ButtonDpadRightPressed(InputController.ButtonStates buttonStates);
    }
}
