<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddUser.aspx.cs" Inherits="Management_AddUser" %>

<html>
<head id="Head1" runat="server">
    <title></title>

    <script language="javascript" type="text/javascript">
        function CheckReset() {
            window.document.getElementById("form1").reset();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
            <table align="center" style="margin-top: 10px;" cellpadding="0" cellspacing="0" width="380"
                border="0">
                <tr>
                    <td class="table_label" style="width: 20%">
                        <span style="color: Red">*</span> 工&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;号：
                    </td>
                    <td class="table_text" style="width: 30%">
                        <asp:TextBox ID="tbAccount" Width="100" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbAccount"
                            Display="none" ErrorMessage="用户账号不能为空">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="table_label" style="width: 20%">
                        <span style="color: Red">*</span> 所属部门：
                    </td>
                    <td class="table_text" style="width: 30%">
                        <asp:DropDownList ID="ddlUerGroup" Width="104" runat="server" DataTextField="GroupName"
                            DataValueField="Guid">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="table_label">
                        <span style="color: Red">*&nbsp;</span>用户密码：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbPassword" runat="server" Width="100" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbPassword" Display="none"
                            ErrorMessage="输入用户密码">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="table_label">
                        <span style="color: Red">*</span> 状&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;态：
                    </td>
                    <td class="table_text">
                        <asp:DropDownList ID="ddlLocked" Width="104" runat="server">
                            <asp:ListItem Value="0">正常</asp:ListItem>
                            <asp:ListItem Value="1">锁定</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="table_label">
                        <span style="color: Red">*</span> 重复密码：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbPasswordAgain" Width="100" runat="server" TextMode="Password">
                        </asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                            ControlToValidate="tbPassword" Display="none" ErrorMessage="输入重复密码">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="tbPassword"
                            ControlToValidate="tbPasswordAgain" ErrorMessage="两次输入的密码不一致" Display="none">*</asp:CompareValidator>
                    </td>
                    <td class="table_label">
                        <span style="color: Red">*</span>手机号码：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbPhone" Width="103" runat="server" type="text" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                            ErrorMessage="请输入联系电话" ControlToValidate="tbPhone">*</asp:RequiredFieldValidator>
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="none"
                            ErrorMessage="输入电话号码不合法" ValidationExpression="(^(1[3|5|8])\d{9}$)|(^(([0\+]\d{2,3}-)?(0\d{2,3})-)?(\d{7,8})(-(\d{3,}))?$)"
                            ControlToValidate="tbPhone">*</asp:RegularExpressionValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="table_label">
                        <span style="color: Red">*</span> 用户姓名：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbName" runat="server" Width="100" type="text" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbName"
                            Display="none" ErrorMessage="用户姓名不能为空！">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="table_label">&nbsp;客服&nbsp;&nbsp;QQ：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbEmail" runat="server" Width="103" type="text" />
                    </td>
                </tr>
                <tr>
                    <td class="table_label">
                        <span style="color: Red">*</span> 用户权重：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="txtUserWeight" runat="server" Width="100" type="text" MaxLength="5" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="none"
                            ErrorMessage="权重请输入非负整数" ValidationExpression="^[0-9]*$" ControlToValidate="txtUserWeight">*</asp:RegularExpressionValidator>
                    </td>
                    <td class="table_label">
                        <asp:Label ID="lblUserWeightUsed" runat="server" Visible="false">
                        已用权重:</asp:Label>
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="txtUserWeightUsed" runat="server" Visible="false" Text="0" MaxLength="5"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="table_label">联系电话：
                    </td>
                    <td class="table_text" colspan="3">
                        <asp:TextBox ID="txtTel" runat="server" MaxLength="200" Width="293"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="table_label" valign="top">&nbsp;备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;注：
                    </td>
                    <td colspan="3" class="table_text">
                        <textarea id="tbmeno" style="height: 40px; width: 293px;" cols="34" runat="server"></textarea>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td style="text-align: center; height: 40px;">
                        <asp:Button ID="btsubmit" runat="server" Text="提交" OnClick="btsubmit_Click" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                            ShowSummary="False" />
                    </td>
                    <td style="text-align: center; height: 40px;">
                        <asp:Button ID="Button1" runat="server" Text="重置" UseSubmitBehavior="false" CausesValidation="false"
                            OnClientClick="CheckReset()" />
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
