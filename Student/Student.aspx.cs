﻿using ActivityManager.App_Data;
using MathNet.Numerics.Distributions;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Controls;

namespace ActivityManager.Test
{
    public partial class StudentWebForm : System.Web.UI.Page
    {
        protected void BtnCheck_Click(object sender, EventArgs e)
        {
            /*活动报名详情页返回*/
            CheckActDiv.Style["display"] = "none";
            DivMask.Style["pointer-events"] = "auto";
            Debug.WriteLine("return:" + schoolConnector.Where);
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
            // 数据绑定完后

            Tool.FormatGridView((System.Web.UI.WebControls.GridView)sender, 8);
            Tool.UpdateActivityState((System.Web.UI.WebControls.GridView)sender);
            Tool.FormatActivity((System.Web.UI.WebControls.GridView)sender, Session["ID"].ToString());
        }

        protected void GvTemplate_DataBinding(object sender, EventArgs e)
        {
            /*数据绑定时,主要是根据系统时间更新活动状态和报名人数等*/
            // Tool.UpdateActivityState((GridView)sender);
            // Tool.FormatGridView((GridView)sender, 8);
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
            string actID = ((System.Web.UI.WebControls.GridView)sender).Rows[index].Cells[0].Text;

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

                if (a.ActivityState == 3)
                {
                    LblFail.Text = "审核不通过理由：" + a.FailReason;
                    LblFail.Height = 40;
                    LblFail.Visible = true;
                }

                LblState.Text += Tool.states[a.ActivityState];
                LblActName.Text += a.ActivityName;
                LblActInfo.Text += a.ActivityIntro;

                ActivityManagerDataContext db = new ActivityManagerDataContext();
                var res = from info in db.Place
                          where info.placeID == a.ActivityPlaceID
                          select info.placeName;
                LblPlace.Text += res.First();

                LblHoldDate.Text += a.HoldDate;
                LblSignDate.Text += a.SignStartDate;
                LblMaxSize.Text += a.MaxSigned;
                LblScore.Text += a.AvailableCredit;
                DivMask.Style["pointer-events"] = "none";
            }
            else if (e.CommandName == "appraise")
            {
                ActAppraise(actID);
                DivMask.Style["pointer-events"] = "none";
            }
            else if (e.CommandName == "checkIn")
            {
                DivCheckIn.Style["display"] = "block";
                DivMask.Style["pointer-events"] = "none";
                LblCheckInActID.Text = actID;
            }
            else
            {
                // Operation.SetOperation(e.CommandName, actID, Tool.studentID, (GridView)sender, schoolConnector);
                Operation.SetOperation(e.CommandName, actID, Session["ID"].ToString(), (System.Web.UI.WebControls.GridView)sender);
            }

            GvTemplate.DataBind();
        }

        protected void ActAppraise(string actID)
        {
            Session["Appraised"] = false;
            DivAppraise.Style["display"] = "block";

            ActivityManagerDataContext db = new ActivityManagerDataContext();

            var res = from info in db.ActivityAppraise
                      where info.activityID == actID && info.studentID == Session["ID"].ToString()
                      select info;

            MyActivity act = new MyActivity(actID);
            LblAppraise.Text = act.ActivityName;
            LblActID.Text = act.ActivityID;

            if (res.Any())
            {
                Response.Write("<script>alert('您已对该活动评价，请慎重考虑修改评价！')</script>");
                RblAppraise.SelectedIndex = (int)(res.First().credit) - 1;
                TxtAppraise.Text = res.First().appraise;
                Session["Appraised"] = true;
            }
        }

