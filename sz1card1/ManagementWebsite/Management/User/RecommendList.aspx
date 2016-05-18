<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecommendList.aspx.cs" Inherits="Management_Business_AddValueRecord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>推荐记录</title>
    <link href="../../Css/Public.css" rel="stylesheet" type="text/css" />

    <script src="../../Common/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function f() {
            var obj = document.getElementById("popSearch");
            var input = obj.getElementsByTagName("input");
            for (var i = 0; i < input.length; i++) {
                input[i].onkeypress = function (event) {
                    var event = event ? event : (window.event ? window.event : null); //兼容ie和ff
                    if (event != null && event.keyCode == "13") {
                        document.getElementById("Setsearch").click();
                        return false;
                    }
                }
            }
        }
        function getCancel() {
            PopupDiv.close('popSearch');
        }
    </script>

</head>
<body onload="f()">
    <form id="form1" runat="server">
        <div>
            <sz1card1:Toolbar ID="Toolbar1" runat="server">
                <sz1card1:ToolbarImage ID="ToolbarImage1" ItemCellDistance="20" runat="server" ImageUrl="~/Images/tb.gif"
                    Width="16px" />
                <sz1card1:ToolbarLabel ID="ToolbarLabel1" ItemCellDistance="110" runat="server" Text="推荐记录" />
                <sz1card1:ToolbarSeparator runat="server">
                </sz1card1:ToolbarSeparator>
                <sz1card1:ToolbarButton runat="server" ItemCellDistance="90" ImageUrl="../../Images/add.gif"
                    ItemId="tbsearch" LabelText="查询" OnClientClick="PopupDiv.show('popSearch');document.getElementById('txtAccount').focus();return false;">
                </sz1card1:ToolbarButton>
            </sz1card1:Toolbar>
            <asp:ScriptManager ID="ScriptManager1" ScriptMode="Inherit" runat="server">
            </asp:ScriptManager>
            <sz1card1:EntityGridView ID="gvLogList" runat="server" AllowChangePagesize="True"
                AllowExportToExcel="True" AllowMultiColumnSorting="False" DefaultSortDirection="Ascending"
                ExcelExportFileName="表格.xls" PageSelectorPageSizeInterval="10" PageSize="20"
                AllowSorting="True" AutoGenerateColumns="False" DataSourceID="dataAddValueList">
                <Columns>
                    <asp:BoundField DataField="RecommendName" HeaderText="推荐商家" HeaderStyle-Font-Bold="true"
                        HeaderStyle-ForeColor="Black" />
                    <asp:BoundField DataField="PresenteeName" HeaderText="被推荐商家" HeaderStyle-Font-Bold="true"
                        HeaderStyle-ForeColor="Black" />
                    <asp:BoundField DataField="OperationTime" HeaderText="推荐时间" HeaderStyle-Font-Bold="true"
                        HeaderStyle-ForeColor="Black" />
                    <asp:BoundField DataField="RecommederName" HeaderText="推荐人" HeaderStyle-Font-Bold="true"
                        HeaderStyle-ForeColor="Black" />
                    <asp:BoundField DataField="RecommenderTel" HeaderText="推荐人联系方式" HeaderStyle-Font-Bold="true"
                        HeaderStyle-ForeColor="Black" />
                    <asp:BoundField DataField="SigningTime" HeaderText="签约时间" HeaderStyle-Font-Bold="true"
                        HeaderStyle-ForeColor="Black" />
                    <asp:BoundField DataField="Meno" HeaderText="备注" HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="Black" />
                </Columns>
                <EmptyDataTemplate>
                    当前没有任何推荐记录
                </EmptyDataTemplate>
            </sz1card1:EntityGridView>
            <asp:ObjectDataSource ID="dataAddValueList" runat="server" EnablePaging="True" SelectMethod="GetAllRecommendNote"
                MaximumRowsParameterName="limit" SelectCountMethod="GetCount" StartRowIndexParameterName="start"
                TypeName="sz1card1.Management.Logic.BusinessLogic" SortParameterName="orderBy"
                OnSelecting="dataAddValue_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="where" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <sz1card1:PopupDiv ID="popSearch" Title="查询条件" Message="请输入条件查询!" ShowMessage="true"
                runat="server" Width="400px" Height="170px" IsModal="true">
                <table border="0" cellpadding="0" cellspacing="0" style="margin-left: 0px; margin-top: 5px; margin-right: 6px;">
                    <tr>
                        <td class="table_Label" align="right">商家账号：
                        </td>
                        <td class="table_text" align="left">
                            <asp:TextBox ID="txtAccount" runat="server"></asp:TextBox>
                        </td>
                        <td class="table_Label" align="right">商家名称：
                        </td>
                        <td class="table_text" align="left">
                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_Label" align="right">推荐时间：
                        </td>
                        <td class="table_text" colspan="3" align="left">
                            <input id="tbstartTime" style="width: 120px; height: 20px; border: solid 1px lightblue;"
                                runat="server" class="Wdate" onfocus="WdatePicker()" type="text" />
                            --
                        <input id="tbendTime" style="width: 120px; height: 20px; border: solid 1px lightblue;"
                            runat="server" class="Wdate" onfocus="WdatePicker()" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="table_Label" align="right">推荐人：
                        </td>
                        <td class="table_text" align="left">
                            <asp:TextBox ID="recommenderName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="table_Label" align="right">签约时间：
                        </td>
                        <td class="table_text" colspan="3" align="left">
                            <input id="signingtime_start" style="width: 120px; height: 20px; border: solid 1px lightblue;"
                                runat="server" class="Wdate" onfocus="WdatePicker()" type="text" />
                            --
                        <input id="signingtime_end" style="width: 120px; height: 20px; border: solid 1px lightblue;"
                            runat="server" class="Wdate" onfocus="WdatePicker()" type="text" />
                        </td>
                    </tr>
                </table>
                <div align="center">
                    <br />
                    <br />
                    <asp:Button ID="Setsearch" OnClick="Setsearch_Click" runat="server" Text="搜索" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" OnClientClick="return getCancel()" UseSubmitBehavior="false"
                    CausesValidation="false" runat="server" Text="取消" />
                </div>
            </sz1card1:PopupDiv>
        </div>
    </form>
</body>
</html>
