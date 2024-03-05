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

            string studentID = TxtStudentID.Text;
            string studentName = TxtStudentName.Text;
            int genderId = RblGender.SelectedIndex;
            string major = DdlMajor.Text;
            string sClass = DdlClass.Text;
            string phone = TxtPhone.Text;

            string gender = "";
            switch (genderId)
            {
                case -1:
                    Response.Write("<script>alert('请选择性别！')</script>");
                    return;
                case 0:
                    gender = "男";
                    break;
                case 1:
                    gender = "女";
                    break;
            }


            ActivityManagerDataContext db = new ActivityManagerDataContext();

            var res = from info in db.Student
                      where info.studentID == studentID && info.studentName == studentName && info.gender == gender && info.major == major && info.@class == sClass
                      select info;

            if (res.Count() > 0)
            {
                // 查到信息，说明认证成功，进行添加操作
                StudentIdentified studentIdentified = new StudentIdentified();
                studentIdentified.studentID = studentID;
                studentIdentified.studentPassword = "123456";
                studentIdentified.phone = phone;
                studentIdentified.credit = 0;

                // 插入操作
                try
                {
                    db.StudentIdentified.InsertOnSubmit(studentIdentified);
                    db.SubmitChanges();
                    Response.Write("<script>alert('认证成功！初始密码为123456，请尽快前往修改密码！')</script>");

                    Server.Transfer("../Login.aspx");
                }
                catch
                {
                    /*弹出异常问题待解决*/
                    Response.Write("<script>alert('学生已认证，无需二次认证，请返回登录！')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('学生认证失败，请确认个人信息！')</script>");
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