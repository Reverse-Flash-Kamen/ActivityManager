using ActivityManager.App_Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System;
using MathNet.Numerics.Distributions;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace ActivityManager
{
    public class Operation
    {
        // private static bool flag;

        public static void SetOperation(string commandName, string actID, string ID, GridView gv, LinqDataSource data)
        {
            Operation operation = new Operation();

            if (commandName == "check")
            {
                // 查看操作 前端实现
            }
            else if (commandName == "editA")
            {
                // 编辑操作 前端实现
            }
            else if (commandName == "deleteA")
            {
                operation.ActDelete(actID);
            }
            else if (commandName == "withdraw")
            {
                // 撤回操作
                operation.ActWithdraw(actID);
            }
            else if (commandName == "resubmit")
            {
                // 重新提交操作 = 编辑操作 前端实现
            }
            else if (commandName == "export")
            {
                // 导出报名名单操作
                operation.ActExport(actID, ID);
            }
            else if (commandName == "report")
            {
                // 上报操作
                operation.ActReport(actID);
            }
            else if (commandName == "complete")
            {
                // 完成操作
                operation.ActComplete(actID);
            }
            else if (commandName == "like")
            {
                // 收藏操作
                operation.ActLike(actID, ID);
            }
            else if (commandName == "likeCancel")
            {
                // 取消收藏操作
                operation.ActLikeCancel(actID, ID);
            }
            else if (commandName == "sign")
            {
                // 报名操作
                operation.ActSign(actID, ID);
            }
            else if (commandName == "signCancel")
            {
                // 取消报名操作
                operation.ActSignCancel(actID, ID);
            }
            else if (commandName == "aduit")
            {
                // 审核操作 前端实现
            }
            else if (commandName == "appraise")
            {
                // 评价功能 前端实现
            }
            else if (commandName == "exportAppraise")
            {
                operation.ActExportAppraise(actID, ID);
            }
            else if (commandName == "checkCode")
            {
                operation.ActCheckCode(actID);
            }
            else if (commandName == "checkIn")
            {
                // 签到功能 前端实现
            }

            //  gv.DataBind(); // 重现绑定数据，刷新添加|删除|退回的数据行

            Tool.SetButton(gv, ID);
        }

        private void ActCheckCode(string actID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from info in db.Activity
                      where info.activityID == actID
                      select info;

            if (res.First().checkInCode == "" || res.First().checkOutCode == "" || res.First().checkInCode == null || res.First().checkOutCode == null)
            {
                string num = "0123456789";
                string lower = "abcdefghijklmnopqrstuvwxyz";
                string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                List<string> list = new List<string>();
                list.Add(num);
                list.Add(lower);
                list.Add(upper);

                string checkInCode = "";
                string checkOutCode = "";

                // 生成两个不相同的，带数字，小写字符，大写字符的签到码
                Random rnd = new Random();
                do
                {
                    for (int n = 0; n < 6; n++)
                    {
                        int i = rnd.Next(0, 3);

                        switch (i)
                        {
                            case 0:
                                checkInCode += list[0][rnd.Next(0, 10)];
                                checkOutCode += list[0][rnd.Next(0, 10)];
                                break;

                            case 1:
                                checkInCode += list[1][rnd.Next(0, 26)];
                                checkOutCode += list[1][rnd.Next(0, 26)];
                                break;

                            case 2:
                                checkInCode += list[2][rnd.Next(0, 26)];
                                checkOutCode += list[2][rnd.Next(0, 26)];
                                break;
                        }
                    }
                } while (checkInCode == checkOutCode);

                // HttpContext.Current.Response.Write("ok");

                res.First().checkInCode = checkInCode;
                res.First().checkOutCode = checkOutCode;
                db.SubmitChanges();
            }

            HttpContext.Current.Response.Write("<script>alert('签到码已生成\\r签入码：" + res.First().checkInCode + "\\r签出码：" + res.First().checkOutCode + "');</script>");
        }

        private void ActDelete(string actID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

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
                HttpContext.Current.Response.Write("<script>alert('删除失败,请稍后重试！')</script>");
                return;
            }
            // HttpContext.Current.Response.Write("<script>alert('删除成功！')</script>");
        }

        private void ActWithdraw(string actID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

            /*if (MessageBox.Show("确定要撤回这条活动申请吗？", "WITHDRAW CONFIRM", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
            }*/

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
                HttpContext.Current.Response.Write("<script>alert('撤回失败,请稍后重试！')</script>");
                return;
            }
        }

        private void ActReport(string actID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

            var res = from info in db.Activity
                      where info.activityID == actID
                      select info;

            res.First().activityState = 10; // 更新状态为：已上报
            db.SubmitChanges();
        }

        private void ActComplete(string actID)
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

            // 根据活动ID获取报名学生ID

            var resStudentIDs = from info in db.SignedActivity
                                where info.activityID == actID && info.checkIn == 1 && info.checkOut == 1
                                select info.studentID;

            int type = int.Parse(a.ActivityType);

            foreach (var studentID in resStudentIDs)
            {
                // 给报名学生加学分
                // 需两次均签到

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
        }

        private void ActLike(string actID, string studentID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

            LikedActivity likedActivity = new LikedActivity();
            likedActivity.activityID = actID;
            likedActivity.studentID = studentID;
            db.LikedActivity.InsertOnSubmit(likedActivity);
            db.SubmitChanges();
        }

        private void ActLikeCancel(string actID, string studentID)
        {
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            MyActivity a = new MyActivity(actID);

            var res = from info in db.LikedActivity
                      where info.activityID == actID && info.studentID == studentID
                      select info;
            db.LikedActivity.DeleteOnSubmit(res.First());
            db.SubmitChanges();
        }

        private void ActSign(string actID, string studentID)
        {
            ActivityManagerDataContext dbSign = new ActivityManagerDataContext();

            // 获取本行活动信息
            MyActivity aSign = new MyActivity(actID);

            // 获取学生信息
            var resStuName = from info in dbSign.StudentIdentified
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
                // MessageBox.Show("抱歉！此活动人数已满！", "提示");
                HttpContext.Current.Response.Write("<script>alert('抱歉！此活动人数已满！')</script>");
                return;
            }

            // 确认报名
            var resPlaceName = from info in dbSign.Place
                               where info.placeID == int.Parse(aSign.ActivityPlaceID)
                               select info.placeName;
            string placeName = resPlaceName.First();
            string Text =
                "活动名称：" + aSign.ActivityName + "\\r" +
                "举办地点：" + placeName + "\\r" +
                "举办时间：" + aSign.HoldDate + " " + aSign.HoldStart + ":00 至 " + aSign.HoldDate + " " + aSign.HoldEnd + ":00\\r" +
                "报名者姓名：" + studentName + "\\r" +
                "报名者学号：" + studentID + "\\r" +
                "报名者联系方式：" + phone + "\\r" +
                "**如信息有误，请于‘我的信息’重新认证**";

            // 需要更改
            /*if (MessageBox.Show(Text, "活动报名确认", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
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
            }*/

            try
            {
                SignedActivity signedActivity = new SignedActivity();
                signedActivity.activityID = actID;
                signedActivity.studentID = studentID;

                ++signed;
                aSign.Signed = signed.ToString();
                aSign.Update();

                dbSign.SignedActivity.InsertOnSubmit(signedActivity);
                dbSign.SubmitChanges();
            }
            catch
            {
                HttpContext.Current.Response.Write("<script>alert('报名失败，请稍后重试！')</script>");
                return;
            }

            HttpContext.Current.Response.Write("<script>alert('" + Text + "')</script>");
        }

        private void ActSignCancel(string actID, string studentID)
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

        private void ActExport(string actID, string orgID)
        {
            // 创建一个工作薄
            IWorkbook workbook = new XSSFWorkbook();

            // 创建一个工作表
            ISheet sheet = workbook.CreateSheet("Sheet1");

            // 添加表头
            IRow headerRow = sheet.CreateRow(0);

            // 给表头创建列
            headerRow.CreateCell(0).SetCellValue(""); // 序号
            headerRow.CreateCell(1).SetCellValue("学科部");
            headerRow.CreateCell(2).SetCellValue("学号");
            headerRow.CreateCell(3).SetCellValue("姓名");
            headerRow.CreateCell(4).SetCellValue("性别");
            headerRow.CreateCell(5).SetCellValue("专业班级");
            headerRow.CreateCell(6).SetCellValue("联系电话");

            // 设置列宽
            for (int n = 0; n <= 6; n++)
            {
                sheet.SetColumnWidth(n, 20 * 256);
            }

            // 根据活动ID获取报名学生ID
            ActivityManagerDataContext db = new ActivityManagerDataContext();

            var res = from info in db.SignedActivity
                      where info.activityID == actID
                      select info.studentID;

            var studentIDs = res.ToList();

            int i = 1;
            // 根据学生ID获取学生信息
            foreach (var studentID in studentIDs)
            {
                var resInfo = from info in db.StudentIdentified
                              where info.studentID == studentID
                              select info;

                StudentIdentified studentIdentified = resInfo.First();

                // 创建新行
                IRow dataRow = sheet.CreateRow(i);

                // 创建列写入数据
                dataRow.CreateCell(0).SetCellValue(i);
                dataRow.CreateCell(1).SetCellValue(studentIdentified.faculty);
                dataRow.CreateCell(2).SetCellValue(studentIdentified.studentID);
                dataRow.CreateCell(3).SetCellValue(studentIdentified.studentName);
                dataRow.CreateCell(4).SetCellValue(studentIdentified.gender);
                dataRow.CreateCell(5).SetCellValue(studentIdentified.major + studentIdentified.@class);
                dataRow.CreateCell(6).SetCellValue(studentIdentified.phone);

                i++;
            }

            var resOrg = from info in db.Organization
                         where info.organizationID == orgID
                         select info.organizationName;

            var orgName = resOrg.First();

            // 存储路径不能是桌面，虚拟机没权限访问
            // string path = Path.Combine(Directory.GetCurrentDirectory(), "File", "Excel");
            // string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string path = $"C:\\Code\\Reverse-Flash-Kamen\\ActivityManager\\File\\Org\\{orgName}\\Signed";
            try
            {
                // 查看是否存在,不存在创建
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // 查活动名称
                var resAct = from info in db.Activity
                             where info.activityID == actID
                             select info;
                string actName = resAct.First().activityName;
                string signed = resAct.First().signed.ToString();

                // 保存工作薄
                var fileName = $"{actName}_{actID}_signed_{signed}.xlsx";
                var filePath = Path.Combine(path, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    workbook.Write(fileStream, true);
                    fileStream.Close();
                }

                // 关闭文件
                sheet = null;
                headerRow = null;
                workbook = null;
            }
            catch (System.IO.IOException)
            {
                HttpContext.Current.Response.Write("<script>alert('操作无法完成，因为文件已在Excel中打开！')</script>");
                return;
            }
            catch
            {
                HttpContext.Current.Response.Write("<script>alert('导出失败，请稍后重试！')</script>");
                return;
            }

            HttpContext.Current.Response.Write("<script>alert('导出成功，请在C:/Code/Reverse-Flash-Kamen/ActivityManager/File/#orgName#/Signed中查看！')</script>");
        }

        private void ActExportAppraise(string actID, string orgID)
        {
            // 创建一个工作薄
            IWorkbook workbook = new XSSFWorkbook();

            // 创建一个工作表
            ISheet sheet = workbook.CreateSheet("Sheet1");

            // 添加表头
            IRow headerRow = sheet.CreateRow(0);

            // 给表头创建列
            headerRow.CreateCell(0).SetCellValue(""); // 序号
            headerRow.CreateCell(1).SetCellValue("评分");
            headerRow.CreateCell(2).SetCellValue("评价");

            // 获取数据
            ActivityManagerDataContext db = new ActivityManagerDataContext();
            var res = from info in db.ActivityAppraise
                      where info.activityID == actID
                      select info;

            int i = 1;
            double sum = 0;
            foreach (var item in res)
            {
                // 创建新行
                IRow dataRow = sheet.CreateRow(i);

                // 创建列写入数据
                dataRow.CreateCell(0).SetCellValue(i);
                dataRow.CreateCell(1).SetCellValue(item.credit.ToString());
                dataRow.CreateCell(2).SetCellValue(item.appraise);

                sum += (double)item.credit;
                i++;
            }

            double avg = sum / res.Count();

            IRow avgRow = sheet.CreateRow(i);
            avgRow.CreateCell(0).SetCellValue("总计：");
            avgRow.CreateCell(1).SetCellValue(Math.Round(avg, 2).ToString());

            var resOrg = from info in db.Organization
                         where info.organizationID == orgID
                         select info.organizationName;

            var orgName = resOrg.First();

            string path = $"C:\\Code\\Reverse-Flash-Kamen\\ActivityManager\\File\\Org\\{orgName}\\Appraise";
            try
            {
                // 查看是否存在,不存在创建
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // 查活动名称
                var resAct = from info in db.Activity
                             where info.activityID == actID
                             select info;
                string actName = resAct.First().activityName;
                string signed = resAct.First().signed.ToString();

                // 保存工作薄
                var fileName = $"{actName}_{actID}_appraised_{res.Count()}.xlsx";
                var filePath = Path.Combine(path, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    workbook.Write(fileStream, true);
                    fileStream.Close();
                }

                // 关闭文件
                sheet = null;
                headerRow = null;
                workbook = null;
            }
            catch (System.IO.IOException)
            {
                HttpContext.Current.Response.Write("<script>alert('操作无法完成，因为文件已在Excel中打开！')</script>");
                return;
            }
            catch
            {
                HttpContext.Current.Response.Write("<script>alert('导出失败，请稍后重试！')</script>");
                return;
            }

            HttpContext.Current.Response.Write("<script>alert('导出成功，请在C:/Code/Reverse-Flash-Kamen/ActivityManager/File/#orgName#/Appraised中查看！')</script>");
        }
    }
}