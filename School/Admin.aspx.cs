using ActivityManager.App_Data;
using NPOI.SS.Formula.Functions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;

namespace ActivityManager.Test
{
    public partial class AdminWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 调试用
            // Session["ID"] = "ndky000001";
            if (Session["ID"] == null)
            {
                Server.Transfer("../Login.aspx");
                // Response.Write("<script>alert('请登录后再访问！');</script>");
                return;
            }

            Tool.curUser = 0;

            /*LinkButton1.ForeColor = System.Drawing.Color.Brown;
            LinkButton1.Font.Underline = true;

            LinkButton2.Font.Underline = false;
            LinkButton3.Font.Underline = false;
            LinkButton2.ForeColor = System.Drawing.Color.Black;
            LinkButton3.ForeColor = System.Drawing.Color.Black;*/

            /*schoolConnector.Where = null;
            schoolConnector.Where = "activityState >= 2 ";*/

            Tool.FormatActivityHeader(GvTemplate);
            if (!IsPostBack)
            {
                Tool.UpdataAllActivityState();
                Tool.FormatActivityHeader(GvTemplate); // 更新表头
                schoolConnector.Where = "(activityState >= 2) ";
            }
            else
            {
                schoolConnector.Where = ActivityManagerDataContext.connectorWhere;
                Tool.FormatGridView(GvTemplate, 9);
            }
        }

        protected void Esc_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Server.Transfer("../Login.aspx");
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            Tool.FormatActivity(GvTemplate, Session["ID"].ToString());
            Tool.FormatGridView(GvTemplate, 9);
            Tool.UpdateActivityState(GvTemplate);
        }

        protected void commit_Click(object sender, EventArgs e)
        {
            /*schoolConnector.Where = null;
            schoolConnector.Where = "activityState >= 2 ";*/

            if (LinkButton1.Font.Underline == true)
                schoolConnector.Where = "(activityState >= 2) ";
            else if (LinkButton2.Font.Underline == true)
                schoolConnector.Where = "(activityState = 2) ";
            else if (LinkButton3.Font.Underline == true)
                schoolConnector.Where = "(activityState = 10) ";

            if (schoolConnector.Where == "") schoolConnector.Where = "(activityState >= 0) ";

            string s1 = name.Text.Trim();
            string s2 = org.Text.Trim();
            string s3 = state.SelectedValue.Trim();
            string s4 = type.SelectedValue.Trim();

            if (s1 != "")
            {
                schoolConnector.Where += " and (activityName = \"" + s1 + "\") ";
            }

            if (s2 != "")
            {
                ActivityManagerDataContext db = new ActivityManagerDataContext();
                var res = from org in db.Organization
                          where org.organizationName == s2
                          select org;

                if (res.Any())
                    schoolConnector.Where += " and (activityOrgID = \"" + res.First().organizationID.ToString() + "\") ";
            }

            if (s3 != "")
            {
                if (s3 != "0")
                    schoolConnector.Where += " and (activityState = " + s3 + ") ";
            }

            if (s4 != "")
            {
                if (s4 != "0")
                    schoolConnector.Where += " and (activityType = " + s4 + ") ";
            }

            ActivityManagerDataContext.connectorWhere = schoolConnector.Where;
        }

        protected void flush_Click(object sender, EventArgs e)
        {
            name.Text = null;
            org.Text = null;
            state.SelectedIndex = 0;
            type.SelectedIndex = 0;

            schoolConnector.Where = null;
            if (LinkButton1.Font.Underline == true)
            {
                schoolConnector.Where = "(activityState >= 2) ";
            }
            else if (LinkButton2.Font.Underline == true)
            {
                schoolConnector.Where = "(activityState = 2) ";
            }
            else if (LinkButton3.Font.Underline == true)
            {
                schoolConnector.Where = "(activityState = 10) ";
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton1.ForeColor = System.Drawing.Color.Brown;
            LinkButton1.Font.Underline = true;

            LinkButton2.Font.Underline = false;
            LinkButton3.Font.Underline = false;
            LinkButton2.ForeColor = System.Drawing.Color.Black;
            LinkButton3.ForeColor = System.Drawing.Color.Black;

            GvTemplate.PageIndex = 0;

            /*schoolConnector.Where = null;
            schoolConnector.Where = "(activityState >= 2) ";*/
            flush_Click(sender, e);

            ActivityManagerDataContext.connectorWhere = schoolConnector.Where.ToString(); // 存储当前查询条件
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            LinkButton2.ForeColor = System.Drawing.Color.Brown;
            LinkButton2.Font.Underline = true;

            LinkButton1.Font.Underline = false;
            LinkButton3.Font.Underline = false;
            LinkButton1.ForeColor = System.Drawing.Color.Black;
            LinkButton3.ForeColor = System.Drawing.Color.Black;

            /*schoolConnector.Where = null;
            schoolConnector.Where = "(activityState = 2) ";*/
            flush_Click(sender, e);

            GvTemplate.PageIndex = 0;

            ActivityManagerDataContext.connectorWhere = schoolConnector.Where.ToString(); // 存储当前查询条件
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            LinkButton3.ForeColor = System.Drawing.Color.Brown;
            LinkButton3.Font.Underline = true;

            LinkButton2.Font.Underline = false;
            LinkButton1.Font.Underline = false;
            LinkButton2.ForeColor = System.Drawing.Color.Black;
            LinkButton1.ForeColor = System.Drawing.Color.Black;

            /*schoolConnector.Where = null;
            schoolConnector.Where = "(activityState = 10) ";*/
            flush_Click(sender, e);

            GvTemplate.PageIndex = 0;

            ActivityManagerDataContext.connectorWhere = schoolConnector.Where.ToString(); // 存储当前查询条件
        }

        protected void GvTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Page") return;

            int index = int.Parse(e.CommandArgument.ToString());
            string actID = GvTemplate.Rows[index].Cells[0].Text;

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
            else if (e.CommandName == "aduit")
            {
                checkAct(actID);
                DivMask.Style["pointer-events"] = "none";
            }
            else
            {
                // Operation.SetOperation(e.CommandName, actID, Tool.studentID, (GridView)sender, schoolConnector);
                Operation.SetOperation(e.CommandName, actID, Session["ID"].ToString(), GvTemplate);
            }

            GvTemplate.DataBind();
        }

        protected void checkAct(string actID)
        {
            Session["activityID"] = actID;
            display.Style["display"] = "block";
        }

        protected void BtnCheck_Click(object sender, EventArgs e)
        {
            CheckActDiv.Style["display"] = "none";
            DivMask.Style["pointer-events"] = "auto";
        }

        protected void ActMan_Click(object sender, EventArgs e)
        {
            DivSearchPlace.Style["display"] = "none";
            DivNavPlace.Style["display"] = "none";
            DivPlaceGv.Style["display"] = "none";

            DivSearchAct.Style["display"] = "block";
            DivNavAct.Style["display"] = "block";
            DivActGv.Style["display"] = "block";

            DivPlaceMan.Style["background-color"] = "#ccad9f";
            DivActMan.Style["background-color"] = "red";

            LinkButton1_Click(sender, e);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            display.Style["display"] = "none";
            passRadio.Checked = false;
            noPassRadio.Checked = false;
            failReason.Text = string.Empty;
            DivMask.Style["pointer-events"] = "auto";
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
                a.ActivityState = 5;
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
                    a.ActivityState = 3;
                    a.FailReason = failReason.Text;
                    a.Update();
                }
            }

            btnCancel_Click(sender, e);

            GvTemplate.DataBind();
            DivMask.Style["pointer-events"] = "auto";
        }

        protected void GvTemplate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvTemplate.PageIndex = e.NewPageIndex;
            schoolConnector.Where = ActivityManagerDataContext.connectorWhere;
        }

        protected void PlaceMan_Click(object sender, EventArgs e)
        {
            DivSearchPlace.Style["display"] = "block";
            DivNavPlace.Style["display"] = "block";
            DivPlaceGv.Style["display"] = "block";

            DivSearchAct.Style["display"] = "none";
            DivNavAct.Style["display"] = "none";
            DivActGv.Style["display"] = "none";

            DivActMan.Style["background-color"] = "#ccad9f";
            DivPlaceMan.Style["background-color"] = "red";

            BtnFlush_Click(sender, e);
        }

        protected void GvPlace_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Page") return;
            int index = int.Parse(e.CommandArgument.ToString());
            int placeID = int.Parse(GvPlace.Rows[index].Cells[0].Text);
            Debug.Write("placeID" + placeID);

            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from info in db.Place
                      where info.placeID == placeID
                      select info;

            if (e.CommandName == "editP")
            {
                // 调用创建
                LblAddPlaceID.Text = placeID.ToString();
                DivAddPlace.Style["display"] = "block";
                DivMask.Style["pointer-events"] = "none";
                LblAddPlace.Text = "编辑场地";
                TxtAddPlaceName.Text = res.First().placeName;
                TxtAddPlaceVolume.Text = res.First().volume.ToString();
            }
            else if (e.CommandName == "deletP")
            {
                db.Place.DeleteOnSubmit(res.First());
                db.SubmitChanges();
            }
            else if (e.CommandName == "enableP")
            {
                // 启用
                res.First().placeState = 0;
                db.SubmitChanges();
            }
            else if (e.CommandName == "disableP")
            {
                // 停用
                res.First().placeState = -1;
                db.SubmitChanges();
            }

            GvPlace.DataBind();
        }

        protected void GvPlace_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GvPlace.Rows)
            {
                // 获取场地状态
                int placeState = int.Parse(row.Cells[3].Text);

                // 格式化按钮
                if (placeState == 1)
                {
                    ((LinkButton)row.Cells[5].Controls[0]).Text = "";
                    ((LinkButton)row.Cells[5].Controls[0]).CommandName = "null";
                    ((LinkButton)row.Cells[6].Controls[0]).Text = "";
                    ((LinkButton)row.Cells[6].Controls[0]).CommandName = "null";
                    ((LinkButton)row.Cells[7].Controls[0]).Text = "";
                    ((LinkButton)row.Cells[7].Controls[0]).CommandName = "null";
                }
                else
                {
                    ((LinkButton)row.Cells[5].Controls[0]).Text = "编辑";
                    ((LinkButton)row.Cells[5].Controls[0]).CommandName = "editP";

                    if (placeState == -1)
                    {
                        ((LinkButton)row.Cells[7].Controls[0]).Text = "启用";
                        ((LinkButton)row.Cells[7].Controls[0]).CommandName = "enableP";
                    }
                    else if (placeState == 0)
                    {
                        ((LinkButton)row.Cells[7].Controls[0]).Text = "停用";
                        ((LinkButton)row.Cells[7].Controls[0]).CommandName = "disableP";
                    }
                }

                // 格式化场地状态
                switch (placeState)
                {
                    case -1:
                        row.Cells[3].Text = "已停用";
                        break;

                    case 0:
                        row.Cells[3].Text = "空闲中";
                        break;

                    case 1:
                        row.Cells[3].Text = "使用中";
                        break;
                }

                // 统计使用次数
                int placeID = int.Parse(row.Cells[0].Text);
                ActivityManagerDataContext db = new ActivityManagerDataContext();
                var res = from info in db.Activity
                          where info.activityPlaceID == placeID
                          select info;
                row.Cells[4].Text = res.Count().ToString();
            }

            // 添加空行
            Tool.FormatGridView(GvPlace, 1);
        }

        protected void BtnCommit_Click(object sender, EventArgs e)
        {
            string placeName = TxtSearchPlaceName.Text.Trim();
            string placeState = DropDownListPlaceState.SelectedItem.ToString();

            PlaceLinqDataSource.Where = "placeState >= -1 ";

            if (placeName != "")
            {
                PlaceLinqDataSource.Where += " and (placeName = \"" + placeName + "\") ";
            }

            if (placeState != "")
            {
                if (placeState == "停用中")
                {
                    PlaceLinqDataSource.Where += " and (placeState = -1) ";
                }
                else if (placeState == "空闲中")
                {
                    PlaceLinqDataSource.Where += " and (placeState = 0) ";
                }
                else if (placeState == "使用中")
                {
                    PlaceLinqDataSource.Where += " and (placeState = 1) ";
                }
            }

            ActivityManagerDataContext.connectorWhere = PlaceLinqDataSource.Where;
        }

        protected void BtnFlush_Click(object sender, EventArgs e)
        {
            TxtSearchPlaceName.Text = "";
            DropDownListPlaceState.SelectedIndex = 0;

            PlaceLinqDataSource.Where = "";
        }

        protected void GvPlace_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvPlace.PageIndex = e.NewPageIndex;
            PlaceLinqDataSource.Where = ActivityManagerDataContext.connectorWhere;
        }

        protected void BtnApply_Click(object sender, EventArgs e)
        {
            DivAddPlace.Style["display"] = "block";
            DivMask.Style["pointer-events"] = "none";
            LblAddPlace.Text = "新增场地";
        }

        protected void BtnAddPlaceCancel_Click(object sender, EventArgs e)
        {
            DivAddPlace.Style["display"] = "none";
            DivMask.Style["pointer-events"] = "auto";
        }

        protected void BtnAddPlaceSubmit_Click(object sender, EventArgs e)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from info in db.Place
                      where info.placeID == int.Parse(LblAddPlaceID.Text)
                      select info;

            if (res.Any())
            {
                // 编辑
                res.First().placeName = TxtAddPlaceName.Text.Trim();
                res.First().volume = int.Parse(TxtAddPlaceVolume.Text.Trim());
                db.SubmitChanges();
                Response.Write("<script>alert('场地编辑成功！')</script>");
            }
            else
            {
                // 新增
                Place place = new Place()
                {
                    placeName = TxtAddPlaceName.Text.Trim(),
                    volume = int.Parse(TxtAddPlaceVolume.Text.Trim()),
                };
                db.Place.InsertOnSubmit(place);
                db.SubmitChanges();
                Response.Write("<script>alert('场地增加成功！')</script>");
            }

            LblAddPlaceID.Text = "-1";
            GvPlace.DataBind();
            BtnAddPlaceCancel_Click(sender, e);
        }
    }
}