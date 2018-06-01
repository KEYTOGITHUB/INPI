//jsrender 扩展
var jsrenderPagingLang = {
    'zh-cn': {
        pageSize: '每页显示：',
        nodata: '暂无数据',
        gopage: '去第',
        page: '页',
        jump: 'GO',
        total: '共',
        record: '条'
    },
    'en-us': {
        pageSize: 'Per Page:',
        nodata: 'There is no data.',
        gopage: 'goto',
        page: '',
        jump: 'Goto',
        total: 'Total ',
        record: ''
    }
};
var currentLanguage = jsrenderPagingLang["zh-cn"];
var currentPageIndex = 0;
var currentPgaeSize = 0;

(function ($) {
    var _jsRenderSetting = {
        isPage: true, //是否分页
        pageIndex: 1,//请求的页数
        pageSize: 10,//每页的个数
        templateID: null,//自定义模板
        parameter: null,
        pagingCallback: null,
        jsRenderSortField: '',//默认排序字段
        jsRenderASC: 'DESC',//默认排序方式
        recordCount: 0,//数据的总数
        sourceUrl: '',//请求的数据源连接
        funCallback: null,//回调函数
        dataSource: null,//定义数据源
        funBefore: null,//数据获取之前
        pagetype: 0,//分页状态 0 后台分页 1 学生端前台 2 教师端分页 3 排行榜分页
        isNodata: true,//是否显示 暂无数据 默认显示
        isShowNodataImage: true,//是否显示 暂无数据 默认显示
    };


    var getUrlCookieKey = function (url) {
        var urlh = url.split('?')[0].split('#')[0];
        return urlh.getHashCode();
    };

    //返回指定长度截取的字符串 add by yxt 主要用于jsrender
    $.views.helpers({
        ShowLimitString: function (str, len) {
            if (str == undefined || str == null || str == "") {
                return "--";
            }
            if (len == undefined || len == null) {
                return str;
            }
            return str.length > len ? (str.substring(0, len) + "…") : str;
        },
        ShowLimitString2: function (str, len) {
            if (str == undefined || str == null || str == "") {
                return "--";
            }
            if (len == undefined || len == null) {
                return str;
            }
            return str.length > len ? (str.substring(0, len)) : str;
        },
        parseInt: function (str, redix) {
            if (str == undefined || str == null || str == "") {
                return 0;
            }
            if (redix == undefined || redix == null) {
                redix = 10;
            }
            return parseInt(str, redix);
        },
        fromCharCode: function (i) {

            return String.fromCharCode((64 + i));
        },
        ShowDecimal: function (i) {
            //保留两位有效数字
            if (i == undefined || i == null || i == "") {
                return "--";
            }
            i = i + "";//转换成字符串
            var charList = i.split('.');
            if (charList.length > 2) {
                return "--";
            }
            if (charList.length == 1) {
                return i + ".00";
            }
            if (charList.length == 2) {
                if (charList[1].length == 0) {
                    return charList[0] + ".00";
                }
                else if (charList[1].length == 1) {
                    return charList[0] + "." + charList[1] + "0";
                }
                else if (charList[1].length == 2) {
                    return charList[0] + "." + charList[1];
                }
                else {
                    var num = new Number(i);

                    return num.toFixed(2);
                }
            }
        },
        GetIndex: function () {
            return this.index + 1 + (currentPageIndex - 1) * currentPgaeSize;
        },
        IfNull: function (str, replacestr) {
            if (replacestr == undefined || replacestr == null) {
                return "暂无";
            }
            if (str == undefined || str == null || str == "") {
                return replacestr;
            }
            return str;
        }
    });

    $.fn.JsRenderData = function (args) {
        //  debugger;
        var setting = $.extend({}, _jsRenderSetting, args);

        //  if (setting.pageSize < 10 && (!window.isDialog)) setting.pageSize = 10;

        if ($.cookie) {
            var cookieKey = setting.templateID + "_" + getUrlCookieKey(window.location.href);
            var pageSize = $.cookie(cookieKey);
            if (pageSize)
                setting.pageSize = pageSize;
            else
                $.cookie(cookieKey, setting.pageSize);
        }

        var me = this;
        if (me.length == 0) {
            return me;
        }
        if (setting.sourceUrl) {
            //url请求
            if (setting.isPage) {
                if (setting.sourceUrl.indexOf("?") == -1) {
                    setting.sourceUrl += "?";
                    setting.sourceUrl += "pageIndex=" + setting.pageIndex + "&pageSize=" + setting.pageSize;
                } else {
                    setting.sourceUrl = setting.sourceUrl.replace(/&?(pageIndex|pageSize|_)=([^&]*)/gi, ""); //过滤url的翻页参数
                    setting.sourceUrl += "&pageIndex=" + setting.pageIndex + "&pageSize=" + setting.pageSize;
                }

                currentPageIndex = setting.pageIndex;
                currentPgaeSize = setting.pageSize;
            }
            if (setting.jsRenderSortField != null && setting.jsRenderSortField != undefined && setting.jsRenderSortField != "") {
                setting.sourceUrl += "&jsRenderSortField=" + setting.jsRenderSortField + (setting.jsRenderASC.indexOf("ASC") >= 0 ? " ASC " : " DESC ");
            }

            if (setting.parameter == null) {
                $.getJSON(setting.sourceUrl, function (data) {
                    //debugger;
                    setting.dataSource = data;
                    setting.recordCount = data.recordCount;
                    if (setting.funBefore != null && setting.funBefore != undefined)
                        setting.funBefore(data);
                    if (setting.pagetype == 0) {
                        if ($(me)[0].tagName == "DIV") {
                            if ($(me).find("#listbody").length == 0) {
                                $(me).append("<div id='listbody' class='ln-pic-list fn-clear'></div>");
                            }
                            if ($(me).find("#list-page").length == 0) {
                                $(me).append("<div id='list-page' class='fn-clear'></div>");
                            }
                        } else {
                            $(me).parent().find("tfoot").html("<tr><td colspan='" + $(me).parent().find("thead tr th").length + "'><div id='list-page' class='fn-clear'></div></td></tr>");
                        }
                    }

                    me.JsRenderBody(me, setting);
                    me.JsRenderPaging(me, setting);

                    fnTableBindSort(me, setting);
                    //排序标志
                    $(me).parent().find("thead th").each(function () {
                        var sort = $(this).attr("jsRenderSortField");
                        if (sort) {
                            $(this).css("cursor", "pointer");

                            $(this).removeClass("sorting_desc");
                            $(this).removeClass("sorting_asc");
                            $(this).removeClass("sorting");

                            $(this).addClass("sorting");
                            if (setting.sourceUrl.indexOf("jsRenderSortField=" + sort) >= 0) {
                                $(this).removeClass("sorting");
                                $(this).addClass("sorting_" + (setting.sourceUrl.toLocaleLowerCase().indexOf(" asc") > 0 ? "asc" : "desc"));
                            }
                        }
                    });
                    if (setting.funCallback != null && setting.funCallback != undefined) {
                        //alert(JSON.stringify(data));
                        setting.funCallback(data, setting.pageIndex, setting.pageSize);
                    }

                });
            } else {
                $.post(setting.sourceUrl, setting.parameter, function (data) {
                    setting.dataSource = data;
                    if ($(me).parent().find("tbody").length == 0) {
                        if ($(me).find("#listbody").length == 0) {
                            $(me).append("<div id='listbody' class='ln-pic-list fn-clear'></div>");
                        }
                        if ($(me).find("#list-page").length == 0) {
                            $(me).append("<div id='list-page' class='fn-clear'></div>");
                        }
                    } else {
                        $(me).parent().find("tfoot").html("<tr><td colspan='" + $(me).parent().find("thead tr th").length + "'><div id='list-page' class='fn-clear'></div></td></tr>");
                    }
                    me.JsRenderBody(me, setting);
                    me.JsRenderPaging(me, setting);

                    fnTableBindSort(me, setting);
                    //排序标志
                    $(me).parent().find("thead th").each(function () {
                        var sort = $(this).attr("jsRenderSortField");
                        if (sort) {
                            $(this).css("cursor", "pointer");

                            $(this).removeClass("sorting_desc");
                            $(this).removeClass("sorting_asc");
                            $(this).removeClass("sorting");

                            $(this).addClass("sorting");
                            if (setting.sourceUrl.indexOf("jsRenderSortField=" + sort) >= 0) {
                                $(this).removeClass("sorting");
                                $(this).addClass("sorting_" + (setting.sourceUrl.toLocaleLowerCase().indexOf(" asc") > 0 ? "asc" : "desc"));
                            }
                        }
                    });
                    if (setting.funCallback != null && setting.funCallback != undefined)
                        setting.funCallback(data, setting.pageIndex, setting.pageSize);
                });
            }
        } else {
            if (setting.dataSource != null && setting.dataSource != undefined) {

                if ($(me).parent().find("tbody").length == 0)
                    $(me).html("<div id='listbody'></div><div id='list-page' class='fn-clear'></div>");
                me.JsRenderBody(me, setting);
                me.JsRenderPaging(me, setting);
                if (setting.funCallback != null && setting.funCallback != undefined) {

                    setting.funCallback(setting.dataSource, setting.pageIndex, setting.pageSize);
                }

            }
        }
        return me;
    };
    //生成主体部分
    $.fn.JsRenderBody = function (obj, setting) {
        //debugger;
        if (setting.dataSource.dataList.length > 0) {
            if ($(obj)[0].tagName == "DIV" && setting.pagetype == 0) {
                $(obj).find("#listbody").eq(0).html($("#" + setting.templateID).render(setting.dataSource.dataList));
                //鼠标悬浮事件
                $(obj).find("div#listbody").eq(0).append('<div class="clear"></div>');
                //$(obj).find("ul#listbody").eq(0).append('<li class="clear"></li>');
                $(obj).find("#listbody dl").bind("mouseover", function () {
                    $(this).find(".Doing").show();
                }).bind("mouseleave", function () {
                    $(this).find(".Doing").hide();
                });
            } else {
                $(obj).html($("#" + setting.templateID).render(setting.dataSource.dataList));
            }
        } else {
            if (setting.pagetype == 0) {
                if ($(obj)[0].tagName == "DIV") {
                    if (setting.isNodata) {
                        $(obj).find("#listbody").eq(0).html("<div class='tc c38 line_h30'>" + currentLanguage.nodata + "</div>");
                    }
                } else {
                    if (setting.isNodata) {
                        var num = $(obj).parent().find("thead tr th").length;
                        var num2 = 0;
                        $(obj).parent().find("thead tr th[colspan]").each(function () {
                            num2 += parseInt($(this).attr("colspan")) - 1;
                        });
                        num += num2;

                        $(obj).parent().find("tbody").eq(0).html("<tr><td colspan='" + num + "'><div class='tc c38 line_h30'>" + currentLanguage.nodata + "</div></td></tr>");

                    }

                }
            }
            else {
                if (setting.isNodata) {
                    if (setting.pagetype == 2) {
                        if ($(obj)[0].tagName == "DIV") {
                            if (setting.isShowNodataImage) {
                                $(obj).html("<div class='tc c38 line_h30 list-none'><img src='../../themes/newteacher/img/none.jpg' /></div>");
                            }
                            else {
                                $(obj).html("<div class='tc c38 line_h30 list-none'><p>" + currentLanguage.nodata + "</p></div>");
                            }
                        }
                        else {
                            var num = $(obj).parent().find("thead tr th").length;
                            var num2 = 0;
                            $(obj).parent().find("thead tr th[colspan]").each(function () {
                                num2 += parseInt($(this).attr("colspan")) - 1;
                            });
                            num += num2;
                            if (setting.isShowNodataImage) {
                                $(obj).html("<tr><td colspan='" + num + "'><div style='text-align: center;'><img src='../../themes/newteacher/img/none.jpg' /></div></td></tr>");
                            }
                            else {
                                $(obj).html("<tr><td colspan='" + num + "'><div style='text-align: center;'>" + currentLanguage.nodata + "</div></td></tr>");
                            }

                        }
                    }
                    else if (setting.pagetype == 1) {
                        if ($(obj)[0].tagName == "DIV") {
                            //debugger;
                            //if ($(obj).parent().find(".pages-turning").length == 1)
                            //{
                            //    $(obj).html("<div class='noinformation'></div>");
                            //}
                            //else
                            //{
                            if (setting.isShowNodataImage) {
                                $(obj).html("<div class='noinformation' style='text-align: center;'></div>");
                            }
                            else {
                                $(obj).html("<div style='text-align: center;'>" + currentLanguage.nodata + "</div>");
                            }
                            // }
                        }
                        else {
                            var num = $(obj).parent().find("thead tr th").length;
                            var num2 = 0;
                            $(obj).parent().find("thead tr th[colspan]").each(function () {
                                num2 += parseInt($(this).attr("colspan")) - 1;
                            });
                            num += num2;
                            if (setting.isShowNodataImage) {
                                $(obj).parent().find("tbody").eq(0).html("<tr><td colspan='" + num + "'><div class='noinformation' style='text-align: center;'></div></td></tr>");
                            }
                            else {
                                $(obj).parent().find("tbody").eq(0).html("<tr><td colspan='" + num + "'><div style='text-align: center;'>" + currentLanguage.nodata + "</div></td></tr>");
                            }
                        }
                        // <tr><td colspan="7"><div class="noinformation"></div></td></tr>
                    }
                    else {
                        if (setting.isShowNodataImage) {
                            $(obj).html("<div class='noinformation' style='text-align: center;'></div>");
                        }
                        else {
                            $(obj).html("<div style='text-align: center;'>" + currentLanguage.nodata + "</div>");
                        }
                    }

                }
            }

        }
    };

    //公共的分页部分
    $.fn.PageHtml = function (recordCount, pageCount, pageSize, pageIndex, templateType) {
        var html = '';
        html += '<div class="num"><label>' + currentLanguage.pageSize + '</label><ul><li>10</li><li>20</li><li>' + (templateType == 0 ? 50 : 40) + '</li><li>100</li><li>200</li></ul></div>';
        pageCount = pageCount == 0 ? 1 : pageCount;
        if (parseInt(pageIndex) > pageCount)
            pageIndex = pageCount;

        //html += '<span class="c38">第' + pageIndex + '页/共' + pageCount + '页(共' + recordCount + '条)</span>';
        html += '<div class="jump">' + currentLanguage.gopage + '&nbsp;<input type="text" id="txtpagegoto" class="span2" maxlength="3" />&nbsp;' + currentLanguage.page + '&nbsp;&nbsp;<input type="button" value="' + currentLanguage.jump + '" class="btn btn-default" id="btnpagegoto" /></div>';

        if (parseInt(pageIndex) == 1) {
            //html += "<span class='first status_disabled'>&nbsp;首页&nbsp;</span>";
            html += "<div class='page fn-clear'><span class='previous status_disabled'><</span>";
        } else {
            //html += "<a index='first' class='first status-default' href=\"#1\">&nbsp;首页&nbsp;</a>";
            html += "<div class='page fn-clear'><a index='previous' class='previous' href=\"#" + (pageIndex - 1) + "\"><</a>";
        }
        if (true) {
            var maxIndex = 1; //当前要显示的最大索引
            if (pageCount <= 5)
                maxIndex = pageCount;
            else if (parseInt(pageIndex) + 2 <= 5)
                maxIndex = 5;
            else if (parseInt(pageIndex) + 2 >= pageCount)
                maxIndex = pageCount;
            else
                maxIndex = parseInt(pageIndex) + 2;

            for (var i = 4; i >= 0; i--) {
                if (maxIndex - i == parseInt(pageIndex))
                    html += "<span class='status_disabled status-on'>&nbsp;" + (maxIndex - i) + "&nbsp;</span>";
                else if (maxIndex - i > 0)
                    html += "<a index='" + (maxIndex - i) + "' class='status-default' href='#" + (maxIndex - i) + "'>&nbsp;" + (maxIndex - i) + "&nbsp;</a>";
            }
        }
        if (pageCount == parseInt(pageIndex)) {
            html += "<span class='next status_disabled'>></span></div>";
            //html += "<span class='last status_disabled'>&nbsp;末页&nbsp;</span></div>";
        } else {
            html += "<a index='next' class='next status-default' href=\"#" + (parseInt(pageIndex, 10) + 1) + "\">></a></div>";
            //html += "<a index='last' class='last status-default' href=\"#" + pageCount + "\">&nbsp;末页&nbsp;</a></div>";
        }
        html += '<div class="total">' + currentLanguage.total + '<strong>' + recordCount + '</strong>' + currentLanguage.record + '</div>';
        return html;
    };

    //生成分页部分
    $.fn.JsRenderPaging = function (obj, setting) {
        if (!setting.isPage) return obj;
        setting.recordCount = parseInt(setting.dataSource.recordCount || 0);
        setting.pageCount = Math.ceil(setting.dataSource.recordCount / setting.pageSize);

        if (setting.pagetype == 0) {
            var templateType = 0; //模板的类型；0：列表；1：图片(每行4个)
            if ($(obj).parent().find("tfoot").length == 0) {
                templateType = 1;
            }

            var html = this.PageHtml(setting.recordCount, setting.pageCount, setting.pageSize, setting.pageIndex, templateType);
            if ($(obj).parent().find("tfoot").length == 0) {
                $(obj).find("#list-page").eq(0).show().html(html);
            } else {
                $(obj).parent().find("#list-page").eq(0).show().html(html);
            }


        }
        else if (setting.pagetype == 1) {

            var html = this.NewPageHtml(setting.recordCount, setting.pageCount, setting.pageSize, setting.pageIndex);
            $(obj).parent().parent().find(".pages-turning").html(html);
        }
        else if (setting.pagetype == 2) {
            //debugger;
            console.log($($(obj).parent().find(".pagination")));
            console.log(setting)
            var html = this.TeacherNewPageHtml(setting.recordCount, setting.pageCount, setting.pageSize, setting.pageIndex);
            $(obj).parent().find(".pagination").html(html);
            if ($(obj).parent().find(".pagination").length > 0) {
                $(obj).parent().find(".pagination").html(html);
            }
            else {
                $(obj).parent().next().find(".pagination").html(html);
            }

        }
        else {
            var html = this.NewPageHtmlForRank(setting.recordCount, setting.pageCount, setting.pageSize, setting.pageIndex);
            $(obj).parent().parent().find(".pages-turning").html(html);
        }
        //翻页需要存储数据到table的属性中
        this.setListPageLink(setting); //设置分页的链接

        this.setListGotoPageLink(setting); //设置'转向'分页的事件

        this.setPageSizeLink(setting);
        return this;
    };

    $.fn.setPageSizeLink = function (setting) {
        var me = this;
        $(me).parent().find(".num li").unbind("click").bind("click", function () {
            var lnk = $(this);
            var newPageSize = lnk.text();
            lnk.siblings().removeClass("active");
            lnk.addClass("active");
            setting.pageIndex = 1;
            setting.pageSize = newPageSize;
            if ($.cookie) {
                var cookieKey = setting.templateID + "_" + getUrlCookieKey(window.location.href);
                $.cookie(cookieKey, newPageSize);
            }
            me.JsRenderData(setting);
            if ($.isFunction(setting.pagingCallback)) {
                setting.pagingCallback(1, newPageSize);
            }
        });
        $(me).parent().find(".num li").each(function (idx, item) {
            if ($(item).text() == setting.pageSize) {
                $(item).addClass("active");
            }
        });
    };

    //设置分页连接
    $.fn.setListPageLink = function (setting) {
        var me = this;
        if (setting.pagetype == 0) {
            $(me).parent().find("#list-page a").unbind("click").click(function () {
                var thisPageIndex = $(this).attr("href");
                thisPageIndex = thisPageIndex.substring(thisPageIndex.lastIndexOf("#") + 1);
                setting.pageIndex = thisPageIndex;
                me.JsRenderData(setting);
                if ($.isFunction(setting.pagingCallback)) {
                    setting.pagingCallback(thisPageIndex, setting.pageSize);
                }
            });
        }
        else if (setting.pagetype == 1) {
            //debugger;
            $(me).parent().parent().find(".pages-turning a").unbind("click").click(function () {
                //  debugger;
                //  $(this).addClass(".active");
                var thisPageIndex = $(this).attr("href");
                thisPageIndex = thisPageIndex.substring(thisPageIndex.lastIndexOf("#") + 1);
                setting.pageIndex = thisPageIndex;
                me.JsRenderData(setting);
                //debugger;
                if ($.isFunction(setting.pagingCallback)) {
                    setting.pagingCallback(thisPageIndex, setting.pageSize);
                }
            });
        }
        else if (setting.pagetype == 2) {
            $(me).parent().parent().find(".pagination a").unbind("click").click(function () {
                var thisPageIndex = $(this).attr("href");
                thisPageIndex = thisPageIndex.substring(thisPageIndex.lastIndexOf("#") + 1);
                setting.pageIndex = thisPageIndex;



                me.JsRenderData(setting);
                if ($.isFunction(setting.pagingCallback)) {
                    setting.pagingCallback(thisPageIndex, setting.pageSize);
                }


            });
        }
        else if (setting.pagetype == 3) {
            $(me).parent().parent().find(".pages-turning a").unbind("click").click(function () {
                var thisPageIndex = $(this).attr("href");
                thisPageIndex = thisPageIndex.substring(thisPageIndex.lastIndexOf("#") + 1);
                setting.pageIndex = thisPageIndex;
                me.JsRenderData(setting);
                if ($.isFunction(setting.pagingCallback)) {
                    setting.pagingCallback(thisPageIndex, setting.pageSize);
                }
            });
        }
        return me;
    };

    //设置转向连接
    $.fn.setListGotoPageLink = function (setting) {
        var me = this;
        var o = $(me).parent().find("#list-page #txtpagegoto").eq(0);
        $(me).parent().find("#list-page #btnpagegoto").unbind("click").click(function () {
            if (o.val() != "") {
                var pattern = /^[0-9]+$/;
                if (pattern.test(o.val())) {
                    var thisGotoIndex = parseInt(o.val());
                    if (thisGotoIndex <= setting.pageCount && thisGotoIndex > 0) {
                        setting.pageIndex = thisGotoIndex;
                        me.JsRenderData(setting);
                        if ($.isFunction(setting.pagingCallback)) {
                            setting.pagingCallback(thisGotoIndex, setting.pageSize);
                        }
                    }
                }
            }
        });
        o.unbind('keypress').bind('keypress', function (e) {
            var keyCode = window.event ? event.keyCode : e.which;
            if (keyCode == 13) {
                //Enter键
                $(me).parent().find("#list-page #btnpagegoto").click();
            }
        });
        return me;
    };

    $.fn.NewPageHtml = function (recordCount, pageCount, pageSize, pageIndex) {

        var html = '';
        if (pageCount == 1) return html;
        if (parseInt(pageIndex) > 1) {
            // <a href="" class="prev dis"><i class="icon-angle-left"></i></a>
            //<a href="" class="number active">1</a>
            html += "<a href=\"#" + (parseInt(pageIndex) - 1) + ' " class="prev dis"><i class="icon-angle-left"></i></a>';

        }

        if (true) {

            var maxIndex = 1; //当前要显示的最大索引
            if (pageCount <= 5)
                maxIndex = pageCount;
            else if (parseInt(pageIndex) + 2 <= 5)
                maxIndex = 5;
            else if (parseInt(pageIndex) + 2 >= pageCount)
                maxIndex = pageCount;
            else
                maxIndex = parseInt(pageIndex) + 2;

            for (var i = 4; i >= 0; i--) {
                if (maxIndex - i == parseInt(pageIndex))
                    html += "<a href=\"#" + (maxIndex - i) + '" class="active">' + (maxIndex - i) + '</a>';
                else if (maxIndex - i > 0)
                    html += "<a href=\"#" + (maxIndex - i) + '">' + (maxIndex - i) + '</a>';
            }
        }
        if (pageIndex < pageCount) {
            // <a href="" class="next"><i class="icon-angle-right"></i></a>
            html += "<a href=\"#" + (parseInt(pageIndex) + 1) + ' " class="next"><i class="icon-angle-right"></i></a>';
        }

        return html;
    };

    $.fn.NewPageHtmlForRank = function (recordCount, pageCount, pageSize, pageIndex) {
        //debugger;
        var html = '';
        if (pageCount == 1) return html;
        if (parseInt(pageIndex) > 1) {
            html += "<a href=\"#" + (parseInt(pageIndex) - 1) + '" class="prev"><i class="icon-angle-left"></i></a>';
        }

        //小于9页
        if (parseInt(pageCount) <= 9) {
            for (var i = 1; i <= pageCount; i++) {
                html += ' <a href=\"#' + i + '" class="number ' + (parseInt(pageIndex) == i ? "active" : "") + '">' + i + '</a>';
            }
        }
        else {
            var startp = 1;
            var endp = 9;

            if (parseInt(pageIndex) % 9 == 1) {
                startp = parseInt(pageIndex);
            }
            else if (parseInt(pageIndex) % 9 == 0) {
                startp = parseInt(pageIndex) - 9 + 1;
            }
            else {
                startp = parseInt(pageIndex) - parseInt(pageIndex) % 9 + 1;
            }

            endp = startp + 8;

            if (endp > parseInt(pageCount)) {
                endp = parseInt(pageCount);
            }

            for (var i = startp; i <= endp; i++) {
                html += ' <a href=\"#' + i + '" class="number ' + (parseInt(pageIndex) == i ? "active" : "") + '">' + i + '</a>';

            }
        }

        if (pageIndex < pageCount) {

            html += '<a href=\"#' + (parseInt(pageIndex) + 1) + '" class="next"><i class="icon-angle-right"></i></a>';
        }

        return html;
    };

    $.fn.TeacherNewPageHtml = function (recordCount, pageCount, pageSize, pageIndex) {
        //debugger;
        var html = '';
        if (pageCount == 1) return html;
        if (parseInt(pageIndex) > 1) {
            html += "<li><a href=\"#" + (parseInt(pageIndex) - 1) + ' " aria-label="Previous"><span aria-hidden="true">«</span></a></li>';

        }

        if (true) {

            var maxIndex = 1; //当前要显示的最大索引
            if (pageCount <= 5)
                maxIndex = pageCount;
            else if (parseInt(pageIndex) + 2 <= 5)
                maxIndex = 5;
            else if (parseInt(pageIndex) + 2 >= pageCount)
                maxIndex = pageCount;
            else
                maxIndex = parseInt(pageIndex) + 2;

            for (var i = 4; i >= 0; i--) {
                if (maxIndex - i == parseInt(pageIndex))
                    html += "<li class='active'><a href=\"#" + (maxIndex - i) + '">' + (maxIndex - i) + '</a></li>';
                else if (maxIndex - i > 0)
                    html += "<li><a href=\"#" + (maxIndex - i) + '">' + (maxIndex - i) + '</a></li>';
            }
        }
        if (pageIndex < pageCount) {
            html += "<li><a href=\"#" + (parseInt(pageIndex) + 1) + '" aria-label="Next"><span aria-hidden="true">»</span></a></li>';
        }

        return html;
    };

    function fnTableBindSort(me, setting) {
        $(me).parent().find('th').each(function () {
            var sort = $(this).attr("jsRenderSortField");
            if (sort != null && sort != undefined) {
                $(this).attr("sort", $(this).attr("sort") == "asc" ? "desc" : "asc");

                $(this).unbind("click");
                $(this).bind("click", function () {
                    setting.sourceUrl = setting.sourceUrl.replace(/&?(jsRenderSortField|jsRenderASC)=([^&]*)/gi, ""); //过滤url的排序参数
                    setting.sourceUrl += "&jsRenderSortField=" + sort + ' ' + $(this).attr("sort");
                    setting.jsRenderSortField = sort;
                    setting.jsRenderASC = $(this).attr("sort");
                    me.JsRenderData(setting);
                    var removestr = $(this).attr("sort") == "desc" ? "do-down" : "do-up";
                    var addstr = $(this).attr("sort") == "desc" ? "do-up" : "do-down";
                    $(this).find(".sort-do").eq(0).removeClass(removestr).addClass(addstr);
                });
            }
        });
    }
})(jQuery);



