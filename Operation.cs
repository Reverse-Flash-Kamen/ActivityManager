using ActivityManager.App_Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows;

namespace ActivityManager
{
    public class Operation
    {
        public static void SetOperation(string commandName, string actID, string studentID, GridView gv, LinqDataSource data)
        {
            Operation operation = new Operation();

            if (commandName == "check")
            {
                // 查看操作
                MessageBox.Show("Check");
            }
            else if (commandName == "editA")
            {
                // 编辑操作
                MessageBox.Show("Edit");
            }
            else if (commandName == "deleteA")
            {
                // 删除操作
                operation.ActDelete(actID);
            }
            else if (commandName == "withdraw")
            {
                // 撤回操作
                operation.ActWithdraw(actID);
            }
            else if (commandName == "resubmit")
            {
                // 重新提交操作 = 编辑操作
                MessageBox.Show("Resubmit");
            }
            else if (commandName == "export")
            {
                // 导出报名名单操作
                MessageBox.Show("Export");
            }
            else if (commandName == "report")
            {
                // 上报操作
                operation.ActReport(actID);
            }
            else if (commandName == "complete")
            {
                // 完成操作
                operation.ActComplete(actID, studentID);
            }
            else if (commandName == "like")
            {
                // 收藏操作
                operation.ActLike(actID, studentID);
            }
            else if (commandName == "likeCancel")
            {
                // 取消收藏操作
                operation.ActLikeCancel(actID, studentID);
            }
            else if (commandName == "sign")
            {
                // 报名操作
                operation.ActSign(actID, studentID);
            }
            else if (commandName == "signCancel")
            {
                // 取消报名操作
                operation.ActSignCancel(actID, studentID);
            }
            else if (commandName == "aduit")
            {
                // 审核操作
                MessageBox.Show("审核操作！");
            }
            else if (commandName == "exprotFinal")
            {
                // 导出完成名单操作
                MessageBox.Show("导出名单成功！");
            }

            Tool.SetButton(gv, studentID);
        }

        public void ActDelete(string actID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

            if (MessageBox.Show("确定要删除这条活动记录吗？", "DELETE CONFIRM", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var res = from info in db.Activity
                              where info.activityID == actID
                              select info;
                    db.Activity.DeleteOnSubmit(res.First());
                    db.SubmitChanges();
                }
                catch
                {
                    MessageBox.Show("未知错误，删除失败！");
                    return;
                }

                MessageBox.Show("删除成功！", "DELETE", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ActWithdraw(string actID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

            if (MessageBox.Show("确定要撤回这条活动申请吗？", "WITHDRAW CONFIRM", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var res = from info in db.Activity
                              where info.activityID == actID
                              select info;
                    res.First().activityState = 1; // 更新状态为：未提交
                    db.SubmitChanges();
                }
                catch
                {
                    MessageBox.Show("未知错误，撤回失败！");
                    return;
                }
            }
        }

        public void ActReport(string actID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

            var res = from info in db.Activity
                      where info.activityID == actID
                      select info;

            res.First().activityState = 10; // 更新状态为：已上报
            db.SubmitChanges();
        }

        public void ActComplete(string actID, string studentID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

            var resState = from info in db.Activity
                           where info.activityID == actID
                           select info;
            resState.First().activityState = 11; // 更新状态为：已完成
            db.SubmitChanges();

            // 完成后给学生加学分
            int credit = int.Parse(a.AvailableCredit); // 获取活动学分
            int type = int.Parse(a.ActivityType);
            var resCredit = from info in db.StudentIdentified
                            where info.studentID == studentID
                            select info;

            // 根据不同类别活动加不同学分
            switch (type)
            {
                case 1:
                    resCredit.First().credit_1 += credit;
                    break;

                case 2:
                    resCredit.First().credit_2 += credit;
                    break;

                case 3:
                    resCredit.First().credit_3 += credit;
                    break;

                default:
                    break;
            }

            db.SubmitChanges();
        }

        public void ActLike(string actID, string studentID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

            LikedActivity likedActivity = new LikedActivity();
            likedActivity.activityID = actID;
            likedActivity.studentID = studentID;
            db.LikedActivity.InsertOnSubmit(likedActivity);
            db.SubmitChanges();
        }

        public void ActLikeCancel(string actID, string studentID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

            var res = from info in db.LikedActivity
                      where info.activityID == actID && info.studentID == studentID
                      select info;
            db.LikedActivity.DeleteOnSubmit(res.First());
            db.SubmitChanges();
        }

        public void ActSign(string actID, string studentID)
        {
            ActivityManagerDataContext dbSign = new ActivityManagerDataContext();

            // 获取本行活动信息
            MyActivity aSign = new MyActivity(actID);

            // 获取学生信息
            var resStuName = from info in dbSign.Student
                             where info.studentID == studentID
                             select info.studentName;
            string studentName = resStuName.First();

            var resStuPhone = from info in dbSign.StudentIdentified
                              where info.studentID == studentID
                              select info.phone;

            string phone = resStuPhone.First();

            // 判断是否可以报名（基于人数）
            int signed = int.Parse(aSign.Signed); // 已报名人数
            int maxSigned = int.Parse(aSign.MaxSigned); // 最大可报名人数

            if (signed >= maxSigned)
            {
                MessageBox.Show("抱歉！此活动人数已满！", "提示");
                return;
            }

            // 确认报名
            var resPlaceName = from info in dbSign.Place
                               where info.placeID == int.Parse(aSign.ActivityPlaceID)
                               select info.placeName;
            string placeName = resPlaceName.First();
            string Text =
                "活动名称：" + aSign.ActivityName + "\n" +
                "举办地点：" + placeName + "\n" +
                "举办时间：" + aSign.HoldDate + " " + aSign.HoldStart + ":00 至 " + aSign.HoldDate + " " + aSign.HoldEnd + ":00\n" +
                "报名者姓名：" + studentName + "\n" +
                "报名者学号：" + studentID + "\n" +
                "报名者联系方式：" + phone;

            if (MessageBox.Show(Text, "活动报名确认", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                // 点击确认，插入报名信息
                SignedActivity signedActivity = new SignedActivity();
                signedActivity.activityID = actID;
                signedActivity.studentID = studentID;

                ++signed;
                aSign.Signed = signed.ToString();
                aSign.Update();

                dbSign.SignedActivity.InsertOnSubmit(signedActivity);
                dbSign.SubmitChanges();
            }
        }

        public void ActSignCancel(string actID, string studentID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity();

            int signed = int.Parse(a.Signed); // 获取报名人数
            if (signed != 0)
                signed--;

            // 更新活动已报名人数
            //MyActivity a2 = new MyActivity(actID);

            var resState = from info in db.Activity
                           where info.activityID == actID
                           select info;
            resState.First().signed = signed;
            db.SubmitChanges();

            // 删除报名信息
            var resDel = from info in db.SignedActivity
                         where info.activityID == actID && info.studentID == studentID
                         select info;
            db.SignedActivity.DeleteOnSubmit(resDel.First());
            db.SubmitChanges();
        }
    }
}