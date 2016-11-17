// JavaScript Document
//
$(function () {
    $.fn.Reg_Input_Text = function (regstr, errstr, curstr) {
        var idstr = $(this).attr("id");
        idstr = idstr.substring(2);
        var textid0 = $("#text" + idstr);
        var lbid0 = $("#lb" + idstr);
        if (regstr != "") {
            var reg = regstr;
            if (!reg.test(textid0.val())) {
                lbid0.text(errstr); return false;
            }
            else {
                lbid0.text(curstr); return true;
            }
        }
    }
    $.fn.Empty_Input_Text = function (errstr,curstr) {
        var idstr = $(this).attr("id");
        idstr = idstr.substring(2);
        var textid0 = $("#text" + idstr);
        var lbid0 = $("#lb" + idstr);
        if (textid0.val() == "") {
            lbid0.text(errstr);
            return false;
        }
        else {
            lbid0.text(curstr);
            return true;
        }
    }
})
function check0(jsons)
{
    var return_str = true;
    for (var i = 0; i < jsons.length; i++) {
        if (jsons[i]["type"] == "re") {
            return_str = $("#" + jsons[i]["id"]).Reg_Input_Text(jsons[i]["regstr"], jsons[i]["newerr"], jsons[i]["cureinfo"]);
        }
        else if (jsons[i]["type"] == "em") {
            return_str = $("#" + jsons[i]["id"]).Empty_Input_Text(jsons[i]["newerr"], jsons[i]["cureinfo"]);
        }
        if (!return_str) break;
    }
    return return_str;
}