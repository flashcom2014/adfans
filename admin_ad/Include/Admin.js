$(function () {
    //$(".menu_title").css({ background: "url(images/title_bg_show.gif)" });
    $(".menu_title a").click(function (e) {
        e.stopPropagation();
    })
    $(".menu_title").click(function () {
        if (!$(this).parent().parent().find(".sec_menu").is(':visible')) {
            $(this).css({ background: "url(images/title_bg_show.gif)" });
        } else {
            $(this).css({ background: "url(images/title_bg_quit.gif)" });
        }
        $(this).parent().next().find(".sec_menu").slideToggle("fast");
    });
    $(".menu_right").click(function () {
        var t = $(this).attr("style");
        if (t.indexOf("m_hidden.png") > -1) {
            $(this).css({ background: "url(images/m_show.png) #bebfd9 left center no-repeat" });
            $(window.parent.document).find("#main").children().eq(0).attr("scrolling", "no");
            $(window.parent.document).find("#main").attr("cols", "8,*");
        } else {
            $(this).css({ background: "url(images/m_hidden.png) #bebfd9  left center no-repeat" });
            $(window.parent.document).find("#main").children().eq(0).attr("scrolling", "auto");
            $(window.parent.document).find("#main").attr("cols", "185,*");
        }

    });
})