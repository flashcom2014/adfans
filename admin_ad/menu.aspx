<%@ Page ResponseEncoding="UTF-8" Language="C#" AutoEventWireup="true" CodeFile="menu.aspx.cs" Inherits="admin_menu" %>
<html>
<head runat="server">
    <link rel="shortcut icon" type="image/x-icon" href="http://www.weimaqi.net/adfans/images/favicon.ico" media="screen" />
    <meta http-equiv="content-type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <title>管理页面</title>
	<link rel="stylesheet" href="/css/bootstrap.min.css" />
    <style>
        BODY
        {
            margin: 0px;
            FONT-SIZE: 12px;
            FONT-FAMILY: "微软雅黑", "Verdana", "Arial", "Helvetica", "sans-serif";
            background-color: #BEBFD9;/*#799ae1;*/
            scrollbar-face-color: #EAEAF2;
            scrollbar-highlight-color: #FFFFFF;
            scrollbar-shadow-color: #9395C0;
            scrollbar-3dlight-color: #F3F3F8;
            scrollbar-arrow-color: #9395C0;
            scrollbar-track-color: #F3F3F8;
            scrollbar-darkshadow-color: #F3F3F8;
        }

        table
        {
            border: 0px;
        }

        td
        {
            font: normal 12px 微软雅黑;
        }

        img
        {
            vertical-align: middle;
            border: 0px;
            padding-left:5px;
        }

        a
        {
            font: normal 12px 微软雅黑;
            color: #111111;
            text-decoration: none;
        }

            a:hover
            {
                color: #6C70AA;
                text-decoration: underline;
            }

        .sec_menu
        {
            /*border-left: 1px solid white;
            border-right: 1px solid white;
            border-bottom: 1px solid white;*/
            overflow: hidden;
            background: #FBFBFC;
            padding: 8px 0px;
            border-radius: 5px;
            display:none
        }
        
        .menu_title
        {
            cursor: pointer;
            background: url(images/title_bg_quit.gif);
        }
            .menu_title span
            {
                position: relative;
                /*top: 2px;*/
                left: 15px;
                color: #4A4F80;
                font-weight: bold;
                font-size: 13px;
            }
        input, select, Textarea
        {
            font-family: 微软雅黑,Verdana, Arial, Helvetica, sans-serif;
            font-size: 12px;
        }

        .div
        {
            padding-top: 5px;
        }

        .menu_toptd
        {
            height: 38px;
            background: url(images/admin_title1.gif) no-repeat;
            font-weight: bold;
            color: white;
            padding-left: 37px;
            letter-spacing: 1px;
            font-size: 14px;
            padding-top: 10px;
        }

        .menu_right
        {
            /*background-position: left center;
            background-repeat: no-repeat;*/
            cursor: pointer;
            height: 100%;
            position:fixed;
            right: 0;
            top: 0;
            width: 8px;
        }
		#lbl_complaint_count {
			font-weight:bold;
			margin-left: 3px;
		}
    </style>
    <script src="/js/jQuery-2.2.0.min.js"></script>
    <script src="/js/jquery.timers-1.2.js"></script>
    <script src="Include/Admin.js"></script>
    <script>
        
    </script>
