/**
 * Created with JetBrains PhpStorm.
 * User: kk
 * Date: 13-8-28
 * Time: 下午4:44
 */
function U() {
    var url = arguments[0] || [];
    var param = arguments[1] || {};
    var url_arr = url.split('/');

    if (!$.isArray(url_arr) || url_arr.length < 2 || url_arr.length > 3) {
        return '';
    }

    if (url_arr.length == 2)
        url_arr.unshift(_GROUP_);

    var pre_arr = ['g', 'm', 'a'];

    var arr = [];
    for (d in pre_arr)
        arr.push(pre_arr[d] + '=' + url_arr[d]);

    for (d in param)
        arr.push(d + '=' + param[d]);

    return _APP_+'?'+arr.join('&');
}

function request(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return "";
}

//保持成功
var OnSuccessAjax = function (data) {    
    if (data.code > 0) {
        //关闭当前模态
        
        if (data.message&&data.message != "") {
            alert(data.message);
        }
        
        if (data.redirect && data.redirect != "") {
            location.href = data.redirect;
            return;
        }

        if (data.noClose) {

        } else {
            $(parent.document.getElementsByClassName('close')).click();
        }
       
        //点击关闭按钮
        if (data.isdelete) {
            window.location.reload(); 
        } else {
            parent.location.reload();
 
        }
    } else if (data.code == -1) {
        alert(data.message);
    } else {
        $.growl.warning({ message: "操作失败", title: '验证提示' });
    }
    $('#btnSubmit').css('display', 'inline');
}

//请求出错
var OnFailureAjaxBeginForm = function (id) {
    OnCompleteAjaxBeginForm(id);
}
//请求开始
OnBeginAjaxBeginForm = function (id) {
    if ($.type(id) === "string") {
        $('#' + id).addClass('disabled').attr("disabled", true);

    } else {
        $('#btnSubmit').addClass('disabled').attr("disabled", true);
    }
}
//请求结束
var OnCompleteAjaxBeginForm = function (id) {
    if ($.type(id) === "string") {
        $('#' + id).removeClass('disabled').attr("disabled", false);
    } else {
        try {
            $('#btnSubmit').removeClass('disabled').attr("disabled", false);
        } catch (e) {

        }
        try {
            if (id != undefined) {
                if (id.responseText.indexOf('管理后台登录') > 0) {
                    window.location.href = '/Home/Login';
                }
            }
        } catch (e) {

        }
    }
}

//组织数据
function prepareData() {
    var stringdata = "";
    $('.feeName').each(function () {
        var money = 0;
        if ($(this).val() == "") {
            return false;
        }
        var baiw = $(this).parent().siblings().eq(1).find('input');
        var shiw = $(this).parent().siblings().eq(2).find('input');
        var wan = $(this).parent().siblings().eq(3).find('input');
        var qian = $(this).parent().siblings().eq(4).find('input');
        var bai = $(this).parent().siblings().eq(5).find('input');
        var shi = $(this).parent().siblings().eq(6).find('input');
        var yuan = $(this).parent().siblings().eq(7).find('input');
        var jiao = $(this).parent().siblings().eq(8).find('input');
        var fen = $(this).parent().siblings().eq(9).find('input');
        //费用名称
        if ($(this).val() != "" && $(this).val() != "￥") {
            stringdata += $(this).val().replace(/&/g, '＆').replace(/#/g, '＃') + "&";
        }
        //百万
        if (baiw.val() != "" && baiw.val() != "￥") {
            if (isNaN(baiw.val())) {
                baiw.focus();
                return false;
            }
            stringdata += baiw.val();
            money += baiw.val();
        } else {
            stringdata += "0";
            money += "0";
        }
        //十万
        if (shiw.val() != "" && shiw.val() != "￥") {
            if (isNaN(shiw.val())) {
                shiw.focus();
                return false;
            }
            stringdata += shiw.val();
            money += shiw.val();
        } else {
            stringdata += "0";
            money += "0";
        }
        //万
        if (wan.val() != "" && wan.val() != "￥") {
            if (isNaN(wan.val())) {
                wan.focus();
                return false;
            }
            stringdata += wan.val();
            money += wan.val();
        } else {
            stringdata += "0";
            money += "0";
        }
        //千
        if (qian.val() != "" && qian.val() != "￥") {
            if (isNaN(qian.val())) {
                qian.focus();
                return false;
            }
            stringdata += qian.val();
            money += qian.val();
        } else {
            stringdata += "0";
            money += "0";
        }
        //百
        if (bai.val() != "" && bai.val() != "￥") {
            if (isNaN(bai.val())) {
                bai.focus();
                return false;
            }
            stringdata += bai.val();
            money += bai.val();
        } else {
            stringdata += "0";
            money += "0";
        }
        //十
        if (shi.val() != "" && shi.val() != "￥") {
            if (isNaN(shi.val())) {
                shi.focus();
                return false;
            }
            stringdata += shi.val();
            money += shi.val();
        } else {
            stringdata += "0";
            money += "0";
        }
        //元
        if (yuan.val() != "" && yuan.val() != "￥") {
            if (isNaN(yuan.val())) {
                yuan.focus();
                return false;
            }
            stringdata += yuan.val();
            money += yuan.val();
        } else {
            stringdata += "0";
            money += "0";
        }
        //加小数点
        stringdata += "."
        money += "."
        //角
        if (jiao.val() != "" && jiao.val() != "￥") {
            if (isNaN(jiao.val())) {
                jiao.focus();
                return false;
            }
            stringdata += jiao.val();
            money += jiao.val();
        } else {
            stringdata += "0";
            money += "0";
        }
        //分
        if (fen.val() != "" && fen.val() != "￥") {
            if (isNaN(fen.val())) {
                fen.focus();
                return false;
            }
            stringdata += fen.val();
            money += fen.val();
        } else {
            stringdata += "0";
            money += "0";
        }
        summoney += parseFloat(money);
        stringdata = stringdata + "#";
    });
    stringdata = stringdata.substring(0, stringdata.length - 1);
    $('#stringdata').val(stringdata);
    return true;
}

//金额转成大写
function ConvertDX(num) {
    var strOutput = "";
    var strUnit = '仟佰拾亿仟佰拾万仟佰拾元角分';
    num += "00";
    var intPos = num.indexOf('.');
    if (intPos >= 0)
        num = num.substring(0, intPos) + num.substr(intPos + 1, 2);
    strUnit = strUnit.substr(strUnit.length - num.length);
    for (var i = 0; i < num.length; i++)
        strOutput += '零壹贰叁肆伍陆柒捌玖'.substr(num.substr(i, 1), 1) + strUnit.substr(i, 1);
    return strOutput.replace(/零角零分$/, '整').replace(/零[仟佰拾]/g, '零').replace(/零{2,}/g, '零').replace(/零([亿|万])/g, '$1').replace(/零+元/, '元').replace(/亿零{0,3}万/, '亿').replace(/^元/, "零元");
};