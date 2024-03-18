<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Student.aspx.cs" Inherits="ActivityManager.Test.StudentWebForm" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .hidden {
            display:none;
        }

        .auto-style2 {
            width:15%;
            height: 750px;
            background-color:#e5ddd7;
            float:left;
        }
        .auto-style1 {
            height: 750px;
            margin-left: 275px;
            margin-right: 275px;
            margin-top: 100px;
            background-color:whitesmoke;
            border:outset
        }
        .auto-style3 {
            background-color:whitesmoke;
            height:225px;
        }
        .auto-style5 {
            margin-left: 31px;
        }
        .auto-style6 {
            margin-left: 150px;
            float:left;
        }
        .auto-style7 {
            margin-left: 50px;
        }
        .auto-style8 {
            padding-top: 85px;
        }
        .auto-style9 {
            padding-top:78px;
        }
        .head{
            height:50px;
        }
        .divCheck{
            position:absolute;
            top:19%;
            left:45%;
            width:300px;
            height:600px;
            background-color:#F7F6F3;
            padding:26px 50px 0px 50px;
            border:solid 1px;
            display:none;
        }
        .divInfo{
            position:absolute;
            top:11%;
            left:26%;
            width:1020px;
            height:720px;
            background-color:#F7F6F3;
            /*background-color: lightblue;*/
            padding:26px 50px 0px 50px;
            display:none;
            }
        .auto-style11 {
            margin-left: 229px;
        }
        .auto-style12 {
            margin-left: 10px;
        }
        .auto-style13 {
            margin-top: 15px;
            text-align:center;
            margin-bottom:15px;
        }
        .auto-style14 {
            text-align:center;
            background-color:#ccad9f 
        }
        .auto-style15 {
            padding:20px 5px 5px;
        }
        .li-style {
            list-style-type:none;
            padding:10px;
             Font-Size:larger;
        }
    </style>
