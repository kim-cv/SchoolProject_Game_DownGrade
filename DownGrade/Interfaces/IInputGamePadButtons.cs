using DownGrade;

namespace DownGrade.Framework
{
    public interface IInputGamePadButtons
    {
        void ButtonADown(InputController.ButtonStates buttonStates);
        void ButtonBDown(InputController.ButtonStates buttonStates);
        void ButtonXDown(InputController.ButtonStates buttonStates);
        void ButtonYDown(InputController.ButtonStates buttonStates);
        void ButtonLeftShoulderDown(InputController.ButtonStates buttonStates);
        void ButtonRightShoulderDown(InputController.ButtonStates buttonStates);
        void ButtonStartDown(InputController.ButtonStates buttonStates);
        void ButtonBackDown(InputController.ButtonStates buttonStates);
        void ButtonLeftStickDown(InputController.ButtonStates buttonStates);
        void ButtonRightStickDown(InputController.ButtonStates buttonStates);
        
    }
}
