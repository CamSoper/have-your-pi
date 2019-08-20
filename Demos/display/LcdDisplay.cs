using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;
using System;
using System.Device.I2c;
using System.Threading;
using System.Threading.Tasks;

namespace display
{
    public class LcdDisplay : IDisposable
    {
        I2cDevice _i2c;
        Pcf8574 _ioExpander;
        Lcd1602 _lcd;

        public LcdDisplay(I2cDevice i2c)
        {
            _i2c = i2c;
            _ioExpander = new Pcf8574(_i2c);
            _lcd = new Lcd1602(registerSelectPin: 0, enablePin: 2, dataPins: new int[] { 4, 5, 6, 7 }, backlightPin: 3, readWritePin: 1, controller: _ioExpander);
        }

        public async Task DisplayText(string text)
        {
            int screenWidth = 16;
            string padding = new string(' ', screenWidth);
            string paddedText = $"{padding}{text}{padding}";

                        _lcd.Clear();
            for (int i = 0; i <= (text.Length + screenWidth); i++)
            {
                string frame = paddedText.Substring(i, screenWidth);
                _lcd.SetCursorPosition(0, 0);
                _lcd.Write(frame);
                await Task.Delay(TimeSpan.FromMilliseconds(250));
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
                    _lcd.Dispose();
                    _ioExpander.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Display()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

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
