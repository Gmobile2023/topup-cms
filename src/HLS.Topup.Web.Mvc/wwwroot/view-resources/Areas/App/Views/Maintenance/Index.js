﻿(function () {
    $(function () {

        var _cachingService = abp.services.app.caching;
        var _webLogService = abp.services.app.webLog;

        //Caching
        function clearCache(cacheName) {
            _cachingService.clearCache({
                id: cacheName
            }).done(function () {
                abp.message.success(app.localize('CacheSuccessfullyCleared'));
            });
        }

        function clearAllCaches() {
            _cachingService.clearAllCaches().done(function () {
                abp.message.success(app.localize('AllCachesSuccessfullyCleared'));
            });
        }

        $('.btn-clear-cache').click(function (e) {
            e.preventDefault();
            var cacheName = $(this).attr('data-cache-name');
            clearCache(cacheName);
        });

        $('#ClearAllCachesButton').click(function (e) {
            e.preventDefault();
            clearAllCaches();
        });

        //Web Logs
        function getWebLogs() {
            _webLogService.getLatestWebLogs({}).done(function (result) {
                var logs = getFormattedLogs(result.latestWebLogLines);
                $('#WebSiteLogsContent').html(logs);
                fixWebLogsPanelHeight();
            });
        }

        function downloadWebLogs() {
            _webLogService.downloadWebLogs({}).done(function (result) {
                app.downloadTempFile(result);
            });
        }

        function getFormattedLogs(logLines) {
            var resultHtml = '';
            $.each(logLines, function (index, logLine) {
                resultHtml += '<span class="log-line">' + _.escape(logLine)
                    .replace('DEBUG', '<span class="label label-inline label-dark">DEBUG</span>')
                    .replace('INFO', '<span class="label label-inline label-info">INFO</span>')
                    .replace('WARN', '<span class="label label-inline label-warning">WARN</span>')
                    .replace('ERROR', '<span class="label label-inline label-danger">ERROR</span>')
                    .replace('FATAL', '<span class="label label-inline label-danger">FATAL</span>') + '</span>';
            });
            return resultHtml;
        }

        function fixWebLogsPanelHeight() {
            var windowHeight = $(window).height();
            var panelHeight = $('.full-height').height();
            var difference = windowHeight - panelHeight;
            var fixedHeight = panelHeight + difference;
            $('.full-height').css('height', (fixedHeight - 350) + 'px');
        }

        $('#DownloadAllLogsbutton').click(function (e) {
            e.preventDefault();
            downloadWebLogs();
        });

        $('#RefreshButton').click(function (e) {
            e.preventDefault();
            getWebLogs();
        });

        $(window).resize(function () {
            fixWebLogsPanelHeight();
        });

        getWebLogs();
    });
})();
