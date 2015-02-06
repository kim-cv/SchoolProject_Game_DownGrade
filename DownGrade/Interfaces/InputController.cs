using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using DownGrade.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DownGrade
{
    public class InputController : IUpdateable
    {
        
        private float _analogStickDeadZone = 0.05f;
        private PlayerIndex _playerIndex;
        private GamePadState _gamePadState;

        // used to remember last state of the buttons
        private bool _buttonADown = false;
        private bool _buttonBDown = false;
        private bool _buttonXDown = false;
        private bool _buttonYDown = false;
        private bool _buttonLeftShoulderDown = false;
        private bool _buttonRightShoulderDown = false;
        private bool _buttonStartDown = false;
        private bool _buttonBackDown = false;
        private bool _buttonLeftStickDown = false;
        private bool _buttonRightStickDown = false;
        private bool _buttonDpadDown = false;
        private bool _buttonDpadUp = false;
        private bool _buttonDpadLeft = false;
        private bool _buttonDpadRight = false;
        private List<IInputGamePadLeftStick> _inputGamePadLeftStickListeners;
        private List<IInputGamePadRightStick> _inputGamePadRightStickListeners;
        private List<IInputGamePadButtons> _inputGamePadButtonListeners;
        private List<IInputGamePadDigitalDpad> _inputGamePadDigitalDpadListeners;
        private List<IInputGamePadAnalogTriggers> _inputGamePadAnalogTriggerListeners;
        
       public float AnalogStickDeadZone
        {
            get { return _analogStickDeadZone; }
            set { _analogStickDeadZone = value; }
        }

        // The two states your pressed button can have;
        public enum ButtonStates
        {
            JustPressed,
            StillPressed
        }

        public enum Controller
        {
            One,
            Two,
            Three,
            Four
        }

        public List<IInputGamePadLeftStick> InputGamePadLeftStickListeners
        {
            get { return _inputGamePadLeftStickListeners; }
            set { _inputGamePadLeftStickListeners = value; }
        }

        public List<IInputGamePadRightStick> InputGamePadRightStickListeners
        {
            get { return _inputGamePadRightStickListeners; }
            set { _inputGamePadRightStickListeners = value; }
        }
        
        public List<IInputGamePadButtons> InputGamePadButtonListeners
        {
            get { return _inputGamePadButtonListeners; }
            set { _inputGamePadButtonListeners = value; }
        }

        public List<IInputGamePadDigitalDpad> InputGamePadDigitalDpadListeners
        {
            get { return _inputGamePadDigitalDpadListeners; }
            set { _inputGamePadDigitalDpadListeners = value; }
        }

        public List<IInputGamePadAnalogTriggers> InputGamePadAnalogTriggerListeners
        {
            get { return _inputGamePadAnalogTriggerListeners; }
            set { _inputGamePadAnalogTriggerListeners = value; }
        }

        private void CheckRightStick()
        {
            if (Math.Abs(_gamePadState.ThumbSticks.Right.X) > _analogStickDeadZone ||
                Math.Abs(_gamePadState.ThumbSticks.Right.Y) > _analogStickDeadZone)
            {
                foreach (var inputGamePadRightStickListener in InputGamePadRightStickListeners)
                {
                    inputGamePadRightStickListener.RightStickMove(_gamePadState.ThumbSticks.Right);
                }
            }
        }

        private void CheckLeftStick()
        {
            if (Math.Abs(_gamePadState.ThumbSticks.Left.X) > _analogStickDeadZone ||
                Math.Abs(_gamePadState.ThumbSticks.Left.Y) > _analogStickDeadZone)
            {
                foreach (var inputGamePadLeftStickListener in InputGamePadLeftStickListeners)
                {
                    inputGamePadLeftStickListener.LeftStickMove(_gamePadState.ThumbSticks.Left);
                }
            }
        }

        private void CheckDigitalTriggers()
        {
            if (_gamePadState.Triggers.Left > 0) TriggerLeftPressed(_gamePadState.Triggers.Left);
            if (_gamePadState.Triggers.Right > 0) TriggerRightPressed(_gamePadState.Triggers.Right);
        }

        private void TriggerRightPressed(float pressure)
        {
            foreach (var inputGamePadDigitalTriggerListener in InputGamePadAnalogTriggerListeners)
            {
                inputGamePadDigitalTriggerListener.RightTriggerPressed(pressure);
            }
        }

        private void TriggerLeftPressed(float pressure)
        {
            foreach (var inputGamePadDigitalTriggerListener in InputGamePadAnalogTriggerListeners)
            {
                inputGamePadDigitalTriggerListener.LeftTriggerPressed(pressure);
            }
        }

        //used inside the update method
        private void CheckButtons()
        {
            if (_gamePadState.IsButtonDown(Buttons.A) && _buttonADown) ButtonADown(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.A))
            {
                ButtonADown(ButtonStates.JustPressed);
                _buttonADown = true;
            }
            else _buttonADown = false;

            if (_gamePadState.IsButtonDown(Buttons.B) && _buttonBDown) ButtonBDown(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.B))
            {
                ButtonBDown(ButtonStates.JustPressed);
                _buttonBDown = true;
            }
            else _buttonADown = false;

            if (_gamePadState.IsButtonDown(Buttons.X) && _buttonADown) ButtonXDown(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.X))
            {
                ButtonXDown(ButtonStates.JustPressed);
                _buttonXDown = true;
            }
            else _buttonXDown = false;

            if (_gamePadState.IsButtonDown(Buttons.Y) && _buttonYDown) ButtonYDown(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.Y))
            {
                ButtonYDown(ButtonStates.JustPressed);
                _buttonYDown = true;
            }
            else _buttonADown = false;

            if (_gamePadState.IsButtonDown(Buttons.LeftShoulder) && _buttonLeftShoulderDown) ButtonShoulderLeftDown(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.LeftShoulder))
            {
                ButtonShoulderLeftDown(ButtonStates.JustPressed);
                _buttonLeftShoulderDown = true;
            }
            else _buttonADown = false;

            if (_gamePadState.IsButtonDown(Buttons.RightShoulder) && _buttonRightShoulderDown) ButtonShoulderRightDown(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.RightShoulder))
            {
                ButtonShoulderRightDown(ButtonStates.JustPressed);
                _buttonRightShoulderDown = true;
            }
            else _buttonRightShoulderDown = false;

            if (_gamePadState.IsButtonDown(Buttons.Start) && _buttonRightShoulderDown) ButtonStartDown(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.Start))
            {
                ButtonStartDown(ButtonStates.JustPressed);
                _buttonStartDown = true;
            }
            else _buttonRightShoulderDown = false;

            if (_gamePadState.IsButtonDown(Buttons.Back) && _buttonRightShoulderDown) ButtonBackDown(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.Back))
            {
                ButtonBackDown(ButtonStates.JustPressed);
                _buttonBackDown = true;
            }
            else _buttonBackDown = false;

            if (_gamePadState.IsButtonDown(Buttons.LeftStick) && _buttonRightShoulderDown) ButtonLeftStickDown(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.LeftStick))
            {
                ButtonLeftStickDown(ButtonStates.JustPressed);
                _buttonLeftStickDown = true;
            }
            else _buttonBackDown = false;

            if (_gamePadState.IsButtonDown(Buttons.RightStick) && _buttonRightShoulderDown) ButtonRightStickDown(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.RightStick))
            {
                ButtonRightStickDown(ButtonStates.JustPressed);
                _buttonRightStickDown = true;
            }
            else _buttonRightStickDown = false;                  
        }

        private void ButtonRightStickDown(ButtonStates buttonStates)
        {
            
        }

        private void ButtonLeftStickDown(ButtonStates buttonStates)
        {
            foreach (var inputGamePadButtonListener in InputGamePadButtonListeners)
            {
                inputGamePadButtonListener.ButtonLeftStickDown(buttonStates);
            }
        }

        private void ButtonBackDown(ButtonStates buttonStates)
        {
            foreach (var inputGamePadButtonListener in InputGamePadButtonListeners)
            {
                inputGamePadButtonListener.ButtonBackDown(buttonStates);
            }
        }

        private void ButtonStartDown(ButtonStates buttonStates)
        {
            foreach (var inputGamePadButtonListener in InputGamePadButtonListeners)
            {
                inputGamePadButtonListener.ButtonStartDown(buttonStates);
            }
        }

        private void ButtonShoulderRightDown(ButtonStates buttonStates)
        {
            foreach (var inputGamePadButtonListener in InputGamePadButtonListeners)
            {
                inputGamePadButtonListener.ButtonRightStickDown(buttonStates);
            }
        }

        private void ButtonShoulderLeftDown(ButtonStates buttonStates)
        {
            foreach (var inputGamePadButtonListener in InputGamePadButtonListeners)
            {
                inputGamePadButtonListener.ButtonLeftShoulderDown(buttonStates);
            }
        }

        private void ButtonYDown(ButtonStates buttonStates)
        {
            foreach (var inputGamePadButtonListener in InputGamePadButtonListeners)
            {
               inputGamePadButtonListener.ButtonYDown(buttonStates); 
            }
        }

        private void ButtonXDown(ButtonStates buttonStates)
        {
            foreach (var inputGamePadButtonListener in InputGamePadButtonListeners)
            {
                inputGamePadButtonListener.ButtonXDown(buttonStates);
            }
        }

        private void ButtonBDown(ButtonStates buttonStates)
        {
            foreach (var inputGamePadButtonListener in InputGamePadButtonListeners)
            {
                inputGamePadButtonListener.ButtonBDown(buttonStates);
            }
        }

        private void ButtonADown(ButtonStates buttonStates)
        {
            foreach (var inputGamePadButtonListener in InputGamePadButtonListeners)
            {
                inputGamePadButtonListener.ButtonADown(buttonStates);
            }
        }

        //used inside the update method
        private void CheckDpad()
        {
            //if (gamePadState.DPad.Down == ButtonState.Pressed) DpadDown(playerIndex, ButtonStates.JustPressed);
            
            if (_gamePadState.IsButtonDown(Buttons.DPadDown) && _buttonDpadDown) DpadDown(ButtonStates.StillPressed); 
            else if (_gamePadState.IsButtonDown(Buttons.DPadDown))
            {
                DpadDown(ButtonStates.JustPressed);
                _buttonDpadDown = true;
            }
            else _buttonDpadDown = false;

            if (_gamePadState.IsButtonDown(Buttons.DPadUp) && _buttonDpadUp) DpadUp(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.DPadUp))
            {
                DpadUp(ButtonStates.JustPressed);
                _buttonDpadUp = true;
            }
            else _buttonDpadUp = false;

            if (_gamePadState.IsButtonDown(Buttons.DPadLeft) && _buttonDpadLeft) DpadLeft(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.DPadLeft))
            {
                DpadLeft(ButtonStates.JustPressed);
                _buttonDpadLeft = true;
            }
            else _buttonDpadLeft = false;

            if (_gamePadState.IsButtonDown(Buttons.DPadRight) && _buttonDpadRight) DpadRight(ButtonStates.StillPressed);
            else if (_gamePadState.IsButtonDown(Buttons.DPadRight))
            {
                DpadRight(ButtonStates.JustPressed);
                _buttonDpadRight = true;
            }
            else _buttonDpadRight = false;

                      
        }

        private void DpadRight(ButtonStates buttonStates)
        {
            foreach (var inputGamePadDigitalDpadListener in InputGamePadDigitalDpadListeners)
            {
                inputGamePadDigitalDpadListener. ButtonDpadRightPressed(buttonStates);
            }
        }

        private void DpadDown(ButtonStates buttonStates)
        {
            foreach (var inputGamePadDigitalDpadListener in InputGamePadDigitalDpadListeners)
            {
                inputGamePadDigitalDpadListener.ButtonDpadDownPressed(buttonStates);
            }
        }

        private void DpadLeft(ButtonStates buttonStates)
        {
            foreach (var inputGamePadDigitalDpadListener in InputGamePadDigitalDpadListeners)
            {
                inputGamePadDigitalDpadListener.ButtonDpadLeftPressed(buttonStates);
            }
        }

        private void DpadUp(ButtonStates buttonStates)
        {
            foreach (var inputGamePadDigitalDpadListener in InputGamePadDigitalDpadListeners)
            {
                inputGamePadDigitalDpadListener.ButtonDpadUpPressed(buttonStates);
            }
        }

        //Constructor - check for number of gamepads
        public InputController(PlayerIndex playerIndex)
        {
            _playerIndex = playerIndex;
           
            _inputGamePadButtonListeners = new List<IInputGamePadButtons>();
            _inputGamePadDigitalDpadListeners = new List<IInputGamePadDigitalDpad>();
            _inputGamePadAnalogTriggerListeners = new List<IInputGamePadAnalogTriggers>();
            _inputGamePadLeftStickListeners = new List<IInputGamePadLeftStick>();
            _inputGamePadRightStickListeners = new List<IInputGamePadRightStick>();
        }

        public InputController(PlayerIndex playerIndex, float analogStickDeadZone ) : this(playerIndex)
        {
            _analogStickDeadZone = analogStickDeadZone;
        }

       // implemented by Iupdateable
        public void Update(GameTime gameTime)
        {
            
            _gamePadState = GamePad.GetState(_playerIndex);
            if (InputGamePadDigitalDpadListeners.Count > 0) CheckDpad();
            if (InputGamePadButtonListeners.Count > 0) CheckButtons();
            if (InputGamePadAnalogTriggerListeners.Count > 0) CheckDigitalTriggers();
            if (InputGamePadLeftStickListeners.Count > 0) CheckLeftStick();
            if (InputGamePadRightStickListeners.Count > 0) CheckRightStick();
        }

       public bool Enabled { get; private set; }
        public int UpdateOrder { get; private set; }
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
    }
}
