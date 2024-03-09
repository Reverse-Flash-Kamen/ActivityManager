using ActivityManager.App_Data;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace ActivityManager.Test
{
    public partial class StudentWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LinkButton1.ForeColor = System.Drawing.Color.Brown;
            LinkButton1.Font.Underline = true;

            LinkButton2.Font.Underline = false;
            LinkButton2.ForeColor = System.Drawing.Color.Black;

            schoolConnector.Where = null;
            schoolConnector.Where = "activityState >= 5 ";      // 学生只能看到状态5以上的活动
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
            schoolConnector.Where = "activityState >= 5 ";

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
            schoolConnector.Where = "activityState >= 5 ";
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton1.ForeColor = System.Drawing.Color.Brown;
            LinkButton1.Font.Underline = true;

            LinkButton2.Font.Underline = false;
            LinkButton2.ForeColor = System.Drawing.Color.Black;

            schoolConnector.Where = null;
            schoolConnector.Where = "activityState >= 5 ";
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            LinkButton2.ForeColor = System.Drawing.Color.Brown;
            LinkButton2.Font.Underline = true;

            LinkButton1.Font.Underline = false;

            LinkButton1.ForeColor = System.Drawing.Color.Black;

            schoolConnector.Where = null;
            schoolConnector.Where = "activityState = 6 ";
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
            LinkButton1_Click(sender, e);
        }

        protected void GvTemplate_DataBinding(object sender, EventArgs e)
        {
            Tool.UpdateActivityState((GridView)sender);
        }
    }
}