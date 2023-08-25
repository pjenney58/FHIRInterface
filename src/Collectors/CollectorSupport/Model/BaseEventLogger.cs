/*
 MIT License - BaseEventLogger.cs

Copyright (c) 2021 - Present by Sand Drift Software, LLC
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using Hl7Harmonizer.Adapters.Model;
using Hl7Harmonizer.Support.Interface;

namespace Hl7Harmonizer.Support.Model
{
    public class BaseEventLogger : IBaseEventLogger, IDisposable
    {
#pragma warning disable CA1822 // Mark members as static

        private static readonly NLog.Logger EventLogger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly MailAlert MailAlert = new();

        private readonly string _eventLogName = "SandDriftSoftware";

        public BaseEventLogger(string? logname = null)
        {
            if (logname != null)
            {
                _eventLogName = logname;
            }
        }

        private void LogSendError(string message)
        {
            EventLogger.Error($"Error logging message: {message}");
        }

        public string ReportFatal(string message, bool sendEmail = true)
        {
            try
            {
                if (sendEmail)
                {
                    MailAlert.SendAlert(message);
                }

                EventLogger.Error($"[{_eventLogName}] FATAL: {message}");
            }
            catch (Exception ex)
            {
                LogSendError($"[{_eventLogName}] FATAL logging problem: {ex.Message}");
            }

            return message;
        }

        public string ReportError(string message, bool sendEmail = true)
        {
            try
            {
                if (sendEmail)
                {
                    MailAlert.SendAlert(message);
                }

                EventLogger.Error($"[{_eventLogName}] ERROR: {message}");
            }
            catch (Exception ex)
            {
                LogSendError($"[{_eventLogName}] ERROR logging problem: {ex.Message}");
            }

            return message;
        }

        public string ReportWarning(string message, bool sendEmail = false)
        {
            try
            {
                if (sendEmail)
                {
                    MailAlert.SendAlert(message);
                }

                EventLogger.Warn($"[{_eventLogName}] WARNING: {message}");
            }
            catch (Exception ex)
            {
                LogSendError($"WARNING logging problem: {ex.Message}");
            }

            return message;
        }

        public string ReportInfo(string message, bool sendEmail = false)
        {
            try
            {
                if (sendEmail)
                {
                    MailAlert.SendAlert(message);
                }

                EventLogger.Info($"[{_eventLogName}] INFO: {message}");
            }
            catch (Exception ex)
            {
                LogSendError($"INFO logging problem: {ex.Message}");
            }

            return message;
        }

        public string ReportDebug(string message, bool sendEmail = false)
        {
            try
            {
                if (sendEmail)
                {
                    //mailAlert.SendAlert(message);
                }

                EventLogger.Debug($"[{_eventLogName}] DEBUG: {message}");
            }
            catch (Exception ex)
            {
                LogSendError($"DEBUG Error logging problem: {ex.Message}");
            }

            return message;
        }

        public string ReportTrace(string message, bool sendEmail = false)
        {
            try
            {
                if (sendEmail)
                {
                    MailAlert.SendAlert(message);
                }

                EventLogger.Trace($"[{_eventLogName}] TRACE: {message}");
            }
            catch (Exception ex)
            {
                LogSendError($"TRACE Error logging problem: {ex.Message}");
            }

            return message;
        }

        public string Alert(string message, bool sendEmail = true)
        {
            try
            {
                if (sendEmail)
                {
                    MailAlert.SendAlert(message);
                }

                EventLogger.Info($"[{_eventLogName}] ALERT: {message}");
            }
            catch (Exception ex)
            {
                LogSendError($"ALERT Error logging problem: {ex.Message}");
            }

            return message;
        }

#pragma warning restore CA1822 // Mark members as static

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                NLog.LogManager.Flush();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}