        protected void LbtnAllAct_Click(object sender, EventArgs e)
        {
            /*活动总览页面*/
            DivChangePsw.Style["dispaly"] = "none";
            DivMyInfoR.Style["display"] = "none";
            DivSearch.Style["display"] = "block";
            DivTopNov.Style["display"] = "block";
            DivBuildActTeam.Style["display"] = "none";

            DivAct.Style["display"] = "block";
            DivTeam.Style["display"] = "none";

            DivActTopNav.Style["display"] = "block";
            DivTeamTopNav.Style["display"] = "none";

            LinkButton1.Text = "全部活动"; // 要在按钮点击事件之前
            LinkButton2.Text = "可报名";

            DivAllAct.Style["background-color"] = "red";
            DivMyAct.Style["background-color"] = "#ccad9f";
            DivMyInfo.Style["background-color"] = "#ccad9f";
            DivActPlaza.Style["background-color"] = "#ccad9f";

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
            DivBuildActTeam.Style["display"] = "none";

            DivAct.Style["display"] = "block";
            DivTeam.Style["display"] = "none";

            DivActTopNav.Style["display"] = "block";
            DivTeamTopNav.Style["display"] = "none";

            // 更新导航条
            DivAllAct.Style["background-color"] = "#ccad9f";
            DivMyAct.Style["background-color"] = "red";
            DivMyInfo.Style["background-color"] = "#ccad9f";
            DivActPlaza.Style["background-color"] = "#ccad9f";

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
            DivActPlaza.Style["background-color"] = "#ccad9f";

            DivMyInfoR.Style["display"] = "block";
            DivTeamTopNav.Style["display"] = "none";

            // 隐藏不需要的模块
            DivTeam.Style["display"] = "none";
            DivBuildActTeam.Style["display"] = "none";
            DivSearch.Style["display"] = "none";
            DivTopNov.Style["display"] = "none";
            DivChangePsw.Style["display"] = "none";
            schoolConnector.Where = "activityState < 0"; // 如果把GV隐藏再显示,大小会出问题

            // MyImage.ImageUrl = "~/image/users/" + Tool.studentID + ".jpg";
            MyImage.ImageUrl = "~/image/users/" + Session["ID"] + ".jpg";

            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res1 = from info in db.StudentIdentified
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
                     * 先用学生ID把所有报名的活动ID存在数组里
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
            // Session["ID"] = 7020820312;
            Tool.curUser = 2;

            if (Session["ID"] == null)
            {
                Server.Transfer("../Login.aspx");
                // Response.Write("<script>alert('请登录后再访问！');</script>");
                return;
            }

            Session["Info"] = "1"; // 允许二次认证
            if (!IsPostBack)
            {
                // 第一次加载时
                Tool.FormatActivityHeader(GvTemplate); // 更新表头
                schoolConnector.Where = "(activityState >= 5 and activityState <= 8)";
                ActivityManagerDataContext.connectorWhere = schoolConnector.Where.ToString();
                Tool.UpdataAllActivityState(); // 更新所有活动状态，数据量太大，所以之后只在数据绑定时更新Gv当前页
            }
            else
            {
                schoolConnector.Where = ActivityManagerDataContext.connectorWhere;
                Tool.FormatGridView(GvTemplate, 8); // 不是这
            }

            // Tool.UpdateActivityState(GvTemplate);

            Debug.WriteLine("Page_Load:" + schoolConnector.Where);
        }

