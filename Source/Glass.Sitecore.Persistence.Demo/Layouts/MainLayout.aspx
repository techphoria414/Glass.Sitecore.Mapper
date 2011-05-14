<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainLayout.aspx.cs" Inherits="Glass.Sitecore.Persistence.Demo.Layouts.MainLayout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Glass Demo</title>
    <style>
        div
        {
        	float:left;
        	clear:left;
        }
        
    </style>
</head>
<body>
    <form runat="server">
    <sc:Placeholder runat="server" Key="content" />
    </form>
</body>
</html>
