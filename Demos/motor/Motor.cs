
using System;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;

namespace motor
{
    /// <summary>
    /// This class drives a three relays to make a reversible motor control.
    /// See the schematic for details.
    /// </summary>
    class Motor : IDisposable
    {
        private int _powerPin;
        private int[] _polarityPins;
        private GpioController _gpio;

        public Motor(int powerPin, int[] polarityPins)
        {
            _powerPin = powerPin;
            _polarityPins = polarityPins;

            _gpio = new GpioController(PinNumberingScheme.Logical, new RaspberryPi3Driver());

            this.Off();
            this.Forward();
        }

        public void On()
        {
            if (!_gpio.IsPinOpen(_powerPin))
            {
                _gpio.OpenPin(_powerPin, PinMode.Output);
            }
        }

        public void Off()
        {
            if (_gpio.IsPinOpen(_powerPin))
            {
                _gpio.ClosePin(_powerPin);
            }
        }
        public void Forward()
        {
            foreach (int p in _polarityPins)
            {
                if (_gpio.IsPinOpen(p))
                {
                    _gpio.ClosePin(p);
                }
            }
        }
        public void Reverse()
        {
            foreach (int p in _polarityPins)
            {
                if (!_gpio.IsPinOpen(p))
                {
                    _gpio.OpenPin(p, PinMode.Output);
                }
            }
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
                    _gpio.Dispose();
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
