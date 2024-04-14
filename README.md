# ActivityManager
通过Session会话限制只能从Login端进入

## 学生端登录
1. 7020820312 xlf123123
2. 7020820300 zzy123123 
3. 7020820297 wcc123123

## 组织端登录
org2022121201
qzx123456789

## 学校端登录
ndky000000
123456

## 数据库连接
于 **ActivityManager\App_Data\ActivityManager.designer.cs** 修改 **C:\\Code\\Reverse-Flash-Kamen\\**

private static string conStr = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Code\\Reverse-Flash-Kamen\\ActivityManager\\App_Data\\ActivityManager.mdf;Integrated Security=True";

## 未能找到路径“~ActivityManager\bin\roslyn\csc.exe”的一部分
重新生成解决方案

## 学生端无数据可能原因
每首次运行时，程序会根据计算机系统时间更新活动状态。而学生只能查看特定状态的活动，于是导致无数据。

**解决方法：** 
win11下->右键窗口右下角任务栏时间->调整日期和时间->自动设置时间->关->更改时间使活动状态刷新至需要。
## 2024/4/10
下载add_info_1.0分支
