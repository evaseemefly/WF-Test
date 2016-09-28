using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace WWF
{

    public sealed class myWait4BookMark : NativeActivity
    {
        // 定义一个字符串类型的活动输入参数
        public InArgument<string> Text { get; set; }

        public InArgument<string> BookMarkName { get; set; }

        public OutArgument<string> InputStr { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            string bookMarkName = context.GetValue(BookMarkName);
            context.CreateBookmark(bookMarkName, new BookmarkCallback(ConExecuteWorkFlow));
        }

        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }

        private void ConExecuteWorkFlow(NativeActivityContext context, Bookmark bookmark, object value)
        {
            context.SetValue(InputStr, value.ToString());
        }
    }
}
