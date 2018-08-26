using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using System.Reflection;
using War_Server.Servers;


namespace War_Server.Controller
{
    public class ControllerManager
    {
        private Server server;
        private Dictionary<RequestCode, BaseController> controllerDic = new Dictionary<RequestCode, BaseController>();
        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }
        void InitController()
        {
            DefaultController defaultController = new DefaultController();
            controllerDic.Add(RequestCode.None, defaultController);
            controllerDic.Add(RequestCode.User, new UserController());
            controllerDic.Add(RequestCode.Room, new RoomController());
            controllerDic.Add(RequestCode.Game, new GameController());
            controllerDic.Add(RequestCode.Chat, new ChatController());
        }

        //处理请求，通过request获得对应的Controller，通过action取得Controller里面的方法
        public void HandleRequest(RequestCode request,ActionCode action,string data,Client client)
        {
            BaseController controller;
            //尝试在字典中获取对应的controler
            bool isGet = controllerDic.TryGetValue(request, out controller);
            if(isGet == false)
            {
                Console.WriteLine("无法得到RequestCode:[" + request + "]对应的Controller");
            }
            //通过反射获得相应对象中名为methodName的方法
            string methodName = Enum.GetName(typeof(ActionCode), action);
            Console.WriteLine("得到method: " + methodName);
            MethodInfo method = controller.GetType().GetMethod(methodName);
            Console.WriteLine("methodInfo:" + method.Name);
            if(method == null)
            {
                Console.WriteLine("Controller：[" + controller.GetType() + "]没有对应的方法：[" + methodName + "]");
            }
            object[] parameters = new object[] { data,client,server };
            //在controller对象中调用method,参数为parameters
            object o = method.Invoke(controller, parameters);
            //当返回值为空时，不需要服务器响应请求
            if(string.IsNullOrEmpty(o as string))
            {
                return;
            }
            //调用Server响应该请求
            server.ResponseMsg(client, action, o as string);
        }

        public BaseController GetController(RequestCode requestCode)
        {
            if (controllerDic.TryGetValue(requestCode, out BaseController controller))
            {
                return controller;
            }
            else return null;
        }

    }
}
