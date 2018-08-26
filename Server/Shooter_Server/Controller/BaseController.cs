using System;
using Common;
using War_Server.Servers;

namespace War_Server.Controller
{
    public abstract class BaseController
    {
        protected  RequestCode request = RequestCode.None;
        public RequestCode RequestCode
        {
            get { return request; }
        }
        public virtual string DefaultHandle(string data,Client client,Server server)
        {
            return null;
        }
    }
}
