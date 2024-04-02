using ActivityManager.App_Data;
using System;
using System.Linq;

namespace ActivityManager.Student
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnIdentifying_Click(object sender, EventArgs e)
        {
            /*
             * 获取信息
             * 数据库比对信息
             * res.Count > 0  则 Student 表中有该学生信息
             * 将学生信息插入 StudentIdentified 表
             */

            string studentID = TxtStudentID.Text.Trim();
            string studentName = TxtStudentName.Text.Trim();
            string ID = TxtID.Text.Trim();

            ActivityManagerDataContext db = new ActivityManagerDataContext();

            var res = from info in db.StudentIdentified
                      where info.studentID == studentID
                      select info;

            if (res.Any())
            {
                Response.Write("<script>alert('无需二次认证，请返回登录界面！');</script>");
                return;
            }

            var resI = from info in db.Student
                       where info.studentID == studentID && info.studentName == studentName && info.ID == ID
                       select info;

            if (resI.Any())
            {
                Response.Write("<script>alert('请继续填写基本信息！');</script>");
                Session["ID"] = studentID;
                Session["name"] = TxtStudentName.Text.Trim();
                Server.Transfer("StudentInfo.aspx");
            }
            else
            {
                Response.Write("<script>alert('请检查认证信息！');</script>");
            }
        }

        protected void BtnReturn_Click(object sender, EventArgs e)
        {
            /*
             * 返回登录页面
             */

            Server.Transfer("../Login.aspx");
        }
    }
}