var ajax = $.ajax;
$.ajax = function (opt) {
    //备份opt中error和success方法
    var fn = {
        success: function (data, textStatus, jqXHR) {
        }
    }
    if (opt.success) {
        fn.success = opt.success;
    }
    //扩展增强处理
    var _opt = $.extend(opt, {
        success: function (data, textStatus, jqXHR) {
            //alert('重写success事件');
            if (data.status == '408') {
                location.href = data.url;
                return;
            }
            fn.success(data, textStatus, jqXHR);
        }
    });
    var def = ajax.call($, _opt);                                                                                                                             // 兼容不支持异步回调的版本
    if ('done' in def) {
        var done = def.done;
        def.done = function (func) {
            function _done(data) {
                if (data.status == '408') {
                    location.href = data.url;
                    return;
                }
                func(data);
            }

            done.call(def, _done);
            return def;
        };
    }
    return def;
};  