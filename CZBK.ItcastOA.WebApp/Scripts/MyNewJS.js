$(window).resize(function () {
    Twidth = $(window).width() / 1.2;
    Theight = $(window).height() / 1.2;
});
//查看图片
$(function () {
    $("#SeeImageDIV").css("display", "none");
   
})
function SeeImage() {
    var rows = $('#tt').datagrid('getSelections');
    if (!rows || rows.length == 0) {
        //alert("请选择要修改的商品！");
        $.messager.alert("提醒", "请选择要查看的记录!", "error");
        return;
    }
    if (rows.length > 1) {
        $.messager.alert("提醒", "仅可查看一条信息！", "error");
        return;
    }
    if (rows[0].Image_str != "有") {
        $('#tt').datagrid('clearChecked');
        $.messager.alert("提醒", "信息没有图片可以预览！", "error");
        return;
    }
    $.post("/HrefInfo/SeeImage", { "id": rows[0].ID }, function (serverData) {
        $('#tt').datagrid('clearChecked');
        var data = $.parseJSON(serverData);
        if (data.msg == "ok") {
            var t = data.serverData;
            var c = t.split("---");
            var width = $(document.body).width() - $(document.body).width() / 2;
            var height = $(document.body).height() - $(document.body).height() / 5;
            var Pageimage = 0, MaxImage = c.length;
            $("#Timage").attr("src", c[Pageimage]);
            $("#Timage").attr("width", width - 30);
            $("#Timage").attr("height", height - 80);
            $("#SeeImageDIV").css("display", "block");
            $('#SeeImageDIV').dialog({
                title: "编辑用户信息",
                width: width,
                height: height,
                collapsible: true,
                resizable: true,
                modal: true,
                buttons: [
                    {
                        text: '上一页',
                        iconCls: 'icon-ok',
                        handler: function () {
                            Pageimage = Pageimage - 1 < 0 ? MaxImage : Pageimage - 1;
                            $("#Timage").attr("src", c[Pageimage]);
                        }
                    },
                    {
                        text: '下一页',
                        iconCls: 'icon-ok',
                        handler: function () {
                            Pageimage = Pageimage + 1 > MaxImage ? 0 : Pageimage + 1;
                            $("#Timage").attr("src", c[Pageimage]);
                        }
                    }, {
                        text: '关闭',
                        handler: function () {
                            $('#SeeImageDIV').dialog('close');
                        }
                    }]
            });
        } else {
            $.messager.alert("提醒", "展示数据错误!!", "error");
        }
    });
}
function SeeMap() {
    var rows = $('#tt').datagrid('getSelections');
    if (rows.length != 1) {
        //alert("请选择要修改的商品！");
        $.messager.alert("提醒", "请选择要分配角色权限的一条记录!", "error");
        return;
    }
    $("#SeeMapFrame").attr("src", "/hrefinfo/GetMap/?Address=" + rows[0].Address);
    $("#SeeMapFrame").attr("height", "100%");
    $("#SeeMapFrame").attr("width", "100%");
    $("#SeeMap").css("display", "block");
    $('#SeeMap').dialog({
        title: "百度地图",
        width: $(window).width()/1.5,
        height: $(window).height()/1.5,
        collapsible: true,
        resizable: true,
        modal: true,
        buttons: [ {
            text: '取消',
            handler: function () {
                $('#SeeMap').dialog('close');
            }
        }]
    });
}
    
