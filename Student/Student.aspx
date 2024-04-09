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
            padding:22px 5px 5px;
        }
        .li-style {
            list-style-type:none;
            padding:10px;
             Font-Size:larger;
        }
        .mask{
                background-color: rgba(0, 0, 0, .3);
                position: absolute;
                width:100%;
                height:100%;
                z-index: 999;
                display: none;
        }
        .overlay {
          position: absolute;
          top: 1px;
          left: -10px;
          width: 100%;
          height: 100%;
          background-color: rgba(0, 0, 0, 0);
          pointer-events:auto;
        }
    </style>
</head>
<body style="background-color:white">
    <form id="form1" runat="server">
        <%--遮罩--%>
       <div runat="server" class="overlay" id="DivMask" >
            <%--主界面--%>
            <div class="auto-style1" runat="server">
                <%--左导航栏--%>
                <div class="auto-style2">
               <%--标题--%>
               <div style="background-color:#758b9e; text-align:center;">
                   <h1 style="padding:50px 0px; color:white; margin:0;">校生通</h1>
               </div>
               <%--导航--%>
               <div class="auto-style14" id="DivAllAct" runat="server" style="background-color:red">
                   <asp:LinkButton ID="LbtnAllAct" runat="server" Font-Underline="False" Font-Size="Larger" ForeColor="White" OnClick="LbtnAllAct_Click" Height="39px" Width="180px" CssClass="auto-style15" CausesValidation="False">活动总览</asp:LinkButton>
               </div>
               <div class="auto-style14" id="DivActPlaza" runat="server">
                   <asp:LinkButton ID="LbtnActPlaza" runat="server" Font-Underline="False" Font-Size="Larger" ForeColor="White" OnClick="LbtnActPlaza_Click" Height="40px" Width="180px" CssClass="auto-style15" CausesValidation="False">活动广场</asp:LinkButton>
               </div>
               <div class="auto-style14" id="DivMyAct" runat="server">
                   <asp:LinkButton ID="LbtnMyAct" runat="server" Font-Underline="False" Font-Size="Larger" ForeColor="White" OnClick="LbtnMyAct_Click" Height="40px" Width="180px" CssClass="auto-style15" CausesValidation="False">我的活动</asp:LinkButton>
               </div>
               <div class="auto-style14" id="DivMyInfo" runat="server">
                   <asp:LinkButton ID="LbtnMyInfo" runat="server" Font-Underline="False" Font-Size="Larger" ForeColor="White" OnClick="LbtnMyInfo_Click" Height="40px" Width="180px" CssClass="auto-style15" CausesValidation="False">我的信息</asp:LinkButton>
               </div>
               <%--校徽--%>
               <div>
                   <asp:Image ID="Image" runat="server" Height="190px" ImageUrl="~/image/Ndky.png" Width="190px" style="margin-left:5px; margin-top: 100px; opacity:50%"/>
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
                <div style="width:320px; position:absolute; top: 242px; left: 1243px;">
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

               <div id="DivBuildActTeam" runat="server" style="display:none; padding-top:35px;" >
                   <asp:Button ID="BtnBuildActTeam" runat="server" Text="+ 申请组队" style="margin-left:16px" Font-Size="Large" Height="50px" Width="130px" OnClick="BtnBuildActTeam_Click" CausesValidation="False"/>
               </div>
           </div>

                <%--活动列表--%>
                <div id="DivAct" runat="server">
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
                </div><!-活动列表-> 
                
                <%--队伍列表--%>
                <div id="DivTeam" runat="server" style="display:none;">
                    <asp:GridView ID="GridView1" runat="server">
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
                </div><!-队伍列表->
            </div><!-主界面->
        </div><!-遮罩->
            
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

         <%--评价功能--%>
        <div runat="server" id="DivAppraise" style="position:absolute; width:300px; height:200px; top:40%; left:50%; background-color:#F7F6F3; border: solid 1px; display:none;">
            <div style="margin:10px">
                <asp:Label ID="LblAppraise" runat="server" style="margin:10px 0px;" Font-Bold="True" Font-Size="Medium"></asp:Label>
                <asp:Label ID="LblActID" runat="server" Text="" style="display:none"></asp:Label>
                <br />
                <asp:RadioButtonList ID="RblAppraise" runat="server" RepeatDirection="Horizontal" style="margin:5px 0px">
                    <asp:ListItem Text="1"></asp:ListItem>
                    <asp:ListItem Text="2"></asp:ListItem>
                    <asp:ListItem Text="3"></asp:ListItem>
                    <asp:ListItem Text="4"></asp:ListItem>
                    <asp:ListItem Text="5"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:TextBox ID="TxtAppraise" runat="server" Columns="30" Rows="5" TextMode="MultiLine" style="resize:none; margin:5px 0px" ></asp:TextBox>
                <br />
                <asp:Button ID="BtnAppraiseCommit" runat="server" Text="确定" OnClick="BtnAppraiseCommit_Click" CausesValidation="False" style="padding:2px 10px; margin: 5px 0px" />
                <asp:Button ID="BtnAppraiseCancel" runat="server" Text="取消" OnClick="BtnAppraiseCancel_Click" CausesValidation="False" style="padding:2px 10px; margin: 5px" />
            </div>
        </div>

        <%--签到功能--%>
        <div runat="server" id="DivCheckIn" style="position:absolute; width:200px; height:100px; top:40%; left:50%; background-color:#F7F6F3; border: solid 1px; display:none;">
            <div style="margin:10px">
                <asp:Label ID="LblCheckIn" runat="server" Text="签到码"></asp:Label>
                <br />
                <asp:TextBox ID="TxtCheckIn" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="BtnCheckInCommit" runat="server" Text="确定" OnClick="BtnCheckInCommit_Click" CausesValidation="False" style="padding:2px 5px; margin:5px 0px;"/>
                <asp:Button ID="BtnCheckInCancel" runat="server" Text="取消" OnClick="BtnCheckInCancel_Click" CausesValidation="False" style="padding:2px 5px; margin:5px;"/>
                <asp:Label ID="LblCheckInActID" runat="server" Text="" style="display:none"></asp:Label>
            </div>
        </div>    

        <%--信息面板--%>
        <div class="divInfo" runat="server" id="DivMyInfoR">
            <div style="width:120px; height:250px; padding:5px; float:left;">
                <asp:ImageButton ID="MyImage" runat="server" Height="100px" ImageUrl="~/image/users/7020820000.jpg" Width="100px" style="border-radius:50%; " OnClick="MyImage_Click" CausesValidation="False" ToolTip="更换头像"/>
                <div id="DivUploadImage" style="display:none" runat="server">
                    <asp:FileUpload ID="ImageUpload" runat="server" Width="70px" style="margin:10px;" accept="image/*"/>
                    <asp:Button ID="BtnImage" runat="server" Text="更换头像" style="margin:10px;" OnClick="BtnImage_Click" CausesValidation="False"/>
                </div>
            </div>
            <div style="width:600px; height:280px; padding-left:10px;">
                <ul>
                    <li class="li-style">姓名：<asp:Label ID="LblStuName" runat="server" Text="张三"></asp:Label> <asp:ImageButton ID="ImageButtonInfo" runat="server" OnClick="ImageButtonInfo_Click" ImageUrl="~/image/info.png" Width="15px" style="margin-left:5px" ToolTip="信息修改" CausesValidation="False"/></li>
                    <li class="li-style">学号：<asp:Label ID="LblStuID" runat="server" Text="7020820000"></asp:Label></li>
                    <li class="li-style">专业班级：<asp:Label ID="LblMajor" runat="server" Text="计算机科学与技术206班"></asp:Label></li>
                    <li class="li-style">性别：<asp:Label ID="LblGender" runat="server" Text="男"></asp:Label></li>
                    <li class="li-style">学分：<asp:Label ID="LblCredit" runat="server" Text="0"></asp:Label></li>
                    <li class="li-style">
                        <asp:Button ID="BtnChangePsw" runat="server" Text="修改密码" style="padding:5px; margin:5px 5px 5px 0px;" OnClick="BtnChangePsw_Click" CausesValidation="False"/>
                        <asp:Button ID="BtnCredit" runat="server" Text="学分详情" style="padding:5px; margin:5px 5px 5px 15px;" CausesValidation="False" OnClick="BtnCredit_Click"/>
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

            <%--学分详情--%>
            <div id="DivCredit" style="width:620px; height:400px; margin-left:130px; display:none;" runat="server">

                <asp:DropDownList ID="DropDownList1" runat="server">
                    <asp:ListItem Selected="True" Value="0">全活动</asp:ListItem>
                    <asp:ListItem Value="1">已发放</asp:ListItem>
                    <asp:ListItem Value="2">未发放</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="DropDownList2" runat="server">
                    <asp:ListItem Value="0">全类别</asp:ListItem>
                    <asp:ListItem Value="1">创新创业与就业见习</asp:ListItem>
                    <asp:ListItem Value="2">社会实践与志愿公益</asp:ListItem>
                    <asp:ListItem Value="3">思想引领与文体素质拓展</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="Button1" runat="server" Text="查询" CausesValidation="False" OnClick="Button1_Click" />
                <asp:Label ID="LblTotal" runat="server" Text=""></asp:Label>
                
                <div id="DivTitle" style="margin-top:5px">
                    <asp:Table ID="Table1" runat="server" CellPadding="5" CellSpacing="0">
                        <asp:TableHeaderRow Height="40px">
                            <asp:TableHeaderCell BackColor="#8D9BA5" ForeColor="White" Width="40px" >类别</asp:TableHeaderCell>
                            <asp:TableHeaderCell BackColor="#8D9BA5" ForeColor="White" Width="214px" >活动名称</asp:TableHeaderCell>
                            <asp:TableHeaderCell BackColor="#8D9BA5" ForeColor="White" Width="144px" >获得学分</asp:TableHeaderCell>
                            <asp:TableHeaderCell BackColor="#8D9BA5" ForeColor="White" Width="176px" >发放时间</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>

                <div style="width:630px; height:300px; overflow:auto">
                   <asp:GridView ID="GvCredit" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="LinqDataSourceCredit" ForeColor="#333333" GridLines="None"  PageSize="1" Width="614px" style="margin-top:10px" OnDataBound="GvCredit_DataBound" ShowHeader="False">
                   <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                   <Columns>
                       <asp:BoundField DataField="activityID" ReadOnly="True" SortExpression="activityID" HeaderText="activityID" >    
                           <ControlStyle CssClass="hidden" />
                           <FooterStyle CssClass="hidden" />
                           <ItemStyle CssClass="hidden" />
                       </asp:BoundField>
                       <asp:BoundField DataField="activityType" HeaderText="类别" ReadOnly="True" SortExpression="activityType" ItemStyle-Width="40px" ItemStyle-Height="25px">
                       </asp:BoundField>
                       <asp:BoundField DataField="activityName" HeaderText="活动名称" ReadOnly="True" SortExpression="activityName" ItemStyle-Width="310px" ItemStyle-Height="25px">
                       </asp:BoundField>
                       <asp:BoundField DataField="availableCredit" HeaderText="获得学分" ReadOnly="True" SortExpression="availableCredit" ItemStyle-Width="210px" ItemStyle-Height="25px">
                       </asp:BoundField>
                       <asp:BoundField DataField="holdDate" HeaderText="发放时间" ReadOnly="True" SortExpression="holdDate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="220px" ItemStyle-Height="25px">
                       </asp:BoundField>
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
                </div>
                <asp:LinqDataSource ID="LinqDataSourceCredit" runat="server" ContextTypeName="ActivityManager.App_Data.ActivityManagerDataContext" EntityTypeName="" TableName="Activity" OrderBy="holdDate, holdStart" Select="new (activityID, activityName, availableCredit, holdDate, activityType)">
                </asp:LinqDataSource>
            </div>
        </div>

        <%--退出功能--%>
       <asp:ImageButton ID="IBtnEsc" runat="server" style="position:absolute; top: 120px; left: 1580px;" ImageUrl="~/image/esc.png" Width="20"  OnClick="Esc_Click" CausesValidation="False" ToolTip="返回登陆"/>

        <%--组队页面--%>
        <div id="DivBuildTeam" runat="server" style="border-style: solid; border-color: inherit; border-width: 1px; position:absolute; background-color:#F7F6F3; top: 300px; left: 858px; padding:5px;">
            <div style="text-align:center; padding:5px;">
                <asp:Label ID="LblBuildTeam" runat="server" Text="组队详情" Font-Bold="True" Font-Size="Large"></asp:Label>
            </div>
            <div style="padding:5px 0px 0px;">
                <asp:Label ID="LblBuildTeamAct" runat="server" Text="活动名称："></asp:Label>
                <asp:DropDownList ID="DdlBuildTeamAct" runat="server" DataSourceID="ActEnableSign" DataTextField="activityName" DataValueField="activityID"></asp:DropDownList>
                <asp:LinqDataSource ID="ActEnableSign" runat="server" ContextTypeName="ActivityManager.App_Data.ActivityManagerDataContext" EntityTypeName="" TableName="Activity" Where="activityState >= @activityState">
                    <WhereParameters>
                        <asp:Parameter DefaultValue="10" Name="activityState" Type="Int32" />
                    </WhereParameters>
                </asp:LinqDataSource>
            </div>
            <div style="padding:5px 0px 0px;">
                <asp:Label ID="LblTeamVolume" runat="server" Text="人数上限："></asp:Label>
                <asp:DropDownList ID="DdlBulidTeamVolume" runat="server">
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div style="padding:5px 0px 0px;">
                <asp:Label ID="LblTeamAudit" runat="server" Text="入队审核："></asp:Label>
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" style="margin-left:78px; margin-top:-23px;" RepeatDirection="Horizontal">
                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div style=" margin:5px; float:right;">
                <asp:Button ID="BtnBuildTeamSubmit" runat="server" Text="确认" Width="60px" CausesValidation="False" />
                <asp:Button ID="BtnBuildTeamCancel" runat="server" Text="取消" Width="60px" OnClick="BtnBuildTeamCancel_Click" CausesValidation="False" style="margin-left:5px;"/>
            </div>
        </div>
    </form>
</body>
</html>