        protected void MyImage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (DivUploadImage.Style["display"] == "none")
                DivUploadImage.Style["display"] = "block";
            else
                DivUploadImage.Style["display"] = "none";
        }

        protected void Esc_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Server.Transfer("../Login.aspx");
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
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
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

            LinqDataSourceCredit.Where = LinqDataSourceCreditChange(index1, index2, ActivityManagerDataContext.connectorCredit);
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
            // 格式化时间显示
            foreach (GridViewRow row in GvCredit.Rows)
                row.Cells[4].Text = Convert.ToDateTime(row.Cells[4].Text).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        protected void ImageButtonInfo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Write("<script>if(confirm('是否信息有误需要二次认证？'))location.href = 'StudentIdentifying.aspx'</script>;");
        }

        protected void BtnAppraiseCommit_Click(object sender, EventArgs e)
        {
            if (RblAppraise.SelectedIndex == -1 || TxtAppraise.Text == "")
            {
                Response.Write("<script>alert('请评分且评价后提交！');</script>");
                return;
            }

            // < script > alert('原密码输入错误！');</ script >

            ActivityManagerDataContext db = new ActivityManagerDataContext();

            ActivityAppraise activityAppraise = new ActivityAppraise()
            {
                activityID = LblActID.Text.ToString().Trim(),
                studentID = Session["ID"].ToString(),
                credit = RblAppraise.SelectedIndex + 1,
                appraise = TxtAppraise.Text.ToString().Trim(),
            };

            if ((Boolean)Session["Appraised"] == true)
            {
                // 已评价，进行修改
                var res = from info in db.ActivityAppraise
                          where info.activityID == activityAppraise.activityID && info.studentID == activityAppraise.studentID
                          select info;
                res.First().credit = activityAppraise.credit;
                res.First().appraise = activityAppraise.appraise;

                db.SubmitChanges();
            }
            else
            {
                // 第一次评价
                db.ActivityAppraise.InsertOnSubmit(activityAppraise);
                db.SubmitChanges();
            }

            Response.Write("<script> alert('感谢您的评价！');</script>");
            DivAppraise.Style["display"] = "none";
        }

        protected void BtnAppraiseCancel_Click(object sender, EventArgs e)
        {
            DivAppraise.Style["display"] = "none";
            DivMask.Style["pointer-events"] = "auto";
        }

        public static string LinqDataSourceCreditChange(int index1, int index2, string connectWhere)
        {
            switch (index1)
            {
                case 0:
                    break;

                case 1:
                    connectWhere += "and (activityState = 11)";
                    break;

                case 2:
                    connectWhere += "and (activityState != 11)";
                    break;

                default: break;
            }

            switch (index2)
            {
                case 0:
                    break;

                case 1:
                    connectWhere += "and (activityType = 1)";
                    break;

                case 2:
                    connectWhere += "and (activityType = 2)";
                    break;

                case 3:
                    connectWhere += "and (activityType = 3)";
                    break;

                default: break;
            }

            return connectWhere;
        }

        protected void BtnCheckInCommit_Click(object sender, EventArgs e)
        {
            if (TxtCheckIn.Text == "")
            {
                Response.Write("<script>alert('请输入签到码！');</script>");
                return;
            }

            string actID = LblCheckInActID.Text.Trim();

            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

            try
            {
                var res = from info in db.SignedActivity
                          where info.studentID == Session["ID"].ToString() && info.activityID == actID
                          select info;

                if (TxtCheckIn.Text.ToString().Trim() == a.CheckInCode)
                {
                    if (res.First().checkIn == 1)
                    {
                        Response.Write("<script>alert('已签入，请勿重复签入！');</script>");
                        return;
                    }

                    res.First().checkIn = 1;
                }
                else if (TxtCheckIn.Text.ToString().Trim() == a.CheckOutCode)
                {
                    if (res.First().checkOut == 1)
                    {
                        Response.Write("<script>alert('已签出，请勿重复签出！');</script>");
                        return;
                    }
                    res.First().checkOut = 1;
                }
                else
                {
                    Response.Write("<script>alert('签到码错误，请向主办方确认！');</script>");
                    return;
                }

                db.SubmitChanges();
            }
            catch
            {
                Response.Write("<script>alert('签到失败，请稍后重试！');</script>");
                return;
            }

            Response.Write("<script>alert('签到成功！');</script>");
            DivMask.Style["pointer-events"] = "auto";
            DivCheckIn.Style["display"] = "none";
        }

        protected void BtnCheckInCancel_Click(object sender, EventArgs e)
        {
            DivCheckIn.Style["display"] = "none";
            DivMask.Style["pointer-events"] = "auto";
        }

        protected void LbtnActPlaza_Click(object sender, EventArgs e)
        {
            DivChangePsw.Style["dispaly"] = "none";
            DivMyInfoR.Style["display"] = "none";
            DivSearch.Style["display"] = "block";
            DivTopNov.Style["display"] = "none";
            DivBuildActTeam.Style["display"] = "block";

            DivAct.Style["display"] = "none";
            DivTeam.Style["display"] = "block";

            DivActTopNav.Style["display"] = "none";
            DivTeamTopNav.Style["display"] = "block";

            DivAllAct.Style["background-color"] = "#ccad9f";
            DivMyAct.Style["background-color"] = "#ccad9f";
            DivMyInfo.Style["background-color"] = "#ccad9f";
            DivActPlaza.Style["background-color"] = "red";

            GvTeam.DataBind();
            // Tool.FormatGridView(GvTeam, 7);
        }

        protected void BtnBuildTeamCancel_Click(object sender, EventArgs e)
        {
            DivBuildTeam.Style["display"] = "none";
            DivMask.Style["pointer-events"] = "auto";

            DdlBuildTeamAct.SelectedIndex = -1;
            DdlBulidTeamVolume.SelectedIndex = -1;
            TxtTeamName.Text = null;
            RblBuildTeam.SelectedIndex = -1;

            GvTeam.DataBind();
            // Tool.FormatGridView(GvTeam, 7);
        }

        protected void DdlBuildTeamAct_SelectedIndexChanged(object sender, EventArgs e)
        {
            string actID = DdlBuildTeamAct.SelectedValue;

            SetDdlBulidTeamVolume(actID);
        }

        /// <summary>
        /// 队伍创建者默认为队长，captain == 1
        /// 队长选择队伍成员加入是否需要审核 audit == 0/1
        /// 审核权由队长拥有
        /// 当需要审核时，只有audit == 1 and member == 1 才能算是小队成员
        /// 无需审核，audit == 0 and member == 1 是小队成员
        /// 待审核成员 audit == 1 and member == 0
        /// audit == 0 and member == 0 不创建这种情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnBuildTeamSubmit_Click(object sender, EventArgs e)
        {
            string mes = "";
            if (TxtTeamName.Text.Length <= 0)
            {
                mes += "-请输入队伍名称！\\r";
            }

            if (RblBuildTeam.SelectedIndex == -1)
            {
                mes += "-请选择是否启用审核功能\\r";
            }

            if (mes != "")
            {
                Response.Write("<script>alert('" + mes + "');</script>");
                Tool.FormatGridView(GvTeam, 7);
                return;
            }

            string actID = DdlBuildTeamAct.SelectedValue.ToString();
            string teamName = TxtTeamName.Text.Trim();
            int volume = int.Parse(DdlBulidTeamVolume.SelectedValue);
            int audit = int.Parse(RblBuildTeam.SelectedValue);

            ActivityManagerDataContext db = new ActivityManagerDataContext();

            // 判断是否单人报名
            try
            {
                var resOne = from info in db.SignedActivity
                             where info.activityID == actID && info.studentID == Session["ID"].ToString()
                             select info;

                if (resOne.Any())
                {
                    Response.Write("<script>alert('您已单人报名该活动,请取消报名后再进行申请！');</script>");
                    Tool.FormatGridView(GvTeam, 7);
                    return;
                }
            }
            catch
            {
                Response.Write("<script>alert('报名信息查询错误，请稍后重试！');</script>");
                Tool.FormatGridView(GvTeam, 7);
                return;
            }

            // 判断队伍名称是否重名
            try
            {
                var resTeamName = from info in db.ActivitySignTeam
                                  where info.activityID == actID && info.teamName == teamName
                                  select info.teamName;

                if (resTeamName.Any())
                {
                    Response.Write("<script>alert('队伍名称已使用，请重试！');</script>");
                    Tool.FormatGridView(GvTeam, 7);
                    return;
                }
            }
            catch
            {
                Response.Write("<script>alert('队伍名称查询错误，请稍后重试！');</script>");
                Tool.FormatGridView(GvTeam, 7);
                return;
            }

            // 判断是否已经有队伍
            try
            {
                var resHad = from info in db.ActivitySignTeam
                             where info.activityID == actID && info.studentID == Session["ID"].ToString()
                             select info;
                if (resHad.Any())
                {
                    Response.Write("<script>alert('您已组队报名该活动,请解散或退出原队伍后再进行申请！');</script>");
                    Tool.FormatGridView(GvTeam, 7);
                    return;
                }
            }
            catch
            {
                Response.Write("<script>alert('队伍信息查询错误，请稍后重试！');</script>");
                Tool.FormatGridView(GvTeam, 7);
                return;
            }

            string teamID = "";
            try
            {
                // 创建团队ID
                var res = from info in db.ActivitySignTeam
                          where info.teamID.StartsWith(actID)
                          select info;

                if (res.Count() > 0)
                    teamID = (long.Parse(res.ToList().Last().teamID) + 1).ToString(); // 如果不是该活动第一支队伍,取最后一个创建的团队ID+1
                else
                    teamID = actID + "01"; // 如果是第一次创建,生成01

                ActivitySignTeam team = new ActivitySignTeam()
                {
                    teamID = teamID,
                    activityID = actID,
                    teamName = teamName,
                    studentID = Session["ID"].ToString(),
                    captain = 1,
                    audit = audit,
                    member = 1,
                    volume = volume,
                };

                db.ActivitySignTeam.InsertOnSubmit(team);
                db.SubmitChanges();
            }
            catch
            {
                Response.Write("<script>alert('创建队伍ID错误，请稍后重试！\\r错误ID" + teamID + "');</script>");
                // Tool.FormatGridView(GvTeam, 7);
                return;
            }

            // GvTeam.DataBind();

            BtnBuildTeamCancel_Click(sender, e);
        }

        protected void BtnBuildTeam_Click(object sender, EventArgs e)
        {
            DivBuildTeam.Style["display"] = "block";
            DivMask.Style["pointer-events"] = "none";

            // 绑定可团队报名且状态为等待报名或报名中的活动
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from infoTeam in db.ActivityEnableTeam
                      join infoAct in db.Activity on infoTeam.activityID equals infoAct.activityID into temp
                      from info in temp
                      where info.activityState == 5 || info.activityState == 6
                      select info;

            DdlBuildTeamAct.Items.Clear();
            DdlBuildTeamAct.DataSource = res;
            DdlBuildTeamAct.DataTextField = "activityName";
            DdlBuildTeamAct.DataValueField = "activityID";
            DdlBuildTeamAct.DataBind();

            SetDdlBulidTeamVolume(res.First().activityID);
            GvTeam.DataBind();
        }

        private void SetDdlBulidTeamVolume(string actID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from info in db.ActivityEnableTeam
                      where info.activityID == actID
                      select info;

            int minVolume = res.First().minVolume;
            int maxVolume = res.First().maxVolume;

            DdlBulidTeamVolume.Items.Clear();
            for (int i = minVolume; i <= maxVolume; i++)
            {
                DdlBulidTeamVolume.Items.Add(i.ToString());
            }
        }

        protected void GvTeam_DataBound(object sender, EventArgs e)
        {
            // Response.Write("<script>alert('签到成功！');</script>");

            ActivityManagerDataContext db = new ActivityManagerDataContext();

            foreach (GridViewRow row in GvTeam.Rows)
            {
                MyActivity act = new MyActivity(row.Cells[2].Text);
                string actID = row.Cells[2].Text;
                row.Cells[3].Text = act.ActivityName;
                string teamID = row.Cells[1].Text;

                // 设置是否需要审核
                int audit = int.Parse(row.Cells[7].Text);
                if (row.Cells[7].Text == "1")
                    row.Cells[7].Text = "是";
                else
                    row.Cells[7].Text = "否";

                int n = row.Cells.Count;

                // 队长ID在此条里
                string captainID = row.Cells[5].Text;
                ((ImageButton)row.Cells[10].Controls[0]).ImageUrl = "~/image/users/" + captainID + ".jpg";

                var resCap = from info in db.StudentIdentified
                             where info.studentID == captainID
                             select info.studentName;
                row.Cells[10].ToolTip = captainID + " " + resCap.First() + " 队长";

                // 队长操作
                if (captainID == Session["ID"].ToString())
                {
                    var resSigned = from info in db.SignedActivity
                                    where info.studentID == Session["ID"].ToString() && info.activityID == actID
                                    select info;

                    // 可报名
                    if (act.ActivityState == 6)
                    {
                        // 已报名
                        if (resSigned.Any())
                        {
                            ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "取消<br/>报名";
                            ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "teamSignCancel";
                        }
                        // 未报名
                        else
                        {
                            ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "队伍<br/>报名";
                            ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "teamSign";
                        }
                    }
                }

                // 已入队成员操作
                var resIn = from info in db.ActivitySignTeam
                            where info.teamID == teamID && info.studentID == Session["ID"].ToString()
                            select info;

                if (resIn.Any() && act.ActivityState != 8)
                {
                    if (captainID == Session["ID"].ToString())
                    {
                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "解散<br/>队伍";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "delTeam";
                    }
                    else
                    {
                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "退出<br/>队伍";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "outTeam";
                    }
                }

                // 关闭超出容量的报名ImageButton
                int extraVolume = 11 - int.Parse(row.Cells[9].Text);
                for (int i = 0; i < extraVolume; i++)
                {
                    // row.Cells[n - i - 2].Visible = false;
                    // row.Cells[n - i - 2].Style["display"] = "none";
                    row.Cells[n - i - 3].Enabled = false;
                    // row.Cells[n - i - 2].ToolTip = "未启用";
                    row.Cells[n - i - 3].Style["opacity"] = "0%";
                }

                // 加载成员
                var res = from info in db.ActivitySignTeam
                          where info.teamID == teamID && info.captain != 1
                          select info;

                int begin = 1;
                ((ImageButton)row.Cells[10].Controls[0]).CommandName = "checkMember_10";
                ((ImageButton)row.Cells[10].Controls[0]).BorderColor = System.Drawing.Color.GreenYellow;
                ((ImageButton)row.Cells[10].Controls[0]).BorderWidth = 1;
                ((ImageButton)row.Cells[10].Controls[0]).BorderStyle = BorderStyle.Outset;
                foreach (var member in res)
                {
                    ((ImageButton)row.Cells[10 + begin].Controls[0]).ImageUrl = "~/image/users/" + member.studentID + ".jpg";

                    var resStuName = from info in db.StudentIdentified
                                     where info.studentID == member.studentID
                                     select info.studentName;

                    string toolTip = member.studentID + " " + resStuName.First();

                    ((ImageButton)row.Cells[10 + begin].Controls[0]).CommandName = "checkMember_" + (10 + begin).ToString();
                    // ((ImageButton)row.Cells[10 + begin].Controls[0]).CommandArgument = member.studentID;

                    // 设置待审核学生样式
                    if (audit == 1 && member.member == 0)
                    {
                        toolTip += " 待审核";
                        // ((ImageButton)row.Cells[10 + begin].Controls[0]).Style["opacity"] = "60%";
                        ((ImageButton)row.Cells[10 + begin].Controls[0]).BorderColor = System.Drawing.Color.OrangeRed;
                        ((ImageButton)row.Cells[10 + begin].Controls[0]).BorderWidth = 1;
                        ((ImageButton)row.Cells[10 + begin].Controls[0]).BorderStyle = BorderStyle.Outset;
                    }
                    else
                    {
                        toolTip += " 已入队";
                        ((ImageButton)row.Cells[10 + begin].Controls[0]).BorderColor = System.Drawing.Color.GreenYellow;
                        ((ImageButton)row.Cells[10 + begin].Controls[0]).BorderWidth = 1;
                        ((ImageButton)row.Cells[10 + begin].Controls[0]).BorderStyle = BorderStyle.Outset;
                    }

                    row.Cells[10 + begin].ToolTip = toolTip;

                    begin++;
                }
            }

            Tool.FormatGridView((System.Web.UI.WebControls.GridView)sender, 7);
        }

        protected void GvTeam_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = int.Parse(e.CommandArgument.ToString());

            string teamID = GvTeam.Rows[rowIndex].Cells[1].Text;
            string actID = GvTeam.Rows[rowIndex].Cells[2].Text;
            string teamName = GvTeam.Rows[rowIndex].Cells[4].Text;
            // int audit = int.Parse(GvTeam.Rows[rowIndex].Cells[6].Text);
            int audit = GvTeam.Rows[rowIndex].Cells[7].Text == "是" ? 1 : 0;
            int volume = int.Parse(GvTeam.Rows[rowIndex].Cells[9].Text);

            if (e.CommandName == "Page") return;

            if (e.CommandName == "memberSign")
            {
                MemberSign(teamID, teamName, actID, audit, volume);
            }
            else if (e.CommandName.StartsWith("checkMember"))
            {
                int colIndex = int.Parse(e.CommandName.Split('_')[1]);
                string stuID = GvTeam.Rows[rowIndex].Cells[colIndex].ToolTip.Split(' ')[0];
                int needAudit = GvTeam.Rows[rowIndex].Cells[colIndex].ToolTip.Split(' ')[2] == "待审核" ? 1 : 0;
                if (GvTeam.Rows[rowIndex].Cells[colIndex].ToolTip.Split(' ')[2] == "队长") needAudit = -1;
                // Response.Write("<script>alert('" + stuID + "');</script>");

                int isCaptain = 0;
                if (Session["ID"].ToString() == GvTeam.Rows[rowIndex].Cells[5].Text)
                {
                    isCaptain = 1;
                }

                CheckMember(stuID, teamID, needAudit, isCaptain);
            }
            else if (e.CommandName == "teamSign")
            {
                TeamSign(teamID, actID);
            }
            else if (e.CommandName == "outTeam")
            {
                OutTeam(teamID, Session["ID"].ToString());
            }
            else if (e.CommandName == "delTeam")
            {
                DelTeam(teamID, actID);
            }
            else if (e.CommandName == "teamSignCancel")
            {
                TeamSignCancel(teamID, actID);
            }

            GvTeam.DataBind();
        }

        private void TeamSignCancel(string teamID, string actID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var resTeam = from info in db.ActivitySignTeam
                          where info.teamID == teamID && info.activityID == actID
                          select info;

            Response.Write("<script>alert('您为《" + resTeam.First().teamName + "》队长！\\r将取消所有小队成员报名信息！')</script>");
            try
            {
                foreach (var member in resTeam)
                {
                    var resSignCancel = from info in db.SignedActivity
                                        where info.activityID == actID && info.studentID == member.studentID
                                        select info;

                    db.SignedActivity.DeleteOnSubmit(resSignCancel.First());
                }
                db.SubmitChanges();

                MyActivity act = new MyActivity(actID);
                act.Signed -= resTeam.Count();
                act.Update();
            }
            catch
            {
                Response.Write("<script>alert('队伍取消报名错误，请稍后重试！')</script>");
                return;
            }

            Response.Write("<script>alert('队伍取消报名成功！')</script>");
        }

        private void TeamSign(string teamID, string actID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();

            var res = from info in db.ActivitySignTeam
                      where info.teamID == teamID && info.activityID == actID
                      select info;

            var resAudit = from info in res
                           where info.audit == 1 && info.member == 0
                           select info;

            if (resAudit.Any())
            {
                // 存在待审核成员
                Response.Write("<script>alert('您的队伍仍存在待审核成员！')</script>");
                return;
            }

            // 活动团队报名最低人数限制
            var resVolume = from info in db.ActivityEnableTeam
                            where info.activityID == actID
                            select info.minVolume;

            MyActivity act = new MyActivity(actID);

            int actTeamMinVlome = resVolume.First(); // 团队报名最低人数限制
            int teamSigned = res.Count(); // 团队已报名人数
            int teamVolume = res.First().volume; // 团队容量

            if (act.Signed >= act.MaxSigned)
            {
                // 活动已报满
                Response.Write("<script>alert('抱歉！此活动人数已满！')</script>");
                return;
            }

            if (act.Signed + teamSigned >= act.MaxSigned)
            {
                // 活动剩余容量不足
                Response.Write("<script>alert('抱歉！此活动剩余容量小于您的小队人数！')</script>");
                return;
            }

            if (teamSigned < teamVolume && teamSigned < actTeamMinVlome)
            {
                // 团队未满时
                // 当团队人数低于活动团队报名最低人数限制
                Response.Write("<script>alert('队伍人数低于活动团队报名最低人数限制，请至少组队" + actTeamMinVlome + "人后进行报名！');</script>");
                return;
            }

            // 获取场地名称
            var resPlaceName = from info in db.Place
                               where info.placeID == act.ActivityPlaceID
                               select info.placeName;
            string placeName = resPlaceName.First();

            int signed = act.Signed; // 已报名人数
            List<SignedActivity> signedActivityList = new List<SignedActivity>();

            try
            {
                foreach (var stu in res)
                {
                    // 获取学生信息
                    var resStu = from info in db.StudentIdentified
                                 where info.studentID == stu.studentID
                                 select info;

                    string studentName = resStu.First().studentName;
                    string phone = resStu.First().phone;

                    string Text =
                        "活动名称：" + act.ActivityName + "\\r" +
                        "举办地点：" + placeName + "\\r" +
                        "举办时间：" + act.HoldDate + " " + act.HoldStart + ":00 至 " + act.HoldDate + " " + act.HoldEnd + ":00\\r" +
                        "报名者姓名：" + studentName + "\\r" +
                        "报名者学号：" + stu.studentID + "\\r" +
                        "报名者联系方式：" + phone + "\\r" +
                        "**如信息有误，请于‘我的信息’重新认证**";

                    // 报名
                    signed++;

                    SignedActivity signedActivity = new SignedActivity()
                    {
                        activityID = actID,
                        studentID = stu.studentID,
                    };
                    signedActivityList.Add(signedActivity);

                    Response.Write("<script>alert('" + Text + "')</script>");
                }
            }
            catch
            {
                Response.Write("<script>alert('获取成员信息失败，请稍后重试！');</script>");
                return;
            }

            try
            {
                act.Signed = signed;
                act.Update();
                db.SignedActivity.InsertAllOnSubmit(signedActivityList);

                db.SubmitChanges();
            }
            catch
            {
                Response.Write("<script>alert('队伍报名错误，请稍后重试！');</script>");
                return;
            }

            Response.Write("<script>alert('队伍报名成功！');</script>");
        }

        private void DelTeam(string teamID, string actID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from info in db.ActivitySignTeam
                      where info.teamID == teamID && info.activityID == actID
                      select info;
            try
            {
                foreach (var member in res)
                {
                    db.ActivitySignTeam.DeleteOnSubmit(member);
                    db.SubmitChanges();
                }
            }
            catch
            {
                Response.Write("<script>alert('队伍解散错误，请稍后重试！');</script>");
                return;
            }
            Response.Write("<script>alert('队伍解散成功！');</script>");
        }

        private void OutTeam(string teamID, string stuID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from info in db.ActivitySignTeam
                      where info.teamID == teamID && info.studentID == stuID
                      select info;
            try
            {
                db.ActivitySignTeam.DeleteOnSubmit(res.First());
                db.SubmitChanges();
            }
            catch
            {
                Response.Write("<script>alert('离队失败，请稍后重试！');</script>");
                return;
            }
        }

        private void CheckMember(string stuID, string teamID, int needAudit, int isCaptain)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();

            try
            {
                var res = from info in db.StudentIdentified
                          where info.studentID == stuID
                          select info;

                var stu = res.First();

                LblMemberTeamID.Text = teamID;
                LblMemberName.Text = stu.studentName;
                LblMemberStudentID.Text = stuID;
                LblMemberClass.Text = stu.major + stu.@class;
                LblMemberGender.Text = stu.gender;
                LblMemberPhone.Text = stu.phone;

                DivMemberInfo.Style["display"] = "block";
            }
            catch
            {
                Response.Write("<script>alert('查看失败，请稍后重试！');</script>");
                return;
            }

            // 成员状态
            if (needAudit == 1)
            {
                LblMemberState.Text = "待审核";
            }
            else if (needAudit == 0)
            {
                LblMemberState.Text = "已入队";
            }
            else if (needAudit == -1)
            {
                LblMemberState.Text = "队长";
            }

            // 队长操作
            if (isCaptain == 1)
            {
                if (needAudit == 1)
                {
                    // 待审核操作
                    BtnAuditAgree.Visible = true;
                    BtnAuditReject.Visible = true;

                    BtnAuditAgree.Text = "通过";
                    BtnAuditReject.Text = "拒绝";
                }
                else if (needAudit == 0)
                {
                    // 已入队操作
                    BtnAuditAgree.Visible = false;
                    BtnAuditReject.Text = "移出";
                }
                else if (needAudit == -1)
                {
                    BtnAuditAgree.Visible = false;
                    BtnAuditReject.Visible = false;
                }
            }
            else
            {
                BtnAuditAgree.Visible = false;
                BtnAuditReject.Visible = false;
            }

            DivMask.Style["pointer-events"] = "none";
        }

        private void MemberSign(string teamID, string teamName, string actID, int audit, int volume)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();

            var resTeam = from info in db.ActivitySignTeam
                          where info.activityID == actID && info.studentID == Session["ID"].ToString()
                          select info;

            if (resTeam.Any())
            {
                Response.Write("<script>alert('您已组队报名该活动,请解散或退出原队伍后再进行申请！');</script>");
                return;
            }

            var res = from info in db.ActivitySignTeam
                      where info.activityID == actID && info.studentID == Session["ID"].ToString()
                      select info;

            if (res.Any())
            {
                Response.Write("<script>alert('您已单人报名该活动,请取消报名后再进行申请！');</script>");
                return;
            }

            int member = audit == 1 ? 0 : 1;
            ActivitySignTeam signTeam = new ActivitySignTeam()
            {
                teamID = teamID,
                teamName = teamName,
                activityID = actID,
                studentID = Session["ID"].ToString(),
                captain = 0,
                audit = audit,
                member = member,
                volume = volume,
            };

            try
            {
                db.ActivitySignTeam.InsertOnSubmit(signTeam);
                db.SubmitChanges();
            }
            catch
            {
                Response.Write("<script>alert('入队错误，请稍后重试！');</script>");
                return;
            }

            if (audit == 1)
            {
                Response.Write("<script>alert('已提交申请，等待队长审核！\\r请注意，团队报名活动后将无法退出小队或自行取消报名');</script>");
            }
            else
            {
                Response.Write("<script>alert('入队成功！\\r请注意，团队报名活动后将无法退出小队或自行取消报名');</script>");
            }
        }

        protected void BtnMemberBack_Click(object sender, EventArgs e)
        {
            DivMask.Style["pointer-events"] = "block";
            DivMemberInfo.Style["display"] = "none";
            GvTeam.DataBind();
        }

        protected void BtnAuditAgree_Click(object sender, EventArgs e)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            try
            {
                var res = from info in db.ActivitySignTeam
                          where info.studentID == LblMemberStudentID.Text && info.teamID == LblMemberTeamID.Text
                          select info;
                if (res.Any())
                {
                    res.First().member = 1;
                    db.SubmitChanges();
                }
            }
            catch
            {
                Response.Write("<script>alert('审核错误，请稍后重试！');</script>");
                return;
            }

            Response.Write("<script>alert('审核通过，成员已入队！');</script>");
            BtnMemberBack_Click(sender, e);
        }

        protected void BtnAuditReject_Click(object sender, EventArgs e)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();

            try
            {
                var res = from info in db.ActivitySignTeam
                          where info.studentID == LblMemberStudentID.Text && info.teamID == LblMemberTeamID.Text
                          select info;

                db.ActivitySignTeam.DeleteOnSubmit(res.First());
                db.SubmitChanges();
            }
            catch
            {
                Response.Write("<script>alert('审核错误，请稍后重试！');</script>");
                return;
            }

            if (BtnAuditReject.Text == "拒绝")
            {
                Response.Write("<script>alert('已拒绝成员入队！');</script>");
            }
            else if (BtnAuditReject.Text == "移出")
            {
                Response.Write("<script>alert('已将成员移出队伍！');</script>");
            }

            BtnMemberBack_Click(sender, e);
        }

        protected void BtnTeamSearch_Click(object sender, EventArgs e)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from infoTeam in db.ActivitySignTeam
                      where infoTeam.captain == 1
                      select infoTeam;

            string actName = TxtTeamActName.Text.ToString().Trim();
            if (actName != "" && actName != null)
            {
                res = from info in res
                      where info.ActivityEnableTeam.Activity.activityName == actName
                      select info;
            }

            string teamName = TxtNavTeamName.Text.ToString().Trim();
            if (teamName != "" && teamName != null)
            {
                res = from info in res
                      where info.teamName == teamName
                      select info;
            }

            if (DdlTeamAudit.SelectedValue != "-1")
            {
                res = from info in res
                      where info.audit == int.Parse(DdlTeamAudit.SelectedValue)
                      select info;
            }

            if (DdlTeamActType.SelectedValue != "-1")
            {
                res = from info in res
                      where info.ActivityEnableTeam.Activity.activityType == int.Parse(DdlTeamActType.SelectedValue)
                      select info;
            }

            GvTeam.DataSourceID = null;
            GvTeam.DataSource = res;
            GvTeam.DataBind();
        }

        protected void BtnTeamReset_Click(object sender, EventArgs e)
        {
            TxtTeamActName.Text = null;
            TxtNavTeamName.Text = null;
            DdlTeamAudit.SelectedIndex = -1;
            DdlTeamActType.SelectedIndex = -1;

            GvTeam.DataSource = null;
            GvTeam.DataSourceID = TeamLinqDataSource.ID;
            GvTeam.DataBind();
        }
    }
}