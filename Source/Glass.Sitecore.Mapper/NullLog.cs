/*
   Copyright 2011 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Glass.Sitecore.Mapper
{
    public class NullLog : ILog
    {
        #region ILog Members

        public void Debug(object message, Exception exception)
        {
            
        }

        public void Debug(object message)
        {
            
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            
        }

        public void DebugFormat(string format, object arg0)
        {
            
        }

        public void DebugFormat(string format, params object[] args)
        {
            
        }

        public void Error(object message, Exception exception)
        {
            
        }

        public void Error(object message)
        {
            
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            
        }

        public void ErrorFormat(string format, object arg0)
        {
            
        }

        public void ErrorFormat(string format, params object[] args)
        {
            
        }

        public void Fatal(object message, Exception exception)
        {
            
        }

        public void Fatal(object message)
        {
            
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            
        }

        public void FatalFormat(string format, object arg0)
        {
            
        }

        public void FatalFormat(string format, params object[] args)
        {
            
        }

        public void Info(object message, Exception exception)
        {
            
        }

        public void Info(object message)
        {
            
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            
        }

        public void InfoFormat(string format, object arg0)
        {
            
        }

        public void InfoFormat(string format, params object[] args)
        {
            
        }

        public bool IsDebugEnabled
        {
            get;
            set;
        }

        public bool IsErrorEnabled
        {
            get;set;
        }

        public bool IsFatalEnabled
        {
            get;set;
        }

        public bool IsInfoEnabled
        {
            get;set;
        }

        public bool IsWarnEnabled
        {
            get;set;
        }

        public void Warn(object message, Exception exception)
        {
            
        }

        public void Warn(object message)
        {
            
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            
        }

        public void WarnFormat(string format, object arg0)
        {
            
        }

        public void WarnFormat(string format, params object[] args)
        {
            
        }

        #endregion

        #region ILoggerWrapper Members

        public log4net.Core.ILogger Logger
        {
            get;set;
        }

        #endregion
    }
}
