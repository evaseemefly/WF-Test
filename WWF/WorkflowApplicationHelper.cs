using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WWF
{
    public class WorkflowApplicationHelper
    {
        static AutoResetEvent syncEven = new AutoResetEvent(false);
        //WorkflowApplication application = null;
        public static WorkflowApplication CreateWorkflowApplication(Activity activity,IDictionary<string,object>dict)
        {            

            //1 创建工作流对象（需要传入参数）
            WorkflowApplication application = new WorkflowApplication(activity, dict);

            SqlWorkflowInstanceStore store = new SqlWorkflowInstanceStore(ConfigurationManager.AppSettings["workflowConnection"]);

            application.InstanceStore = store;

            application.Unloaded += OnUnloaded;
            application.Aborted += OnAborted;
            application.Idle += OnIdle;
            application.PersistableIdle += OnPersistableIdle;
            application.OnUnhandledException += OnUnhandledException;
            //2 开始或恢复工作流
            application.Run();
            return application;
        }

        /// <summary>
        /// 加载一个持久化的工作流
        /// </summary>
        /// <param name="activity">具体工作流</param>
        /// <param name="guid">guid</param>
        /// <returns></returns>
        public static WorkflowApplication LoadWorkflowApplication(Activity activity,Guid guid)
        {
            //application.ResumeBookmark(this.txtBookMarkName.Text, int.Parse(this.txtValue.Text));
            WorkflowApplication application = new WorkflowApplication(activity);

            SqlWorkflowInstanceStore store = new SqlWorkflowInstanceStore(ConfigurationManager.AppSettings["workflowConnection"]);

            application.InstanceStore = store;

            application.Unloaded += OnUnloaded;
            application.Aborted += OnAborted;
            application.Idle += OnIdle;
            application.PersistableIdle += OnPersistableIdle;
            application.OnUnhandledException += OnUnhandledException;
            //this.txtWFID.Text = application.Id.ToString();
            //将该工作流实例从数据库读取到内存中
            application.Load(guid);
            return application;
        }

        private static UnhandledExceptionAction OnUnhandledException(WorkflowApplicationUnhandledExceptionEventArgs arg)
        {
            Console.WriteLine("主线程：异常处理");
            syncEven.Set();
            return UnhandledExceptionAction.Abort;
        }

        private static PersistableIdleAction OnPersistableIdle(WorkflowApplicationIdleEventArgs arg)
        {
            Console.WriteLine("主线程：工作流持久化");
            return PersistableIdleAction.Unload;
        }

        private static void OnIdle(WorkflowApplicationIdleEventArgs obj)
        {
            syncEven.Set();
            Console.WriteLine("主线程：工作流空闲");
        }

        private static void OnAborted(WorkflowApplicationAbortedEventArgs obj)
        {
            syncEven.Set();
            Console.WriteLine("主线程：工作流中止");
        }

        private static void OnUnloaded(WorkflowApplicationEventArgs obj)
        {
            syncEven.Set();
            Console.WriteLine("主线程：工作流卸载");
        }
    }
}
