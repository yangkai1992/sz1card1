<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="ManagementWebsite.Management.User.UserList" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>用户信息</title>
    <link href="../../Css/Public.css" rel="stylesheet" type="text/css" />

    <script src="../../JavaScript/rightpage.js" type="text/javascript"></script>

    <script src="../../JavaScript/AllCheckBox.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function f() {
            var obj = document.getElementById("popSearchUser");
            var input = obj.getElementsByTagName("input");
            for (var i = 0; i < input.length; i++) {
                input[i].onkeypress = function (event) {
                    var event = event ? event : (window.event ? window.event : null); //兼容ie和ff
                    if (event != null && event.keyCode == "13") {
                        document.getElementById("btsearch").click();
                        return false;
                    }
                }
            }
        }

        function AddUserResult(result) {
            if (result == "success") {
                alert("添加用户成功");
                PopupDiv.close('popAddUser');
                window.location = 'UserList.aspx';
            }
        }
        function UpdateUserResult(result) {
            if (result == "success") {
                alert("修改用户成功");
                PopupDiv.close('popEditUser');
                window.location = 'UserList.aspx';
            }
        }
        function prompt() {
            alert("系统管理员不能删除!");
        }
        //判断是否选中
        function isCheck(text) {
            var chs = document.getElementsByTagName("input")
            var n = 0;
            for (var i = 0; i < chs.length; i++) {
                if (chs[i].checked) {
                    n++;
                }
            }
            if (n == 0) {
                alert("您没有选中任何记录，请选择！");
                return false;
            } else {
                return confirm("您选中了" + n + "条记录，确定要" + text + "吗？");
            }
        }
        function getClose() {
            PopupDiv.close('PopupEditUser');
        }
        //全选
        function SelectAll() {
            var chks = document.getElementsByTagName("input")
            for (var i = 0; i < chks.length; i++) {
                // if (chks[i].type == 'checkbox' && chks[i].group == 'Business') {
                if (chks[i].type == 'checkbox') {
                    chks[i].checked = !chks[i].checked;
                }
            }
        }

        function getCancel() {
            PopupDiv.close('popSearchUser');
        }

    </script>

</head>
<body onload="f()">
    <form id="form1" runat="server">
    <div>
        <webUserControl:Toolbar ID="barHeader" runat="server">
            <webUserControl:ToolbarImage ID="ToolbarImage1" ItemCellDistance="20" runat="server" ImageUrl="~/Images/tb.gif"
                Width="16px" />
            <webUserControl:ToolbarLabel ID="ToolbarLabel1" ItemCellDistance="110" runat="server" Text=" 用户基本信息列表" />
            <webUserControl:ToolbarSeparator runat="server">
            </webUserControl:ToolbarSeparator>
            <webUserControl:ToolbarButton runat="server" ItemCellDistance="60" ImageUrl="../../Images/add.gif"
                ItemId="tbAdd" LabelText="添加" OnClientClick="PopupDiv.showDialog('popAddUser','AddUser.aspx');return false;">
            </webUserControl:ToolbarButton>
            <webUserControl:ToolbarButton runat="server" ItemCellDistance="60" ImageUrl="../../Images/del.gif"
                ItemId="tbUnlock" LabelText="解锁" OnClientClick="return  isCheck('解锁')">
            </webUserControl:ToolbarButton>
            <webUserControl:ToolbarButton runat="server" ItemCellDistance="60" ImageUrl="../../Images/del.gif"
                ItemId="tbLock" LabelText="锁定" OnClientClick="return  isCheck('锁定')">
            </webUserControl:ToolbarButton>
            <webUserControl:ToolbarButton runat="server" ItemCellDistance="60" ImageUrl="../../Images/add.gif"
                ItemId="tbselect" LabelText="查询" OnClientClick="PopupDiv.show('popSearchUser');document.getElementById('tbuserAcount').focus();return false;">
            </webUserControl:ToolbarButton>
        </webUserControl:Toolbar>
        <webUserControl:EntityGridView ID="gvUserList" runat="server" AllowCustomPaging="true"
             PageSize="10" PagerButtons="NextPreviousFirstLast" OnPageIndexChanging ="gvUserList_PageIndexChanging"
            OnRowDataBound="gvUserList_RowDataBound" AutoGenerateColumns="false" >
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input id="chkAll" type="checkbox" onclick="javascript: SelectAll();" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input id="isCheck" type="checkbox" value='<%#Eval("IsLocked") %>' runat="server"
                            group="Business" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Account" HeaderText="工 号" HeaderStyle-Font-Bold="true"
                    HeaderStyle-ForeColor="Black" SortExpression="Account" />
                <asp:BoundField DataField="TrueName" HeaderText="用户姓名" />
