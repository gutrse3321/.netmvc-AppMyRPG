$(function () {
    //获取可视高度和宽度
    var mwidth = $(window).width();
    var mheight = $(window).height();

    //游戏简介显示
    $(".item-dec-cont").hide();
    $(".item-dec img").each(function () {
        $(this).mouseover(function () {
            $(this).next().fadeIn();
        });
        $(this).next().mouseout(function() {
            $(this).fadeOut();
         });
    });
    //下拉框点击显示事件
    var toggleNum = 0;
    $(".downlist-btn").click(function () {
        if (toggleNum === 0) {
            $(this).next().show();
            toggleNum = 1;
        } else {
            $(this).next().hide();
            toggleNum = 0;
        }
    });
    //滑条滑动事件
    $(window).scroll(function () {
        if ($(window).scrollTop() > 500) {
            $("#toTop").fadeIn();
        } else {
            $("#toTop").fadeOut();
        }
    });
    //回到顶部
    $("#toTop").click(function () {
        $("html,body").animate({
            "scrollTop": 0
        }, 200);
    });

    //用户信息选项卡选中样式
    $(".detail-show-left a:first").css("background", "#ccc");
    $(".detail-show-left a").each(function () {
        $(this).click(function () {
            $(this).css("background", "#ccc").siblings().css("background", "");
        });
    });

    //临时显示图片
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
    $(".user-file").click(function () {
        $(this).next().trigger("click");
    });

    //验证
    //评论框验证
    $(".cmt-reg").click(function () {
        if ($(".cmt-textarea").val() === "") {
            alert("未填入任何东西哦！");
            $(".cmt-textarea").focus();
            return false;
        } else if ($(".cmt-textarea").val().length < 10) {
            alert("字数小于10位~");
            return false;
        }
        else {
            return true;
        }
    });

    //注册验证
    var regok1 = false;     //用户名
    var regok2 = false;     //密码
    var regok3 = false;     //邮箱
    var regok4 = false;     //头像
    $("input[name='UserUID']").blur(function () {
        if ($(this).val() !== "") {
            regok1 = true;
        } else {
            regok1 = false;
        }
    });
    $("input[name='UserPWD']").blur(function () {
        if ($(this).val().length < 6) {
            $(this).css("border-color", "#f67b7b");
            regok2 = false;
        } else {
            $(this).css("border-color", "");
            regok2 = true;
        }
    });
    $("input[name='pwd2']").blur(function () {
        if ($(this).val() !== $("#UserPWD").val()) {
            $(this).css("border-color", "#f67b7b");
        } else {
            $(this).css("border-color", "");
        }
    });
    //邮箱输入验证
    $("input[name='UserEmail']").blur(function () {
        var rep = /^\w+@[a-z0-9]+\.[a-z]+$/i;
        if (rep.test($(this).val())) {
            $(this).css("border-color", "");
            regok3 = true;
            return true;
        } else {
            $(this).css("border-color", "#f67b7b");
            $(this).attr("placeholder", "邮箱格式不正确");
            $(this).focus();
            regok3 = false;
        }
    });
    $(".reg-submit").click(function () {
        if ($("input[name='UserUID']").val() === "" || $("input[name='UserPWD']").val() === "" || $("input[name='pwd2']").val() === "" || $("input[name='UserEmail']").val() === "") {
            regok1 = false;
        } else {
            regok1 = true;
        }
        $(".userFace").val() === "" ? regok4 = false : regok4 = true;
        if (!regok1 && !regok2 && !regok3) {
            alert("注册失败，请检查~");
            return false;
        } else if (!regok4) {
            alert("头像未添加");
            return false;
        }
        else {
            return true;
        }
    });
    //重设密码验证
    var newok = false;  //验证密码
    $("input[name='pwd']").blur(function () {
        if ($(this).val() !== $("input[name='UserPWD']").val()) {
            $(this).css("border-color", "#f67b7b");
            newok = false;
        } else {
            $(this).css("border-color", "");
            newok = true;
        }
    });
    $(".new-btn").click(function () {
        if ($("input[name='pwd']").val() === "") {
            newok = false;
        }
        if (newok) {
            alert("密码重设成功");
            return true;
        } else {
            alert("密码不能为空或密码不正确");
            return false;
        }
    });
});