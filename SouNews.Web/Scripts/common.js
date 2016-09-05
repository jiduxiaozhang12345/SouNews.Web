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

    return _APP_ + '?' + arr.join('&');
}

function request(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return "";
}

$(function () {
    $('body').on('keydown', keyLogin);
});

//设置回车键事件
function keyLogin() {
    if (event.keyCode == 13)  //回车键的键值为13
        $('#submit').click();
}

//保持成功
var OnSuccessAjax = function (data) {
    if (data.code > 0) {
        //关闭当前模态
        $(parent.document.getElementsByClassName('close')).click();

        //点击关闭按钮
        if (data.isdelete) {
            window.location.reload();
        } else {
            parent.location.reload();
        }
    } else if (data.code == -1) {
        alert(data.message);
    } else {
        alert("操作失败");
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
