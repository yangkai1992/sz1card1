//表格id，展示列，获取数据url，传到后台的参数
function bindGrid(id, col, url, para) {
    $.ajax({
        url: url,
        type: "POST",
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: unescape(para),
        success: function (result, textStatus) {
            var res = JSON.parse(result.d);
            if (res.success.toLocaleLowerCase() == "true") {
                renderHtml(id, col, res, url, para);
            }
            else {
                alert(res.message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("获取数据失败");
        }
    });
}

function renderHtml(docid, cols, res, url, param) {
    var data = res.data;
    var col = JSON.parse(unescape(cols));
    var para = JSON.parse(unescape(param));
    var collen = col.col.length + (col.button.length > 0 ? 1 : 0);

    var table = "<style type=\"text/css\">.selfgridtable tr:hover td{background:#D5F4FE;}</style>" +
        "<table class='selfgridtable' width='100%'>";
    var thead = "<thead><tr>";
    $.each(col.col, function (key, value) {
        var item = value;
        $.each(item, function (key, value) {
            thead += "<th>" + value + "</th>";
        })
    })
    if (col.button.length > 0) {
        thead += "<th>操作</th>";
    }
    table += thead + "</tr></thead>";
    var tbody = "<tbody>";
    if (data && data.length > 0) {
        $.each(data, function (key, value) {
            var item = value;
            var tr = "<tr>";
            $.each(item, function (key, value) {
                var keys = key;
                var values = value;
                $.each(col.col, function (key, value) {
                    var items = value;
                    $.each(items, function (key, value) {
                        if (key == keys) {
                            if (key == "Text") {
                                tr += "<td style='word-break:break-all;'>" + values + "</td>";
                            }
                            else {
                                tr += "<td>" + values + "</td>";
                            }
                        }
                    })
                })
            });
            var oper = "";
            if (col.button.length > 0) {
                oper = "<td>";
                $.each(col.button, function (key, value) {
                    var item1 = value;
                    $.each(item1, function (key, value) {
                        oper += "<a id='" + item.Account + "' class='" + key + "' href='###'>" + value + "</a>"
                    });
                });
                oper += "</td>";
            }
            tr += oper + "</tr>";
            tbody += tr;
        });
    }
    else {
        tbody += "<tr><td colspan='" + collen + "'>暂无数据</td></tr>";
    }
    table += tbody + "</tbody>";

    var totalPage = Math.ceil(res.total / para.rows);
    var para1 = CopyJson(para);
    para1.currentPage = 1;
    var para2 = CopyJson(para);
    para2.currentPage = para2.currentPage > 1 ? (para2.currentPage - 1) : 1;
    var para3 = CopyJson(para);
    para3.currentPage = para3.currentPage >= totalPage ? totalPage : (para3.currentPage + 1);
    var para4 = CopyJson(para);
    para4.currentPage = totalPage;
    var homePage = "<a href='###' onclick=\"bindGrid('" + docid + "','" + cols + "','" + url + "','" + escape(JSON.stringify(para1)) + "')\")>首页</a>&nbsp;&nbsp;";
    var prePage = "<a href='###' onclick=\"bindGrid('" + docid + "','" + cols + "','" + url + "','" + escape(JSON.stringify(para2)) + "')\">上一页</a>&nbsp;&nbsp;";
    var nextPage = "<a href='###' onclick=\"bindGrid('" + docid + "','" + cols + "','" + url + "','" + escape(JSON.stringify(para3)) + "')\">下一页</a>&nbsp;&nbsp;";
    var lastPage = "<a href='###' onclick=\"bindGrid('" + docid + "','" + cols + "','" + url + "','" + escape(JSON.stringify(para4)) + "')\">尾页</a>&nbsp;&nbsp;";
    var pages = para.currentPage + "/" + totalPage + "&nbsp;&nbsp;";
    var total = "总数:" + res.total + "&nbsp;&nbsp;";
    var arr = [10, 20, 40];
    var option = "";
    for (var i in arr) {
        if (arr[i] == para.rows) {
            option = option + "<option value='" + arr[i] + "' selected='selected'>" + arr[i] + "</option>";
        }
        else {
            option = option + "<option value='" + arr[i] + "'>" + arr[i] + "</option>";
        }
    }
    var rows = "每页条数:<select id='" + docid + "_rows' onchange=\"selectPagesRows('" + docid + "','" + cols + "','" + url + "','" + param + "')\">" + option + "</select>";
    var refresh = "&nbsp;&nbsp;<a href='###' onclick=\"bindGrid('" + docid + "','" + cols + "','" + url + "','" + param + "')\">刷新</a>";
    var tfoot = "<tfoot><tr><td colspan='" + collen + "'>" + homePage + prePage + nextPage + lastPage + pages + total + rows + refresh + "</td></tr></tfoot>"
    table += tfoot;
    table += "</table>";
    $("#" + docid).html(table);
}

function selectPagesRows(docid, cols, url, param) {
    var row = document.getElementById(docid + "_rows") ? document.getElementById(docid + "_rows").value : 10;
    var para = JSON.parse(unescape(param));
    para.currentPage = 1;
    para.rows = row;
    bindGrid(docid, cols, url, escape(JSON.stringify(para)))
}

function CopyJson(json) {
    var obj = {};
    if (json) {
        for (var item in json) {
            obj[item] = json[item];
        }
    }
    return obj
}
