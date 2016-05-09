<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ManagementWebsite.Management.Login" %>

<html>
<head id="Head1" runat="server">
    <title>一卡易管理支撑系统--登录</title>

    <script src="../JavaScript/CodeImg.js" type="text/javascript"></script>
    <script type="text/javascript">
        function load() {
            var username = document.getElementById("TextName").value;
            var pwd = document.getElementById("TextPass").value;
            if (username.length == 0) {
                document.getElementById("TextName").focus();
            }
            else {
                if (pwd.length == 0) {
                    document.getElementById("TextPass").focus();
                }
                else {
                    document.getElementById("TextCode").focus();
                }
            }
        }
    </script>
    <style type="text/css">
        body
        {
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
            background-image: url(../images/login_01.gif);
            overflow: hidden;
        }
    </style>
</head>
<body onload="load()">
    <form id="form1" runat="server">
    <div>
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td background="../images/login_03.gif">
                                &nbsp;
                            </td>
                            <td width="876">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="299" valign="top" background="../images/login_01.gif">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="54">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="394" height="69" background="../images/login_02.jpg">
                                                        &nbsp;
                                                    </td>
                                                    <td width="199" background="../images/login_03.jpg">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="22%" height="22">
                                                                    <div align="center">
                                                                        <span>用户名</span></div>
                                                                </td>
                                                                <td width="51%" height="22">
                                                                    <input id="TextName" runat="server" type="text" style="background-color: #032e49;
                                                                        color: #88b5d1; border: solid 1px #88b5d1; width: 107" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextName"
                                                                        Display="none" ErrorMessage="请输入用户名~">*</asp:RequiredFieldValidator>
                                                                </td>
                                                                <td width="27%" height="22">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td height="22" valign="middle">
                                                                    <div align="center">
                                                                        <span>密&nbsp; 码</span></div>
                                                                </td>
                                                                <td height="22" valign="bottom">
                                                                    <input id="TextPass" runat="server" type="password" style="background-color: #032e49;
                                                                        color: #88b5d1; border: solid 1px #88b5d1; width: 107; height: 19" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextPass"
                                                                        Display="none" ErrorMessage="请输入密码~">*</asp:RequiredFieldValidator>
                                                                </td>
                                                                <td height="22" valign="bottom">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td height="22" valign="middle">
                                                                    <div align="center">
                                                                        验证码</div>
                                                                </td>
                                                                <td height="22" valign="middle">
                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td width="36%" height="22">
                                                                                <input id="TextCode" runat="server" type="text" size="5
						" style="background-color: #032e49; color: #88b5d1; border: solid 1px #88b5d1;" />
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextCode"
                                                                                    Display="none" ErrorMessage="请输入验证码~">*</asp:RequiredFieldValidator>
                                                                            </td>
<%--                                                                            <td width="64%">
                                                                                <img id="ImageCheckAdmin" runat="server" src="../Common/Codeimg.aspx" style="cursor: hand"
                                                                                    width="45" height="20" onclick="javascript:ChangeCodeImg('ImageCheckAdmin');"
                                                                                    title="点击更换验证码图片!" />
                                                                            </td>--%>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td height="22" valign="middle">
                                                                    &nbsp;&nbsp;
                                                                    <asp:ImageButton runat="server" ImageUrl="../images/dl.gif" ID="btlogin" BorderWidth="1"
                                                                        OnClick="btlogin_Click" />
                                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                                                        ShowSummary="False" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="283" background="../images/login_04.jpg">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="225" background="../images/login_05.jpg">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td background="../images/login_03.gif">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
