<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserGroup.aspx.cs" Inherits="Management_User_UserGroup" %>

<html>
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Css/Public.css" rel="stylesheet" type="text/css" />
    <script src="../../JavaScript/LeftMeun.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function ColseEditPopedom(result) {
            if (result == "ok") {
                alert("编辑成功");
                PopupDiv.close('popEditGroup');
                window.location = 'UserGroup.aspx';
            }
        }
        function getClose() {
            PopupDiv.close('popEditGroup');
        }
    </script>

</head>
<body style="background-image: url('../../Images/dialog/back.JPG');">
    <form id="form1" runat="server">
    <div>
        <sz1card1:Toolbar ID="barHeader" runat="server">
            <sz1card1:ToolbarImage ID="ToolbarImage1" ItemCellDistance="20" runat="server" ImageUrl="~/Images/tb.gif"
                Width="16px" />
            <sz1card1:ToolbarLabel ID="ToolbarChanel" ItemCellDistance="110" runat="server" Text=" 部门级别列表" />
            <sz1card1:ToolbarSeparator runat="server">
            </sz1card1:ToolbarSeparator>
        </sz1card1:Toolbar>
        <sz1card1:EntityGridView ID="gvUserGroupList" runat="server" AllowChangePagesize="True"
            AllowExportToExcel="True" AllowMultiColumnSorting="False" DefaultSortDirection="Ascending"
            ExcelExportFileName="表格.xls" PageSelectorPageSizeInterval="10" PageSize="20" AutoGenerateColumns="False"
            DataSourceID="dataUserGroup" RowMouseOverColor="" AllowSorting="True">
            <Columns>
                <asp:BoundField DataField="GroupName" HeaderText="部门名称" HeaderStyle-Font-Bold="true"
                    HeaderStyle-ForeColor="Black" SortExpression="GroupName"></asp:BoundField>
                <asp:BoundField DataField="GroupWeight" HeaderText="权重" HeaderStyle-Font-Bold="true"
                    HeaderStyle-ForeColor="Black" SortExpression="GroupWeight" Visible="false"></asp:BoundField>
                <asp:BoundField DataField="GroupWeightUsed" HeaderText="已用权重" HeaderStyle-Font-Bold="true"
                    HeaderStyle-ForeColor="Black" SortExpression="GroupWeightUsed" Visible="false"></asp:BoundField>
                <asp:BoundField DataField="Meno" HeaderText="备注" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href="javascript:;" onclick="PopupDiv.showDialog('popEditGroup','EditGroup.aspx?groupGuid=<%#Eval("Guid") %>');return false;">
                            编辑</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </sz1card1:EntityGridView>
        <asp:ObjectDataSource ID="dataUserGroup" runat="server" SelectMethod="GetUserGroupList"
            TypeName="sz1card1.Management.Logic.UserLogic"></asp:ObjectDataSource>
    </div>
    <sz1card1:PopupDiv ID="popEditGroup" Height="350" Width="370" Title="部门权限设置" runat="server">
    </sz1card1:PopupDiv>
    </form>
</body>
</html>
