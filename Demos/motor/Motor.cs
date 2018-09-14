using CamTheGeek.GpioDotNet;
using System;

namespace motor
{
    /// <summary>
    /// This class drives a three relays to make a reversible motor control.
    /// See the schematic for details.
    /// </summary>
    class Motor : IDisposable
    {
        private GpioPin _powerPin;
        private GpioPin _polarityPin;
        
        public Motor(int powerPinNumber, int polarityPinNumber)
        {
            _powerPin = new GpioPin(powerPinNumber, Direction.Out, PinValue.High);
            _polarityPin = new GpioPin(polarityPinNumber, Direction.Out, PinValue.High);

            this.Off();
            this.Forward();
        }

        public void On()
        {
            _powerPin.Value = PinValue.Low;
        }

        public void Off()
        {
            _powerPin.Value = PinValue.High;
        }
        public void Forward()
        {
            _polarityPin.Value = PinValue.High;
        }
        public void Reverse()
        {
            _polarityPin.Value = PinValue.Low;
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
                    _powerPin.Dispose();
                    _polarityPin.Dispose();
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
