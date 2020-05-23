using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using Mohammad.EventsArgs;
using Mohammad.Globalization;
using Mohammad.Helpers;
using Timer = System.Timers.Timer;

namespace Mohammad.IO
{
    public sealed class PrinterOlivettiPr4Sl : IDisposable
    {
        private bool _DontQueryStatus;
        private Encoding _Encoding;
        private TextWriter _Log;

        private SerialPort _SerialPort;
        private PrinterStatus _Status;
        private Timer _Timer;
        private const int WAIT_FOR_QUERY_STATUS_TIMEOUT_MILLISECONDS = 1200;
        public static ComPortPrinterCharacterSize DefaultCharacterSize = ComPortPrinterCharacterSize.Normal;
        public static ComPortPrinterCharacterCodeTable DefaultCharacterCodeTable = ComPortPrinterCharacterCodeTable.USA;
        public TextWriter Log { get { return this._Log ?? TextWriter.Null; } set { this._Log = value; } }
        public string LastMessage { get; private set; }
        public Encoding Encoding { get { return this._Encoding ?? (this._Encoding = Encoding.UTF8); } set { this._Encoding = value; } }
        public bool? CanConvertToPersian { get; set; }
        public bool CanThrowExceptions { get; set; }
        public Exception LastException { get; private set; }
        public bool HasException => this.LastException != null;
        public bool IsDebugMode { get; set; }
        public bool IsStateless { get; set; }
        public bool IsDisposed { get; private set; }
        public string Name { get; set; }
        private bool IsStatusRequested { get; set; }

        public PrinterStatus Status
        {
            get { return this._Status; }
            private set
            {
                if (this._Status == value)
                    return;
                this._Status = value;
                this.OnStatusChanged();
            }
        }

        public PrinterOlivettiPr4Sl(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One,
            int readTimeout = 750, int writeTimeout = 750)
        {
            this._SerialPort = new SerialPort
                               {
                                   PortName = portName,
                                   BaudRate = baudRate,
                                   Parity = parity,
                                   DataBits = dataBits,
                                   StopBits = stopBits,
                                   ReadTimeout = readTimeout,
                                   WriteTimeout = writeTimeout
                               };
            this.Initialize();
        }

        public event EventHandler StatusChanged;
        public event EventHandler<ItemActedEventArgs<string>> DataReceived;
        public event EventHandler<ItemActingEventArgs<Exception>> ExceptionOccurred;

        private void Initialize()
        {
            this.Name = this.GetType().Name;
            this.IsDebugMode = false;
            this.CanConvertToPersian = true;
            this.CanThrowExceptions = true;
            this._SerialPort.Open();
            this._SerialPort.DataReceived += this.SerialPort_OnDataReceived;
            this.InitializePrinter();
            this.SetCharacterSize(DefaultCharacterSize);
            this.SetCharacterCodeTable(DefaultCharacterCodeTable);
            this.SetLeftMargin();
            this.RealtimeStatusTransmission();
        }

        public void InitializePrinter() { this.SendCommand(ComPortPrinterCommands.InitializePrinter); }

        private void SerialPort_OnDataReceived(object sender, SerialDataReceivedEventArgs e) { this.CheckDataReceived(); }

        public void SetCharacterSize(ComPortPrinterCharacterSize value) { this.SendCommand(ComPortPrinterCommands.SelectCharacterSize, Convert.ToByte(value)); }

        public void SetCharacterCodeTable(ComPortPrinterCharacterCodeTable value)
        {
            this.SendCommand(ComPortPrinterCommands.SelectCharacterCodeTable, Convert.ToByte(value));
        }

