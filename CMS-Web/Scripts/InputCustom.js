/* CKEDITOR */
function init_Ckeditor() {
    $('.ckeditor').ckeditor();
    //CKEDITOR.replace("ckeditor", { customConfig: "/Libraries/plugins/ckeditor/config.js" });
};
/* END CKEDITOR */

/* SWITCHERY */
function init_Switch() {
    if ($(".js-switch")[0]) {
        var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
        elems.forEach(function (html) {
            var switchery = new Switchery(html, {
                color: '#26B99A'
            });
        });
    }
    if ($(".js-switch-chk")[0]) {
        var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch-chk'));
        elems.forEach(function (html) {
            if (html.style.display != "none") {
                var switchery = new Switchery(html, {
                    color: '#26B99A'
                });
            }
        });
    }
};
/* END SWITCHERY */

/* ICHECK */
function init_ICheck() {
    if ($("input.icheck")[0]) {
        $(document).ready(function () {
            $('input.icheck').iCheck({
                checkboxClass: 'icheckbox_flat-green',
                radioClass: 'iradio_flat-green'
            });
        });
    }
};
/* END ICHECK */

function init_ICheckMoney() {
    //keyMoney
    $(".keyMoney").keypress(function (e) {
        if (window.event) {
            if (window.event.keyCode != 46 && window.event.keyCode > 31
                && (window.event.keyCode < 48 || window.event.keyCode > 57)) {
                return (false);//chrome and IE
            }
        } else {
            if (e.which != 46 && e.which > 31
                && (e.which < 48 || e.which > 57)) {
                return (false);//firefox
            }
        }
    });
}


function change_alias(alias) {
    var str = alias;
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g, " ");
    str = str.replace(/ + /g, " ");
    str = str.trim().replace(/ /g, "-");
    $("#Alias").val(str);
    return str;
}

//function change_alias(text) {
//    debugger;
//    var arr1 = new arr1[
//        "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
//        "đ",
//        "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ",
//        "í", "ì", "ỉ", "ĩ", "ị",
//        "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ",
//        "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự",
//        "ý", "ỳ", "ỷ", "ỹ", "ỵ"];
//    var arr2 = new arr2[
//        "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
//        "d",
//        "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e",
//        "i", "i", "i", "i", "i",
//        "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o",
//        "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u",
//        "y", "y", "y", "y", "y"];
//    for (var i = 0; i < arr1.Length; i++) {
//        text = text.Replace(arr1[i], arr2[i]);
//        text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
//    }
//    return text;
//}

$(document).ready(function () {
    init_Switch();
    init_ICheck();
    init_Ckeditor();
    init_ICheckMoney();
});