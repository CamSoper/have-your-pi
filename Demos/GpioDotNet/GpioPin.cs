using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CamTheGeek.GpioDotNet
{
    public enum Direction { In, Out };
    public enum PinValue { Low = 0, High = 1 }


    /// <summary>
    /// This class wraps /sys/class/gpio in the Raspian OS.
    /// </summary>
    public class GpioPin : IDisposable
    {
        Task _pinWatcher;
        CancellationTokenSource _pinWatcherTokenSource;
        public int Number { get; }
        public Direction Direction { get; }
        public PinValue Value { 
            get{
                return Enum.Parse<PinValue>(File.ReadAllText($"/sys/class/gpio/gpio{Number}/value"));
            } 
            set{
                if (this.Direction == Direction.Out)
                {
                    File.WriteAllText($"/sys/class/gpio/gpio{Number}/value", ((int)value).ToString());
                }
             }    
        }

         public GpioPin(int pinNumber, Direction pinDirection, PinValue pinValue = PinValue.Low)
        {
            this.Number = pinNumber;
            this.Direction = pinDirection;

            try
            {
                // Open the pin
                File.WriteAllText("/sys/class/gpio/export", Number.ToString());
            }
            catch(System.IO.IOException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Pin already open? Stack trace: \r\n {ex}");
            }

            // Allow a few milliseconds so the pin gets opened before we set the direction
            Thread.Sleep(100);
            
            // Set pin direction
            File.WriteAllText($"/sys/class/gpio/gpio{Number}/direction", Direction.ToString().ToLower());
            
            //Set default pin value
            this.Value = pinValue;


            // If it's an OUT pin, spin up a watcher task to raise 
            // events when the value changes...
            if(this.Direction == Direction.In)
            {
                LaunchPinWatcher();
            }
        }

        
        private void LaunchPinWatcher()
        {
            _pinWatcherTokenSource = new CancellationTokenSource();
            var pinWatcherToken = _pinWatcherTokenSource.Token;
            _pinWatcher = Task.Run(() =>
            {
                var lastValue = this.Value;
                while (true)
                {
                    Thread.Sleep(10);
                    if (this.Value != lastValue)
                    {
                        lastValue = this.Value;
                        if (lastValue == PinValue.High)
                            High(this, EventArgs.Empty);
                        else
                            Low(this, EventArgs.Empty);
                    }

                    if (pinWatcherToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }, pinWatcherToken);
        }

        #region Event Handlers
        public event EventHandler High;
        public void OnHigh()
        {
            High?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Low;
        public void OnLow()
        {
            Low?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _pinWatcherTokenSource?.Cancel();
                }

                File.WriteAllText("/sys/class/gpio/unexport", Number.ToString());

                disposedValue = true;
            }
        }

        ~GpioPin()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
 

}