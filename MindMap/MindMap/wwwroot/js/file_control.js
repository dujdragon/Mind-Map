var btn_open_folder = document.getElementById("#btn_add_folder");
var foldername;
var filename;
jQuery(document).ready(function () {
    $('#dialog_folder').hide();
    $('#dialog_file').hide();

    var username = getUrlParam("username");
    $(document).on("click", "#btn_add_folder", function (e) {
        console.log(username);
        $("#dialog_folder").dialog({
            draggalbe: true,
            minWidth: 400,
            buttons: {
                "OK": function () {
                    foldername = $('#folderSender').val();
                    var oList = document.querySelectorAll('.list h2');
                  
                    var folder = "<li class=\"list\"><h2><i></i>" + foldername + "</h2><div class=\"hide\"><div class=\"item" + oList.length + "\"" + "></div></div></div></li>";
                    $(".nav ul").append(folder);
                    initLogic();
                    $.ajax({
                        type: "post",
                        url: "/File/CreateFile",
                        contentType: 'application/json;charset=UTF-8',
                        dataType: "text",
                        data: JSON.stringify({
                            "username": username,
                            "filepath": "\\",
                            "filename": foldername,
                            "type" : 0
                        }),
                        success: function (data, status) {
                            if (status == "success") {
                                if (data == "existed") {
                                   console.log("创建成功")
                                } 
                            }
                        }
                    });
                    $(this).dialog("destroy");

                },
                "cancel": function () {
                    $(this).dialog("destroy");
                }
            }
        });
    });
    $(document).on("click", "#btn_add_file", function (e) {
        $("#dialog_file").dialog({
            draggalbe: true,
            minWidth: 400,
            buttons: {
                "OK": function () {
                    filename = $('#fileSender').val();
                    var path = $(".nav .list #selected").text();
                    var file = "<h5>" + filename + "</h5>";
                    $(".nav ul #selected").next().children("div").append(file);
                    initLogic();
                    $.ajax({
                        type: "post",
                        url: "/File/CreateFile",
                        contentType: 'application/json;charset=UTF-8',
                        dataType: "text",
                        data: JSON.stringify({
                            "username": username,
                            "filepath": path,
                            "filename": filename,
                            "type": 1
                        }),
                        success: function (data, status) {
                            if (status == "success") {
                                if (data == "existed") {
                                    console.log("创建成功")
                                }
                            }
                        }
                    });
                    $(this).dialog("destroy");

                },
                "cancel": function () {
                    $(this).dialog("destroy");
                }
            }
        });
    });
    //获取url中的参数
    function getUrlParam(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg);  //匹配目标参数
        if (r != null) return unescape(r[2]); return null; //返回参数值
    }
});