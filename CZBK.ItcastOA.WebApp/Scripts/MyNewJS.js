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
//将序列化成json格式后日期(毫秒数)转成日期格式
function ChangeDateFormat(cellval) {
    var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    return date.getFullYear() + "-" + month + "-" + currentDate ;
}
function ChangeDateFormatHours_Minute(cellval) {
    var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    return date.getFullYear() + "-" + month + "-" + currentDate + " &nbsp" + date.getHours() + ":" + date.getMinutes();
}
//数字验证
function num(obj) {
    obj.value = obj.value.replace(/[^\d.]/g, ""); //清除"数字"和"."以外的字符
    obj.value = obj.value.replace(/^\./g, ""); //验证第一个字符是数字
    obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个, 清除多余的
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
    obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3'); //只能输入两个小数
}
//金额小写转大写
function Arabia_to_Chinese(Num) {

    for (i = Num.length - 1; i >= 0; i--) {
        Num = Num.replace(",", "")//替换tomoney()中的“,”
        Num = Num.replace(" ", "")//替换tomoney()中的空格
    }

    //替换掉可能出现的￥字符
    Num = Num.replace("￥", "")
    if (isNaN(Num)) { //验证输入的字符是否为数字
        alert("请检查小写金额是否正确");
        return;
    }
    //---字符处理完毕，开始转换，转换采用前后两部分分别转换---//
    part = String(Num).split(".");
    newchar = "";
    //小数点前进行转化
    for (i = part[0].length - 1; i >= 0; i--) {
        if (part[0].length > 10) { alert("位数过大，无法计算"); return ""; } //若数量超过拾亿单位，提示
        tmpnewchar = ""
        perchar = part[0].charAt(i);
        switch (perchar) {
            case "0": tmpnewchar = "零" + tmpnewchar; break;
            case "1": tmpnewchar = "壹" + tmpnewchar; break;
            case "2": tmpnewchar = "贰" + tmpnewchar; break;
            case "3": tmpnewchar = "叁" + tmpnewchar; break;
            case "4": tmpnewchar = "肆" + tmpnewchar; break;
            case "5": tmpnewchar = "伍" + tmpnewchar; break;
            case "6": tmpnewchar = "陆" + tmpnewchar; break;
            case "7": tmpnewchar = "柒" + tmpnewchar; break;
            case "8": tmpnewchar = "捌" + tmpnewchar; break;
            case "9": tmpnewchar = "玖" + tmpnewchar; break;
        }
        switch (part[0].length - i - 1) {
            case 0: tmpnewchar = tmpnewchar + "元"; break;
            case 1: if (perchar != 0) tmpnewchar = tmpnewchar + "拾"; break;
            case 2: if (perchar != 0) tmpnewchar = tmpnewchar + "佰"; break;
            case 3: if (perchar != 0) tmpnewchar = tmpnewchar + "仟"; break;
            case 4: tmpnewchar = tmpnewchar + "万"; break;
            case 5: if (perchar != 0) tmpnewchar = tmpnewchar + "拾"; break;
            case 6: if (perchar != 0) tmpnewchar = tmpnewchar + "佰"; break;
            case 7: if (perchar != 0) tmpnewchar = tmpnewchar + "仟"; break;
            case 8: tmpnewchar = tmpnewchar + "亿"; break;
            case 9: tmpnewchar = tmpnewchar + "拾"; break;
        }
        newchar = tmpnewchar + newchar;
    }

    //小数点之后进行转化
    if (Num.indexOf(".") != -1) {
        if (part[1].length > 2) {
            alert("小数点之后只能保留两位,系统将自动截段");
            part[1] = part[1].substr(0, 2)
        }
        for (i = 0; i < part[1].length; i++) {
            tmpnewchar = ""
            perchar = part[1].charAt(i)
            switch (perchar) {
                case "0": tmpnewchar = "零" + tmpnewchar; break;
                case "1": tmpnewchar = "壹" + tmpnewchar; break;
                case "2": tmpnewchar = "贰" + tmpnewchar; break;
                case "3": tmpnewchar = "叁" + tmpnewchar; break;
                case "4": tmpnewchar = "肆" + tmpnewchar; break;
                case "5": tmpnewchar = "伍" + tmpnewchar; break;
                case "6": tmpnewchar = "陆" + tmpnewchar; break;
                case "7": tmpnewchar = "柒" + tmpnewchar; break;
                case "8": tmpnewchar = "捌" + tmpnewchar; break;
                case "9": tmpnewchar = "玖" + tmpnewchar; break;
            }
            if (i == 0) tmpnewchar = tmpnewchar + "角";
            if (i == 1) tmpnewchar = tmpnewchar + "分";
            newchar = newchar + tmpnewchar;
        }
    }
    //替换所有无用汉字
    while (newchar.search("零零") != -1)
        newchar = newchar.replace("零零", "零");
    newchar = newchar.replace("零亿", "亿");
    newchar = newchar.replace("亿万", "亿");
    newchar = newchar.replace("零万", "万");
    newchar = newchar.replace("零元", "元");
    //newchar = newchar.replace("零角", "");
    newchar = newchar.replace("零分", "");

    if (newchar.charAt(newchar.length - 1) == "元" || newchar.charAt(newchar.length - 1) == "角")
        newchar = newchar + "整"
    //  document.write(newchar);
    newchar = newchar.replace("零角整", "整");
    return newchar;

}