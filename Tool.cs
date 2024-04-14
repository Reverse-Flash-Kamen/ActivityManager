using ActivityManager.App_Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivityManager
{
    public static class Tool
    {
        public static int curUser = 0;     // 0表示校方，1表示社团，2表示学生
        public static string ID = "";
        public static int cnt = 0;

        private static Dictionary<string, int> map = new Dictionary<string, int>(); // 存放列表头及对应列序号（三端列表头不同所以需根据列表头名查询列序号）

        public static Hashtable states = new Hashtable() { { 1, "未提交" }, { 2, "待审核" }, { 3, "未通过" },
                    { 4, "审核过期" }, { 5, "待报名" }, { 6, "报名中" }, { 7, "待开始" }, { 8, "活动中" },
                    { 9, "已结束" }, { 10, "已上报" }, { 11, "已完成" }};

        public static Hashtable statesBack = new Hashtable() { { "未提交", 1 }, { "待审核", 2 }, { "未通过", 3 },
                    { "审核过期", 4 }, { "待报名", 5 }, { "报名中", 6 }, { "待开始", 7 }, { "活动中", 8 },
                    { "已结束", 9 }, { "已上报", 10 }, { "已完成", 11 }};

        public static void FormatActivity(GridView gv, string ID)
        {
            SetButton(gv, ID);  // 根据各端及活动状态设置功能按钮

            /*
             * Gridview通过Linq绑定数据库获取的是ID
             * 优化用户体验需将ID格式化为名称
             * 对于时间也需格式化为日常使用习惯
             */

            ActivityManagerDataContext db = new ActivityManagerDataContext();
            foreach (GridViewRow row in gv.Rows)
            {
                // 格式化申办组织名称
                int index = map["activityOrgID"];   // 根据列表头名获取列序号
                string tmp = row.Cells[index].Text;     // 根据列序号获取当前行对应信息
                var res = from org in db.Organization
                          where org.organizationID.Equals(tmp)
                          select org;

                if (res.Any())
                    row.Cells[index].Text = res.First().organizationName.ToString();

                // 格式化场地名称
                index = map["activityPlaceID"];
                tmp = row.Cells[index].Text;
                var res2 = from place in db.Place
                           where place.placeID.Equals(tmp)
                           select place;
                if (res2.Any())
                    row.Cells[index].Text = res2.First().placeName.ToString();

                // 报名时间
                index = map["signStartDate"];
                string start = Convert.ToDateTime(row.Cells[index].Text).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                string end = Convert.ToDateTime(row.Cells[map["signEndDate"]].Text).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);

                row.Cells[index].Text = start + " 12:00 至<br />" + end + " 12:00";

                // 举办时间
                index = map["holdDate"];
                tmp = Convert.ToDateTime(row.Cells[index].Text).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                start = row.Cells[map["holdStart"]].Text;
                end = row.Cells[map["holdEnd"]].Text;

                if (start.Length < 2)
                    start = "0" + start;
                if (end.Length < 2)
                    end = "0" + end;

                row.Cells[index].Text = tmp + " " + start + ":00 至<br/>" + tmp + " " + end + ":00";

                // 活动状态
                index = map["activityState"];
                tmp = row.Cells[index].Text;
                row.Cells[index].Text = states[Convert.ToInt32(tmp)].ToString();

                // 申请时间 yyyy/dd hh:mm:ss
                index = map["submitTime"];
                tmp = row.Cells[index].Text;
                if (row.Cells[map["activityState"]].Text == "未提交")
                    row.Cells[index].Text = "/";
                else
                    // row.Cells[index].Text = Convert.ToDateTime(tmp).ToString("yyyy-MM-dd H:m:s", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    row.Cells[index].Text = Convert.ToDateTime(tmp).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);

                // 报名人数
                index = map["signed"];
                string signed = row.Cells[index].Text;
                string maxSigned = row.Cells[map["maxSigned"]].Text;
                row.Cells[index].Text = signed + "/" + maxSigned;
            }
        }

        public static void FormatActivityHeader(GridView gv)
        {
            /*
             * 初始化列表头map，只运行一次减少工作量
             * map包含Act原数据表所有表头
             * gv.row 隐藏或添加部分数据项,三端列号不同
             * 通过原表名称获取真正列号
             */
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                string s = gv.Columns[i].AccessibleHeaderText.ToString();
                if (!map.ContainsKey(s))
                    map.Add(s, i);
                else map[s] = i;
            }
        }

        public static void FormatGridView(GridView gridView, int n)
        {
            /*当行数不够时，插入空白行保证美观*/
            if (gridView.Rows.Count != 0 && gridView.Rows.Count != gridView.PageSize)
            {
                // 如果分页有数据但不等于pagesize
                Control table = gridView.Controls[0];
                if (table != null)
                {
                    for (int i = 0; i < gridView.PageSize - gridView.Rows.Count; i++)
                    {
                        int rowIndex = gridView.Rows.Count + i + 1;
                        GridViewRow row = new GridViewRow(rowIndex, -1, DataControlRowType.Separator, DataControlRowState.Normal);

                        row.BackColor = (rowIndex % 2 == 0) ? System.Drawing.Color.White : System.Drawing.Color.WhiteSmoke;
                        for (int j = 0; j < gridView.Columns.Count - n; j++)
                        {
                            // 根据Gv隐藏了多少列，需要改动
                            TableCell cell = new TableCell();
                            cell.Text = "&nbsp";
                            cell.Height = 84;
                            row.Controls.Add(cell);
                        }
                        table.Controls.AddAt(rowIndex, row);
                    }
                }
            }
        }

        public static void UpdateActivityState(GridView gv)
        {
            foreach (GridViewRow row in gv.Rows)
            {
                string id = row.Cells[0].Text; // 必须确保第一列是actID
                MyActivity a = new MyActivity(id); // 用id创建活动实例
                a.UpdateState(); // 更新活动状态
            }
        }

        public static void UpdataAllActivityState()
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from info in db.Activity
                      select info;
            foreach (var item in res)
            {
                MyActivity a = new MyActivity(item);
                a.UpdateState();
            }
        }

        public static void SetButton(GridView gv, string ID)
        {
            // 根据用户码绑定事件
            if (curUser == 0)
            {
                // 校方端口

                // 获取当前行数据
                foreach (GridViewRow row in gv.Rows)
                {
                    string activityID = row.Cells[0].Text;
                    ActivityManagerDataContext db = new ActivityManagerDataContext();
                    var res = from a in db.Activity
                              where a.activityID == activityID
                              select a;

                    int state = -1;
                    if (res.Any())
                        state = Convert.ToInt32(res.First().activityState);

                    int n = gv.Columns.Count;

                    if (state == 2)
                    {
                        // 待审核
                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "审核";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "aduit";
                        //((Button)row.Cells[n - 1].Controls[0]).Attributes.Add("")
                    }
                    else if (state == 10)
                    {
                        // 已上报
                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "完成";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "complete";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).ToolTip = "注意！！！将会发放学分";
                    }
                    else if (state == 11)
                    {
                        // 已完成
                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "导出<br/>名单";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "export";
                    }
                    else
                    {
                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "null";
                    }

                    //((LinkButton)row.Cells[n - 1].Controls[0]).CommandArgument = row.RowIndex.ToString();
                }
            }
            else if (curUser == 1)
            {
                // 组织端口

                foreach (GridViewRow row in gv.Rows)
                {
                    string activityID = row.Cells[0].Text;
                    ActivityManagerDataContext db = new ActivityManagerDataContext();
                    var res = from a in db.Activity
                              where a.activityID == activityID
                              select a;

                    int state = 0;
                    if (res.Any())
                        state = Convert.ToInt32(res.First().activityState);

                    int n = gv.Columns.Count;

                    if (state == 1)
                    {
                        // 未提交
                        ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "编辑";
                        ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "editA";

                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "删除";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "deleteA";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).ToolTip = "将会无法恢复";

                        // ((LinkButton)row.Cells[n - 1].Controls[0]).Attributes.Add("onclick", "return ActConfirm()");

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "null";
                    }
                    else if (state == 2)
                    {
                        // 待审核
                        ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "撤回";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "withdraw";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "null";
                    }
                    else if (state == 3)
                    {
                        // 未通过
                        ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "重新<br/>提交";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "resubmit";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "null";
                    }
                    else if (state == 4)
                    {
                        // 审核过期
                        ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "重新<br/>提交";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "resubmit";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "null";
                    }
                    else if (state == 5)
                    {
                        // 待报名
                        ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "null";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "null";
                    }
                    else if (state == 6)
                    {
                        // 报名中
                        ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "导出名单";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "export";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "null";
                    }
                    else if (state == 7)
                    {
                        // 待开始
                        ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "导出名单";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "export";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "生成签到";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "checkCode";
                    }
                    else if (state == 8)
                    {
                        // 活动中
                        ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "导出名单";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "export";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "生成签到";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "checkCode";
                    }
                    else if (state == 9)
                    {
                        // 已结束
                        ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "导出名单";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "export";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "上报";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "report";
                    }
                    else if (state == 10)
                    {
                        // 已上报
                        ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "导出名单";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "export";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "导出评价";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "exportAppraise";
                    }
                    else if (state == 11)
                    {
                        // 已完成
                        ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "查看";
                        ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "check";

                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "导出名单";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "export";

                        ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "导出评价";
                        ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "exportAppraise";
                    }
                }
            }
            else if (curUser == 2)
            {
                // 学生端口
                // string studentID = Tool.studentID; // 测试用ID，Session这里调用不了，要传参
                ActivityManagerDataContext db = new ActivityManagerDataContext();

                foreach (GridViewRow row in gv.Rows)
                {
                    string activityID = row.Cells[0].Text;
                    ActivityManagerDataContext db1 = new ActivityManagerDataContext();
                    var res = from a in db1.Activity
                              where a.activityID == activityID
                              select a;

                    int state = Convert.ToInt32(res.First().activityState);

                    string actID = row.Cells[0].Text; // 活动ID
                    int n = gv.Columns.Count;

                    ((LinkButton)row.Cells[n - 3].Controls[0]).Text = "查看";
                    ((LinkButton)row.Cells[n - 3].Controls[0]).CommandName = "check";

                    // 判断是否已收藏
                    if (state >= 5)
                    {
                        var resLiked = from info in db.LikedActivity
                                       where info.studentID == ID && info.activityID == actID
                                       select info;
                        if (resLiked.Count() > 0)
                        {
                            ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "取消<br/>收藏";
                            ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "likeCancel";
                        }
                        else
                        {
                            ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "收藏";
                            ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "like";
                        }
                    }
                    else
                    {
                        ((LinkButton)row.Cells[n - 2].Controls[0]).Text = "";
                        ((LinkButton)row.Cells[n - 2].Controls[0]).CommandName = "null";
                    }

                    // 判断是否报名活动
                    var resSign = from info in db.SignedActivity
                                  where info.studentID == ID && (info.activityID == actID)
                                  select info;
                    if (resSign.Any())
                    {
                        // 已报名
                        if (state == 6)
                        {
                            // 可报名
                            ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "取消\n报名";
                            ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "signCancel";
                        }
                        else if (state == 8)
                        {
                            // 活动中
                            ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "签到";
                            ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "checkIn";
                        }
                        else if (state >= 10)
                        {
                            // 已上报，已完成
                            ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "评分";
                            ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "appraise";
                        }
                        else
                        {
                            ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "";
                            ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "null";
                        }
                    }
                    else
                    {
                        // 未报名
                        if (state == 6)
                        {
                            ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "报名";
                            ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "sign";
                        }
                        else
                        {
                            ((LinkButton)row.Cells[n - 1].Controls[0]).Text = "";
                            ((LinkButton)row.Cells[n - 1].Controls[0]).CommandName = "null";
                        }
                    }
                }
            }

            // gv.DataBind();
        }
    }
}