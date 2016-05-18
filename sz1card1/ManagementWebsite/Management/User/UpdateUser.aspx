<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateUser.aspx.cs" Inherits="Management_UpdateUser" %>

<html>
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript" language="javascript">
        function getCanSel() {
            window.parent.getClose();
            return false;
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
                        <asp:TextBox ID="tbAccount" Width="100" Enabled="false" runat="server"></asp:TextBox>
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
                    <td class="table_label">&nbsp; 用户密码：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbPassword" Width="100" runat="server" TextMode="Password"></asp:TextBox>
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
                    <td class="table_label">&nbsp; 重复密码：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbPasswordAgain" Width="100" runat="server" TextMode="Password">
                        </asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="tbPasswordAgain"
                            ControlToValidate="tbPassword" ErrorMessage="两次输入的密码不一致" Display="none">*</asp:CompareValidator>
                    </td>
                    <td class="table_label">
                        <span style="color: Red">*</span> 手机号码：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbPhone" Width="104" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                            ErrorMessage="请输入联系电话" ControlToValidate="tbPhone" Display="None">*</asp:RequiredFieldValidator>
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="none"
                            ErrorMessage="输入电话号码不合法" ValidationExpression="(^(\d{3,4}-)?\d{7,8})$|(^(13[0-9]|11[8]|15[0|3|6|7|8|9]|18[8|9])\d{8}$)"
                            ControlToValidate="tbPhone">*</asp:RegularExpressionValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="table_label">
                        <span style="color: Red">*</span> 用户姓名：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbName" runat="server" Width="100" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbName"
                            Display="none" ErrorMessage="用户姓名不能为空！">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="table_label">&nbsp;客服&nbsp;&nbsp;QQ：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbEmail" runat="server" type="text" Width="104" />
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbEmail"
                        Display="none" ErrorMessage="输入邮件地址不合法！" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="table_label">
                        <span style="color: Red">*</span> 用户权重：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="txtUserWeight" runat="server" Width="100" type="text" MaxLength="5" OnTextChanged="txtUserWeight_onTextChange" AutoPostBack="true" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="none"
                            ErrorMessage="权重请输入非负整数" ValidationExpression="^[0-9]*$" ControlToValidate="txtUserWeight">*</asp:RegularExpressionValidator>
                    </td>
                    <td class="table_label">已用权重：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="txtUserWeightUsed" runat="server" Width="104" Enabled="false" MaxLength="5"></asp:TextBox>
                        <asp:HiddenField ID="hidUserWeightUsed" runat="server" />
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
                    <td class="table_label" valign="top" width="20%">&nbsp;备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;注：
                    </td>
                    <td colspan="3" class="table_text">
                        <textarea id="tbmeno" style="height: 40px; width: 293px;" cols="34" runat="server"></textarea>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center; height: 40px;">
                        <asp:Button ID="btsubmit" runat="server" Text="提交" OnClick="btsubmit_Click" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                            ShowSummary="False" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="TbCanSel" runat="server" UseSubmitBehavior="false" OnClientClick="return getCanSel()"
                        Text="取消" CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
