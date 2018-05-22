using System;

namespace SharpSettings
{
    public interface ISharpSettingsLogger
    {
        void Critical(string message);
        void Critical(Exception ex);
        void Critical(Exception ex, string message);
        
        void Debug(string message);
        void Debug(Exception ex);
        void Debug(Exception ex, string message);
        
        void Error(string message);
        void Error(Exception ex);
        void Error(Exception ex, string message);
        
        void Information(string message);
        void Information(Exception ex);
        void Information(Exception ex, string message);
        
        void Trace(string message);
        void Trace(Exception ex);
        void Trace(Exception ex, string message);
        
        void Warn(string message);
        void Warn(Exception ex);
        void Warn(Exception ex, string message);
    }
}