<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditGroup.aspx.cs" Inherits="Management_User_EditGroup" %>

<html>
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript" language="javascript">
        function getCanSel() {
            window.parent.getClose();
            return false;
        }
    </script>

    <style type="text/css">
        #MovieDiv
        {
            position: absolute;
            background-color: White;
            top: 37px;
            left: 27px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top: 6px; margin-left: 21px; margin-bottom: 10px;">
        <table>
            <tr>
                <td>
                    <span style="color: Red">*</span> 部门名称：
                </td>
                <td>
                    <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr visible="false" runat="server">
                <td>
                    <span style="color: Red">*</span> 部门权重：
                </td>
                <td>
                    <asp:TextBox ID="txtGroupWeight" runat="server" MaxLength="5"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="none"
                        ErrorMessage="权重请输入非负整数" ValidationExpression="^[0-9]*$" ControlToValidate="txtGroupWeight">*</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr visible="false" runat="server">
                <td>
                    &nbsp;&nbsp;&nbsp;已用权重：
                </td>
                <td>
                    <asp:TextBox ID="txtGroupWeightUsed" runat="server" Enabled="false" MaxLength="5"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div id="MovieDiv">
        请勾选所需要的权限</div>
    <div style="margin-bottom: 2px; margin-left: 20px; margin-right: 15px; border-color: Gray;
        border: 1px; border-style: solid; width: 330px;">
        <div style="margin-top: 7px;">
            <sz1card1:RightsSelector ID="rspopedom" runat="server" XmlSourceFile="~/menu.xml"
                RepeatColumns="4" />
        </div>
    </div>
    <div style="margin-top: 2px; margin-left: 21px; margin-bottom: 2px;">
        <table>
            <tr>
                <td>
                    备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;注：
                </td>
                <td>
                    <textarea id="tbmeno" style="height: 30px; width: 266px;" runat="server" cols="31"
                        rows="2"></textarea>
                </td>
            </tr>
        </table>
    </div>
    <div align="center">
        <asp:Button ID="btSubmit" runat="server" Text="保存" OnClick="btSubmit_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnCansel" runat="server" OnClientClick="return getCanSel()" Text="取消" />
    </div>
    </form>
</body>
</html>
