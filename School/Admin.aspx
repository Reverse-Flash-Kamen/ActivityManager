﻿  <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="ActivityManager.Test.AdminWebForm" %>

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
        .overlay {
          position: absolute;
          top: 0;
          left: 0;
          width: 100%;
          height: 100%;
          background-color: rgba(0, 0, 0, 0);
          pointer-events:auto;
        }
    </style>
</head>
<body>  
    <form id="form1" runat="server">

        <%--遮罩--%>
        <div class="overlay" id="DivMask" runat="server">
            <%--主界面--%>        
            <div class="auto-style1" runat="server">
                <%--左导航栏--%>
                <div class="auto-style2">
                    <%--标题--%>
                    <div style="background-color:#758b9e; text-align:center;">
                        <h1 style="padding:50px 0px; color:white; margin:0;">校生通</h1>
                    </div>
                    <%--导航--%>
                    <div style="text-align:center; background-color:red "  id ="DivActMan" runat="server">
                        <asp:LinkButton ID="ActMan" runat="server" Font-Underline="False" Font-Size="Larger" ForeColor="White" OnClick="ActMan_Click" Height="39px" Width="180px" style="padding:22px 5px 5px;">活动管理</asp:LinkButton>
                    </div>
                    <div style="text-align:center; background-color:#ccad9f " id="DivPlaceMan" runat="server">
                        <asp:LinkButton ID="PlaceMan" runat="server" Font-Underline="False" Font-Size="Larger" ForeColor="White" OnClick="PlaceMan_Click" Height="40px" Width="180px" style="padding:22px 5px 5px;">场地管理</asp:LinkButton>
                    </div>
                    <%--校徽--%>
                    <div>
                        <asp:Image ID="Image" runat="server" Height="190px" ImageUrl="~/image/Ndky.png" Width="190px" style="margin-left:5px; margin-top: 220px; opacity:50%"/>
                    </div>
                </div>

               <div class="auto-style3" id="DivTopNav" runat="server">
                   <%--活动查询--%>
                   <div class="auto-style8" style="display:block;" id="DivSearchAct" runat="server">
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
                       <span class="auto-style7"> 活动类别&nbsp; 
                           <asp:DropDownList ID="type" runat="server">
                               <asp:ListItem Value="0">活动类别</asp:ListItem> 
                           <asp:ListItem Value="1">创新创业与就业见习</asp:ListItem>
                           <asp:ListItem Value="2">社会实践与志愿公益</asp:ListItem>
                           <asp:ListItem Value="3">思想引领与文体素质拓展</asp:ListItem>
                       </asp:DropDownList>
                       </span>
                   <div style="width:320px; position:absolute; top: 242px; left: 1236px;">
                       <asp:Button ID="commit" runat="server" Text="查询" OnClick="commit_Click" CssClass="auto-style6" Width="60px" CausesValidation="False" />
                       <asp:Button ID="flush" runat="server" Text="重置"  OnClick="flush_Click" CssClass="auto-style5" Width="60px" CausesValidation="False" />
                   </div>  
                </div>

                   <%--活动上导航栏--%>
                   <div class="auto-style9" style="display:block;" id="DivNavAct" runat="server">
                       &nbsp;&nbsp;
                       <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="False" Font-Size="Large" Font-Underline="True" ForeColor="Brown" OnClick="LinkButton1_Click">全部活动</asp:LinkButton>
                       &nbsp;&nbsp;
                       <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="False" Font-Size="Large" Font-Underline="False" ForeColor="Black" OnClick="LinkButton2_Click">待审核</asp:LinkButton>
                       &nbsp;&nbsp;
                       <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="False" Font-Size="Large" Font-Underline="False" ForeColor="Black" OnClick="LinkButton3_Click">待完成</asp:LinkButton>
                   </div>

                   <%--场地查询--%>
                   <div id="DivSearchPlace" runat="server" style="padding:85px 10px 10px 10px; float:right; width:50%; display:none;">
                       <div style="padding-left:130px">
                           <div style="float:left; padding:0px 10px">
                               <asp:Label ID="LblSearchPlaceName" runat="server" Text="场地名称"></asp:Label>
                               <asp:TextBox ID="TxtSearchPlaceName" runat="server"></asp:TextBox>
                           </div>

                           <div style="float:left; margin:0px 0px 0px 40px">
                               <asp:Label ID="LblPlaceState" runat="server" Text="场地状态"></asp:Label>
                               <asp:DropDownList ID="DropDownListPlaceState" runat="server">
                                   <asp:ListItem Value="0" Text="场地状态" Selected="True"></asp:ListItem>
                                   <asp:ListItem Value="1" Text="停用中"></asp:ListItem>
                                   <asp:ListItem Value="2" Text="空闲中"></asp:ListItem>
                                   <asp:ListItem Value="3" Text="使用中"></asp:ListItem>
                               </asp:DropDownList>
                           </div>
                       </div>

                       <br />
                       <div style="margin:30px 0px 0px 0px; float:right; padding-right:97px;">
                           <asp:Button ID="BtnCommit" runat="server" Text="查询" Width="60px" OnClick="BtnCommit_Click"/>
                           <asp:Button ID="BtnFlush" runat="server" Text="重置" Width="60px" style="margin-left:20px;" OnClick="BtnFlush_Click"/>
                       </div>
                   </div>

                   <%--场地上导航栏--%>
                   <div id="DivNavPlace" runat="server" style="width:80%; float:left; margin:-9px 0px 0px 15px; display:none;">
                       <asp:Button ID="BtnApply" runat="server" Text="+ 新增场地" Font-Size="Large" Height="50px" Width="130px" OnClick="BtnApply_Click"/>
                   </div>
               </div>

                <%--活动列表--%>
               <div id="DivActGv" runat="server" style="display:block">
                    <asp:GridView ID="GvTemplate" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="0" DataKeyNames="activityID" DataSourceID="schoolConnector" ForeColor="#333333" Height="525px" Width="85%" PageSize="5" OnDataBound="GridView1_DataBound" OnRowCommand="GvTemplate_RowCommand" HorizontalAlign="Center" GridLines="None" OnPageIndexChanging="GvTemplate_PageIndexChanging">
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

                <%--场地详情--%>
                <div id="DivPlaceGv" runat="server" style="display:none;">
                    <asp:GridView ID="GvPlace" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="0" DataSourceID="PlaceLinqDataSource" ForeColor="#333333" Height="525px" Width="85%" PageSize="5"  HorizontalAlign="Center" GridLines="None" DataKeyNames="placeID" OnDataBound="GvPlace_DataBound" OnRowCommand="GvPlace_RowCommand" OnPageIndexChanging="GvPlace_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="placeID" HeaderText="placeID" ReadOnly="True" SortExpression="placeID" InsertVisible="False" >
                                <ControlStyle CssClass="hidden" />
                                <FooterStyle CssClass="hidden" />
                                <HeaderStyle CssClass="hidden" />
                                <ItemStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="placeName" HeaderText="场地名称" SortExpression="placeName" HeaderStyle-Width="200px" ItemStyle-Height="84px" />
                            <asp:BoundField DataField="volume" HeaderText="场地容量" SortExpression="volume" HeaderStyle-Width="200px" ItemStyle-Height="84px"/>
                            <asp:BoundField DataField="placeState" HeaderText="场地状态" SortExpression="placeID" HeaderStyle-Width="200px" ItemStyle-Height="84px"/>
                            <asp:BoundField DataField="placeState" HeaderText="活动次数" SortExpression="placeID" HeaderStyle-Width="200px" ItemStyle-Height="84px"/>
                            <asp:ButtonField Text="按钮" >
                            <HeaderStyle Width="45px" />
                            </asp:ButtonField>
                            <asp:ButtonField HeaderText="操作" Text="按钮" >
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
                    <asp:LinqDataSource ID="PlaceLinqDataSource" runat="server" ContextTypeName="ActivityManager.App_Data.ActivityManagerDataContext" EntityTypeName="" TableName="Place" EnableDelete="True" EnableInsert="True" EnableUpdate="True" OrderBy="placeID">
                    </asp:LinqDataSource>
                </div>

            </div>
        </div>

        <%--退出功能--%>
        <asp:ImageButton ID="IBtnEsc" runat="server" style="position:absolute; top: 120px; left: 1600px;" ImageUrl="~/image/esc.png" Width="20"  OnClick="Esc_Click" CausesValidation="False"/>
        <%--审核功能--%>
        <div id="display" runat="server" style="display:none">
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

        <%--活动详情--%>
        <div runat="server" class="divCheck" id="CheckActDiv" style="display:none">
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

        <%--新增场地--%>
        <div id="DivAddPlace" runat="server" style="position:absolute; top: 303px; left: 875px; border: solid 1px; background-color:#F7F6F3; text-align:center; display:none;">
            <asp:Label ID="LblAddPlace" runat="server" Text="新增场地"></asp:Label>
            <asp:Label ID="LblAddPlaceID" runat="server" Text="-1" style="display:none"></asp:Label>
            <div style="padding:5px">
                <asp:Label ID="LblAddPlaceName" runat="server" Text="场地名称"></asp:Label>
                <asp:TextBox ID="TxtAddPlaceName" runat="server"></asp:TextBox>
            </div>
            <div style="padding:5px">
                <asp:Label ID="LblAddPlaceVolume" runat="server" Text="场地容量"></asp:Label>
                <asp:TextBox ID="TxtAddPlaceVolume" runat="server" TextMode="Number"></asp:TextBox>
            </div>
            <div style="padding:5px; float:right;">
            <asp:Button ID="BtnAddPlaceSubmit" runat="server" Text="确定" Width="60px" OnClick="BtnAddPlaceSubmit_Click"/>
            <asp:Button ID="BtnAddPlaceCancel" runat="server" Text="取消" Width="60px" style="margin-left:5px" OnClick="BtnAddPlaceCancel_Click"/>
            </div>
        </div>

    </form>
</body>
</html>

