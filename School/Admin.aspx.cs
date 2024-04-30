using ActivityManager.App_Data;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows;

namespace ActivityManager.Test
{
    public partial class AdminWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 调试用
            Session["ID"] = "ndky000001";
            Tool.curUser = 0;

            LinkButton1.ForeColor = System.Drawing.Color.Brown;
            LinkButton1.Font.Underline = true;

            LinkButton2.Font.Underline = false;
            LinkButton3.Font.Underline = false;
            LinkButton2.ForeColor = System.Drawing.Color.Black;
            LinkButton3.ForeColor = System.Drawing.Color.Black;
            schoolConnector.Where = null;
            schoolConnector.Where = "activityState >= 2 ";

            if (!IsPostBack)
            {
                Tool.UpdataAllActivityState();
                Tool.FormatActivityHeader(GvTemplate);
            }
            else
                Tool.FormatGridView(GvTemplate, 9);
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
            schoolConnector.Where = "activityState >= 2 ";

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
            schoolConnector.Where = "activityState >= 2 ";
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton1.ForeColor = System.Drawing.Color.Brown;
            LinkButton1.Font.Underline = true;

            LinkButton2.Font.Underline = false;
            LinkButton3.Font.Underline = false;
            LinkButton2.ForeColor = System.Drawing.Color.Black;
            LinkButton3.ForeColor = System.Drawing.Color.Black;

            schoolConnector.Where = null;
            schoolConnector.Where = "activityState >= 2 ";
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            LinkButton2.ForeColor = System.Drawing.Color.Brown;
            LinkButton2.Font.Underline = true;

            LinkButton1.Font.Underline = false;
            LinkButton3.Font.Underline = false;
            LinkButton1.ForeColor = System.Drawing.Color.Black;
            LinkButton3.ForeColor = System.Drawing.Color.Black;

            schoolConnector.Where = null;
            schoolConnector.Where = "activityState = 2";
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            LinkButton3.ForeColor = System.Drawing.Color.Brown;
            LinkButton3.Font.Underline = true;

            LinkButton2.Font.Underline = false;
            LinkButton1.Font.Underline = false;
            LinkButton2.ForeColor = System.Drawing.Color.Black;
            LinkButton1.ForeColor = System.Drawing.Color.Black;

            schoolConnector.Where = null;
            schoolConnector.Where = "activityState = 10";
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
            else if (e.CommandName == "aduit")
            {
                checkAct(actID);
            }
            else
            {
                // Operation.SetOperation(e.CommandName, actID, Tool.studentID, (GridView)sender, schoolConnector);
                Operation.SetOperation(e.CommandName, actID, Session["ID"].ToString(), (GridView)sender, schoolConnector);
            }

            GvTemplate.DataBind();
        }

        protected void checkAct(string actID)
        {
            Session["activityID"] = actID;
            display.Visible = true;
        }

        protected void BtnCheck_Click(object sender, EventArgs e)
        {
            CheckActDiv.Visible = false;
        }

        protected void ActMan_Click(object sender, EventArgs e)
        {
            LinkButton1_Click(sender, e);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            display.Visible = false;
            passRadio.Checked = false;
            noPassRadio.Checked = false;
            failReason.Text = string.Empty;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            int state = 0;
            if (passRadio.Checked)
            {
                state = 1;
            }
            if (noPassRadio.Checked)
            {
                state = 2;
            }

            if (state == 0)
            {
                // MessageBox.Show("请选择是否通过审核！");
                Response.Write("<script>alert('请选择是否通过审核！')</script>");
            }
            else if (state == 1)
            {
                MyActivity a = new MyActivity(Session["activityID"].ToString());
                a.ActivityState = "5";
                a.Update();
            }
            else if (state == 2)
            {
                if (failReason.Text.Trim() == "" || failReason.Text == null)
                {
                    // MessageBox.Show("请填写审核不通过理由！");
                    Response.Write("<script>alert('请填写审核不通过理由！')</script>");
                    return;
                }
                else
                {
                    MyActivity a = new MyActivity(Session["activityID"].ToString());
                    a.ActivityState = "3";
                    a.FailReason = failReason.Text;
                    a.Update();
                }
            }

            btnCancel_Click(sender, e);

            GvTemplate.DataBind();
        }
    }
}