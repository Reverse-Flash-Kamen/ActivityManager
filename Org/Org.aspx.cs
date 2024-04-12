using ActivityManager.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace ActivityManager.Test
{
    public partial class OrgWebForm : System.Web.UI.Page
    {
        private static int mode = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 调试用
            // Session["ID"] = "org2022121201";
            Tool.curUser = 1;

            if (Session["ID"] == null)
            {
                Server.Transfer("../Login.aspx");
                // Response.Write("<script>alert('请登录后再访问！');</script>");
                return;
            }

            Tool.FormatActivityHeader(GvTemplate);
            if (!IsPostBack)
            {
                Tool.UpdataAllActivityState();
                Tool.FormatActivityHeader(GvTemplate); // 更新表头
            }
            else
                Tool.FormatGridView(GvTemplate, 9);

            schoolConnector.Where = null;
            schoolConnector.Where = "activityOrgID = \"" + Session["ID"].ToString() + "\"";
        }

        protected void Esc_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Server.Transfer("../Login.aspx");
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            Tool.FormatActivity((GridView)sender, Session["ID"].ToString());
            Tool.FormatGridView((GridView)sender, 9);
            Tool.UpdateActivityState((GridView)sender);
        }

        protected void commit_Click(object sender, EventArgs e)
        {
            schoolConnector.Where = null;
            schoolConnector.Where = "activityOrgID = \"" + Session["ID"].ToString() + "\"";

            string s1 = name.Text.Trim();
            string s2 = org.Text.Trim();
            string s3 = state.SelectedValue.Trim();
            string s4 = type.SelectedValue.Trim();

            if (s1 != "")
            {
                schoolConnector.Where += " and activityName = \"" + s1 + "\"";
            }

            if (s2 != "")
            {
                ActivityManagerDataContext db = new ActivityManagerDataContext();
                var res = from org in db.Organization
                          where org.organizationName == s2
                          select org;

                if (res.Any())
                    schoolConnector.Where += " and activityOrgID = \"" + res.First().organizationID.ToString() + "\"";
            }

            if (s3 != "")
            {
                if (s3 != "0")
                    schoolConnector.Where += " and activityState = " + s3;
            }

            if (s4 != "")
            {
                if (s4 != "0")
                    schoolConnector.Where += " and activityType = " + s4;
            }
        }

        protected void flush_Click(object sender, EventArgs e)
        {
            name.Text = null;
            org.Text = null;
            state.SelectedIndex = 0;
            type.SelectedIndex = 0;

            schoolConnector.Where = null;
            schoolConnector.Where = "activityOrgID = \"" + Session["ID"].ToString() + "\"";
        }

        protected void GvTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // if (GvTemplate.PageSize > ((GridView)sender).Rows.Count) return;
            if (e.CommandName == "Page") return;

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
            else if (e.CommandName == "editA" || e.CommandName == "resubmit")
            {
                DivMask.Style["pointer-events"] = "none";
                editAct(actID);
            }
            else
            {
                // Operation.SetOperation(e.CommandName, actID, Tool.studentID, (GridView)sender, schoolConnector);
                Operation.SetOperation(e.CommandName, actID, Session["ID"].ToString(), (GridView)sender);
            }

            GvTemplate.DataBind();
        }

        protected void BtnCheck_Click(object sender, EventArgs e)
        {
            CheckActDiv.Visible = false;
            DivMask.Style["pointer-events"] = "auto";
        }

        protected void ActMan_Click(object sender, EventArgs e)
        {
            flush_Click(sender, e);
        }

        protected void setHoldDate_Click(object sender, EventArgs e)
        {
            /*点击显示日历*/
            aHoldDate.Visible = true;
        }

        protected void aHoldDate_SelectionChanged(object sender, EventArgs e)
        {
            /*选择day*/
            aHoldDate.Visible = false; // 选择后日历隐藏

            string s = aHoldDate.SelectedDate.ToShortDateString();
            s = Convert.ToDateTime(s).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            setHoldDate.Text = s; // linkbutton

            /*允许选择hour*/
            aHoldStart.Enabled = true;
            // aHoldEnd.Enabled = true;
            setStartTime(); // 判断day能不能选
        }

        protected void aHoldStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            // aHoldEnd.Enabled = true;
            if (aHoldStart.SelectedValue == "") return; // 空串不触发联动

            setEndTime();
        }

        private void setStartTime()
        {
            /*
             * 根据场地需求、已申请活动等因数确定开始hour可选择的范围
             * 即一个场地不可能同时段举办两个活动
             */

            aHoldStart.Items.Clear(); // 清除下拉列表项目

            List<int> hours = new List<int>(); // 设定10 - 21点为活动可举办时间
            for (int i = 0; i <= 11; ++i)
                hours.Add(i + 10);

            /*
             * 查询选择day是否已有活动申请
             * 在10-21中排除以被占用的时间段
             */
            int placeID = Convert.ToInt32(aPlace.SelectedValue);
            DateTime date = Convert.ToDateTime(aHoldDate.SelectedDate.ToString());

            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from a in db.Activity
                      where a.activityPlaceID == placeID && a.holdDate == date
                      select a;

            if (res.Any())
            {
                foreach (var A in res)
                {
                    int start = Convert.ToInt32(A.holdStart);
                    int end = Convert.ToInt32(A.holdEnd);

                    for (int i = start; i < end; ++i)
                    {
                        if (hours.Contains(i))
                        {
                            hours.Remove(i);
                        }
                    }
                }
            }

            // 剩余时间段已用完
            if (hours.Count == 0)
            {
                // 弹出提示 该场地该日已被占满
                aHoldStart.Text = "开始";
                aHoldStart.Enabled = false;
            }
            else
            {
                aHoldStart.Items.Add("");
                // 将可用时间段添加至开始hour下拉列表
                foreach (int hour in hours)
                {
                    aHoldStart.Items.Add(hour + ":00");
                }
            }
        }

        private void setEndTime()
        {
            /*同setStartTime*/
            aHoldEnd.Items.Clear();

            List<int> hours = new List<int>();
            for (int i = 0; i <= 11; ++i)
                hours.Add(i + 11);

            int placeID = Convert.ToInt32(aPlace.SelectedValue);
            DateTime date = Convert.ToDateTime(aHoldDate.SelectedDate.ToString());

            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from a in db.Activity
                      where a.activityPlaceID == placeID && a.holdDate == date
                      select a;

            int minStart = 24;
            int maxEnd = 0;

            if (res.Any())
            {
                foreach (var A in res)
                {
                    int start = Convert.ToInt32(A.holdStart);
                    int end = Convert.ToInt32(A.holdEnd);

                    minStart = Math.Min(minStart, start);
                    maxEnd = Math.Max(maxEnd, end);
                }
            }

            int holdStart = Convert.ToInt32(aHoldStart.SelectedItem.ToString().Substring(0, 2));

            for (int i = 11; i <= holdStart; ++i)
            {
                if (hours.Contains(i))
                {
                    hours.Remove(i);
                }
            }

            if (holdStart < minStart)
            {
                for (int i = minStart + 1; i <= 22; ++i)
                {
                    if (hours.Contains(i))
                        hours.Remove(i);
                }
            }

            List<string> strings = new List<string>();

            foreach (int hour in hours)
            {
                // aHoldEnd.Items.Add(hour + ":00"); // 数据绑定？
                strings.Add(hour.ToString() + ":00");
            }

            aHoldEnd.DataSource = strings;
            aHoldEnd.DataBind();
        }

        protected void setSignStartDate_Click(object sender, EventArgs e)
        {
            aSignStartDate.Visible = true;
        }

        protected void setSignEndDate_Click(object sender, EventArgs e)
        {
            aSignEndDate.Visible = true;
        }

        protected void aSignStartDate_SelectionChanged(object sender, EventArgs e)
        {
            aSignStartDate.Visible = false;

            string s = aSignStartDate.SelectedDate.ToShortDateString();
            s = Convert.ToDateTime(s).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            setSignStartDate.Text = s;
        }

        protected void aSignEndDate_SelectionChanged(object sender, EventArgs e)
        {
            aSignEndDate.Visible = false;

            string s = aSignEndDate.SelectedDate.ToShortDateString();
            s = Convert.ToDateTime(s).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            setSignEndDate.Text = s;
        }

        protected void creditUp_Click(object sender, EventArgs e)
        {
            if (aCredit.Text != null && aCredit.Text != "")
                aCredit.Text = (Convert.ToInt32(aCredit.Text) + 1).ToString();
        }

        protected void creditDown_Click(object sender, EventArgs e)
        {
            if (aCredit.Text != null && aCredit.Text != "")
                aCredit.Text = (Convert.ToInt32(aCredit.Text) - 1).ToString();
        }

        protected void aCredit_TextChanged(object sender, EventArgs e)
        {
            int credit = Convert.ToInt32(aCredit.Text);
            if (credit < 1)
            {
                aCredit.Text = "1";
            }
            else if (credit > 8)
            {
                aCredit.Text = "8";
            }
        }

        protected void BtnApply_Click(object sender, EventArgs e)
        {
            display.Visible = true;
            DivMask.Style["pointer-events"] = "none";
        }

        protected void returnA_Click(object sender, EventArgs e)
        {
            display.Visible = false;
            aName.Text = null;
            aIntro.Text = null;
            aPlace.SelectedIndex = 0;
            aCredit.Text = null;
            aVolume.Text = null;
            setSignStartDate.Text = "选择报名开始日期";
            setSignEndDate.Text = "选择报名结束日期";
            setHoldDate.Text = "选择举办日期";
            aHoldStart.Items.Clear();
            aHoldEnd.Items.Clear();
            aHoldStart.Enabled = false;
            aHoldEnd.Enabled = false;

            DivMask.Style["pointer-events"] = "auto";
        }

        /// <summary>
        /// 报名时间|举办时间不能早于系统时间
        /// 报名开始时间|结束时间不能晚于结束时间|举办时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void submit_Click(object sender, EventArgs e)
        {
            if (Save_Click(sender, e))
            {
                MyActivity a = new MyActivity(Session["activityID"].ToString());
                //MessageBox.Show(a.ActivityName);
                a.ActivityState = 2;
                a.Update();

                GvTemplate.DataBind();
                // DivMask.Style["pointer-events"] = "auto";
            }
        }

        protected void save_Click(object sender, EventArgs e)
        {
            Save_Click(sender, e);
        }

        protected bool Save_Click(object sender, EventArgs e)
        {
            // Response.Write("<script language='javascript'>if(confirm('确定删除?'))</script>");

            // 验证选择日期
            if (setSignStartDate.Text == "选择报名开始日期" || setSignEndDate.Text == "选择报名结束日期" || setHoldDate.Text == "选择举办日期")
            {
                Response.Write("<script>alert('申请活动需选择日期！');</script>");
                return false;
            }

            int signStartInt = int.Parse(Regex.Replace(setSignStartDate.Text, "[-]", ""));
            int signEndInt = int.Parse(Regex.Replace(setSignEndDate.Text, "[-]", ""));
            int holdDateInt = int.Parse(Regex.Replace(setHoldDate.Text, "[-]", ""));

            string msg = "";
            if (signStartInt > signEndInt)
            {
                /*Response.Write("<script>alert('报名开始日期不得晚于报名结束日期！');</script>");
                return false;*/

                msg += "-报名开始日期不得晚于报名结束日期！\\r";
            }

            if (signStartInt > holdDateInt)
            {
                /*Response.Write("<script>alert('报名日期不得晚于活动开始日期！');</script>");
                return false;*/

                msg += "-报名日期不得晚于活动开始日期！\\r";
            }

            int nowTimeInt = int.Parse(DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo));

            if (signStartInt < nowTimeInt || signEndInt < nowTimeInt || holdDateInt < nowTimeInt)
            {
                /*Response.Write("<script>alert('活动日期不得早于当前日期！');</script>");
                return false;*/

                msg += "-活动日期不得早于当前日期！\\r";
            }

            if (aHoldStart.SelectedValue == "")
            {
                /*Response.Write("<script>alert('请选择活动举办具体时间！');</script>");
                return false;*/

                msg += "-请选择活动举办具体时间！\\r";
            }

            // 验证场地人数
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var resVolume = from info in db.Place
                            where info.placeID == int.Parse(aPlace.SelectedValue)
                            select info.volume;
            if (int.Parse(aVolume.Text) > resVolume.First())
            {
                /*Response.Write("<script>alert('活动人数超过场地人数限制！');</script>");
                return false;*/

                msg += "-活动人数超过场地人数限制！\\r";
            }

            if (RblEanbleTeam.SelectedValue == "1")
            {
                int minVolume = int.Parse(DdlMinVloume.SelectedValue);
                int maxVolume = int.Parse(DdlMaxVloume.SelectedValue);

                if (minVolume <= 1 || maxVolume < minVolume)
                {
                    /*Response.Write("<script>alert('请输入合法团队容量！');</script>");
                    return false;*/

                    msg += "-请输入合法团队容量！\\r";
                }
            }
            else if (RblEanbleTeam.SelectedIndex == -1)
            {
                msg += "-请选择是否启用团队报名！\\r";
            }

            if (msg != "")
            {
                Response.Write("<script>alert('" + msg + "');</script>");
                return false;
            }

            MyActivity a;
            if (mode == 1)
                a = new MyActivity();
            else
                a = new MyActivity(Session["activityID"].ToString());

            a.ActivityName = aName.Text;
            a.ActivityIntro = aIntro.Text;
            a.ActivityPlaceID = int.Parse(aPlace.SelectedValue);
            a.ActivityOrgID = Session["ID"].ToString();
            a.AvailableCredit = int.Parse(aCredit.Text);
            a.MaxSigned = int.Parse(aVolume.Text);
            a.SignStartDate = setSignStartDate.Text.ToString();
            a.SignEndDate = setSignEndDate.Text.ToString();
            a.HoldDate = setHoldDate.Text.ToString();
            a.HoldStart = int.Parse(aHoldStart.SelectedItem.ToString().Substring(0, 2));
            a.HoldEnd = int.Parse(aHoldEnd.SelectedItem.ToString().Substring(0, 2));
            a.ActivityType = int.Parse(DropDownListType.SelectedValue);

            if (mode == 1)
                a.Create();
            else
                a.Update();

            Session["activityID"] = a.ActivityID;

            if (RblEanbleTeam.SelectedValue == "1")
            {
                ActivityEnableTeam enableTeam = new ActivityEnableTeam()
                {
                    activityID = a.ActivityID,
                    minVolume = int.Parse(TxtMinVolume.Text.Trim()),
                    maxVolume = int.Parse(TxtMaxVolume.Text.Trim()),
                };

                db.ActivityEnableTeam.InsertOnSubmit(enableTeam);
                db.SubmitChanges();
            }

            display.Visible = false;

            aName.Text = null;
            aIntro.Text = null;
            aPlace.SelectedIndex = 0;
            aCredit.Text = null;
            aVolume.Text = null;
            setSignStartDate.Text = "选择报名开始日期";
            setSignEndDate.Text = "选择报名结束日期";
            setHoldDate.Text = "选择举办日期";
            aHoldStart.Items.Clear();
            aHoldEnd.Items.Clear();
            aHoldStart.Enabled = false;
            aHoldEnd.Enabled = false;
            TxtMaxVolume.Text = null;
            TxtMinVolume.Text = null;
            RblEanbleTeam.SelectedIndex = -1;

            Tool.SetButton(GvTemplate, Session["ID"].ToString());

            DivMask.Style["pointer-events"] = "auto";
            GvTemplate.DataBind();
            return true;
        }

        public void editAct(string activityID)
        {
            Session["activityID"] = activityID;

            mode = 2;

            display.Visible = true;
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from a in db.Activity
                      where a.activityID == activityID
                      select a;
            var act = res.First();

            aName.Text = act.activityName;
            aIntro.Text = act.activityIntro;
            aPlace.SelectedValue = act.activityPlaceID.ToString();
            aCredit.Text = act.availableCredit.ToString();
            aVolume.Text = act.maxSigned.ToString();
            setSignStartDate.Text = Convert.ToDateTime(act.signStartDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            setSignEndDate.Text = Convert.ToDateTime(act.signEndDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            setHoldDate.Text = Convert.ToDateTime(act.holdDate).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);

            aHoldStart.Enabled = true;
            aHoldEnd.Enabled = true;
            aHoldStart.Items.Add(act.holdStart + ":00");
            aHoldEnd.Items.Add(act.holdEnd + ":00");
        }

        protected void aHoldEnd_DataBound(object sender, EventArgs e)
        {
            aHoldEnd.Enabled = true;
        }

        protected void RblEanbleTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RblEanbleTeam.SelectedValue == "1")
            {
                DivTeamVolume.Style["display"] = "block";
            }
            else
            {
                DivTeamVolume.Style["display"] = "none";
                TxtMinVolume.Text = "";
                TxtMaxVolume.Text = "";
            }
        }
    }
}