//GO TO TOP
(function ($) {
    var top = 0;
    var bottom = 0;
    var tophrefpoint = "";
    var bottomhrefpoint = "";
    var pingshow = true;
    var topshow = true;
    $.fn.createTop = function () {
        if (arguments.length > 0) {
            top = arguments[0].top == undefined ? 0 : arguments[0].top;
            bottom = arguments[0].bottom == undefined ? 0 : arguments[0].bottom;
            tophrefpoint = arguments[0].tophrefpoint == undefined ? "" : arguments[0].tophrefpoint;
            bottomhrefpoint = arguments[0].bottomhrefpoint == undefined ? "" : arguments[0].bottomhrefpoint;
            pingshow = arguments[0].pingshow == undefined ? pingshow : arguments[0].pingshow;
            topshow = arguments[0].topshow == undefined ? topshow : arguments[0].topshow;
        }
        $("body").append('<div class="positiontop">' + (topshow ? '<a href="#' + (tophrefpoint == "" ? "@" : tophrefpoint) + '" class="gototop">&nbsp;</a>' : '') + (pingshow ? '<a href="#' + (tophrefpoint == "" ? "@" : tophrefpoint) + '" class="evlution">&nbsp;</a>' : '') + '</div>');
        $(".positiontop").css({ "left": ((document.body.clientWidth + 950) / 2) + "px" });
        $(this).bindClick();
    };

    $.fn.bindClick = function () {
        if (tophrefpoint == "") {
            $(".gototop").bind("click", function () {
                $('body,html').animate({ scrollTop: top }, 1000);
            });
        }
        if (bottomhrefpoint == "") {
            $(".evlution").bind("click", function () {
                $('body,html').animate({ scrollTop: document.body.scrollHeight }, 1000);
            });
        }
    };

    $.fn.preInput = function () {
        var o = this;
        if ($(o).attr("tip") == $(o).val())
            $(o).css("color", "#999");
        $(o).bind("blur", function () {
            if ($(o).val() == "" || $(o).attr("tip") == $(o).val()) {
                $(o).val($(o).attr("tip"));
                $(o).css("color", "#999");
            }
        }).bind("focus", function () {
            if ($(o).attr("tip") == $(o).val()) {
                $(o).val("");
                $(o).css("color", "#000");
            }
            $(o).css("color", "#000");
        });
    };
})(jQuery);

