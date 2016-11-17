<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" type="image/x-icon" href="http://www.weimaqi.net/adfans/images/favicon.ico" media="screen" />
    <title>快得粉 - 后台管理系统</title>
</head>
<frameset framespacing="0" border="false" cols="185,*" frameborder="0" id="main"> 
<frame name="left"  scrolling="auto" marginwidth="0" marginheight="0" src="<%=menu_url0 %>"/>
<frame name="right" src="<%=menu_url1 %>" scrolling="yes" id="fright"/>
</frameset>
</html>
<script type="text/javascript">
    function login() {
        location.href = "login.aspx";
    }
</script>