        public void SetJustification(ComPortPrinterJustification value)
        {
            switch (value)
            {
                case ComPortPrinterJustification.Left:
                    this.SendCommand(ComPortPrinterCommands.SelectJustification, 0, 48);
                    break;
                case ComPortPrinterJustification.Center:
                    this.SendCommand(ComPortPrinterCommands.SelectJustification, 1, 49);
                    break;
                case ComPortPrinterJustification.Right:
                    this.SendCommand(ComPortPrinterCommands.SelectJustification, 2, 50);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public void SetInternationalCharacterSet(ComPortPrinterInternationalCharacterSet value)
        {
            this.SendCommand(ComPortPrinterCommands.SelectInternationalCharacterSet, Convert.ToByte(value));
        }

        public void PrintLine(params string[] lines)
        {
            this._DontQueryStatus = true;
            this.Do(() =>
                {
                    try
                    {
                        if (!lines.Any())
                        {
                            this.UnsafeSendCommand(ComPortPrinterCommands.NewLine);
                            return;
                        }
                        this._SerialPort.Encoding = this.Encoding;
                        foreach (var line in lines)
                        {
                            byte[] bytes;
                            if (this.CanConvertToPersian.HasValue)
                                bytes = this.CanConvertToPersian.Value ? PersianAlphabetConverter.Convert(line) : this.Encoding.GetBytes(line);
                            else
                                bytes = line.HasPersian() ? PersianAlphabetConverter.Convert(line) : this.Encoding.GetBytes(line);
                            this._SerialPort.Write(bytes, 0, bytes.Length);
                            this.UnsafeSendCommand(ComPortPrinterCommands.NewLine);
                        }
                    }
                    finally
                    {
                        this._DontQueryStatus = false;
                    }
                },
                "Printing");
        }

        public void Print(string line)
        {
            this._DontQueryStatus = true;
            this.Do(() =>
                {
                    try
                    {
                        this._SerialPort.Encoding = this.Encoding;
                        byte[] bytes;
                        if (this.CanConvertToPersian.HasValue)
                            bytes = this.CanConvertToPersian.Value ? PersianAlphabetConverter.Convert(line) : this.Encoding.GetBytes(line);
                        else
                            bytes = line.HasPersian() ? PersianAlphabetConverter.Convert(line) : this.Encoding.GetBytes(line);
                        this._SerialPort.Write(bytes, 0, bytes.Length);
                    }
                    finally
                    {
                        this._DontQueryStatus = false;
                    }
                },
                "Printing");
        }

        public void PrintLine(IEnumerable<string> lines) { this.InnerDoOnPaperReady(() => this.PrintLine(lines)); }

        public void PrintLineOnPaperReady(params string[] lines)
        {
            if (lines != null)
                this.InnerDoOnPaperReady(() => this.PrintLine(lines));
        }

        private void InnerDoOnPaperReady(Action action, bool autoResetCheckStatusTimer = true, TimeSpan? timeout = null)
        {
            this.RealtimeStatusTransmission();
            if (this.Status == PrinterStatus.PaperReady)
            {
                this.HandleException(action);
                return;
            }
            EventHandler onStatusChanged;
            using (var statusChangeWaitHandle = new AutoResetEvent(false))
            {
                onStatusChanged = (_, __) =>
                {
                    if (this.Status == PrinterStatus.PaperReady)
                        // ReSharper disable once AccessToDisposedClosure
                        statusChangeWaitHandle.Set();
                };
                this.StatusChanged += onStatusChanged;
                if (autoResetCheckStatusTimer)
                    this.StartCheckStatusTimer();
                if (timeout != null)
                    statusChangeWaitHandle.WaitOne(timeout.Value);
                else
                    statusChangeWaitHandle.WaitOne();
            }
            if (autoResetCheckStatusTimer)
                this.StopCheckStatusTimer();
            this.StatusChanged -= onStatusChanged;
            this.HandleException(action);
        }

        public void DoEachOnPaperReady(Action<int> action, int count) { this.DoEachOnPaperReady(Enumerable.Repeat(action, count).ToArray()); }

        //TODO: Work on this. This is so error-prone.
        public void DoEachOnPaperReady(params Action<int>[] actions)
        {
            this.StartCheckStatusTimer();
            for (var index = 0; index < actions.Length; index++)
            {
                var action = actions[index];
                var index1 = index;
                this.InnerDoOnPaperReady(() => action(index1), false);
                Thread.Sleep(WAIT_FOR_QUERY_STATUS_TIMEOUT_MILLISECONDS);
            }
            this.StopCheckStatusTimer();
        }

        private void HandleException(Action action)
        {
            this.HandleException(() =>
            {
                action();
                return true;
            });
        }

        private TResult HandleException<TResult>(Func<TResult> action, TResult defaultResult = default(TResult))
        {
            try
            {
                this.LastException = null;
                return action();
            }
            catch (TimeoutException ex)
            {
                this.LogEvent("Timeout exception while printing check (natural exception): {0}", ex.GetBaseException().Message);
                return defaultResult;
            }
            catch (Exception ex)
            {
                this.OnExceptionOccurred(ex);
                return defaultResult;
            }
        }

        public void EjectPaper() { this.SendCommand(ComPortPrinterCommands.PaperEject); }
        public void FeedLine(byte lineCount = 1) { this.SendCommand(ComPortPrinterCommands.PrintAndFeedLines, lineCount); }
        public void FeedLinesReverse(byte lineCount = 1) { this.SendCommand(ComPortPrinterCommands.PrintAndReverseFeedLines, lineCount); }
        public void SetLeftMargin(byte l = 0, byte h = 0) { this.SendCommand(ComPortPrinterCommands.SetLeftMargin, l, h); }
        public void SetSlipPaperWaitingTime(byte t1 = 0, byte t2 = 5) { this.SendCommand(ComPortPrinterCommands.SetSlipPaperWaitingTime, t1, t2); }

        private void SendCommand(IEnumerable<byte> command, params byte[] args) { this.Do(() => this.UnsafeSendCommand(command, args), null); }

        private void UnsafeSendCommand(IEnumerable<byte> command, params byte[] args)
        {
            var cmd = new List<byte>(command);
            cmd.AddRange(args);
            var buffer = cmd.ToArray();
            this._SerialPort.Write(buffer, 0, buffer.Length);
        }

        private void Do(Action action, string eventName)
        {
            this.Do(() =>
                {
                    action();
                    return true;
                },
                eventName);
        }

        private TResult Do<TResult>(Func<TResult> action, string eventName, TResult defaultResult = default(TResult))
        {
            lock (this._SerialPort)
            {
                if (this.IfDisposed())
                    return defaultResult;
                if (!string.IsNullOrEmpty(eventName) && this.IsDebugMode)
                    this.LogEvent(eventName);
                var wasOpen = false;
                try
                {
                    return this.HandleException(() =>
                        {
                            wasOpen = this._SerialPort.IsOpen;
                            if (!wasOpen)
                                this._SerialPort.Open();
                            return action();
                        },
                        defaultResult);
                }
                finally
                {
                    if (!wasOpen)
                        if (this.IsStateless)
                            this._SerialPort?.Close();
                    if (!string.IsNullOrEmpty(eventName) && this.IsDebugMode)
                        this.LogEvent("{0} done", eventName);
                    GC.Collect();
                }
            }
        }

        private bool IfDisposed()
        {
            if (!this.IsDisposed)
                return false;
            this.OnExceptionOccurred(new ObjectDisposedException("ComPortPrinter"));
            return true;
        }

        private void OnExceptionOccurred(Exception ex)
        {
            if (ex.Message.StartsWith("The I/O operation has been aborted because of either a thread exit or an application request."))
            {
                this.Dispose();
                return;
            }
            this.LastException = ex;
            this.LogEvent("Exception: {0}", ex.GetBaseException().Message);
            if (this.ExceptionOccurred != null)
            {
                var e = new ItemActingEventArgs<Exception>(ex);
                this.ExceptionOccurred(this, e);
                if (e.Handled)
                    return;
            }
            if (this.CanThrowExceptions)
                throw ex;
        }

        private void LogEvent(string log)
        {
            Debug.WriteLine($"[{this.Name}]: {log}");
            this._Log.WriteLine(log);
        }

        private void LogEvent(string format, params object[] args) { this.LogEvent(string.Format(format, args)); }

        public static bool TryDetectPrinter(out string portName, TextWriter log = null)
        {
            portName = null;
            foreach (var prtName in SerialPort.GetPortNames())
                try
                {
                    var printer = new PrinterOlivettiPr4Sl(prtName);
                    printer.InitializePrinter();
                    portName = prtName;
                    return true;
                }
                catch (Exception ex)
                {
                    log?.WriteLine($"Exception: {ex.GetBaseException().Message}");
                }
            return false;
        }

        public string ReadExisting() { return this.Do(() => this._SerialPort.ReadExisting(), "ReadExisting"); }

        public byte[] Read()
        {
            return this.Do(() =>
                {
                    var result = new byte[this._SerialPort.ReadBufferSize];
                    this._SerialPort.Read(result, 0, result.Length);
                    return result.Where(element => element != '\0').ToArray();
                },
                "Read");
        }

        public string ReadLine() { return this.Do(() => this._SerialPort.ReadLine(), "ReadLine"); }

        public void StartCheckStatusTimer(int interval = 1000)
        {
            this.StopCheckStatusTimer();
            this._Timer = new Timer(interval);
            this._Timer.Elapsed += (_, __) => this.RealtimeStatusTransmission();
            this._Timer.Start();
        }

        private void CheckDataReceived()
        {
            if (this.IsDisposed || this._SerialPort == null)
                return;
            if (this._DontQueryStatus)
                return;
            this._DontQueryStatus = true;
            lock (this._SerialPort)
            {
                try
                {
                    if (!this._SerialPort.IsOpen)
                        return;
                    var buffer = new byte[this._SerialPort.ReadBufferSize];
                    if (this.IsStatusRequested)
                        try
                        {
                            this.IsStatusRequested = false;
                            this._SerialPort.Read(buffer, 0, buffer.Length);
                            var read = buffer.Where(element => element != '\0').ToArray();
                            if (read.Any())
                            {
                                var status = read.First();
                                // ReSharper disable InconsistentNaming
                                const byte TofNoPaper = 32;
                                const byte BofNoPaper = 64;
                                // ReSharper restore InconsistentNaming
                                var hasPaper = (status & TofNoPaper) != TofNoPaper && (status & BofNoPaper) != BofNoPaper;
                                this.Status = hasPaper ? PrinterStatus.PaperReady : PrinterStatus.NoPaper;
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.LogEvent("{0} Exception occurred while receiving data from check printer: {1}", ex.GetBaseException().Message);
                        }
                    var readExisting = this._SerialPort.ReadExisting();
                    if (!string.IsNullOrEmpty(readExisting))
                    {
                        if (readExisting == this.LastMessage)
                            return;
                        this.LastMessage = readExisting;
                        this.OnDataReceived(this, readExisting);
                        return;
                    }

                    var readLine = this._SerialPort.ReadLine();
                    if (string.IsNullOrEmpty(readLine))
                        return;
                    if (readLine == this.LastMessage)
                        return;
                    this.LastMessage = readLine;
                    this.OnDataReceived(this, readLine);
                }
                catch (Exception ex)
                {
                    this.LogEvent("{0} Exception occurred while receiving data from check printer: {1}", ex.GetBaseException().Message);
                }
                finally
                {
                    this._DontQueryStatus = false;
                }
            }
        }

        public void RealtimeStatusTransmission()
        {
            this.IsStatusRequested = true;
            this.SendCommand(ComPortPrinterCommands.RealtimeStatusTransmission, 5);
            Thread.Sleep(WAIT_FOR_QUERY_STATUS_TIMEOUT_MILLISECONDS);
        }

        private void OnDataReceived(object sender, string message)
        {
            if (this.IsDebugMode)
                this.LogEvent("Data Received: {0}", message);
            this.DataReceived?.Invoke(sender, new ItemActedEventArgs<string>(message));
        }

        public void StopCheckStatusTimer()
        {
            if (this._Timer == null)
                return;
            this._Timer.Stop();
            this._Timer.Dispose();
            this._Timer = null;
        }

        ~PrinterOlivettiPr4Sl() { this.Dispose(false); }

        private void Dispose(bool disposing)
        {
            if (!disposing || this.IsDisposed)
                return;
            lock (this)
            {
                try
                {
                    if (this.IsDebugMode)
                        this.LogEvent("Disposing");
                    this.StopCheckStatusTimer();
                    if (this._SerialPort == null)
                        return;
                    if (this._SerialPort.IsOpen)
                        this._SerialPort.Close();
                    this._SerialPort.Dispose();
                    this._SerialPort = null;
                }
                catch (Exception ex)
                {
                    this.LogEvent("{0} Exception occurred while disposing: {1}", ex.GetBaseException().Message);
                }
                finally
                {
                    this.IsDisposed = true;
                    if (this.IsDebugMode)
                        this.LogEvent("Disposing done.");
                }
            }
        }

        private void OnStatusChanged()
        {
            this.StatusChanged?.Invoke(this, EventArgs.Empty);
            //if (this.PrintQueue.Any())
            //    this.InnerDoOnPaperReady(this.PrintQueue.Dequeue());
        }

        public void ProcessQueue() { this.StartCheckStatusTimer(); }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static class ComPortPrinterCommands
        {
            public static readonly byte[] NewLine = {0x0A, 0x0D};
            public static readonly byte[] HorizontalTab = {0x09};
            public static readonly byte[] CancelPrint = {0x18, 0x21};
            public static readonly byte[] PaperEject = {0x0C, 0x1B, 0x63, 0x30, 0x01, 0x1B, 0x74, 0x13, 0x1B, 0x71, 0x1B, 0x46, 0x00};
            public static readonly byte[] SetAutomaticStatusBack = {0x1D, 0x61};
            public static readonly byte[] TransmitStatus = {0x1D, 0x72};
            public static readonly byte[] ReadCheckPaper = {0x1C, 0x61, 0x30};
            public static readonly byte[] PrintAndFeedPaper = {0x27, 0x74};
            public static readonly byte[] TransmitPaperSensorStatus = {0x27, 0x64};
            public static readonly byte[] TurnUnderlineMode = {0x27, 0x45};
            public static readonly byte[] PrintAndReverseFeed = {0x27, 0x75};
            public static readonly byte[] SelectCharacterCodeTable = {0x1B, 0x74};
            public static readonly byte[] SelectInternationalCharacterSet = {0x1B, 0x52};
            public static readonly byte[] SelectCharacterSize = {0x1D, 0x21};
            public static readonly byte[] SelectPrintDirection = {0x1B, 0x54};
            public static readonly byte[] SelectJustification = {0x1B, 0x61};
            public static readonly byte[] ExecuteSelfTest = {0x1F, 0x40};
            public static readonly byte[] SelectSlip1 = {0x1B, 0x63, 0x30, 0x04, 0x1B, 0x74, 0x13};
            public static readonly byte[] EndSlip1 = {0x0C, 0x1B, 0x63, 0x30, 0x01, 0x1B, 0x74, 0x13, 0x1B, 0x71, 0x1B, 0x46, 0x00};
            public static readonly byte[] SelectPaperFeeder = {0x1B, 0x63, 0x30};
            public static readonly byte[] PrintAndFeedLines = {0x1B, 0x64};
            public static readonly byte[] PrintAndReverseFeedLines = {0x1B, 0x65};
            public static readonly byte[] RealtimeStatusTransmission = {0x10, 0x04};
            public static readonly byte[] SetLeftMargin = {0x1D, 0x4C};
            public static readonly byte[] InitializePrinter = {0x1B, 0x40, 0x1B, 0x74, 0x13, 0x1B, 0x4D, 0x00};
            public static readonly byte[] SetSlipPaperWaitingTime = {0x1B, 0x66};
        }

        #region R&D

        //[Obsolete("Not done yet.")]
        //public void SetPrinterPrintDirection(ComPortPrinterPrintDirection value)
        //{
        //    switch (value)
        //    {
        //        case ComPortPrinterPrintDirection.LeftToRight:
        //            this.SendCommand(ComPortPrinterCommands.SelectPrintDirection, 0, 48);
        //            break;
        //        case ComPortPrinterPrintDirection.BottomToTop:
        //            this.SendCommand(ComPortPrinterCommands.SelectPrintDirection, 1, 49);
        //            break;
        //        case ComPortPrinterPrintDirection.RightToLeft:
        //            this.SendCommand(ComPortPrinterCommands.SelectPrintDirection, 2, 50);
        //            break;
        //        case ComPortPrinterPrintDirection.TopToBottom:
        //            this.SendCommand(ComPortPrinterCommands.SelectPrintDirection, 3, 51);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("value", value, null);
        //    }
        //}
        //[Obsolete("Not done yet.")]
        //public void SetAutomaticStatusBack(ComPortPrinterAutomaticStatusBack value)
        //{
        //    byte commandArgument = 255;
        //    switch (value)
        //    {
        //        case ComPortPrinterAutomaticStatusBack.Disabled:
        //            this.SendCommand(ComPortPrinterCommands.SetAutomaticStatusBack, Convert.ToByte(commandArgument & 0));
        //            break;
        //        case ComPortPrinterAutomaticStatusBack.PaperSensorDisabled:
        //            this.SendCommand(ComPortPrinterCommands.SetAutomaticStatusBack, commandArgument, commandArgument, commandArgument, commandArgument);
        //            break;
        //        case ComPortPrinterAutomaticStatusBack.PaperSensorEnabled:
        //            this.SendCommand(ComPortPrinterCommands.SetAutomaticStatusBack, commandArgument, commandArgument, commandArgument, commandArgument);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("value", value, null);
        //    }
        //}

        #endregion
    }

    public enum PrinterStatus
    {
        Unknown,
        NoPaper,
        PaperReady
    }

    public enum ComPortPrinterCharacterSize
    {
        Normal = 0,
        DoubleHeight = 1,
        DoubleWidth = 16,
        Quadruple = 17
    }

    public enum ComPortPrinterInternationalCharacterSet
    {
        Usa = 0,
        France = 1,
        Germany = 2,
        Uk = 3,
        Denmark1 = 4,
        Sweden = 5,
        Italy = 6,
        Spain = 7,
        Japan = 8,
        Norway = 9,
        Denmark2 = 10
    }

    public enum ComPortPrinterPrintDirection
    {
        LeftToRight,
        BottomToTop,
        RightToLeft,
        TopToBottom
    }

    public enum ComPortPrinterJustification
    {
        Left,
        Center,
        Right
    }

    public enum ComPortPrinterAutomaticStatusBack
    {
        Disabled = 0,

        /// <summary>
        ///     Slip paper sensor and status disabled
        /// </summary>
        PaperSensorDisabled,
        PaperSensorEnabled
    }

    public enum ComPortPrinterTransmitStatus
    {
        PaperSensorStatus,
        DrawerKickoutConnectorStatus,
        SlipPaperStatus
    }

    public enum ComPortPrinterCharacterCodeTable
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        ///     PC 437 (USA, Standard Europe)
        /// </summary>
        USA = 0,
        Katakana = 1,

        /// <summary>
        ///     PC 858 (Multilingual + Euro)
        /// </summary>
        Multilingual = 2,

        /// <summary>
        ///     PC 860 (Portugal)
        /// </summary>
        Portugal = 3,

        /// <summary>
        ///     PC 863 (Canadian-French)
        /// </summary>
        CanadianFrench = 4,

        /// <summary>
        ///     PC 865 (Nordic)
        /// </summary>
        Nordic = 5,

        /// <summary>
        ///     Font A: space
        ///     Font B: special characters
        /// </summary>
        Custom = 255
    }
}