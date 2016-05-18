<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserMessage.aspx.cs" Inherits="Management_UserMessage" %>

<html>
<head id="Head1" runat="server">
    <link href="../../Css/Public.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table style="margin-left: 8px;" align="center" cellpadding="0" width="340px" cellspacing="0"
            border="0">
            <tr>
                <td class="table_label">
                    <span lang="zh-cn">工&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;号：</span>
                </td>
                <td class="table_text">
                    &nbsp;
                    <asp:TextBox ID="tbAccount" Width="75" Enabled="false" runat="server" />
                </td>
                <td class="table_label">
                    <span lang="zh-cn">用户类型：</span>
                </td>
                <td class="table_text">
                    &nbsp;
                    <asp:TextBox ID="tbType" Width="75" Enabled="false" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="table_label">
                    <span lang="zh-cn">用户姓名：</span>
                </td>
                <td class="table_text">
                    &nbsp;
                    <asp:TextBox ID="tbName" Width="75" Enabled="false" runat="server" />
                </td>
                <td class="table_label">
                    <span lang="zh-cn">状&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 态：</span>
                </td>
                <td class="table_text">
                    <span lang="zh-cn">&nbsp; </span>
                    <asp:TextBox ID="tbState" Width="75" Enabled="false" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="table_label">
                    电子<span lang="zh-cn">邮件：</span>
                </td>
                <td class="table_text" colspan="3">
                    &nbsp;
                    <asp:TextBox ID="tbEmail" runat="server"  Enabled="false" Width="244" />
                </td>
            </tr>
            <tr>
                <td class="table_label">
                    备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 注：
                </td>
                <td valign="top" class="table_text" colspan="3">
                    &nbsp;
                    <textarea id="tbmeno" runat="server"  style="height:40px; width:244px;" disabled="disabled" cols="28"></textarea>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
