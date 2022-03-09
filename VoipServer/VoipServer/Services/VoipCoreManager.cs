using AsterNET.Manager;
using AsterNET.Manager.Action;

namespace VoipServer.Services
{
    public sealed class VoipCoreManager
    {
        private static VoipCoreManager _instance = null;
        private static readonly object LockObject = new object();
        private readonly ManagerConnection _manager;

        private VoipCoreManager()
        {
            _manager = new ManagerConnection("192.168.3.233", 5038, "crm", "Tehran1400")
            {
                FireAllEvents = true,
                PingInterval = 0,
                KeepAlive = true,
                UseASyncEvents = false,
                ReconnectRetryFast = 1000,
            };
        }

        public ManagerConnection Manager()
        {
            return _manager;
        }


        public bool IsAlive()
        {
            return _instance.IsAlive();
        }
        public bool TurnOnDnd(string extention)
        {
            if (!string.IsNullOrEmpty(extention))
            {
                var put = new DBPutAction
                {
                    Family = "DND",
                    Key = extention,
                    Val = "YES"
                };
                var response = _manager.SendAction(put);
                return response.IsSuccess();
            }

            return false;
        }

        public bool TurnOffDnd(string extention)
        {
            if (!string.IsNullOrEmpty(extention))
            {
                var delete = new DBDelAction
                {
                    Family = "DND",
                    Key = extention
                };
                var response = _manager.SendAction(delete);
                return response.IsSuccess();
            }

            return false;
        }

        public bool MakeCall(string from, string to, bool outerCall = false)
        {
            if (_manager.IsConnected() is false)
            {
                _manager.Login();
            }
            if (_manager.IsConnected())
            {
                if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                {
                    var originateAction = new OriginateAction();
                    if (outerCall is true)
                    {
                        originateAction.Channel = $"SIP/{from}";
                        originateAction.Exten = $"9{to}";
                    }
                    else
                    {
                        originateAction.Channel = "SIP" + to;
                        originateAction.Exten = $"{to}";
                    }

                    originateAction.Context = "internal1";
                    originateAction.Priority = "1";
                    originateAction.Timeout = 30000;
                    originateAction.Async = true;
                    var originateResponse = _manager.SendAction(originateAction);
                    return originateResponse.IsSuccess();
                }
            }
            return false;
        }



        public static VoipCoreManager Instance
        {
            get
            {
                lock (LockObject)
                {
                    return _instance ??= new VoipCoreManager();
                }
            }
        }
    }
}