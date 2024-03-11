using ActivityManager.App_Data;
using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace ActivityManager.Test
{
    public partial class StudentWebForm : System.Web.UI.Page
    {
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
             */
            if (schoolConnector.Where == "") schoolConnector.Where = "activityState >= 5 and activityState <= 8"; 
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            Tool.FormatActivity((GridView)sender);

            /*空白行*/
            //if (GvTemplate.Rows.Count != 0 && GvTemplate.Rows.Count != GvTemplate.PageSize)
            //{
            //    // 如果分页有数据但不等于pagesize
            //    Control table = GvTemplate.Controls[0];
            //    if (table != null)
            //    {
            //        for (int i = 0; i < GvTemplate.PageSize - GvTemplate.Rows.Count; i++)
            //        {
            //            int rowIndex = GvTemplate.Rows.Count + i + 1;
            //            GridViewRow row = new GridViewRow(rowIndex, -1, DataControlRowType.Separator,DataControlRowState.Normal);

            //            row.BackColor = (rowIndex % 2 == 0) ? System.Drawing.Color.White : System.Drawing.Color.WhiteSmoke;
            //            for (int j = 0; j < GvTemplate.Columns.Count; j++)
            //            {
            //                TableCell cell = new TableCell();
            //                cell.Text = "&nbsp";
            //                row.Controls.Add(cell);
            //            }
            //            table.Controls.AddAt(rowIndex, row);
            //        }
            //    }
            //}
        }

        protected void commit_Click(object sender, EventArgs e)
        {
            /*查询功能*/
            schoolConnector.Where = null;
            schoolConnector.Where = "activityState >= 5 and activityState <= 8";

            string s1 = name.Text.Trim();
            string s2 = org.Text.Trim();
            string s3 = state.SelectedValue.Trim();

            if (s1 != "")
            {
                // 根据活动名称查询
                schoolConnector.Where += " and activityName = \"" + s1 + "\"";
            }


            if (s2 != "")
            {
                // 根据组织名称查询,需转ID
                ActivityManagerDataContext db = new ActivityManagerDataContext();
                var res = from org in db.Organization
                          where org.organizationName == s2
                          select org;

                if (res.Any())
                    schoolConnector.Where += " and activityOrgID = \"" + res.First().organizationID.ToString() + "\"";
            }

            if (s3 != "")
            {
                // 根据活动状态查询
                if (s3 != "0")
                    schoolConnector.Where += " and activityState = " + s3;
            }
        }

        protected void flush_Click(object sender, EventArgs e)
        {
            /*重置页面*/
            name.Text = null;
            org.Text = null;
            state.SelectedIndex = 0;

            schoolConnector.Where = null;
            schoolConnector.Where = "activityState >= 5 and activityState <= 8";
        }

        protected void GvTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            /*
             * 报名活动详情确认页面???????
             * 需要传gv的数据
             * 很重要
             */
            if (GvTemplate.PageSize > ((GridView)sender).Rows.Count) return;

            int index = int.Parse(e.CommandArgument.ToString());
            string actID = ((GridView)sender).Rows[index].Cells[0].Text;

            if (e.CommandName == "check")
            {
                // 查看操作
                CheckActDiv.Visible = true;

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
                Operation.SetOperation(e.CommandName, actID, Tool.studentID, (GridView)sender, schoolConnector);
            }
        }

        protected void BtnCheck_Click(object sender, EventArgs e)
        {
            /*活动报名详情页返回*/
            CheckActDiv.Visible = false;
        }

        protected void GvTemplate_DataBinding(object sender, EventArgs e)
        {
            /*数据更新,主要是活动状态和报名人数等*/
            Tool.UpdateActivityState((GridView)sender);
        }
        protected void LbtnAllAct_Click(object sender, EventArgs e)
        {
            /*全部活动页面*/
            DivSearch.Style["display"] = "block";
            DivTopNov.Style["display"] = "block";

            LinkButton1.Text = "全部活动"; // 要在按钮点击事件之前
            LinkButton2.Text = "可报名";

            LinkButton1_Click(sender, e);
            DivAllAct.Style["background-color"] = "red";
            DivMyAct.Style["background-color"] = "#ccad9f";
            DivMyInfo.Style["background-color"] = "#ccad9f";
        }

        protected void LbtnMyAct_Click(object sender, EventArgs e)
        {
            /*
             * 我的活动页面
             * 如何将Liked,Signed和Activity表进行联合查询
             */

            DivSearch.Style["display"] = "block";
            DivTopNov.Style["display"] = "block";

            // 更新导航条
            DivAllAct.Style["background-color"] = "#ccad9f";
            DivMyAct.Style["background-color"] = "red";
            DivMyInfo.Style["background-color"] = "#ccad9f";

            LinkButton1.Text = "已报名";
            LinkButton2.Text = "已收藏";

            // 默认选中已报名
            LinkButton1_Click(sender, e);
        }

        protected void LbtnMyInfo_Click(object sender, EventArgs e)
        {
            /*我的信息页面*/
            DivAllAct.Style["background-color"] = "#ccad9f";
            DivMyAct.Style["background-color"] = "#ccad9f";
            DivMyInfo.Style["background-color"] = "red";

            // 隐藏不需要的模块
            DivSearch.Style["display"] = "none";
            DivTopNov.Style["display"] = "none";
            schoolConnector.Where = "activityState < 0"; // 如果把GV隐藏再显示,大小会出问题
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            /*更新按钮1*/
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
                    schoolConnector.Where = "activityState >= 5"; // 未调试
                    break;

                case "已报名":                  
                    /*
                     * 查询test1
                     * 先用学生ID把所有收藏的活动ID存在数组里
                     * 再给where循环添加条件
                     */

                    ActivityManagerDataContext db = new ActivityManagerDataContext();
                    var res = from info in db.SignedActivity
                              where info.studentID == Tool.studentID
                              select info.activityID;

                    if (res.Count() <= 0)
                    {
                        schoolConnector.Where = "activityState < 0"; // 没有结果
                        break;
                    }

                    string[] actIDs = res.ToArray();
                    schoolConnector.Where = "activityID = \"" + res.First() + "\"";
                    foreach (string actID in actIDs)
                    {
                        Console.WriteLine(actID);
                        schoolConnector.Where += "or activityID = \"" + actID + "\" ";
                    }
                    break;

                default:
                    break;
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            /*更新按钮2*/
            LinkButton2.ForeColor = System.Drawing.Color.Brown;
            LinkButton2.Font.Underline = true;

            LinkButton1.Font.Underline = false;
            LinkButton1.ForeColor = System.Drawing.Color.Black;

            schoolConnector.Where = null;

            string LbtnText2 = LinkButton2.Text.ToString();
            switch (LbtnText2)
            {
                case "可报名":
                    schoolConnector.Where = "activityState = 6 ";
                    break;

                case "已收藏":
                    /*
                     * 查询test1
                     * 先用学生ID把所有收藏的活动ID存在数组里
                     * 再给where循环添加条件
                     */

                    ActivityManagerDataContext db = new ActivityManagerDataContext();
                    var res = from info in db.LikedActivity
                              where info.studentID == Tool.studentID
                              select info.activityID;

                    if (res.Count() <= 0)
                    {
                        schoolConnector.Where = "activityState < 0"; // 没有结果
                        break;
                    }

                    string[] actIDs = res.ToArray();
                    schoolConnector.Where = "activityID = \"" + res.First() + "\"";
                    foreach (string actID in actIDs)
                    {
                        Console.WriteLine(actID);
                        schoolConnector.Where += "or activityID = \"" + actID + "\" ";
                    }
                    break;

                default :
                    break;
            }
        }
    }
}