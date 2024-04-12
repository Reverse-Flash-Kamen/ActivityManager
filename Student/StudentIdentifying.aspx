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

                <div id="DivIdentified" runat="server" >
                    <div style="padding:5px">
                        <asp:Label ID="LblStudentID" runat="server" Text="学&#12288号："></asp:Label>
                        <asp:TextBox ID="TxtStudentID" runat="server" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvStudentID" runat="server" ErrorMessage="请输入学号！" ForeColor="#CC0000" ControlToValidate="TxtStudentID">*</asp:RequiredFieldValidator>
                        <br/>
                        <asp:Label ID="LblStudentName" runat="server" Text="姓&#12288名："></asp:Label>
                        <asp:TextBox ID="TxtStudentName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvStudentName" runat="server" ErrorMessage="请输入姓名！" ForeColor="#CC0000" ControlToValidate="TxtStudentName">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:Label ID="LblID" runat="server" Text="身份证："></asp:Label>
                        <asp:TextBox ID="TxtID" runat="server" TextMode="SingleLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvID" runat="server" ErrorMessage="请输入身份证！" ForeColor="#CC0000" ControlToValidate="TxtStudentName">*</asp:RequiredFieldValidator>
                    </div>

       
                    <div style="padding:5px">
                        <asp:ValidationSummary ID="Vs" runat="server" ShowMessageBox="True" ShowSummary="False" />
                        <asp:Button ID="BtnIdentifying" runat="server" Text="认证" OnClick="BtnIdentifying_Click" style="padding:5px 10px 5px 10px"/>
                        <asp:Button ID="BtnReturn" runat="server" Text="返回" CausesValidation="False" OnClick="BtnReturn_Click" style="margin-left:10px; margin-top:5px; padding:5px 10px 5px 10px"/>
                    </div>
                </div>
            </div>

        </div>

        <%--更改--%>
    </form>
</body>
</html>
