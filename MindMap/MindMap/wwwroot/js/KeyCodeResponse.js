jQuery(document).ready(function () {
    $(document).keydown(function (e) {
        var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
        if (e.altKey && eCode == 38) {
            move_to_first();
        } else if (e.altKey && eCode == 40) {
            move_to_last();
        } else if (eCode == 27) {
            var oldMMW = $(".MindMap").width();
            $(".MindMap").width(oldMMW + Number($(window).width()) * 0.45);
            if ($(".pege_open").css("display") != 'none') {
                $(".page_open").css("display", 'none');
                $(".page_open iframe").attr("src", "none")

            }
        } else if (e.ctrlKey && eCode == 13) {
            var selected_node = _jm.get_selected_node();
            if (!!selected_node) {
                var source = selected_node.topic;
                if ($("#hideWrap").attr("class") == "clicked") {
                    $(".MindMap").width("55%");
                } else {
                    $(".MindMap").width("45%");
                }
                $(".page_open").css("display", "inline");
                $(".page_open iframe").attr("src", source);
            }
        } else if (e.ctrlKey && eCode == 83) {
            // 文件保存
            e.preventDefault();
            $(".MindMap #save_file").click();
        } else if (e.altKey && eCode == 39) {
            // 一键展开
            expand_all();
        } else if (e.altKey && eCode == 37) {
            // 一键收回
            collapse_all();
        } else if (e.ctrlKey && eCode == 39) {
            // 展开子节点
            expand();
        } else if (e.ctrlKey && eCode == 37) {
            // 收回子节点
            collapse();
        }
 });
});