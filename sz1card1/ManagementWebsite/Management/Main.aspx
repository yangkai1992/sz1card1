<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="ManagementWebsite.Management.Main" %>

<html>
<head id="Head1" runat="server">
    <title>一卡易管理支撑系统</title>
    <link href="../Css/Public.css" rel="stylesheet" type="text/css" />

    <script src="../JavaScript/CenterBat.js" type="text/javascript"></script>

    <style type="text/css">
        .operation_img {
            vertical-align: middle;
            cursor: pointer;
        }

        * {
            margin: 0;
            padding: 0;
        }

        #div_pannel {
            height: 94%;
        }

        #menu {
            padding: 10px 0 0 10px;
        }

            #menu a {
                color: #333;
            }

                #menu a.crent {
                    color: #ff0000;
                }

        #div_tab {
            background: #fff url(../Images/t_tab_bg.gif) repeat-x 0 bottom;
            height: 26px;
            padding: 1px 15px 0;
            margin-bottom: 10px;
        }

            #div_tab li {
                float: left;
                text-align: center;
                position: relative;
                list-style: none;
            }

            #div_tab li {
                background: url(../Images/t_tab_uselectbg.gif);
                margin-top: 3px;
                height: 23px;
            }

                #div_tab li span {
                    background: url(../Images/t_tab_uselectbg.gif);
                    height: 23px;
                    line-height: 23px;
                }

                #div_tab li.crent {
                    background: url(../Images/t_tab_selectbg.gif);
                    margin-top: 2px;
                    height: 24px;
                }

                    #div_tab li.crent span {
                        background: url(../Images/t_tab_selectbg.gif);
                        height: 24px;
                        line-height: 24px;
                    }

                #div_tab li, #div_tab li.crent {
                    color: #fff;
                    background-repeat: no-repeat;
                    background-position: 0 0;
                }

                    #div_tab li span, #div_tab li.crent span {
                        display: inline-block;
                        padding: 0 45px 0 10px;
                        background-repeat: no-repeat;
                        background-position: right bottom;
                    }

                    #div_tab li .menua {
                        color: #000;
                        font-size: 12px;
                        text-decoration: none;
                        position: relative;
                    }

                        #div_tab li.crent .menua, #div_tab li .menua:hover {
                            color: #ff0000;
                        }

                    #div_tab li .win_close, #div_tab li.crent .win_close, #div_tab li .win_refresh, #div_tab li.crent .win_refresh {
                        width: 14px;
                        height: 14px;
                        position: absolute;
                        top: 6px;
                        cursor: pointer;
                        display: block;
                        overflow: hidden;
                    }

        .win_close {
            right: 5px;
            background: url(../Images/t_delete_ico.gif) no-repeat;
        }

        .win_refresh {
            right: 23px;
            background: url(../Images/t_refresh_ico.png) no-repeat;
        }

        #div_tab li .win_close, #div_tab li .win_refresh {
            background-position: 0 -14px;
        }

            #div_tab li .win_close:hover, #div_tab li .win_refresh:hover {
                background-position: 0 0;
            }

        .clearfix:after {
            content: ".";
            display: block;
            height: 0;
            clear: both;
            visibility: hidden;
        }

        *html .clearfix {
            height: 1%;
        }

        * + html .clearfix {
            height: 1%;
        }

        .clearfix {
            display: inline-block;
        }
        /* Hide from IE Mac */ .clearfix {
            display: block;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function ClosePopupDiv(result) {
            if (result == "success") {
                alert("修改密码成功");
                PopupDiv.close('popUserPwd');
            }
        }

        function LoadIndexPage() {
            document.getElementById("div_middle").style.height = document.body.clientHeight - 87;
            CreateDiv("Index_aspx", "index.aspx", "首页");
        }

        function CreateDiv(tabid, url, name, isfresh) {
            if (isfresh == 'true' && frames["div_" + tabid] != undefined) {
                CreateDiv(tabid, url, name, 'false');//切换页签展示
                Refresh("div_" + tabid, url);//刷新
                return;
            }
            var tablist = document.getElementById("div_tab").getElementsByTagName('li');
            ///如果当前tabid不存在则创建，存在则直接显示已经打开的tab
            if (document.getElementById("div_" + tabid) == null) {
                if (tablist.length > 10) {
                    alert("您打开的页面太多，请先关闭部分页面再打开！");
                    return;
                }
                //创建iframe
                var box = document.createElement("iframe");
                box.id = "div_" + tabid;
                box.name = "div_" + tabid;
                box.src = url;
                box.height = "100%";
                box.frameBorder = 0;
                box.width = "100%";
                document.getElementById("div_pannel").appendChild(box);

                //遍历并清除开始存在的tab当前效果并隐藏其显示的div
                var pannellist = document.getElementById("div_pannel").getElementsByTagName('iframe');
                if (tablist.length > 0) {
                    for (i = 0; i < tablist.length; i++) {
                        tablist[i].className = "";
                        pannellist[i].style.display = "none";
                        var a = tablist[i].childNodes[0].childNodes;
                        for (var j = 0; j < a.length; j++) {
                            if (a[j].className == "win_refresh") {
                                a[j].style.display = "none";
                            }
                        }
                    }
                }

                //创建li菜单
                var tab = document.createElement("li");
                tab.className = "crent";
                tab.id = tabid;
                var litxt;
                if (name == "首页") {
                    litxt = "<span style=\"padding:0 15px 0 15px;\"><a href=\"javascript:;\" onclick=\"javascript:CreateDiv('" + tabid + "','" + url + "','" + name + "')\" title=" + name + " class=\"menua\">" + name + "</a></span>";
                }
                else {
                    litxt = "<span id='span_tab'><a onclick=\"Refresh('div_" + tabid + "')\" class=\"win_refresh\" title=\"刷新当前窗口\"></a><a href=\"javascript:;\" onclick=\"javascript:CreateDiv('" + tabid + "','" + url + "','" + name + "')\" title=" + name + " class=\"menua\">" + name + "</a><a onclick=\"RemoveDiv('" + tabid + "')\" class=\"win_close\" title=\"关闭当前窗口\"></a></span>";
                }
                tab.innerHTML = litxt;
                document.getElementById("div_tab").appendChild(tab);
            }
            else {
                var pannellist = document.getElementById("div_pannel").getElementsByTagName('iframe');
                for (i = 0; i < tablist.length; i++) {
                    tablist[i].className = "";
                    pannellist[i].style.display = "none";
                    var a = tablist[i].childNodes[0].childNodes;
                    for (var j = 0; j < a.length; j++) {
                        if (a[j].className == "win_refresh") {
                            a[j].style.display = "none";
                        }
                    }
                }
                document.getElementById(tabid).className = "crent";
                document.getElementById("div_" + tabid).style.display = "block";
                var current = document.getElementById(tabid).childNodes[0].childNodes;
                for (var j = 0; j < current.length; j++) {
                    if (current[j].className == "win_refresh") {
                        current[j].style.display = "block";
                    }
                }
            }
        }
        function RemoveDiv(obj) {
            var ob = document.getElementById(obj);
            ob.parentNode.removeChild(ob);
            var obdiv = document.getElementById("div_" + obj);
            obdiv.parentNode.removeChild(obdiv);
            var tablist = document.getElementById("div_tab").getElementsByTagName('li');
            var pannellist = document.getElementById("div_pannel").getElementsByTagName('iframe');
            if (tablist.length > 0) {
                tablist[tablist.length - 1].className = 'crent';
                pannellist[tablist.length - 1].style.display = 'block';

                var current = tablist[tablist.length - 1].childNodes[0].childNodes;
                for (var j = 0; j < current.length; j++) {
                    if (current[j].className == "win_refresh") {
                        current[j].style.display = "block";
                    }
                }
            }
        }
        function Refresh(frameid, url) {
            if (url) {
                if (frames[frameid].location.href.indexOf(url) < 0) {
                    if (url.indexOf("/Interface/UnionPay/Alipay/AlipayCreateStoreIndex.aspx") >= 0)  //口碑开店特殊处理
                    {
                        var href = frames[frameid].location.origin + url;
                        frames[frameid].location.replace(href);
                    }
                    else {
                        frames[frameid].location.search = url.split("aspx")[1];
                        var href = frames[frameid].location.origin + frames[frameid].location.pathname + url.split("aspx")[1];
                        frames[frameid].location.replace(href);
                    }

                }
            }
            else if (frameid.indexOf("Business_BusinessList_aspx") >= 0) {
                frames[frameid].location.search = "";
                var href = frames[frameid].location.origin + frames[frameid].location.pathname;
                frames[frameid].location.replace(href);
            }
            else {
                frames[frameid].location.reload(true);
            }
        }
    </script>
</head>
<body onload="LoadIndexPage()">
    <form id="form1" runat="server" style="overflow: hidden;">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="57" background="../images/main_03.gif">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="378" height="57" background="../images/main_01.gif">&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td width="281" valign="bottom">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="33" height="27">
                                            <img src="../images/main_05.gif" width="33" height="27" />
                                        </td>
                                        <td width="248" background="../images/main_06.gif">
                                            <table width="225" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td height="17">
                                                        <div align="right">
                                                            <a href="javascript:;" onclick="PopupDiv.showDialog('popUserPwd','User/UpdateUserPwd.aspx?way=Main');return false;">
                                                                <img border="0" src="../images/pass.gif" width="69" height="17" /></a>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div align="right">
                                                            <a href="javascript:;" onclick="PopupDiv.showDialog('popUserMessage','User/UserMessage.aspx');return false;">
                                                                <img border="0" src="../images/user.gif" width="69" height="17" /></a>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div align="right">
                                                            <asp:ImageButton ID="ImageButton1" OnClientClick="return confirm('确定退出该操作吗？')" BorderWidth="0"
                                                                ImageUrl="../images/quit.gif" Width="69" Height="17" runat="server" OnClick="ImageButton1_Click" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="30" background="../images/main_31.gif">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 0px">
                        <tr>
                            <td width="147" background="../images/main_29.gif">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 0px">
                                    <tr>
                                        <td width="24%">&nbsp;
                                        </td>
                                        <td width="43%" height="20" valign="bottom">管理菜单
                                        </td>
                                        <td width="33%">&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="39">
                                <img src="../images/main_30.gif" width="39" height="30" />
                            </td>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 0px">
                                    <tr>
                                        <td height="20" valign="bottom">
                                            <span>当前登录用户：<%=CurrentUser.Account%>&nbsp;&nbsp;用户角色：<%= CurrentUser.UserGroup.GroupName%></span>
                                        </td>
                                        <td valign="bottom">
                                            <div align="right">
                                                <img src="../images/sj.gif" width="6" height="7" />
                                                IP：<asp:Label ID="LbIpaddress" runat="server"></asp:Label>
                                            &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                                        &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <!--中间主体部分-->
        <div style="margin-top: 0px" id="div_middle">
            <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="147" id="FrameTitle" valign="top">
                        <iframe height="100%" width="100%" name="Frameleft" scrolling="no" frameborder="0"
                            src="Left.aspx"></iframe>
                    </td>
                    <td width="11" style="background: #add2da; text-align: center;" valign="middle" onclick="switchSysBar()">
                        <span id="switchPoint" title="关闭/打开左栏">
                            <img src="../images/menu_off.gif" style="cursor: hand"></span>
                    </td>
                    <td valign="top">
                        <ul class="clearfix" id="div_tab">
                        </ul>
                        <div id="div_pannel">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <sz1card1:PopupDiv ID="popUserPwd" Height="130" Width="250" Title="修改用户密码" Message="请输入原密码和新密码"
            ShowMessage="true" runat="server">
        </sz1card1:PopupDiv>
        <sz1card1:PopupDiv ID="popUserMessage" Height="180" Width="350" Title="用户信息" runat="server">
        </sz1card1:PopupDiv>
    </form>
</body>
</html>