</head>
<body>
    
    <form id="form1" runat="server">
        <div class="auto-style1" runat="server">
            <%--左导航栏--%>
           <div class="auto-style2">
               <%--标题--%>
               <div style="background-color:#758b9e; text-align:center;">
                   <h1 style="padding:50px 0px; color:white; margin:0;">校生通</h1>
               </div>
               <%--导航--%>
               <div class="auto-style14" id="DivAllAct" runat="server" style="background-color:red">
                   <asp:LinkButton ID="LbtnAllAct" runat="server" Font-Underline="False" Font-Size="Larger" ForeColor="White" OnClick="LbtnAllAct_Click" Height="40px" Width="180px" CssClass="auto-style15" CausesValidation="False">活动总览</asp:LinkButton>
               </div>
               <div class="auto-style14" id="DivMyAct" runat="server">
                   <asp:LinkButton ID="LbtnMyAct" runat="server" Font-Underline="False" Font-Size="Larger" ForeColor="White" OnClick="LbtnMyAct_Click" Height="40px" Width="180px" CssClass="auto-style15" CausesValidation="False">我的活动</asp:LinkButton>
               </div>
               <div class="auto-style14" id="DivMyInfo" runat="server">
                   <asp:LinkButton ID="LbtnMyInfo" runat="server" Font-Underline="False" Font-Size="Larger" ForeColor="White" OnClick="LbtnMyInfo_Click" Height="40px" Width="180px" CssClass="auto-style15" CausesValidation="False">我的信息</asp:LinkButton>
               </div>
               <%--校徽--%>
               <div>
                   <asp:Image ID="Image" runat="server" Height="190px" ImageUrl="~/image/Ndky.png" Width="190px" style="margin-left:5px; margin-top: 160px; opacity:50%"/>
               </div>
           </div>

             <%--上导航栏--%>
           <div class="auto-style3">
               <%--查询--%>
               <div class="auto-style8" id="DivSearch" runat="server">
                    <span class="">&nbsp;&nbsp;&nbsp;活动名称&nbsp;<asp:TextBox ID="name" runat="server"></asp:TextBox></span>
                    <span class="auto-style7">申请组织&nbsp;<asp:TextBox ID="org" runat="server"></asp:TextBox></span>
                    <span class="auto-style7"> 活动状态&nbsp; 
                        <asp:DropDownList ID="state" runat="server">
                            <asp:ListItem Value="0">活动状态</asp:ListItem> 
                        <asp:ListItem Value="5">待报名</asp:ListItem>
                        <asp:ListItem Value="6">报名中</asp:ListItem>
                        <asp:ListItem Value="7">待开始</asp:ListItem>
                        <asp:ListItem Value="8">活动中</asp:ListItem>
                        <asp:ListItem Value="9">已结束</asp:ListItem>
                        <asp:ListItem Value="10">已上报</asp:ListItem>
                        <asp:ListItem Value="11">已完成</asp:ListItem>
                    </asp:DropDownList>
                    </span>
                    <span class="auto-style7"> 活动类别&nbsp; 
                        <asp:DropDownList ID="type" runat="server">
                            <asp:ListItem Value="0">活动类别</asp:ListItem> 
                        <asp:ListItem Value="1">创新创业与就业见习</asp:ListItem>
                        <asp:ListItem Value="2">社会实践与志愿公益</asp:ListItem>
                        <asp:ListItem Value="3">思想引领与文体素质拓展</asp:ListItem>
                    </asp:DropDownList>
                    </span>
                <div style="width:320px; position:absolute; top: 242px; left: 1248px;">
                    <asp:Button ID="commit" runat="server" Text="查询" OnClick="commit_Click" CssClass="auto-style6" Width="60px" CausesValidation="False" />
                    <asp:Button ID="flush" runat="server" Text="重置"  OnClick="flush_Click" CssClass="auto-style5" Width="60px" CausesValidation="False" />
                </div>   
            </div>

               <div class="auto-style9" id="DivTopNov" runat="server">
                   &nbsp;&nbsp;
                   <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="False" Font-Size="Large" Font-Underline="true" ForeColor="Brown" OnClick="LinkButton1_Click" CausesValidation="False">全部活动</asp:LinkButton>
                   &nbsp;&nbsp;
                   <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="False" Font-Size="Large" Font-Underline="False" ForeColor="Black" OnClick="LinkButton2_Click" CausesValidation="False">可报名</asp:LinkButton>
                   &nbsp;&nbsp;
               </div>
           </div>

            <%--列表--%>
           <div>
                <asp:GridView ID="GvTemplate" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="0" DataKeyNames="activityID" DataSourceID="schoolConnector" ForeColor="#333333" Height="525px" Width="85%" PageSize="5" OnDataBound="GridView1_DataBound" OnRowCommand="GvTemplate_RowCommand" HorizontalAlign="Center" GridLines="None" OnDataBinding="GvTemplate_DataBinding" OnPageIndexChanging="GvTemplate_PageIndexChanging">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField AccessibleHeaderText="activityID" DataField="activityID" HeaderText="activityID" ReadOnly="True" SortExpression="activityID">
                        <ControlStyle CssClass="hidden" />
                        <FooterStyle CssClass="hidden" />
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="activityType" DataField="activityType" HeaderText="类别" SortExpression="activityType" >
                            <HeaderStyle Width="44px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="activityName" DataField="activityName" HeaderText="活动名称" SortExpression="activityName" >
                            <HeaderStyle Width="145px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="activityIntro" DataField="activityIntro" HeaderText="活动简介" SortExpression="activityIntro">
                            <ControlStyle CssClass="hidden" />
                            <FooterStyle CssClass="hidden" />
                            <HeaderStyle CssClass="hidden" />
                            <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="activityOrgID" DataField="activityOrgID" HeaderText="主办组织" SortExpression="activityOrgID">
                            <HeaderStyle Width="140px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="submitTime" DataField="submitTime" HeaderText="申报时间" SortExpression="submitTime">
                            <ControlStyle CssClass="hidden" />
                            <FooterStyle CssClass="hidden" />
                            <HeaderStyle Width="100px" CssClass="hidden" />
                            <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="activityPlaceID" DataField="activityPlaceID" HeaderText="举办场地" SortExpression="activityPlaceID">
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="maxSigned" DataField="maxSigned" HeaderText="最大人数" SortExpression="maxSigned">
                            <ControlStyle CssClass="hidden" />
                            <FooterStyle CssClass="hidden" />
                            <HeaderStyle CssClass="hidden" />
                            <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="signStartDate" DataField="signStartDate" HeaderText="报名时间" SortExpression="signStartDate" >
                            <HeaderStyle Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="signEndDate" DataField="signEndDate" HeaderText="signEndDate" SortExpression="signEndDate">
                            <ControlStyle CssClass="hidden" />
                            <FooterStyle CssClass="hidden" />
                            <HeaderStyle CssClass="hidden" />
                            <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="holdDate" DataField="holdDate" HeaderText="举办时间" SortExpression="holdDate" >
                            <HeaderStyle Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="holdStart" DataField="holdStart" HeaderText="holdStart" SortExpression="holdStart">
                            <ControlStyle CssClass="hidden" />
                            <FooterStyle CssClass="hidden" />
                            <HeaderStyle CssClass="hidden" />
                            <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="holdEnd" DataField="holdEnd" HeaderText="holdEnd" SortExpression="holdEnd">
                            <ControlStyle CssClass="hidden" />
                            <FooterStyle CssClass="hidden" />
                            <HeaderStyle CssClass="hidden" />
                            <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="failReason" DataField="failReason" HeaderText="failReason" SortExpression="failReason">
                            <ControlStyle CssClass="hidden" />
                            <FooterStyle CssClass="hidden" />
                            <HeaderStyle CssClass="hidden" />
                            <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="activityState" DataField="activityState" HeaderText="活动状态" SortExpression="activityState" >
                            <HeaderStyle Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="availableCredit" DataField="availableCredit" HeaderText="活动学分" SortExpression="availableCredit">
                            <HeaderStyle Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="signed" DataField="signed" HeaderText="报名人数" SortExpression="signed">
                         <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:ButtonField Text="操作" >
                            <HeaderStyle Width="40px" />
                        </asp:ButtonField>
                            <asp:ButtonField Text="按钮" HeaderText="操作" >
                            <HeaderStyle Width="40px" />
                        </asp:ButtonField>
                            <asp:ButtonField Text="按钮" >
                            <HeaderStyle Width="40px" />
                        </asp:ButtonField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#8d9ba5" Font-Bold="True" ForeColor="White" CssClass="head" />
                    <PagerStyle BackColor="#8d9ba5" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <asp:LinqDataSource ID="schoolConnector" runat="server" ContextTypeName="ActivityManager.App_Data.ActivityManagerDataContext" EntityTypeName="" TableName="Activity" EnableDelete="True" EnableInsert="True" EnableUpdate="True" OrderBy="activityState">
                </asp:LinqDataSource>
           </div>
        </div>
        
         <%--活动详情--%>
        <div runat="server" class="divCheck" id="CheckActDiv">
            <div class="auto-style13"><asp:Label runat="server" Text="活动申请详情" Font-Bold="True" Font-Size="Large"></asp:Label></div>

            <table class="auto-style12" style="vertical-align:middle">
                <tr><td >
                    <asp:Label ID="LblState" runat="server"></asp:Label></td></tr>
                <tr><td >
                    <asp:Label ID="LblFail" runat="server"></asp:Label></td></tr>
                <tr><td >
                    <asp:Label ID="LblActName" runat="server"></asp:Label></td></tr>
                <tr><td >
                    <asp:Label ID="LblActInfo" runat="server"></asp:Label></td></tr>
                <tr><td >
                    <asp:Label ID="LblPlace" runat="server"></asp:Label></td></tr>
                <tr><td >
                    <asp:Label ID="LblHoldDate" runat="server"></asp:Label></td></tr>
                <tr><td >
                    <asp:Label ID="LblSignDate" runat="server"></asp:Label></td></tr>
                <tr><td >
                    <asp:Label ID="LblMaxSize" runat="server"></asp:Label></td></tr>
                <tr><td >
                    <asp:Label ID="LblScore" runat="server"></asp:Label></td></tr>
                <tr><td>
                    <asp:Button ID="BtnCheck" runat="server" Text="返回" CssClass="auto-style11" OnClick="BtnCheck_Click" Width="60px" CausesValidation="False" /></td></tr>
            </table>
        </div>

         <%--信息面板--%>
        <div class="divInfo" runat="server" id="DivMyInfoR">
            <div style="width:120px; height:250px; padding:5px; float:left;">
                <asp:ImageButton ID="MyImage" runat="server" Height="100px" ImageUrl="~/image/users/7020820000.jpg" Width="100px" style="border-radius:50%; " OnClick="MyImage_Click" CausesValidation="False" ToolTip="更换头像"/>
                <div id="DivUploadImage" style="display:none" runat="server">
                    <asp:FileUpload ID="ImageUpload" runat="server" Width="70px" style="margin:10px;" accept="image/*"/>
                    <asp:Button ID="BtnImage" runat="server" Text="更换头像" style="margin:10px;" OnClick="BtnImage_Click"/>
                </div>
            </div>
            <div style="width:600px; height:280px; padding-left:10px;">
                <ul>
                    <li class="li-style">姓名：<asp:Label ID="LblStuName" runat="server" Text="张三"></asp:Label></li>
                    <li class="li-style">学号：<asp:Label ID="LblStuID" runat="server" Text="7020820000"></asp:Label></li>
                    <li class="li-style">专业班级：<asp:Label ID="LblMajor" runat="server" Text="计算机科学与技术206班"></asp:Label></li>
                    <li class="li-style">性别：<asp:Label ID="LblGender" runat="server" Text="男"></asp:Label></li>
                    <li class="li-style">学分：<asp:Label ID="LblCredit" runat="server" Text="0"></asp:Label></li>
                    <li class="li-style">
                        <asp:Button ID="BtnChangePsw" runat="server" Text="修改密码" style="padding:5px; margin:5px 5px 5px 0px;" OnClick="BtnChangePsw_Click" CausesValidation="False"/><asp:Button ID="BtnCredit" runat="server" Text="学分详情" style="padding:5px; margin:5px 5px 5px 15px;" CausesValidation="False"/>
                    </li>
                </ul>
            </div>

            <%--修改密码--%>
            <div style="width:600px; height:200px; margin-left:84px; display:none" id="DivChangePsw" runat="server">
                <ul style="list-style-type:none;">
                    <li class="li-style">
                        <asp:Label ID="LblPsw" runat="server" Text="密&#12288&#12288码："></asp:Label>
                        <asp:TextBox ID="TxtPsw" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvPsw" runat="server" ErrorMessage="请输入原密码！" ControlToValidate="TxtPsw" ForeColor="#CC0000">*</asp:RequiredFieldValidator>
                    </li>
                    <li class="li-style">
                        <asp:Label ID="LblNewPsw" runat="server" Text="新&#8194密&#8194码："></asp:Label>
                        <asp:TextBox ID="TxtNewPsw" runat="server" TextMode="Password" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvNewPsw" runat="server" ErrorMessage="请输入新密码！" ControlToValidate="TxtNewPsw" ForeColor="#CC0000">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CvNewPsw" runat="server" ErrorMessage="新密码不能与原密码相同！" ControlToValidate="TxtNewPsw" ControlToCompare="TxtPsw" Display="Dynamic" Operator="NotEqual" ForeColor="#CC0000" ToolTip="新密码必须是8位以上大小写字母、数字及特殊字符组合"></asp:CompareValidator>
                    </li>
                     <li class="li-style">
                        <asp:Label ID="LblRePsw" runat="server" Text="确认密码："></asp:Label>
                        <asp:TextBox ID="TxtRePsw" runat="server" TextMode="Password"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="RfvRePsw" runat="server" ErrorMessage="请再次输入新密码！" ControlToValidate="TxtRePsw" ForeColor="#CC0000">*</asp:RequiredFieldValidator>
                         <asp:CompareValidator ID="CvRePsw" runat="server" ErrorMessage="新密码两次输入不一致！" ControlToValidate="TxtRePsw" ControlToCompare="TxtNewPsw" Display="Dynamic" ForeColor="#CC0000"></asp:CompareValidator>
                    </li>
                    <li class="li-style">
                        <asp:Button ID="BtnSubmit" runat="server" Text="确  认" OnClick="BtnSubmit_Click" style="padding:5px; margin:5px 5px 5px 0px" ToolTip="密码必须以字母开头，长度在6~18之间，只能包含字符、数字和下划线"/>
                        <asp:Button ID="BtnCanel" runat="server" Text="取  消" CausesValidation="False" OnClick="BtnCanel_Click" style="padding:5px; margin:5px 5px 5px 15px"/>
                    </li>
                    <li class="li-style"><asp:Label ID="LblMessage" runat="server" ForeColor="#CC0000"></asp:Label></li>
                </ul>
                
            </div>
        </div>
    </form>
</body>
</html>