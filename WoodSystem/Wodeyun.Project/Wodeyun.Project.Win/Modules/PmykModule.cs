using System;

using Wodeyun.Device.Controllers;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.Win.Modules
{
    public class PmykModule : IDisposable
    {
        private Pmyk2 _Pmyk = new Pmyk2();

        public void Connect(string pmyk)
        {
            try
            {
                string[] pmyks = pmyk.Split(":".ToCharArray());
                this._Pmyk.Connect(pmyks[0], pmyks[1].ToInt32());
            }
            catch
            {
                throw;
            }
        }

        public bool OpenError()
        {
            return this._Pmyk.OpenRed();
        }

        public bool CloseError()
        {
            return this._Pmyk.CloseRed();
        }

        public bool Check()
        {
            return this._Pmyk.Check();
        }

        public void Dispose()
        {
            this._Pmyk.Dispose();
        }
    }
}
