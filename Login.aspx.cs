using ActivityManager.App_Data;
using System;
using System.Linq;

namespace ActivityManager
{
    public partial class Sign : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void LoginMessageBox(string dbPsw, string loginPsw, string ID, int loginType)
        {
            /*
             * 验证失败，弹出对应信息窗口
             * 验证成功，跳转页面
             */

            if (dbPsw == "" || dbPsw == null)
                // 账户错误
                Response.Write("<script>alert('请确认账户ID！')</script>");
            else if (dbPsw.Equals(loginPsw))
            {
                // 验证通过，创建会话信息
                Response.Write("<script>alert('登录成功！')</script>");
                Session["ID"] = ID;

                // 跳转主页面
                switch (loginType)
                {
                    case 0:
                        // 校方页面
                        Tool.curUser = 0;
                        Tool.studentID = ID;
                        Response.Redirect("School/Admin.aspx");
                        break;

                    case 1:
                        // 组织页面
                        Tool.curUser = 1;
                        Tool.studentID = ID;
                        Response.Redirect("Org/Org.aspx");
                        break;

                    case 2:
                        // 学生页面
                        Tool.curUser = 2;
                        Tool.studentID = ID;
                        Response.Redirect("Student/Student.aspx");
                        break;
                }
            }
            else
            {
                // 密码错误
                Response.Write("<script>alert('密码错误，请确认密码！')</script>");
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            /*
             * 获取文本框登录信息
             * ID比对查询密码
             * 1、密码为空账户错误
             * 2、密码比对成功，登录
             * 3、密码错误，重新输入
             */

            ActivityManagerDataContext db = new ActivityManagerDataContext();

            // 登录文本框信息
            string loginID = TxtID.Text;
            string loginPsw = TxtPsw.Text;
            int loginType = RblType.SelectedIndex;

            string dbPsw = "";
            switch (loginType)
            {
                // 没有选择登录方式
                case -1:
                    Response.Write("<script>alert('请选择登录方式！')</script>");
                    break;

                // 学校端登录
                case 0:
                    var resAdmin = from info in db.Administration
                                   where info.adminID == loginID
                                   select info.adminPassword;
                    if (resAdmin.Count() > 0)
                        dbPsw = resAdmin.First().ToString();
                    LoginMessageBox(dbPsw, loginPsw, loginID, 0);
                    break;

                // 组织端登录
                case 1:
                    var resOrg = from info in db.Organization
                                 where info.organizationID == loginID
                                 select info.organizationPassword;
                    if (resOrg.Count() > 0)
                        dbPsw = resOrg.First().ToString();
                    LoginMessageBox(dbPsw, loginPsw, loginID, 1);
                    break;

                // 学生端登录
                case 2:
                    var resStu = from info in db.StudentIdentified
                                 where info.studentID == loginID
                                 select info.studentPassword;
                    if (resStu.Count() > 0)
                        dbPsw = resStu.First().ToString();
                    LoginMessageBox(dbPsw, loginPsw, loginID, 2);
                    break;
            }
        }

        protected void BtnIdentifying_Click(object sender, EventArgs e)
        {
            /*
             * 转到学生认证页面
             */

            Server.Transfer("Student/StudentIdentifying.aspx");
        }
    }
}