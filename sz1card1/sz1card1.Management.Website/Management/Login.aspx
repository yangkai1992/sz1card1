<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Management_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="account" runat="server"></asp:TextBox>
        <asp:TextBox TextMode="Password" ID="passworld" runat="server"></asp:TextBox>
        <asp:Button ID="btnSubmit" runat="server" Text="登陆" OnClick="btnSubmit_Click" />
    </div>
    </form>
</body>
</html>
