using ActivityManager.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivityManager.Student
{
    public partial class StudentInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int n = 20; n <= 30; n++)
                    DropDownListGrade.Items.Add(n.ToString());

                for (int n = 1; n <= 9; n++)
                    DropDownListClass.Items.Add(n.ToString());
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string pattern = @"^[a-zA-Z]\w{5,17}$";
            if (Regex.IsMatch(TxtNewPsw.Text.ToString(), pattern))
            {
                ActivityManagerDataContext db = new ActivityManagerDataContext();

                StudentIdentified studentIdentified = new StudentIdentified();
                studentIdentified.studentID = Session["ID"].ToString();
                studentIdentified.phone = TxtPhone.Text.Trim();
                studentIdentified.studentName = Session["name"].ToString();
                studentIdentified.gender = RadioButtonListGender.SelectedItem.ToString();
                studentIdentified.faculty = DropDownListFaculty.SelectedValue.ToString();
                studentIdentified.major = DropDownListMajor.SelectedValue.ToString();
                studentIdentified.@class = DropDownListClass.SelectedValue.ToString();
                studentIdentified.studentPassword = TxtRePsw.Text.Trim();
                studentIdentified.credit_1 = 0;
                studentIdentified.credit_2 = 0;
                studentIdentified.credit_3 = 0;

                /*try
                {*/
                db.StudentIdentified.InsertOnSubmit(studentIdentified);
                db.SubmitChanges();
                /*}
                catch
                {
                    Response.Write("<script>alert('信息录入错误，请稍后重试！');</script>");
                    return;
                }*/

                Response.Write("<script>alert('信息录入成功，请重新登录！');location.href='..//Login.aspx';</script>");
            }
            else
                Response.Write("<script>alert('密码必须以字母开头，长度在6~18之间，只能包含字符、数字和下划线！')</script>");
        }

        protected void DropDownListFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = DropDownListFaculty.SelectedIndex;
            DropDownListMajor.Items.Clear();

            switch (i)
            {
                case 0:
                    // 信息学科部
                    DropDownListMajor.Items.Add("自动化");
                    DropDownListMajor.Items.Add("电气工程及其自动化");
                    DropDownListMajor.Items.Add("通信工程");
                    DropDownListMajor.Items.Add("电子商务");
                    DropDownListMajor.Items.Add("软件工程");
                    DropDownListMajor.Items.Add("计算机科学与技术");
                    break;

                case 1:
                    // 人文学科部
                    DropDownListMajor.Items.Add("德语");
                    DropDownListMajor.Items.Add("法学");
                    DropDownListMajor.Items.Add("汉语言文学");
                    DropDownListMajor.Items.Add("环境设计");
                    DropDownListMajor.Items.Add("日语");
                    DropDownListMajor.Items.Add("视觉传达");
                    DropDownListMajor.Items.Add("数字媒体艺术");
                    DropDownListMajor.Items.Add("新闻学");
                    DropDownListMajor.Items.Add("英语");
                    break;

                case 2:
                    // 理工学科部
                    DropDownListMajor.Items.Add("建筑学");
                    DropDownListMajor.Items.Add("工程管理");
                    DropDownListMajor.Items.Add("环境工程");
                    DropDownListMajor.Items.Add("车辆工程");
                    DropDownListMajor.Items.Add("材料成型及控制工程");
                    DropDownListMajor.Items.Add("制药工程");
                    DropDownListMajor.Items.Add("生物工程");
                    DropDownListMajor.Items.Add("机械设计制造及其自动化");
                    DropDownListMajor.Items.Add("土木工程");
                    break;

                case 3:
                    // 财经学科部
                    DropDownListMajor.Items.Add("市场营销");
                    DropDownListMajor.Items.Add("工商管理");
                    DropDownListMajor.Items.Add("旅游管理");
                    DropDownListMajor.Items.Add("金融学");
                    DropDownListMajor.Items.Add("会计学");
                    DropDownListMajor.Items.Add("国际经济与贸易");
                    DropDownListMajor.Items.Add("财务管理");
                    break;
            }
        }
    }
}