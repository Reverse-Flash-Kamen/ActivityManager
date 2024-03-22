using ActivityManager.App_Data;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;

namespace ActivityManager.Test
{
    public partial class StudentWebForm : System.Web.UI.Page
    {
        protected void BtnCheck_Click(object sender, EventArgs e)
        {
            /*活动报名详情页返回*/
            CheckActDiv.Style["display"] = "none";
        }

        protected void commit_Click(object sender, EventArgs e)
        {
            /*查询功能*/
            /*schoolConnector.Where = null;
            schoolConnector.Where = "activityState >= 5 and activityState <= 8";*/

            if (LinkButton1.Font.Underline == true)
                LinkButton1_Click(sender, e);
            else if (LinkButton2.Font.Underline == true)
                LinkButton2_Click(sender, e);

            if (schoolConnector.Where == "") schoolConnector.Where = "(activityState >= 0)";

            string s1 = name.Text.Trim();
            string s2 = org.Text.Trim();
            string s3 = state.SelectedValue.Trim();
            string s4 = type.SelectedValue.Trim();

            if (s1 != "")
            {
                // 根据活动名称查询
                schoolConnector.Where += " and (activityName = \"" + s1 + "\")";
            }

            if (s2 != "")
            {
                // 根据组织名称查询,需转ID
                ActivityManagerDataContext db = new ActivityManagerDataContext();
                var res = from org in db.Organization
                          where org.organizationName == s2
                          select org;

                if (res.Any())
                    schoolConnector.Where += " and (activityOrgID = \"" + res.First().organizationID.ToString() + "\")";
            }

            if (s3 != "")
            {
                // 根据活动状态查询
                if (s3 != "0")
                    schoolConnector.Where += " and (activityState = " + s3 + ")";
            }

            if (s4 != "")
            {
                // 根据活动类别查询
                if (s4 != "0")
                    schoolConnector.Where += "and (activityType = " + s4 + ")";
            }

            GvTemplate.PageIndex = 0; // 查询完回到第一页
            ActivityManagerDataContext.connectorWhere = schoolConnector.Where.ToString(); // 存储当前查询条件
        }

        protected void flush_Click(object sender, EventArgs e)
        {
            /*重置查询条件*/
            name.Text = null;
            org.Text = null;
            state.SelectedIndex = 0;
            type.SelectedIndex = 0;

            if (LinkButton1.Font.Underline == true)
                LinkButton1_Click(sender, e);
            else if (LinkButton2.Font.Underline == true)
                LinkButton2_Click(sender, e);
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            Tool.FormatActivity((GridView)sender, Session["ID"].ToString());
            Tool.FormatGridView((GridView)sender, 8);
        }

        protected void GvTemplate_DataBinding(object sender, EventArgs e)
        {
            /*数据更新,主要是根据系统时间更新活动状态和报名人数等*/
            Tool.UpdateActivityState((GridView)sender);
        }

        protected void GvTemplate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvTemplate.PageIndex = e.NewPageIndex;

            /*根据按钮是否存在下划线判断选中按钮,绑定不同数据源*//*

            if (LinkButton1.Font.Underline == true)
                LinkButton1_Click(sender, e);
            else if (LinkButton2.Font.Underline == true)
                LinkButton2_Click(sender, e);*/

            // 根据不同需求获取提前存储的查询条件
            schoolConnector.Where = ActivityManagerDataContext.connectorWhere;
        }

        protected void GvTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            /*
             * 用于响应表内按钮事件
             * 需要传gv的数据
             */

            /*if (GvTemplate.PageSize > ((GridView)sender).Rows.Count) return; // 防止与分页功能冲突*/
            if (e.CommandName == "Page") return;

            int index = int.Parse(e.CommandArgument.ToString());
            string actID = ((GridView)sender).Rows[index].Cells[0].Text;

