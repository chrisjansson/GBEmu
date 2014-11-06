using System;
using Core;
using Test.CpuTests.Interrupts;
using Xunit;

namespace Test.JoypadTests
{
    public class JoypadInterruptTests_Buttons
    {
        [Fact]
        public void Should_raise_joypad_interrupt_when_input_is_pressed()
        {
            AssertInterruptIsRaised(x => x.A = true);
            AssertInterruptIsRaised(x => x.B = true);
            AssertInterruptIsRaised(x => x.Select = true);
            AssertInterruptIsRaised(x => x.Start = true);
        }

        [Fact]
        public void Should_not_raise_interrupt_if_input_is_already_high()
        {
            AssertInterruptIsNotRaised(x => x.A = true);
            AssertInterruptIsNotRaised(x => x.B = true);
            AssertInterruptIsNotRaised(x => x.Select = true);
            AssertInterruptIsNotRaised(x => x.Start = true);
        }

        [Fact]
        public void Should_not_raise_interrupt_if_input_is_reset()
        {
            AssertInterruptIsNotRaised(x => x.A = false);
            AssertInterruptIsNotRaised(x => x.B = false);
            AssertInterruptIsNotRaised(x => x.Select = false);
            AssertInterruptIsNotRaised(x => x.Start = false);
        }

        [Fact]
        public void Should_not_raise_interrupt_if_buttons_are_selected()
        {
            AssertInterruptIsNotRaisedWhenButtonsAreSelected(x => x.A = true);
            AssertInterruptIsNotRaisedWhenButtonsAreSelected(x => x.B = true);
            AssertInterruptIsNotRaisedWhenButtonsAreSelected(x => x.Select = true);
            AssertInterruptIsNotRaisedWhenButtonsAreSelected(x => x.Start = true);
        }

        private void AssertInterruptIsNotRaisedWhenButtonsAreSelected(Action<Joypad> act)
        {
            var fakeMmu = new FakeMmu();
            var sut = new Joypad(fakeMmu);
            sut.P1 = 0x20;

            act(sut);

            var actual = fakeMmu.Memory[RegisterAddresses.IF];
            Assert.Equal(0, actual);
        }

        private void AssertInterruptIsNotRaised(Action<Joypad> act)
        {
            var fakeMmu = new FakeMmu();
            var sut = new Joypad(fakeMmu);
            sut.P1 = 0x10;

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
            sut.P1 = 0x10;

            act(sut);

            var actual = fakeMmu.Memory[RegisterAddresses.IF];
            Assert.Equal(Interrupt.JoyPad.InterruptMask, actual);
        }
    }
}