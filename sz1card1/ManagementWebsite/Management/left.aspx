<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Left.aspx.cs" Inherits="ManagementWebsite.Management.left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head" runat="server">
    <link href="../Css/Public.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../JS/LeftMeun.js"></script>
    <title>菜单栏</title>
</head>
<body id="leftMenu" onload="DefaltExpand()">
    <form id="form1" runat="server">
        <div id="Topdiv">
            <table id="Str" width="147" height="100%" border="0" align="center" cellpadding="0"
                cellspacing="0">
                <asp:XmlDataSource ID="xmlDSMenu" runat="server" XPath="siteMap/menu" EnableCaching="false"></asp:XmlDataSource>
                <asp:Repeater runat="server" ID="RepeaterDate1" DataSourceID="xmlDSMenu" OnItemDataBound="LeftMenu_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td height="23" style="background: url(../images/main_menu.gif) no-repeat">
                                <table style="height: 100%;" id="table3<%# XPath("id")%>" title="菜单" width="100%"
                                    onclick="shrinkmeun(this,<%# XPath("id")%>)" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="83%" style="cursor: pointer; border-color: #adb9c2;" align="center">
                                            <%# XPath("title")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table title="" id="table3<%# XPath("id")%>" width="100%" border="0" cellspacing="0"
                                    cellpadding="0" style="height: 100%; border: 1px solid #c5ddff; display: none; margin: 0px 2px 2px 2px">
                                    <asp:Repeater runat="server" ID="RepaterDate2">
                                        <ItemTemplate>
                                            <tr>
                                                <td width="12%">&nbsp;
                                                </td>
                                                <td width="8%">
                                                    <img src="../Images/Icons/<%# XPath("icon")%>" />
                                                </td>
                                                <td width="72%" style="cursor: hand; height: 18px" onmouseover="this.className='td_body'"
                                                    onmouseout="this.className='td_none'">
                                                    <div align="center">
                                                        <a alt="<%# XPath("description")%>" onclick="AddUrlToPathArr('<%# XPath("url")%>','<%# XPath("title")%>')"
                                                            style="cursor: pointer;">
                                                            <%# XPath("title")%></a>
                                                    </div>
                                                </td>
                                                <td width="8%">&nbsp;
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr>
                                        <td>
                                            <div id="newDiv<%# XPath("id")%>">
                                                &nbsp;
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div id="downdiv" style="width: 100%; background-image: url('../images/main_69.gif'); position: absolute; bottom: 0px;"
            align="center">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" align="center">
                <tr>
                    <td valign="bottom" align="center">
                        <span style="width: 100%">&nbsp;</span><%--版本：2008 v1.0--%>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
