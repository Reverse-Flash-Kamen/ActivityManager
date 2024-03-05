<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ActivityManager.Sign" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8;"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 210px;
            padding:20px;
            margin:auto;
            margin-top:200px;
            border: solid;
            
        }
        .auto-style2 {
            margin-left: 12px;
        }
        .auto-style4 {
            margin-top: 10px;
            width: 200px;
            margin-left: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="auto-style1">
            <div>
                <asp:Label ID="LblID" runat="server" Text="账号："></asp:Label> 
                <asp:TextBox ID="TxtID" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RfvID" runat="server" ErrorMessage="请输入账户ID！" ControlToValidate="txtID" ForeColor="#CC0000">*</asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="LblPsw" runat="server" Text="密码："></asp:Label>
                <asp:TextBox ID="TxtPsw" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RfvPsw" runat="server" ErrorMessage="请输入密码！" ControlToValidate="txtPsw" ForeColor="#CC0000">*</asp:RequiredFieldValidator>
                <asp:ValidationSummary ID="Vs" runat="server" ShowMessageBox="True" ShowSummary="False"/>
            </div>
            <div>
                <asp:RadioButtonList ID="RblType" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem>校方</asp:ListItem>
                    <asp:ListItem>组织</asp:ListItem>
                    <asp:ListItem>学生</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="auto-style4">
                <asp:Button ID="BtnLogin" runat="server" Text="登  录" OnClick="BtnLogin_Click"  Width="80px" />
                <asp:Button ID="BtnIdentifying" runat="server" Text="学生认证" OnClick="BtnIdentifying_Click" CausesValidation="False" CssClass="auto-style2" Width="80px" />
            </div>
        </div>

    </form>
</body>
</html>