<%--                <asp:BoundField DataField="GroupName" HeaderText="所属部门" HeaderStyle-Font-Bold="true"
                    HeaderStyle-ForeColor="Black" SortExpression="GroupName" />--%>
                <asp:BoundField DataField="Email" HeaderText="客服QQ" />
                <asp:BoundField DataField="Mobile" HeaderText="手机号码" />
                <asp:BoundField DataField="Tel" HeaderText="联系电话" />
                <asp:BoundField DataField="UserWeight" HeaderText="用户权重" HeaderStyle-Font-Bold="true"
                    HeaderStyle-ForeColor="Black" SortExpression="UserWeight" />
                <asp:BoundField DataField="UserWeightUsed" HeaderText="已用权重" HeaderStyle-Font-Bold="true"
                    HeaderStyle-ForeColor="Black" SortExpression="UserWeightUsed" />
                <asp:BoundField DataField="IsLocked" HeaderText="状态" />
                <asp:BoundField DataField="Meno" HeaderText="备注" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href="javascript:;" onclick="PopupDiv.showDialog('PopupEditUser','UpdateUser.aspx?Account=<%#Eval("Account") %>');return false;">
                            编辑</a>
                        <asp:LinkButton ID="lbDelete" CommandArgument='<%#Eval("Account") %>' OnClientClick="return confirm('确定要删除改用户吗？');"
                            runat="server" OnCommand="lbDelete_Command">删除</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                当前没有任何数据
            </EmptyDataTemplate>
        </webUserControl:EntityGridView>
        <webUserControl:PopupDiv ID="popAddUser" runat="server" Width="390px" Height="250px" Title="添加用户信息"
            Message="带*为必填信息，用户创建后用户账号不可修改">
        </webUserControl:PopupDiv>
        <webUserControl:PopupDiv ID="PopupEditUser" runat="server" Width="390px" Height="250px"
            Title="编辑用户信息" Message="提示：不修改密码则可不填">
        </webUserControl:PopupDiv>
         <webUserControl:PopupDiv ID="popSearchUser" Width="430" Height="150" Message="请输入查询的条件!" Title="用户查询" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" style="margin-top: 5px; margin-left: 5px; margin-right: 5px;">
                <tr>
                    <td class="table_label" style="width: 100px;">用户工号：
                    </td>
                    <td class="table_text" style="width: 100px;">
                        <asp:TextBox ID="tbuserAcount" Width="100" runat="server"></asp:TextBox>
                    </td>
                    <td class="table_label">用户姓名：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbuserName" Width="100" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="table_label">所属部门：
                    </td>
                    <td class="table_text">
                        <asp:DropDownList ID="ddlUerGroup" Width="104" runat="server" DataTextField="GroupName"
                            DataValueField="Guid">
                            <asp:ListItem Value="">请选择...</asp:ListItem>
                        </asp:DropDownList>                        
                    </td>
                    <td class="table_label">状&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;态：
                    </td>
                    <td class="table_text">
                        <asp:DropDownList ID="ddlIsLocked" Width="100" runat="server">
                            <asp:ListItem Value="">请选择...</asp:ListItem>
                            <asp:ListItem Value="False">可用</asp:ListItem>
                            <asp:ListItem Value="True">锁定</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>                              
                <tr>
                    <td class="table_label">手机号码：
                    </td>
                    <td class="table_text">
                        <asp:TextBox ID="tbMobile" runat="server"></asp:TextBox>
                    </td>
                    <td class="table_label">
                        客服QQ：
                    </td>
                    <td>
                        <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox>
                    </td>
                </tr>                             
            </table>
            <div style="margin-top: 50px; margin-bottom: 10px;">
                <asp:Button ID="btsearch" runat="server" Text="搜索" OnClick="btsearch_Click" EnableTheming="False" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnCancel" UseSubmitBehavior="false" CausesValidation="false" OnClientClick="return getCancel()"
                runat="server" Text="取消" />
            </div>
        </webUserControl:PopupDiv>
    </div>
    </form>
</body>
</html>