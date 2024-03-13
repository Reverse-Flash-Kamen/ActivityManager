using ActivityManager.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace ActivityManager.Test
{
    public partial class OrgWebForm : System.Web.UI.Page
    {
        private static int mode = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            schoolConnector.Where = null;
            schoolConnector.Where = "activityOrgID = \"" + Session["ID"].ToString() + "\"";
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
            schoolConnector.Where = null;
            schoolConnector.Where = "activityOrgID = \"" + Session["ID"].ToString() + "\"";

            string s1 = name.Text.Trim();
            string s2 = org.Text.Trim();
            string s3 = state.SelectedValue.Trim();

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
        }

        protected void flush_Click(object sender, EventArgs e)
        {
            name.Text = null;
            org.Text = null;
            state.SelectedIndex = 0;

            schoolConnector.Where = null;
            schoolConnector.Where = "activityOrgID = \"" + Session["ID"].ToString() + "\"";
        }

        protected void GvTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
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
            else if (e.CommandName == "editA" || e.CommandName == "resubmit")
            {
                editAct(actID);
            }
            else
            {
                Operation.SetOperation(e.CommandName, actID, Tool.studentID, (GridView)sender, schoolConnector);
            }
        }

        protected void BtnCheck_Click(object sender, EventArgs e)
        {
            CheckActDiv.Visible = false;
        }

        protected void ActMan_Click(object sender, EventArgs e)
        {
            flush_Click(sender, e);
        }

        protected void setHoldDate_Click(object sender, EventArgs e)
        {
            aHoldDate.Visible = true;
        }

        protected void aHoldDate_SelectionChanged(object sender, EventArgs e)
        {
            aHoldDate.Visible = false;

            string s = aHoldDate.SelectedDate.ToShortDateString();
            s = Convert.ToDateTime(s).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            setHoldDate.Text = s;

            aHoldStart.Enabled = true;
            aHoldEnd.Enabled = true;
            setStartTime();
        }

        protected void aHoldStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            aHoldEnd.Enabled = true;
            setEndTime();
        }

        private void setStartTime()
        {
            aHoldStart.Items.Clear();

            List<int> hours = new List<int>();
            for (int i = 0; i <= 11; ++i)
                hours.Add(i + 10);

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

            if (hours.Count == 0)
            {
                // 弹出提示 该场地该日已被占满
                aHoldStart.Text = "开始";
                aHoldStart.Enabled = false;
            }
            else
            {
                foreach (int hour in hours)
                {
                    aHoldStart.Items.Add(hour + ":00");
                }
            }
        }

        private void setEndTime()
        {
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

            foreach (int hour in hours)
            {
                aHoldEnd.Items.Add(hour + ":00");
            }
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
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            save_Click(sender, e);
            MyActivity a = new MyActivity(Session["activityID"].ToString());
            //MessageBox.Show(a.ActivityName);
            a.ActivityState = "2";
            a.Update();
        }

        protected void save_Click(object sender, EventArgs e)
        {
            MyActivity a;
            if (mode == 1)
                a = new MyActivity();
            else
                a = new MyActivity(Session["activityID"].ToString());

            a.ActivityName = aName.Text;
            a.ActivityIntro = aIntro.Text;
            a.ActivityPlaceID = aPlace.SelectedValue;
            a.ActivityOrgID = Session["ID"].ToString();
            a.AvailableCredit = aCredit.Text;
            a.MaxSigned = aVolume.Text;
            a.SignStartDate = setSignStartDate.Text.ToString();
            a.SignEndDate = setSignEndDate.Text.ToString();
            a.HoldDate = setHoldDate.Text.ToString();
            a.HoldStart = aHoldStart.SelectedItem.ToString().Substring(0, 2);
            a.HoldEnd = aHoldEnd.SelectedItem.ToString().Substring(0, 2);

            if (mode == 1)
                a.Create();
            else
                a.Update();

            Session["activityID"] = a.ActivityID;

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
            Tool.SetButton(GvTemplate);
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

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    Session["activityID"] = Activities.Rows[0].Cells[0].Text;
        //    editAct(Activities.Rows[0].Cells[0].Text);
        //}
    }
}