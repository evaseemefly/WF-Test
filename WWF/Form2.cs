using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WWF
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        WorkflowApplication work = null;
        Activity workflow_temp = new Activity2();

        private void ClearWfId()
        {
            this.tb_wfId.Text = string.Empty;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            ClearWfId();
            

            var dic = new Dictionary<string, object>() { { "TempBookMarkName", tb_BookMarkName.Text } };

            work = WorkflowApplicationHelper.CreateWorkflowApplication(workflow_temp, dic);
            //work.Run();
            this.tb_wfId.Text = work.Id.ToString();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string bookName = tb_BookMarkName.Text;
            string inputStr = tb_Input.Text;
            string wfid = tb_wfId.Text;
           var work= WorkflowApplicationHelper.LoadWorkflowApplication(workflow_temp, Guid.Parse(wfid));
            if (work.GetBookmarks().Count(p=>p.BookmarkName==bookName) == 1)
            {
                Console.WriteLine("存在输入的书签");
                work.ResumeBookmark(bookName, inputStr);
            }
        }
    }
}

