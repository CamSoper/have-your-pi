using System;
using System.Collections.Generic;
using System.Devices.Gpio;
using System.Text;

namespace motor
{
    class Motor : IDisposable
    {
        GpioController _controller;
        GpioPin _powerPin;
        GpioPin _polarityPin;
        
        public Motor(int powerPinNumber, int polarityPinNumber)
        {
            _controller = new GpioController(new RaspberryPiDriver());
            _powerPin = _controller.OpenPin(powerPinNumber, PinMode.Output);
            _polarityPin = _controller.OpenPin(polarityPinNumber, PinMode.Output);
        }

        public void On()
        {
            // Since the power is connected to the "Normally Open" terminal
            // (so the motor's default state is off), PinValue.Low is "On"
            _powerPin.Write(PinValue.Low);
        }

        public void Off()
        {
            _powerPin.Write(PinValue.High);
        }
        public void Forward()
        {
            _polarityPin.Write(PinValue.High);
        }
        public void Reverse()
        {
            _polarityPin.Write(PinValue.Low);
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _controller.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~Motor()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
