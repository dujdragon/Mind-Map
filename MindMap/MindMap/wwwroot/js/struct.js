jQuery(document).ready(function () {
    $(".nav ul").attr("cursor", "pointer");
    $(".nav .list i").attr("cursor", "pointer");
    // TODO
    var username = getUrlParam("username");
    function initPage() {
        open_empty();
        $(".toolbar #toolbar_username").text("感谢使用  "+ username);
        $(".toolbar .selected").css("color", "#3389d4").css("fontSize", 25);
        // 初始化文件系统
        $.ajax({
            type: "get",
            url: "/File/GetAllFolders",
            data: {
                "username": username,
            },
            success: function (data, status) {
                if (status == "success") {
                    folderList = data.split(" ");
                    var i = 0;
                    for (i; i < folderList.length - 1; i++) {
                        console.log(folderList[i]);
                        if (folderList[i] == " ") return;
                        addFolderInPage(folderList[i]);
                        $(".nav .list").eq(i).attr("id", "selected");
                        $.ajax({
                            type: "get",
                            url: "/File/GetAllFile",
                            data: {
                                "username": username,
                                "pathname": folderList[i]
                            },
                            success: function (data, status) {
                                if (status == "success") {
                                    fileList = data.split(" ");
                                    for (j = 0; j < fileList.length - 1; j++) {
                                        addFileInPage(fileList[j]);
                                    }
                                    $(".nav .list").eq(i).attr("id", "unselected");
                                    console.log(i);
                                }
                            }
                        });
                    }
                }
            }
        });
    }

    // 页面上增加文件夹
    function addFolderInPage(e) {
        var pos = $(".nav ul");
        var folderStr = "<li class='list' id='unselected'><h2><i></i>"+e+"</h2><div class='hide'><div class='item'></div></div></li>"
        pos.append(folderStr);
    }

    // 页面上增加文件
    function addFileInPage(e) {
        var pos = $(".nav #selected .item");
        // 将正在打开的文件设置为unchosen
        $(".nav #selected .item #chosen").attr("id", "unchosen");
        var fileStr = "<h5 id='chosen'>"+ e + "</h5>"
        pos.append(fileStr);

        var length = $(".nav #selected .hide").height();
        $(".nav #selected .hide").height(length + Number("41"));
    }

    // 从url中获得用户名
    function getUrlParam(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); 
        var r = window.location.search.substr(1).match(reg);  
        if (r != null) return unescape(r[2]); return null; 
    }

         
    // 文件夹点击效果
    $(document).on("click", ".nav .list h2", function (e) {
        var selected = $("#selected");
        if (selected.length > 0 && $(this).parent().attr("id") != "selected") 
        {
            $("#selected .hide").height(0);
            $("#selected .item #chosen").attr("id", "unchosen");
            $(".nav #selected i").attr("class", "");
            selected.attr("id", "unselected");
        }
        if ($(this).parent().attr("id") == "unselected") 
        {
            $(this).parent().attr("id", "selected"); 
            $(".nav #selected i").attr("class", "on");
            $("#selected .hide").height(($("#selected .hide").length + 1) * 41 + "px"); 
        } else {
            $("#selected .hide").height(0);
            $("#selected .item #chosen").attr("id", "unchosen");
            $(".nav #selected i").attr("class", "");
            $(this).parent().attr("id", "unselected");
        }
    });
    // 文件点击效果
    $(document).on("click", ".nav .list .item h5", function () {
        var chosen = $(".nav #selected #chosen");
        // 如果点击的自己不是chosen 则变成chosen并把其他chosen变成unchosen
        if ($(this).attr("id") != "chosen") {
            $(this).attr("id", "chosen");
            chosen.attr("id", "unchosen");
            var filename = $(this).text();
            var filepath = $(".nav #selected h2").text();
            // 然后根据文件名打开文件
            open_file_from_server(filename, filepath);
        }
    });

    /// 对话框操作
    // 文件夹的添加操作
    $(document).on("click", "#btn_add_folder",function showDialog(){
        $(".dialog-content #dialog_folder").dialog({
          closable: false,
          height:300,
          width:200,
          modal:true,
          draggalbe:true,
          minWidth:400,
          buttons:{
            "确认":function(){
                var foldername = $('#folderSender').val();
                if (foldername=="") 
                {
                    $(this).dialog("destroy");
                    return
                }
                // Success: 服务器创建对应文件夹
                $.ajax({
                    type: "post",
                    url: "/File/CreateFile",
                    contentType: 'application/json;charset=UTF-8',
                    dataType: "text",
                    data: JSON.stringify({
                        "username": username,
                        "filename": foldername,
                        "type": 1,
                        "filepath": "/",
                        "tag":"none"
                    }),
                    success: function (data, status) {
                        if (status == "success") {
                            if (data == "samename") {
                                alert("不能创建相同名的文件夹");
                            } else {
                                console.log("文件夹" + foldername + "创建成功!");
                                addFolderInPage(foldername);
                            }
                        }
                    }
                });
                $(this).dialog("destroy");
            },
            "取消":function(){
                $(this).dialog("destroy");
            }
          }
        });
    });
    // 文件的添加操作
    $(document).on("click", "#btn_add_file",function showDialog(){
        $(".dialog-content #dialog_file").dialog({
          closable: false,
          height:300,
          width:200,
          modal:true,
          draggalbe:true,
          minWidth:400,
          buttons:{
            "确认":function(){
                var filename = $('#fileSender').val();
                var tag = $("#tagSender").val();
                if (tag == "") tag = "none";
                var folderN = $(".nav #selected h2").text();
                if (filename=="") 
                {
                    $(this).dialog("destroy");
                    return
                }
                // TODO: 服务器创在文件夹创建对应文件
                $.ajax({
                    type: "post",
                    url: "/File/CreateFile",
                    contentType: 'application/json;charset=UTF-8',
                    dataType: "text",
                    data: JSON.stringify({
                        "username": username,
                        "filename": filename,
                        "type": 0,
                        "filepath": folderN,
                        "tag": tag
                    }),
                    success: function (data, status) {
                        if (status == "success") {
                            if (data == "samename") {
                                alert("同一目录下不能创建相同名的文件");
                            } else {
                                console.log("文件" + filename + "创建成功!");
                                addFileInPage(filename);
                                add_new_mind(username, filename);
                                save_file_in_server();
                            }
                        }
                    }
                });
                $(this).dialog("destroy");
            },
            "取消":function(){
                $(this).dialog("destroy");
            }
          }
        });
    });

    // 帮助窗口打开
    $(document).on("click", "#toolbar_help", function(){
        $(".dialog-content #dialog_help").dialog({
            closeOnEscape: false,
            height:500,
            width:200,
            modal:true,
            draggalbe:true,
            minWidth:400,
            buttons:{
              "确认":function(){
                  $(this).dialog("destroy");
              }
            }
          });
    });

    
    // 上方菜单栏点击事件
    var oldcolor = "";
    $(".toolbar h3").hover(function(){
        oldcolor = $(this).css("background");
        if ($(this).attr("id") == "toolbar_username") return;
        // if ($(this).attr("id") == "toolbar_help") return;
        $(this).css('background', '#6CF3C2');
    }, function() {
        $(this).css('background', oldcolor);
    });

    $(document).on("click", ".toolbar h3" ,function(){
        if ($(this).attr("id") == "toolbar_username") return;
        if ($(this).attr("id") == "toolbar_help") return;
        if ($(this).attr("class") != "selected") {
            $(".toolbar .selected").css("color", "#000").css("fontSize", "1.17em")
            $(".toolbar .selected").attr("class", "unselected");
            $(this).attr("class", "selected");
        }
        $(".toolbar .selected").css("color", "#3389d4").css("fontSize", 25);
    });

    function add_new_mind(username, filename) {
        var mind = {
            "meta": {
                "name": filename,
                "author": username,
                "version": "1.0"
            },
            "format": "node_tree",
            "data": {
                "id": "root", "topic": "NewNode", "children": [
                ]
            }
        }
        _jm.show(mind);
    }

    $(".MindMap #save_file").click(save_file_in_server);

    // 文件的保存
    function save_file_in_server() {
        $(".MindMap #save_file_state").text("保存中...");
        var mind_data = _jm.get_data();
        var filename = mind_data.meta.name;
        var mind_str = jsMind.util.json.json2string(mind_data);
        var foldername = $(".nav #selected h2").text();
        $.ajax({
            type: "post",
            url: "/File/SaveFile",
            contentType: 'application/json;charset=UTF-8',
            dataType: "text",
            data: JSON.stringify({
                "data": mind_str,
                "filename": filename,
                "username": username,
                "foldername":foldername
            }),
            success: function (data, status) {
                if (status == "success") {
                    if (data == "success") {
                        console.log("保存成功")
                        $(".MindMap #save_file_state").text("保存成功");
                    }
                }
            }
        });
    }

    // 文件的获取
    function open_file_from_server(filename, filepath) {
        $.ajax({
            type: "get",
            url: "/File/GetFileByName",
            data: {
                "username": username,
                "filename": filename,
                "filepath": filepath
            },
            success: function (data, status) {
                if (status == "success") {
                    var mind = jsMind.util.json.string2json(data);
                    if (!!mind) {
                        _jm.show(mind);
                    } else {
                        prompt_info('can not open this file as mindmap');
                    }
                }
            }

        });
    }
    initPage();

});
