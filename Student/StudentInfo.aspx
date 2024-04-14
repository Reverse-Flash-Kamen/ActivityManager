<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentInfo.aspx.cs" Inherits="ActivityManager.Student.StudentInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <style type="text/css">
        .auto-style1 {
            width: 1779px;
            height: 300px;
            padding:20px;
            margin-top:150px;
        }
        .auto-style2 {
            width: 455px;
            border:solid;
        }
        .auto-style3 {
            margin-left: 163px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
<div class="auto-style1">
    <div style="margin:auto; text-align:center; padding-bottom:10px" ><asp:Label ID="Label" runat="server" Text="学生信息页面" Font-Size="Larger"></asp:Label></div>

    <div style="margin:auto; text-align:center; padding:5px" class="auto-style2">

              <div id="DivInfo" runat="server">
                    <asp:DropDownList ID="DropDownListFaculty" runat="server" style="position:absolute; top: 230px; left: 781px;" AutoPostBack="True" OnSelectedIndexChanged="DropDownListFaculty_SelectedIndexChanged">
                        <asp:ListItem Text="信息学科部" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="人文学科部" Value="1"></asp:ListItem>
                        <asp:ListItem Text="理工学科部" Value="2"></asp:ListItem>
                        <asp:ListItem Text="财经学科部" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:DropDownList ID="DropDownListMajor" runat="server" style="position:absolute; top: 250px; left: 781px;" AutoPostBack="True">
                        <asp:ListItem Text="自动化" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="电气工程及其自动化" Value="1" ></asp:ListItem>
                        <asp:ListItem Text="通信工程" Value="2" ></asp:ListItem>
                        <asp:ListItem Text="电子商务" Value="3" ></asp:ListItem>
                        <asp:ListItem Text="软件工程" Value="4" ></asp:ListItem>
                        <asp:ListItem Text="计算机科学与技术" Value="5" ></asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:DropDownList ID="DropDownListGrade" runat="server" style="position:absolute; top: 270px; left: 781px;" AutoPostBack="True">
                    </asp:DropDownList>
                    <br />
                    <asp:DropDownList ID="DropDownListClass" runat="server" style="position:absolute; top: 290px; left: 781px;" AutoPostBack="True">
                    </asp:DropDownList>
                    <br />
                    <div style="margin-top:15px">
                        <asp:Label ID="LblNewPsw" runat="server" Text="密&#12288&#12288码："></asp:Label>
                        <asp:TextBox ID="TxtNewPsw" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvNewPsw" runat="server" ErrorMessage="请输入密码！" ForeColor="#CC0000" ControlToValidate="TxtNewPsw" >*</asp:RequiredFieldValidator>
                        <br />
                        <asp:Label ID="LblRePsw" runat="server" Text="确认密码："></asp:Label>
                        <asp:TextBox ID="TxtRePsw" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvRePsw" runat="server" ErrorMessage="请再次输入密码！" ForeColor="#CC0000" ControlToValidate="TxtRePsw">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator" runat="server" ErrorMessage="两次输入的密码不一致！" ControlToValidate="TxtRePsw" ControlToCompare="TxtNewPsw" Display="None"></asp:CompareValidator>
                        <br />
                        <asp:Label ID="LblPhone" runat="server" Text="联系电话："></asp:Label>
                        <asp:TextBox ID="TxtPhone" runat="server" TextMode="Phone"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvPhone" runat="server" ErrorMessage="请输入电话号码！" ForeColor="#CC0000" ControlToValidate="TxtPhone">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:RadioButtonList ID="RadioButtonListGender" runat="server" RepeatDirection="Horizontal" style="margin-left:84px">
                            <asp:ListItem Text="男" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="女" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div style="margin-left:190px; padding:5px">
                        <asp:Button ID="BtnSubmit" runat="server" Text="确定" style="padding:3px 15px" OnClick="BtnSubmit_Click"/>
                    </div>

                </div>
    </div>

</div>
    </form>
</body>
</html>
