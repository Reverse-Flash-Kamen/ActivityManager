<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="ActivityManager.Test.AdminWebForm" %>

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
        }
        .auto-style7 {
            margin-left: 50px;
        }
        .auto-style8 {
            padding-top: 85px;
        }
        .auto-style9 {
            padding-top:65px;
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
            border: solid 1px
         
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

        #display {
            background-color: aquamarine;
            position: absolute;
            top: 39%;
            left: 47%;
            border: 1px solid;
            padding: 10px 26px 10px 26px;
            
            background-color: #f7f6f3;
        }
    </style>
</head>
<body>
    
    <form id="form1" runat="server">
        <div id="display" runat="server" visible="false">
            <h3 style="text-align: center; margin: 0 0 6px 0;">
                审核活动
            </h3>
            <div>
                <asp:RadioButton ID="passRadio" runat="server" Text="审核通过" GroupName="1"/>
                <asp:RadioButton ID="noPassRadio" runat="server" Text="审核不通过" GroupName="1"/>
            </div>
            
            <div style="margin: 6px;">
                不通过原因<asp:TextBox ID="failReason" runat="server" TextMode="MultiLine" Width="100px"></asp:TextBox>
                <br />
                
            </div>

            <div style="text-align: center; margin-top: 17px;">
                <asp:Button ID="btnConfirm" runat="server" Text="确认" OnClick="btnConfirm_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" CausesValidation="False" />
            </div>
            
        </div>
        
        <div class="auto-style1" runat="server">
           <%--左导航栏--%>
           <div class="auto-style2">
               <%--标题--%>
               <div style="background-color:#758b9e; text-align:center;">
                   <h1 style="padding:50px 0px; color:white; margin:0;">校生通</h1>
               </div>
               <%--导航--%>
               <div style="padding:20px 20px; text-align:center; background-color:#ccad9f " >
                   <asp:LinkButton ID="ActMan" runat="server" Font-Underline="False" Font-Size="Larger" ForeColor="White" OnClick="ActMan_Click">活动管理</asp:LinkButton>
               </div>
               <%--校徽--%>
               <div>
                   <asp:Image ID="Image" runat="server" Height="190px" ImageUrl="~/Ndky.png" Width="190px" style="margin-left:5px; margin-top: 250px; opacity:50%"/>
               </div>
           </div>

           <div class="auto-style3">
               <%--查询--%>
               <div class="auto-style8">
                    <span class="">&nbsp;&nbsp;&nbsp;活动名称&nbsp;<asp:TextBox ID="name" runat="server"></asp:TextBox></span>
                    <span class="auto-style7">申请组织&nbsp;<asp:TextBox ID="org" runat="server"></asp:TextBox></span>
                    <span class="auto-style7"> 活动状态&nbsp; 
                        <asp:DropDownList ID="state" runat="server"> 
                        <asp:ListItem Value="0">活动状态</asp:ListItem>
                        <asp:ListItem Value="2">待审核</asp:ListItem>
                        <asp:ListItem Value="3">未通过</asp:ListItem>
                        <asp:ListItem Value="4">审核过期</asp:ListItem>
                        <asp:ListItem Value="5">待报名</asp:ListItem>
                        <asp:ListItem Value="6">报名中</asp:ListItem>
                        <asp:ListItem Value="7">待开始</asp:ListItem>
                        <asp:ListItem Value="8">活动中</asp:ListItem>
                        <asp:ListItem Value="9">已结束</asp:ListItem>
                        <asp:ListItem Value="10">已上报</asp:ListItem>
                        <asp:ListItem Value="11">已完成</asp:ListItem>
                    </asp:DropDownList>
                    </span>

                <asp:Button ID="commit" runat="server" Text="查询" OnClick="commit_Click" CssClass="auto-style6" Width="60px" />
                <asp:Button ID="flush" runat="server" Text="重置"  OnClick="flush_Click" CssClass="auto-style5" Width="60px" />
            </div>

               <%--上导航栏--%>
               <div class="auto-style9">
                   &nbsp;&nbsp;
                   <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="False" Font-Size="Large" Font-Underline="False" ForeColor="Black" OnClick="LinkButton1_Click">全部活动</asp:LinkButton>
                   &nbsp;&nbsp;
                   <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="False" Font-Size="Large" Font-Underline="False" ForeColor="Black" OnClick="LinkButton2_Click">待审核</asp:LinkButton>
                   &nbsp;&nbsp;
                   <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="False" Font-Size="Large" Font-Underline="False" ForeColor="Black" OnClick="LinkButton3_Click">待完成</asp:LinkButton>
               </div>
           </div>

            <%--列表--%>
           <div>
                <asp:GridView ID="GvTemplate" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="0" DataKeyNames="activityID" DataSourceID="schoolConnector" ForeColor="#333333" Height="525px" Width="85%" PageSize="5" OnDataBound="GridView1_DataBound" OnRowCommand="GvTemplate_RowCommand" HorizontalAlign="Center" GridLines="None">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField AccessibleHeaderText="activityID" DataField="activityID" HeaderText="activityID" ReadOnly="True" SortExpression="activityID">
                        <ControlStyle CssClass="hidden" />
                        <FooterStyle CssClass="hidden" />
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="activityName" DataField="activityName" HeaderText="活动名称" SortExpression="activityName" >
                        <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="activityIntro" DataField="activityIntro" HeaderText="活动简介" SortExpression="activityIntro">
                        <ControlStyle CssClass="hidden" />
                        <FooterStyle CssClass="hidden" />
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="activityOrgID" DataField="activityOrgID" HeaderText="申办组织" SortExpression="activityOrgID">
                        <HeaderStyle Width="140px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="submitTime" DataField="submitTime" HeaderText="申报时间" SortExpression="submitTime">
                        <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="activityPlaceID" DataField="activityPlaceID" HeaderText="举办场地" SortExpression="activityPlaceID">
                        <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="availableCredit" DataField="availableCredit" HeaderText="活动学分" SortExpression="availableCredit">
                        <ControlStyle CssClass="hidden" />
                        <FooterStyle CssClass="hidden" />
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="maxSigned" DataField="maxSigned" HeaderText="最大人数" SortExpression="maxSigned">
                        <ControlStyle CssClass="hidden" />
                        <FooterStyle CssClass="hidden" />
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField AccessibleHeaderText="signed" DataField="signed" HeaderText="报名人数" SortExpression="signed">
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
                        <HeaderStyle Width="60px" />
                        </asp:BoundField>
                        <asp:ButtonField HeaderText="操作" Text="操作" >
                        <HeaderStyle Width="45px" />
                        </asp:ButtonField>
                        <asp:ButtonField Text="按钮" >
                        <HeaderStyle Width="45px" />
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
        <div runat="server" class="divCheck" id="CheckActDiv" visible="False">
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
                    <asp:Button ID="BtnCheck" runat="server" Text="返回" CssClass="auto-style11" OnClick="BtnCheck_Click" Width="60px" /></td></tr>
            </table>
        </div>

    </form>
</body>
</html>
