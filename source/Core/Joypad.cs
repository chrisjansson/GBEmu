﻿namespace Core
{
    public class Joypad : IJoypad
    {
        private bool _selectButtonKeys;
        private bool _selectDirectionKeys;
        private IMmu _mmu;
        private bool _right;
        private bool _left;
        private bool _up;
        private bool _down;

        public Joypad(IMmu mmu)
        {
            _mmu = mmu;
        }

        public byte P1
        {
            get
            {
                var buttons = 0x00;
                if (_selectButtonKeys)
                {
                    buttons = buttons | (A ? 0x01 : 0x00);
                    buttons = buttons | (B ? 0x02 : 0x00);
                    buttons = buttons | (Select ? 0x04 : 0x00);
                    buttons = buttons | (Start ? 0x08 : 0x00);
                }
                if (_selectDirectionKeys)
                {
                    buttons = buttons | (Right ? 0x01 : 0x00);
                    buttons = buttons | (Left ? 0x02 : 0x00);
                    buttons = buttons | (Up ? 0x04 : 0x00);
                    buttons = buttons | (Down ? 0x08 : 0x00);
                }
                var result = _selectButtonKeys ? 0 : (1 << 5);
                result |= _selectDirectionKeys ? 0 : (1 << 4);
                result = result | (~buttons & 0x0F);
                return (byte)result;
            }
            set
            {
                _selectButtonKeys = (value & 0x20) == 0;
                _selectDirectionKeys = (value & 0x10) == 0;
            }
        }

        public bool Right
        {
            get { return _right; }
            set
            {
                if (!_right && value)
                    RaiseInterrupt();
                _right = value;
            }
        }

        public bool Left
        {
            get { return _left; }
            set
            {
                if (!_left && value)
                    RaiseInterrupt();
                _left = value;
            }
        }

        public bool Up
        {
            get { return _up; }
            set
            {
                if (!_up && value)
                    RaiseInterrupt();
                _up = value;
            }
        }

        public bool Down
        {
            get { return _down; }
            set
            {
                if (!_down && value)
                    RaiseInterrupt();
                _down = value;
            }
        }

        private void RaiseInterrupt()
        {
            var interruptRequest = _mmu.GetByte(RegisterAddresses.IF);
            interruptRequest |= 0x10;
            _mmu.SetByte(RegisterAddresses.IF, interruptRequest);
        }

        public bool A { get; set; }
        public bool B { get; set; }
        public bool Select { get; set; }
        public bool Start { get; set; }
    }
}