</head>
<body>
    <form runat="server" id="form1">
        <% if (!IsAdmin)
            {  %>
        <table align="center">
            <tr>
                <td valign="top">
                    <table cellpadding="0" cellspacing="0" width="158" align="center">
                        <tr>
                            <td  class="menu_toptd">广告商管理菜单
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" width="158" align="center">
                        <tr>
                            <td class="menu_title" style="height:25px;background: url(images/title_bg_show.gif);"><span><%--<a href="main.aspx" target="right">--%><b>管理</b> | <a href="main.aspx" target="right"><b>首页</b></a> | 
                                <asp:LinkButton Text="退出" runat="server" ID="loginout" OnClick="loginout_Click"></asp:LinkButton></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="sec_menu" style="width:158px;display:block">
                                    <table cellpadding="0" cellspacing="0" align="center" width="158">
                                        <tr>
                                            <td height="20">&nbsp;&nbsp;用户名：<asp:Label runat="server" ID="uid" ForeColor="red" Text=""></asp:Label></td>
                                        </tr>
                                        
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="10"></td>
                        </tr>
                    </table>
                      <!--基本设置-->
                    <div id="p_person" class="div">
	
                        <table cellpadding="0" cellspacing="0" width="158" align="center">
                            <tr>
                                <td class="menu_title" style="height: 25px;background: url(images/title_bg_show.gif);">
                                    <span>基本设置</span> </td>
                            </tr>
                            <tr>
                                <td >
                                    <div class="sec_menu" style="width: 158px;display:block">
                                        <table cellpadding="0" cellspacing="0" align="center" width="158">
                                          
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="person/person.aspx" target="right">个人资料</a> | <a href="person/changepwd.aspx" target="right">修改密码</a></td>
                                            </tr>
                                          
                                           
                                            
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    
                    </div>
                    <!--代理权限 -->
                    <asp:Panel ID="p_agent" runat="server" CssClass="div">
                        <table cellpadding="0" cellspacing="0" width="158" align="center">
                            <tr>
                                <td class="menu_title"     style="height: 25px;background: url(images/title_bg_show.gif);">
                                    <span>代理功能管理</span> </td>
                            </tr>
                            <tr>
                                <td >
                                    <div class="sec_menu sec_menu" style="width: 158px; display:block;">
                                        <table cellpadding="0" cellspacing="0" align="center" width="158">
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="agent/make_share_code.aspx" target="right">添加推广码</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="agent/share_code_manage.aspx" target="right">推广码管理</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="agent/mch_manage.aspx" target="right">广告商管理</a>
                                                </td>
                                            </tr>
                                            <tr>
												<td>
                                                    <img src="images/bullet.gif" border="0"><a href="agent/profit_share.aspx" target="right">分成记录</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <!--提现管理 -->
                    <asp:Panel ID="Panel3" runat="server" CssClass="div">
                        <table cellpadding="0" cellspacing="0" width="158" align="center">
                            <tr>
                                <td class="menu_title"     style="height: 25px;background: url(images/title_bg_show.gif);">
                                    <span>提现功能管理</span> </td>
                            </tr>
                            <tr>
                                <td >
                                    <div class="sec_menu sec_menu" style="width: 158px; display:block;">
                                        <table cellpadding="0" cellspacing="0" align="center" width="158">
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="withdraw/request_withdrawals.aspx" target="right">提现申请</a></td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <!--公众号配置 -->
                    <asp:Panel ID="Panel1" runat="server" CssClass="div">
                        <table cellpadding="0" cellspacing="0" width="158" align="center">
                            <tr>
                                <td class="menu_title"     style="height: 25px;background: url(images/title_bg_show.gif);">
                                    <span>公众号管理</span> </td>
                            </tr>
                            <tr>
                                <td >
                                    <div class="sec_menu sec_menu" style="width: 158px; display:block;">
                                        <table cellpadding="0" cellspacing="0" align="center" width="158">
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="mp/mp_list.aspx" target="right">公众号配置</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="mp/mp_menu.aspx" target="right">公众号菜单配置</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="mp/focusrecord_list.aspx" target="right">吸粉或扣费记录</a></td>
                                            </tr>
                                            
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <!--统计管理 -->
                    <asp:Panel ID="statistics" runat="server" CssClass="div">
                        <table cellpadding="0" cellspacing="0" width="158" align="center">
                            <tr>
                                <td class="menu_title"     style="height: 25px;background: url(images/title_bg_show.gif);">
                                    <span>统计管理</span> </td>
                            </tr>
                            <tr>
                                <td >
                                    <div class="sec_menu sec_menu" style="width: 158px; display:block;">
                                        <table cellpadding="0" cellspacing="0" align="center" width="158">
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="statistics/payamount.aspx" target="right">支出统计</a></td>
                                            </tr>
                                              <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="statistics/appidfensi.aspx" target="right">公众号粉丝统计</a></td>
                                            </tr>
                                            
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <!--版权信息-->
                    <br />
                    <table cellpadding="0" cellspacing="0" width="158" align="center">
                        <tr>
                            <td height="25" class="menu_title"   style="background: url(images/title_bg_show.gif);"><span>版权申明</span></td>
                        </tr>
                        <tr>
                            <td>
                                <div class="sec_menu" style="width: 158px;display:block">
                                    <table cellpadding="0" cellspacing="0" align="center" width="158">
                                        <tr>
                                            <td height="20" style="padding-left: 15px;">
												<%=GetMchComment() %>
                                            </td>
                                        </tr>
										<%=wtx_html %>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>

					<%-- 更新日誌 --%>
					
					<div class="panel panel-warning" style="margin-top:1em;">
						<div class="panel-heading">更新日志</div>
						<div class="panel-body">
							
						</div>
					</div>
					
                </td>
            </tr>
        </table>
        <% }
            else
            { %>
        <table align="center">
            <tr>
                <td valign="top">
                    <table cellpadding="0" cellspacing="0" width="158" align="center">
                        <tr>
                            <td  class="menu_toptd">管理员管理菜单
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" width="158" align="center">
                        <tr>
                            <td class="menu_title" style="height:25px;background: url(images/title_bg_show.gif);"><span><%--<a href="main.aspx" target="right">--%><b>管理</b><%--</a> | <a href="main.aspx" target="right"><b>首页</b></a>--%> | 
                                <asp:LinkButton Text="退出" runat="server" ID="LinkButton1" OnClick="loginout_Click"></asp:LinkButton></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="sec_menu" style="width:158px;display:block">
                                    <table cellpadding="0" cellspacing="0" align="center" width="158">
                                        <tr>
                                            <td height="20">&nbsp;&nbsp;用户名：<asp:Label runat="server" ID="admin_uid" ForeColor="red" Text=""></asp:Label></td>
                                        </tr>
                                        
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="10"></td>
                        </tr>
                    </table>
                    <!--基本设置-->
                    <div id="Div1" class="div">
	
                        <table cellpadding="0" cellspacing="0" width="158" align="center">
                            <tr>
                                <td class="menu_title" style="height: 25px;background: url(images/title_bg_show.gif);">
                                    <span>基本设置</span> </td>
                            </tr>
                            <tr>
                                <td >
                                    <div class="sec_menu" style="width: 158px;display:block">
                                        <table cellpadding="0" cellspacing="0" align="center" width="158">
                                            <% if (Session["admin_ad_flag"] != null && Session["admin_ad_flag"].ToString()== "1")
                                              {  %>
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="admin/personam.aspx" target="right">微信绑定</a></td>
                                            </tr>
                                              <tr>
                                                <td>
                                                     <img src="images/bullet.gif" border="0"><a href="admin/addadmin.aspx" target="right">添加管理员</a></td>
                                            </tr>
                                          <%} %>
                                            <tr>
                                                <td>
                                                     <img src="images/bullet.gif" border="0"><a href="admin/changepwdam.aspx" target="right">修改密码</a></td>
                                            </tr>
                                             
                                            
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    
                    </div>
                   <!--用户管理 -->
                    <asp:Panel ID="Panel2" runat="server" CssClass="div">
                        <table cellpadding="0" cellspacing="0" width="158" align="center">
                            <tr>
                                <td class="menu_title"     style="height: 25px;background: url(images/title_bg_show.gif);">
                                    <span>用户管理</span> </td>
                            </tr>
                            <tr>
                                <td >
                                    <div class="sec_menu sec_menu" style="width: 158px; display:block;">
                                        <table cellpadding="0" cellspacing="0" align="center" width="158">
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="admin/mch_list.aspx" target="right">用户列表</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="admin/share_code_manage.aspx" target="right">推广码管理</a></td>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
					
                    <!--提现管理 -->
                    <asp:Panel ID="Panel4" runat="server" CssClass="div">
                        <table cellpadding="0" cellspacing="0" width="158" align="center">
                            <tr>
                                <td class="menu_title"     style="height: 25px;background: url(images/title_bg_show.gif);">
                                    <span>提现管理</span> </td>
                            </tr>
                            <tr>
                                <td >
                                    <div class="sec_menu sec_menu" style="width: 158px; display:block;">
                                        <table cellpadding="0" cellspacing="0" align="center" width="158">
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="admin/txlist.aspx" target="right">提现列表</a></td>
                                            </tr>
                                            
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <!--公众号管理 -->
                    <asp:Panel ID="Panel6" runat="server" CssClass="div">
                        <table cellpadding="0" cellspacing="0" width="158" align="center">
                            <tr>
                                <td class="menu_title"     style="height: 25px;background: url(images/title_bg_show.gif);">
                                    <span>公众号管理</span> </td>
                            </tr>
                            <tr>
                                <td >
                                    <div class="sec_menu sec_menu" style="width: 158px; display:block;">
                                        <table cellpadding="0" cellspacing="0" align="center" width="158">
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="admin/mp_list.aspx" target="right">公众号配置</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="images/bullet.gif" border="0"><a href="admin/history_list.aspx" target="right">吸粉记录查询</a></td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
					<%-- 更新日誌 --%>
					
					<div class="panel panel-warning" style="margin-top:1em;">
						<div class="panel-heading">更新日志</div>
						<div class="panel-body">
							
						</div>
					</div>
					
                </td>
            </tr>
        </table>
        <%} %>
        <div class="menu_right" style="background:url(images/m_hidden.png) #bebfd9 left center no-repeat"></div>
    </form>
</body>
</html>
