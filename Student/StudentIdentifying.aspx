<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentIdentifying.aspx.cs" Inherits="ActivityManager.Student.WebForm1" %>

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
            <div style="margin:auto; text-align:center; padding-bottom:10px" ><asp:Label ID="Label" runat="server" Text="学生认证页面" Font-Size="Larger"></asp:Label></div>

            <div style="margin:auto; text-align:center; padding:5px" class="auto-style2">
                <div style="padding:5px">
                    <asp:Label ID="LblStudentID" runat="server" Text="学号："></asp:Label>
                    <asp:TextBox ID="TxtStudentID" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RfvStudentID" runat="server" ErrorMessage="请输入学号！" ForeColor="#CC0000" ControlToValidate="TxtStudentID">*</asp:RequiredFieldValidator>
                    <br/>
                    <asp:Label ID="LblStudentName" runat="server" Text="姓名："></asp:Label>
                    <asp:TextBox ID="TxtStudentName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RfvStudentName" runat="server" ErrorMessage="请输入姓名！" ForeColor="#CC0000" ControlToValidate="TxtStudentName">*</asp:RequiredFieldValidator>
                </div>

                <div>
                    <asp:RadioButtonList ID="RblGender" runat="server" RepeatDirection="Horizontal" CssClass="auto-style3">
                        <asp:ListItem>男</asp:ListItem>
                        <asp:ListItem Value="女"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>

                <div style="padding:5px">
                    <asp:DropDownList ID="DdlMajor" runat="server" style="margin-left:40px;">
                        <asp:ListItem>计算机科学与技术</asp:ListItem>
                        <asp:ListItem>软件工程</asp:ListItem>
                        <asp:ListItem>电子商务</asp:ListItem>
                        <asp:ListItem>通讯工程</asp:ListItem>
                        <asp:ListItem>自动化</asp:ListItem>
                    </asp:DropDownList>

                    <asp:DropDownList ID="DdlClass" runat="server">
                        <asp:ListItem>201</asp:ListItem>
                        <asp:ListItem>202</asp:ListItem>
                        <asp:ListItem>203</asp:ListItem>
                        <asp:ListItem>204</asp:ListItem>
                        <asp:ListItem>205</asp:ListItem>
                        <asp:ListItem>206</asp:ListItem>
                    </asp:DropDownList>
                    <br/>
                    <asp:Label ID="LblPhone" runat="server" Text="电话："></asp:Label>
                    <asp:TextBox ID="TxtPhone" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RfvPhone" runat="server" ErrorMessage="请输入电话！" ForeColor="#CC0000" ControlToValidate="TxtPhone">*</asp:RequiredFieldValidator>
                </div>

                <div style="padding:5px">
                    <asp:ValidationSummary ID="Vs" runat="server" ShowMessageBox="True" ShowValidationErrors="False" />
                    <asp:Button ID="BtnIdentifying" runat="server" Text="认证" OnClick="BtnIdentifying_Click" style="padding:5px 10px 5px 10px"/>
                    <asp:Button ID="BtnReturn" runat="server" Text="返回" CausesValidation="False" OnClick="BtnReturn_Click" style="margin-left:10px; margin-top:5px; padding:5px 10px 5px 10px"/>
                </div>
            </div>

        </div>

        <%--更改--%>
    </form>
</body>
</html>
