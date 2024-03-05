<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Org.aspx.cs" Inherits="ActivityManager.Test.OrgWebForm" %>

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
            border:outset;
            
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
            padding-top:35px;
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
        .auto-style14 {
            margin-left: 30px;
        }

        #display {
            background-color: #f7f6f3;
            position: absolute;
            top: 20%;
            left: 36%;
            border: 1px solid;
            padding-left: 26px 52px 17px 52px;
        }

        #display div {
            margin: 17px 17px;
        }
    </style>
</head>
<body>
    
    <form id="form1" runat="server">
        <div id="display" runat="server" visible="False">
            <h2 style="width: 100px; margin: 17px auto;">活动申请</h2>
            <div>
                活动名称
                <asp:TextBox ID="aName" runat="server"></asp:TextBox>
                <asp:Label ID="nameCnt" runat="server"></asp:Label>
                <asp:RequiredFieldValidator ID="nameRequiredFieldValidator" ControlToValidate="aName" runat="server" ErrorMessage="活动名称不得为空!"></asp:RequiredFieldValidator>
            </div>

            <div>
                活动介绍
                <asp:TextBox ID="aIntro" runat="server" TextMode="MultiLine"></asp:TextBox>
                <asp:Label ID="introCnt" runat="server"></asp:Label>
                <asp:RequiredFieldValidator ID="introRequiredFieldValidator" ControlToValidate="aIntro" runat="server" ErrorMessage="活动介绍不得为空!"></asp:RequiredFieldValidator>
            </div>

            <div>
                举办地点
                <asp:DropDownList ID="aPlace" runat="server" DataTextField="placeName" DataValueField="placeID" DataSourceID="placeConnector" Width="120px"></asp:DropDownList><asp:LinqDataSource runat="server" EntityTypeName="" ID="placeConnector" ContextTypeName="ActivityManager.App_Data.ActivityManagerDataContext" TableName="Place"></asp:LinqDataSource>
            </div>
            
            <div>
                报名时间
                <asp:LinkButton ID="setSignStartDate" runat="server" OnClick="setSignStartDate_Click" CausesValidation="False">选择报名开始日期</asp:LinkButton>
                &nbsp; &nbsp;
                <asp:LinkButton ID="setSignEndDate" runat="server" OnClick="setSignEndDate_Click" CausesValidation="False">选择报名结束日期</asp:LinkButton>
                <asp:Calendar ID="aSignStartDate" runat="server" ShowGridLines="True" Visible="false" OnSelectionChanged="aSignStartDate_SelectionChanged"></asp:Calendar>
                <asp:Calendar ID="aSignEndDate" runat="server" ShowGridLines="True" Visible="false" OnSelectionChanged="aSignEndDate_SelectionChanged"></asp:Calendar>
            </div>

            <div>
                举办时间
                <asp:LinkButton ID="setHoldDate" runat="server" OnClick="setHoldDate_Click" CausesValidation="False">选择举办日期</asp:LinkButton>
                <asp:DropDownList ID="aHoldStart" runat="server" Enabled="False" Width="66px" OnSelectedIndexChanged="aHoldStart_SelectedIndexChanged"></asp:DropDownList>
                <asp:DropDownList ID="aHoldEnd" runat="server" Enabled="False" Width="66px"></asp:DropDownList>
                <asp:Calendar ID="aHoldDate" runat="server" ShowGridLines="True" Visible="false" OnSelectionChanged="aHoldDate_SelectionChanged"></asp:Calendar>
            </div>

            <div>
                人数上限
                <asp:TextBox ID="aVolume" runat="server" Columns="1"></asp:TextBox>
                <asp:RangeValidator ID="volumeRangeValidator" runat="server" ErrorMessage="请输入50~800的数字!" MaximumValue="800" MinimumValue="50" ControlToValidate="aVolume" Type="Integer"></asp:RangeValidator>
            </div>
            

            <div>
                活动学分
                 <asp:TextBox ID="aCredit" runat="server" Columns="1" OnTextChanged="aCredit_TextChanged"></asp:TextBox>
                <asp:Button ID="creditUp" runat="server" Text="▲ " Width="26px" OnClick="creditUp_Click" CausesValidation="False" />
                <asp:Button ID="creditDown" runat="server" Text="▼ " Width="26px" OnClick="creditDown_Click" CausesValidation="False" />
                <asp:RangeValidator ID="creditRangeValidator" runat="server" ErrorMessage="请输入1~8的数字!" MaximumValue="8" MinimumValue="1" ControlToValidate="aCredit" Type="Integer"></asp:RangeValidator>
            </div>

            <div>
                <asp:Button ID="submitA" runat="server" Text="提交" OnClick="submit_Click" />
                <asp:Button ID="saveA" runat="server" Text="保存" OnClick="save_Click" />
                <asp:Button ID="returnA" runat="server" Text="返回" CausesValidation="False" OnClick="returnA_Click" />
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
                   <asp:Button ID="BtnApply" runat="server" Text="+ 申请活动" CssClass="auto-style14" Font-Size="Large" Height="50px" Width="130px" OnClick="BtnApply_Click" />
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
                        <ControlStyle CssClass="hidden" />
                        <FooterStyle CssClass="hidden" />
                        <HeaderStyle Width="140px" CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
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
                        <asp:BoundField AccessibleHeaderText="signed" DataField="signed" HeaderText="报名人数" SortExpression="signed">
                        <HeaderStyle Width="100px" />
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
