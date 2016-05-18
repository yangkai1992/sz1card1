<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateUserPwd.aspx.cs" Inherits="Management_UpdateUserPwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script language="javascript" type="text/javascript">
        function CheckSubmit() {
            window.document.getElementById("form1").reset();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <br />
        <table  style=" height:110px;" align="center" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="right">
                   <span style="color:Red">*</span> 旧密码：
                </td>
                <td align="left">
                    <asp:TextBox ID="tbPwd" runat="server" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="请输入旧密码"
                        ControlToValidate="tbPwd" Display="Dynamic">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                   <span style="color:Red">*</span> 新密码：
                </td>
                <td align="left">
                    <asp:TextBox ID="tbnewPwd" runat="server" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="输入新的密码"
                        ControlToValidate="tbnewPwd" Display="Dynamic">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                   <span style="color:Red">*</span> 重输密码：
                </td>
                <td align="left">
                    <asp:TextBox ID="tbpwdagin" runat="server" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tbpwdagin"
                        runat="server" ErrorMessage="输入确认密码">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="tbpwdagin"
                        ErrorMessage="前后密码不一致" ControlToCompare="tbnewPwd">*</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="height: 40px;">
                    <asp:Button ID="BatSubmit" runat="server" Text="提交" OnClick="Button1_Click" BorderWidth="1px"
                        Width="40px" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="BatReset" runat="server"  UseSubmitBehavior="false" Text="重置" BorderWidth="1px" Width="37px"
                        CausesValidation="False" OnClientClick="return CheckSubmit()" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