(function ($) {
    $.fn.textareaCount = function (options, fn) {
        var defaults = {
            maxCharacterSize: -1,
            originalStyle: 'originalTextareaInfo',
            warningStyle: 'warningTextareaInfo',
            warningNumber: 20,
            displayFormat: '已输入 #input 字符',
            messageShow: options.messageShow || ''
        };
        var options = $.extend(defaults, options);

        var container = $(this);

        if (options.messageShow == "")
            $("<div class='charleft'>&nbsp;</div>").insertAfter(container);

        //create charleft css
        var charLeftCss = {
            'width': container.width()
        };

        var charLeftInfo = getNextCharLeftInformation(container);
        charLeftInfo.addClass(options.originalStyle);
        charLeftInfo.css(charLeftCss);

        var numInput = 0;
        var maxCharacters = options.maxCharacterSize;
        var numLeft = 0;
        var numWords = 0;

        container.bind('keyup', function (event) { limitTextAreaByCharacterCount(); })
            .bind('mouseover', function (event) { setTimeout(function () { limitTextAreaByCharacterCount(); }, 10); })
            .bind('paste', function (event) { setTimeout(function () { limitTextAreaByCharacterCount(); }, 10); });


        function limitTextAreaByCharacterCount() {
            charLeftInfo.html(countByCharacters());
            //function call back
            if (typeof fn != 'undefined') {
                fn.call(this, getInfo());
            }
            return true;
        }

        function countByCharacters() {
            var content = container.val();
            var contentLength = content.length;

            //Start Cut
            if (options.maxCharacterSize > 0) {
                //If copied content is already more than maxCharacterSize, chop it to maxCharacterSize.
                if (contentLength >= options.maxCharacterSize) {
                    content = content.substring(0, options.maxCharacterSize);
                }

                var newlineCount = getNewlineCount(content);

                // newlineCount new line character. For windows, it occupies 2 characters
                var systemmaxCharacterSize = options.maxCharacterSize - newlineCount;
                if (!isWin()) {
                    systemmaxCharacterSize = options.maxCharacterSize;
                }
                if (contentLength > systemmaxCharacterSize) {
                    //avoid scroll bar moving
                    var originalScrollTopPosition = this.scrollTop;
                    container.val(content.substring(0, systemmaxCharacterSize));
                    this.scrollTop = originalScrollTopPosition;
                }
                charLeftInfo.removeClass(options.warningStyle);
                if (systemmaxCharacterSize - contentLength <= options.warningNumber) {
                    charLeftInfo.addClass(options.warningStyle);
                }

                numInput = container.val().length + newlineCount;
                if (!isWin()) {
                    numInput = container.val().length;
                }

                numWords = countWord(getCleanedWordString(container.val()));

                numLeft = maxCharacters - numInput;
            } else {
                var newlineCount = getNewlineCount(content);
                numInput = container.val().length + newlineCount;
                if (!isWin()) {
                    numInput = container.val().length;
                }
                numWords = countWord(getCleanedWordString(container.val()));
            }

            return formatDisplayInfo();
        }

        function formatDisplayInfo() {
            var format = options.displayFormat;
            format = format.replace('#input', numInput);
            format = format.replace('#words', numWords);
            if (maxCharacters > 0) {
                format = format.replace('#max', maxCharacters);
                format = format.replace('#left', numLeft);
            }
            return format;
        }

        function getInfo() {
            var info = {
                input: numInput,
                max: maxCharacters,
                left: numLeft,
                words: numWords
            };
            return info;
        }

        function getNextCharLeftInformation(container) {
            if (options.messageShow == "")
                return container.next('.charleft');
            else
                return $("#" + options.messageShow);
        }

        function isWin() {
            var strOS = navigator.appVersion;
            if (strOS.toLowerCase().indexOf('win') != -1) {
                return true;
            }
            return false;
        }

        function getNewlineCount(content) {
            var newlineCount = 0;
            for (var i = 0; i < content.length; i++) {
                if (content.charAt(i) == '\n') {
                    newlineCount++;
                }
            }
            return newlineCount;
        }

        function getCleanedWordString(content) {
            var fullStr = content + " ";
            var initial_whitespace_rExp = /^[^A-Za-z0-9]+/gi;
            var left_trimmedStr = fullStr.replace(initial_whitespace_rExp, "");
            var non_alphanumerics_rExp = rExp = /[^A-Za-z0-9]+/gi;
            var cleanedStr = left_trimmedStr.replace(non_alphanumerics_rExp, " ");
            var splitString = cleanedStr.split(" ");
            return splitString;
        }

        function countWord(cleanedWordString) {
            var word_count = cleanedWordString.length - 1;
            return word_count;
        }
    };
})(jQuery);