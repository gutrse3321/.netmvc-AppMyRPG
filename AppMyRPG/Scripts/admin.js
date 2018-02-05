$(document).ready(function () {
    //获取浏览器可视宽度
    var bwidth = $(window).width();
    //获取有导航蓝宽度
    var lwidth = $("#bd-left").width();
    //得出主面板宽度并设置
    var mwidth = bwidth - lwidth;
    $("#bd-main").css("width", mwidth);

    //resize()事件 浏览器框高宽改变触发
    $(window).resize(function () {
        bwidth = $(window).width();
        mwidth = bwidth - lwidth;
        if (mwidth < 1040) {
            $("#bd-main").css("width", "1040x");
        }
        else {
            $("#bd-main").css("width", mwidth);
        }
    });

    //返回顶部
    $(window).scroll(function () {

        if ($(window).scrollTop() > 500) {
            $("#toTop").fadeIn();
        } else {
            $("#toTop").fadeOut();
        }
    });
    $("#toTop").click(function () {
        $("html,body").animate({
            "scrollTop":0
        },200);
    });

    //toggle()替换方法 参数
    var toggleNum = 0;
    //排序和分类下拉框
    $(".downlist-btn").click(function () {
        if (toggleNum === 0) {
            $(this).next().show();
            toggleNum = 1;
        }
        else {
            $(this).next().hide();
            toggleNum = 0;
        }
    });
    //修改封面、截图或添加封面、截图
    $(".gedit-file").hide();
    $(".gedit-file-img").hide();
    $(".user-file").hide();
    $(".gedit-img").mouseover(function () {
        $(".gedit-file").show();
    });
    $(".gedit-shot").mouseover(function () {
        $(this).next().show();
    });
    $(".user-face").mouseover(function () {
        $(this).next().show();
    });
    $(".gedit-file").mouseout(function () {
        $(this).hide();
    });
    $(".gedit-file-img").mouseout(function () {
        $(this).hide();
    });
    $(".user-file").mouseout(function () {
        $(this).hide();
    });

    //点击调用input file
    $(".gedit-file").click(function () {
        $("#GameFace").trigger("click");
    });
    $(".user-file").click(function () {
        $("#UserFace").trigger("click");
    });

    //临时显示图片
    $("#GameFace").change(function () {
        var objUrl = getObjectURL(this.files[0]);
        if (objUrl) {
            $(".gedit-img").attr("src", objUrl);
        }
    });
    $(".GameShot").each(function () {
        $(this).change(function () {
            var objUrl = getObjectURL(this.files[0]);
            if (objUrl) {
                $(this).prev().prev().attr("src", objUrl);
            }
        });
    });
    $("#UserFace").change(function () {
        var objUrl = getObjectURL(this.files[0]);
        if (objUrl) {
            $(".user-face").attr("src", objUrl);
        }
    });
    //建立存取临时图片的url
    function getObjectURL(file) {
        var url = null;
        if (window.createObjectURL !== undefined) { // 所有浏览器
            url = window.createObjectURL(file);
        } else if (window.URL !== undefined) { // 火狐浏览器
            url = window.URL.createObjectURL(file);
        } else if (window.webkitURL !== undefined) { // 谷歌
            url = window.webkitURL.createObjectURL(file);
        }
        return url;
    }
    //触发上传input
    $(".gedit-file-img").click(function () {
        $(this).next().trigger("click");
    });

    //表单验证
    var ok1 = false;  //名称
    var ok2 = false;  //简介
    var ok3 = false;  //图片
    var ok4 = false; //链接
    $("input[name='GameName']").blur(function () {
        if ($(this).val().length > 0) {
            ok1 = true;
            $(this).css("border-color", "");
        } else {
            $(this).css("border-color","#f67b7b");
            ok1 = false;
        }
    });
    $("textarea[name='GameDec']").blur(function () {
        if ($(this).val().length > 0) {
            ok2 = true;
            $(this).css("border-color", "");
        } else {
            $(this).css("border-color", "#f67b7b");
            ok2 = false;
        }
    });
    $("input[name='Link.LinkURL']").blur(function () {
        if ($(this).val().length > 0) {
            ok4 = true;
            $(this).css("border-color", "");
        } else {
            $(this).css("border-color", "#f67b7b");
            ok4 = false;
        }
    });
    //修改验证
    if ($("input[name='GameName']").val() !== "" && $("textarea[name='GameDec']").val() !== "" && $("input[name='Link.LinkURL']").val !== "") {
        ok1 = true;
        ok2 = true;
        ok4 = true;
    }
    $(".gedit-submit").click(function () {
        if (ok1 && ok2 && ok4) {
            return true;
        } 
        else {
            alert("输入框有没填的哦~检查检查！");
            return false;
        }
    });
    //添加验证
    $(".gadd-submit").click(function () {
        //图片是否添加验证
        if ($(".addShot:even").val() === "" || $(".addShot:odd").val() === "") {
            ok3 = false;
        }
        else {
            ok3 = true;
        }
        if (ok1 && ok2 && ok3 && ok4) {
            return true;
        }
        else {
            alert("图片或是输入框没填~检查检查！");
            return false;
        }
    });
    //游戏类型表单验证
    var typeOk = false; //类型名称
    $("input[name='TypeName']").val() !== "" ? typeOk = true : typeOk = false;
    $("input[name='TypeName']").blur(function () {
        if ($(this).val().length > 0) {
            typeOk = true;
            $(this).css("border-color", "");
        } else {
            $(this).css("border-color", "#f67b7b");
            typeOk = false;
        }
    });
    $(".type-submit").click(function () {
        if (typeOk) {
            return true;
        } else {
            alert("类型名称没有填入哦！");
            return false;
        }
    });
    //用户添加修改表单验证
    var userok1 = false;    //密码验证
    var userok2 = false;    //邮箱是否没填
    var userok3 = false;    //账号验证
    var userok4 = false;    //头像验证
    $("input[name='UserUID']").blur(function () {
        if ($(this).val() !== "") {
            userok3 = true;
        } else {
            userok3 = false;
        }
    });
$("input[name='UserPWD']").blur(function () {
        if ($(this).val().length < 6) {
            $(this).css("border-color", "#f67b7b");
            $(this).focus();
        } else {
            $(this).css("border-color", "");
        }
    });
$("input[name='pwd2']").blur(function () {
        if ($(this).val() !== $("#UserPWD").val()) {
            $(this).css("border-color", "#f67b7b");
            $(this).focus();
        } else {
            $(this).css("border-color", "");
        }
    });
    //设置输入用户名只能输入字母和数字和邮箱输入验证
    $("#UserUID").keyup(function () {
        this.value = this.value.replace(/[\W]/g, "");
    });
    $("input[name='UserEmail']").blur(function () {
        var rep = /^\w+@[a-z0-9]+\.[a-z]+$/i;
        if (rep.test($(this).val())) {
            $(this).css("border-color", "");
            userok2 = true;
            return true;
        } else {
            $(this).css("border-color", "#f67b7b");
            $(this).attr("placeholder", "邮箱格式不正确");
            $(this).focus();
            userok2 = false;
        }
    });
    $(".user-submit").click(function () {
        if ($("input[name='UserUID']").val() === "" || $("input[name='UserPWD']").val() === "" || $("input[name='pwd2']").val() === "" || $("input[name='UserEmail']").val() === "") {
            userok1 = false;
        } else {
            userok1 = true;
        }
        $(".userFace").val() === "" ? userok4 = false : userok4 = true;
        if (!userok1 && !userok2 && !userok3) {
            alert("添加失败，请检查~");
            return false;
        } else if (!userok4) {
            alert("头像未添加");
            return false;
        }
        else {
            return true;
        }
    });
    //用户类型添加修改表单验证
    var utype1 = false;
    var utype2 = false;
    $("input[name='GroupName']").val() !== "" ? utype1 = true : utype1 = false;
    $("input[name='GroupDec']").val() !== "" ? utype2 = true : utype2 = false;
    $("input[name='GroupName']").blur(function () {
        if ($(this).val().length > 0) {
            utype1 = true;
            $(this).css("border-color", "");
        } else {
            $(this).css("border-color", "#f67b7b");
            utype1 = false;
        }
    });
    $("input[name='GroupDec']").blur(function () {
        if ($(this).val().length > 0) {
            utype2 = true;
            $(this).css("border-color", "");
        } else {
            $(this).css("border-color", "#f67b7b");
            utype2 = false;
        }
    });
    $(".utype-submit").click(function () {
        if (utype1 && utype2) {
            return true;
        } else {
            alert("存在没有填入的哦！");
            return false;
        }
    });
    //公告表单验证
    var noticeOk1 = false; //标题名称
    var noticeOk2 = false; //内容
    $("input[name='NoticeTitle']").val() !== "" ? noticeOk1 = true : typeOk1 = false;
    $("textarea[name='NoticeContent']").val() !== "" ? noticeOk2 = true : typeOk2 = false;
    $("input[name='NoticeTitle']").blur(function () {
        if ($(this).val().length > 0) {
            noticeOk1 = true;
            $(this).css("border-color", "");
        } else {
            $(this).css("border-color", "#f67b7b");
            noticeOk1 = false;
        }
    });
    $("textarea[name='NoticeContent']").blur(function () {
        if ($(this).val().length > 0) {
            noticeOk2 = true;
            $(this).css("border-color", "");
        } else {
            $(this).css("border-color", "#f67b7b");
            noticeOk2 = false;
        }
    });
    $(".notice-submit").click(function () {
        if (noticeOk1 && noticeOk2) {
            return true;
        } else {
            alert("存在没有填入的哦！~请检查！");
            return false;
        }
    });
});