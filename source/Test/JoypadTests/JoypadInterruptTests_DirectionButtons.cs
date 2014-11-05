using System;
using Core;
using Test.CpuTests.Interrupts;
using Xunit;

namespace Test.JoypadTests
{
    public class JoypadInterruptTests_DirectionButtons
    {
        [Fact]
        public void Should_raise_joypad_interrupt_when_input_is_pressed()
        {
            AssertInterruptIsRaised(x => x.Right = true);
            AssertInterruptIsRaised(x => x.Left = true);
            AssertInterruptIsRaised(x => x.Up = true);
            AssertInterruptIsRaised(x => x.Down = true);
        }

        [Fact]
        public void Should_not_raise_interrupt_if_input_is_already_high()
        {
            AssertInterruptIsNotRaised(x => x.Right = true);
            AssertInterruptIsNotRaised(x => x.Left = true);
            AssertInterruptIsNotRaised(x => x.Up = true);
            AssertInterruptIsNotRaised(x => x.Down = true);
        }

        [Fact]
        public void Should_not_raise_interrupt_if_buttons_are_selected()
        {
            AssertInterruptIsNotRaisedWhenButtonsAreSelected(x => x.Right = true);
            AssertInterruptIsNotRaisedWhenButtonsAreSelected(x => x.Left = true);
            AssertInterruptIsNotRaisedWhenButtonsAreSelected(x => x.Up = true);
            AssertInterruptIsNotRaisedWhenButtonsAreSelected(x => x.Down = true);
        }

        private void AssertInterruptIsNotRaisedWhenButtonsAreSelected(Action<Joypad> act)
        {
            var fakeMmu = new FakeMmu();
            var sut = new Joypad(fakeMmu);
            sut.P1 = 0x10;

            act(sut);

            var actual = fakeMmu.Memory[RegisterAddresses.IF];
            Assert.Equal(0, actual);
        }

        private void AssertInterruptIsNotRaised(Action<Joypad> act)
        {
            var fakeMmu = new FakeMmu();
            var sut = new Joypad(fakeMmu);
            sut.P1 = 0x20;

            act(sut);
            fakeMmu.Memory[RegisterAddresses.IF] = 0;
            act(sut);

            var actual = fakeMmu.Memory[RegisterAddresses.IF];
            Assert.Equal(0, actual);
        }

        private void AssertInterruptIsRaised(Action<Joypad> act)
        {
            var fakeMmu = new FakeMmu();
            var sut = new Joypad(fakeMmu);
            sut.P1 = 0x20;

            act(sut);

            var actual = fakeMmu.Memory[RegisterAddresses.IF];
            Assert.Equal(Interrupt.JoyPad.InterruptMask, actual);
        }
    }
}