            if (e.CommandName == "check")
            {
                // 查看操作
                CheckActDiv.Style["display"] = "block";

                LblState.Text = "当前状态：";
                LblState.Height = 40;
                LblActName.Text = "活动名称：";
                LblActName.Height = 40;
                LblActInfo.Text = "活动介绍：";
                LblActInfo.Height = 50;
                LblPlace.Text = "举办地点：";
                LblPlace.Height = 50;
                LblHoldDate.Text = "举办时间：";
                LblHoldDate.Height = 50;
                LblSignDate.Text = "报名时间：";
                LblSignDate.Height = 50;
                LblMaxSize.Text = "人数上限：";
                LblMaxSize.Height = 50;
                LblScore.Text = "活动学分：";
                LblScore.Height = 50;
                LblFail.Visible = false;
                //LblFail.Height = 0;

                MyActivity a = new MyActivity(actID);

                if (a.ActivityState == "3")
                {
                    LblFail.Text = "审核不通过理由：" + a.FailReason;
                    LblFail.Height = 40;
                    LblFail.Visible = true;
                }

                LblState.Text += Tool.states[int.Parse(a.ActivityState)];
                LblActName.Text += a.ActivityName;
                LblActInfo.Text += a.ActivityIntro;

                ActivityManagerDataContext db = new ActivityManagerDataContext();
                var res = from info in db.Place
                          where info.placeID == int.Parse(a.ActivityPlaceID)
                          select info.placeName;
                LblPlace.Text += res.First();

                LblHoldDate.Text += a.HoldDate;
                LblSignDate.Text += a.SignStartDate;
                LblMaxSize.Text += a.MaxSigned;
                LblScore.Text += a.AvailableCredit;
            }
            else
            {
                // Operation.SetOperation(e.CommandName, actID, Tool.studentID, (GridView)sender, schoolConnector);
                Operation.SetOperation(e.CommandName, actID, Session["ID"].ToString(), (GridView)sender, schoolConnector);
            }
        }

        protected void LbtnAllAct_Click(object sender, EventArgs e)
        {
            /*活动总览页面*/
            DivChangePsw.Style["dispaly"] = "none";
            DivMyInfoR.Style["display"] = "none";
            DivSearch.Style["display"] = "block";
            DivTopNov.Style["display"] = "block";

            LinkButton1.Text = "全部活动"; // 要在按钮点击事件之前
            LinkButton2.Text = "可报名";

            DivAllAct.Style["background-color"] = "red";
            DivMyAct.Style["background-color"] = "#ccad9f";
            DivMyInfo.Style["background-color"] = "#ccad9f";

            // 重置选中页数,初始页为0
            GvTemplate.PageIndex = 0;

            // 重置查询条件
            flush_Click(sender, e);

            // 默认选中全部活动
            LinkButton1_Click(sender, e);
        }

        protected void LbtnMyAct_Click(object sender, EventArgs e)
        {
            /*
             * 我的活动页面
             * 如何将Liked,Signed和Activity表进行联合查询
             */
            DivChangePsw.Style["dispaly"] = "none";
            DivMyInfoR.Style["display"] = "none";
            DivSearch.Style["display"] = "block";
            DivTopNov.Style["display"] = "block";

            // 更新导航条
            DivAllAct.Style["background-color"] = "#ccad9f";
            DivMyAct.Style["background-color"] = "red";
            DivMyInfo.Style["background-color"] = "#ccad9f";

            LinkButton1.Text = "已报名";
            LinkButton2.Text = "已收藏";

            // 重置选中页数,初始页为0
            GvTemplate.PageIndex = 0;

            // 重置查询条件
            flush_Click(sender, e);

            // 默认选中已报名
            LinkButton1_Click(sender, e);
        }

        protected void LbtnMyInfo_Click(object sender, EventArgs e)
        {
            /*我的信息页面*/
            DivAllAct.Style["background-color"] = "#ccad9f";
            DivMyAct.Style["background-color"] = "#ccad9f";
            DivMyInfo.Style["background-color"] = "red";
            DivMyInfoR.Style["display"] = "block";

            // 隐藏不需要的模块
            DivSearch.Style["display"] = "none";
            DivTopNov.Style["display"] = "none";
            DivChangePsw.Style["display"] = "none";
            schoolConnector.Where = "activityState < 0"; // 如果把GV隐藏再显示,大小会出问题

            // MyImage.ImageUrl = "~/image/users/" + Tool.studentID + ".jpg";
            MyImage.ImageUrl = "~/image/users/" + Session["ID"] + ".jpg";

            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res1 = from info in db.Student
                           // where info.studentID == Tool.studentID
                       where info.studentID == Session["ID"].ToString()
                       select info;

            if (res1.Any())
            {
                var my = res1.First();
                LblStuName.Text = my.studentName.ToString();
                LblStuID.Text = my.studentID.ToString();
                LblMajor.Text = my.major.ToString() + my.@class.ToString();
                LblGender.Text = my.gender.ToString();
            }

            var res2 = from info in db.StudentIdentified
                           // where info.studentID == Tool.studentID
                       where info.studentID == Session["ID"].ToString()
                       select info;

            if (res2.Any())
                LblCredit.Text = (res2.First().credit_1 + res2.First().credit_2 + res2.First().credit_3).ToString();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            /*更新按钮1*/
            if (LinkButton1.Font.Underline == false)
                GvTemplate.PageIndex = 0; // 第一次选中时重置数据页数

            LinkButton1.ForeColor = System.Drawing.Color.Brown;
            LinkButton1.Font.Underline = true;

            LinkButton2.Font.Underline = false;
            LinkButton2.ForeColor = System.Drawing.Color.Black;

            schoolConnector.Where = null;

            string LbtnText1 = LinkButton1.Text.ToString();
            switch (LbtnText1)
            {
                case "全部活动":
                    // 活动总览-全部活动
                    schoolConnector.Where = "(activityState >= 5 and activityState <= 8)"; // 未调试
                    break;

                case "已报名":
                    /*
                     * 查询test1
                     * 先用学生ID把所有收藏的活动ID存在数组里
                     * 再给where循环添加条件
                     */

                    ActivityManagerDataContext db = new ActivityManagerDataContext();
                    try
                    {
                        var res = from info in db.SignedActivity
                                      // where info.studentID == Tool.studentID
                                  where info.studentID == Session["ID"].ToString()
                                  select info.activityID;
                        if (res.Count() <= 0)
                        {
                            schoolConnector.Where = "(activityState < 0)"; // 没有结果
                            break;
                        }

                        string[] actIDs = res.ToArray();
                        schoolConnector.Where = "(activityID = \"" + res.First() + "\"";
                        foreach (string actID in actIDs)
                        {
                            Console.WriteLine(actID);
                            schoolConnector.Where += "or activityID = \"" + actID + "\" ";
                        }
                        schoolConnector.Where += ")";
                    }
                    catch
                    {
                        Response.Write("<script>alert('请登录后再访问！');location.href='..//Login.aspx';</script>");
                    }
                    break;

                default:
                    break;
            }
            ActivityManagerDataContext.connectorWhere = schoolConnector.Where.ToString(); // 存储当前查询条件
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            /*更新按钮2*/
            if (LinkButton2.Font.Underline == false)
                GvTemplate.PageIndex = 0; // 第一次选中时重置数据页数

            LinkButton2.ForeColor = System.Drawing.Color.Brown;
            LinkButton2.Font.Underline = true;

            LinkButton1.Font.Underline = false;
            LinkButton1.ForeColor = System.Drawing.Color.Black;

            schoolConnector.Where = null;

            string LbtnText2 = LinkButton2.Text.ToString();
            switch (LbtnText2)
            {
                case "可报名":
                    schoolConnector.Where = "activityState = 6";
                    break;

                case "已收藏":
                    /*
                     * 查询test1
                     * 先用学生ID把所有收藏的活动ID存在数组里
                     * 再给where循环添加条件
                     */

                    ActivityManagerDataContext db = new ActivityManagerDataContext();
                    var res = from info in db.LikedActivity
                                  // where info.studentID == Tool.studentID
                              where info.studentID == Session["ID"].ToString()
                              select info.activityID;

                    if (res.Count() <= 0)
                    {
                        schoolConnector.Where = "(activityState < 0)"; // 没有结果
                        break;
                    }

                    string[] actIDs = res.ToArray();
                    schoolConnector.Where = "(activityID = \"" + res.First() + "\"";
                    foreach (string actID in actIDs)
                    {
                        Console.WriteLine(actID);
                        schoolConnector.Where += "or activityID = \"" + actID + "\" ";
                    }
                    schoolConnector.Where += ")";
                    break;

                default:
                    break;
            }
            ActivityManagerDataContext.connectorWhere = schoolConnector.Where.ToString(); // 存储当前查询条件
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ////// 点击按钮会触发,不能在这初始化

            /*上导航栏初始效果,选中第一项*//*
            LinkButton1.Text = "初始_全部活动";
            LinkButton1.ForeColor = System.Drawing.Color.Brown;
            LinkButton1.Font.Underline = true;

            LinkButton2.Text = "初始_可报名";
            LinkButton2.Font.Underline = false;
            LinkButton2.ForeColor = System.Drawing.Color.Black;

            *//*左导航栏初始效果,选中活动总览*//*
            DivAllAct.Style["background-color"] = "red";
            DivMyAct.Style["background-color"] = "#ccad9f";
            DivMyInfo.Style["background-color"] = "#ccad9f";

            *//*数据表初始数据*//*
            schoolConnector.Where = null;
            schoolConnector.Where = "activityState >= 5 and activityState <= 8";      // 学生只能看到状态5-8的活动,待报名,报名中,待开始,活动中*/

            /*
             * 此处初始化存在隐患!!!!
             * Where初始值为"",表示*
             * 翻页功能会导致页面刷新加载此项
             */

            // 调试用
            Session["ID"] = 7020820312;
            Tool.curUser = 2;

            if (schoolConnector.Where == "") schoolConnector.Where = "(activityState >= 5 and activityState <= 8)";
            ActivityManagerDataContext.connectorWhere = schoolConnector.Where.ToString();

            Tool.FormatGridView(GvTemplate, 8);
        }

        protected void MyImage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (DivUploadImage.Style["display"] == "none")
                DivUploadImage.Style["display"] = "block";
            else
                DivUploadImage.Style["display"] = "none";
        }

        protected void BtnImage_Click(object sender, EventArgs e)
        {
            if (ImageUpload.HasFile)
            {
                string extension = Path.GetExtension(ImageUpload.FileName);

                if (extension.Equals(".jpg") || extension.Equals(".png"))
                {
                    string savePath = Server.MapPath("~/image/users/");//指定上传文件在服务器上的保存路径

                    //检查服务器上是否存在这个物理路径，如果不存在则创建
                    if (!System.IO.Directory.Exists(savePath))
                    {
                        System.IO.Directory.CreateDirectory(savePath);
                    }
                    // savePath = savePath + "\\" + Tool.studentID + ".jpg";
                    savePath = savePath + "\\" + Session["ID"].ToString() + ".jpg";
                    ImageUpload.SaveAs(savePath);
                    // MyImage.ImageUrl = "~/image/users/" + Tool.studentID + ".jpg";
                    MyImage.ImageUrl = "~/image/users/" + Session["ID"].ToString() + ".jpg";
                }
                else
                {
                    Response.Write("<script>alert('请上传 .jpg | .png 文件！');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('你还没有选择上传文件！');</script>");
            }
        }

        protected void BtnChangePsw_Click(object sender, EventArgs e)
        {
            DivCredit.Style["display"] = "none";

            if (DivChangePsw.Style["display"] == "none")
                DivChangePsw.Style["display"] = "block ";
            else
                DivChangePsw.Style["display"] = "none";
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string pattern = @"^[a-zA-Z]\w{5,17}$";
            if (Regex.IsMatch(TxtNewPsw.Text.ToString(), pattern))
            {
                ActivityManagerDataContext db = new ActivityManagerDataContext();
                var res = from info in db.StudentIdentified
                          where info.studentPassword == TxtPsw.Text
                          select info;

                if (res.Any())
                {
                    try
                    {
                        res.First().studentPassword = TxtRePsw.Text;
                        db.SubmitChanges();
                        DivChangePsw.Style["display"] = "none";
                        Response.Write("<script>alert('密码修改成功');location.href='..//Login.aspx';</script>");
                    }
                    catch
                    {
                        Response.Write("<script>alert('未知错误，请重试！');</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('原密码输入错误！');</script>");
                }
            }
            else
                LblMessage.Text = "密码必须以字母开头，长度在6~18之间，只能包含字符、数字和下划线";
        }

        protected void BtnCanel_Click(object sender, EventArgs e)
        {
            DivChangePsw.Style["display"] = "none";
        }

        protected void BtnCredit_Click(object sender, EventArgs e)
        {
            DivChangePsw.Style["display"] = "none";

            if (DivCredit.Style["display"] == "none")
                DivCredit.Style["display"] = "block";
            else
                DivCredit.Style["display"] = "none";

            GvCredit.PageIndex = 0;

            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from info in db.SignedActivity
                          // where info.studentID == Tool.studentID
                      where info.studentID == Session["ID"].ToString()
                      select info.activityID;

            if (res.Count() <= 0)
            {
                LinqDataSourceCredit.Where = "(activityState < 0)"; // 没有结果
                return;
            }

            string[] actIDs = res.ToArray();
            LinqDataSourceCredit.Where = "(activityID = \"" + res.First() + "\"";
            foreach (string actID in actIDs)
            {
                Console.WriteLine(actID);
                LinqDataSourceCredit.Where += "or activityID = \"" + actID + "\" ";
            }
            LinqDataSourceCredit.Where += ")";

            ActivityManagerDataContext.connectorCredit = LinqDataSourceCredit.Where.ToString(); // 存储默认查询条件
            ActivityManagerDataContext.connectorWhere = LinqDataSourceCredit.Where.ToString();

            int credit = 0;
            foreach (GridViewRow row in GvCredit.Rows)
            {
                credit += int.Parse(row.Cells[3].Text);
            }
            LblTotal.Text = "需得：40";

            foreach (GridViewRow row in GvCredit.Rows)
                row.Cells[4].Text = Convert.ToDateTime(row.Cells[4].Text).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        /// 学分详情
        /// 查学生已报名的所有活动，分为已发放/未发放
        /// 在上述活动条件下查询活动状态为：已完成（11）->已发放，其余->未发放
        /// 根据活动类别将活动显示分为三大类
        ///
        /// 活动类别1
        /// 活动名称1 活动开始日期1 获得学分1
        /// 活动名称2 活动开始日期2 获得学分2
        ///
        /// 活动类别2
        /// ……

        protected void Button1_Click(object sender, EventArgs e)
        {
            int index1 = DropDownList1.SelectedIndex;
            int index2 = DropDownList2.SelectedIndex;

            LinqDataSourceCredit.Where = Tool.LinqDataSourceCreditChange(index1, index2, ActivityManagerDataContext.connectorCredit);
            ActivityManagerDataContext.connectorWhere = LinqDataSourceCredit.Where;

            // 要第一次加载出gv再才能统计,要databound
            /* int credit = 0;
             foreach (GridViewRow row in GvCredit.Rows)
             {
                 credit += int.Parse(row.Cells[3].Text);
             }*/

            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from info in db.StudentIdentified
                          // where info.studentID == Tool.studentID
                      where info.studentID == Session["ID"].ToString()
                      select info;

            switch (index2)
            {
                case 0:
                    LblTotal.Text = "已得：" + (res.First().credit_1 + res.First().credit_2 + res.First().credit_3) + "   需得：40";
                    break;

                case 1:
                    LblTotal.Text = "已得：" + res.First().credit_1 + "   需得：10";
                    break;

                case 2:
                    LblTotal.Text = "已得：" + res.First().credit_2 + "   需得：10";
                    break;

                case 3:
                    LblTotal.Text = "已得：" + res.First().credit_3 + "   需得：20";
                    break;
            }
        }

        protected void GvCredit_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GvCredit.Rows)
                row.Cells[4].Text = Convert.ToDateTime(row.Cells[4].Text).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
    }
}