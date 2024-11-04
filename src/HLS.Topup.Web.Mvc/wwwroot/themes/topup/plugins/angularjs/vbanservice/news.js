//Filter
topupApp.filter('formatdate', [
    '$filter', function ($filter) {
        return function (input, format) {
            return $filter('date')(new Date(input), format);
        };
    }
]);

topupApp.filter("strLimit", ["$filter", function ($filter) {
    return function (input, limit) {
        if (input === null) {
            input = "";
        }
        if (input.length <= limit) {
            return input;
        }

        return $filter("limitTo")(input, limit) + '...';
    };
}]);

topupApp.filter('formatjsondate', [
    '$filter', function ($filter) {
        return function (input, format) {
            var date = new Date(parseInt(input.substr(6)));
            return $filter('date')(new Date(date), format);
        };
    }
]);
topupApp.controller('NewsController', ["$scope", "$locale", function ($scope, $locale) {
    $scope.TopList = [];
    $scope.UnderTopList = [];
    $scope.RightTopList = [];
    $scope.MoreList = [];
    $scope.PageNo = 1;
    $scope.PanelStatus = 1;
    $scope.ProNews = [];
    $scope.ProPageNo = 1;

    $scope.GetNewsByType = function () {
        $.ajax({
            url: "/news/get-news-by-cate",
            data: {
                pageNo: 1,
                pageSize: 8
            },
            type: "POST",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                if (data.Code === "00") {
                    $scope.TopList = data.Data;
                    if ($scope.TopList.length > 0) {
                        if ($scope.TopList.length >= 3) {
                            $scope.UnderTopList.push($scope.TopList[1]);
                            $scope.UnderTopList.push($scope.TopList[2]);
                        } else {
                            $scope.UnderTopList.push($scope.TopList[1]);
                        }

                        if ($scope.TopList.length === 8) {
                            for (var i = 3; i < 8; i++) {
                                $scope.RightTopList.push($scope.TopList[i]);
                            }
                          
                        } else if ($scope.TopList.length > 3 && $scope.TopList.length < 8) {
                            for (var ii = 3; ii < $scope.TopList.length ; ii++) {
                                $scope.RightTopList.push($scope.TopList[ii]);
                            }
                        }

                    }
                }
               
                if (!$scope.$$phase)
                    $scope.$apply();
            },
            error: ""
        });
    };

    $scope.LoadMoreNews = function () {
        if ($scope.PageNo <= 8) {
            $scope.PageNo = $scope.PageNo + 1;
            $.ajax({
                url: "/news/get-news-by-cate",
                data: {
                    pageNo: $scope.PageNo,
                    pageSize: 8
                },
                type: "POST",
                beforeSend: function () {
                    $("#service-loader-wrapper").css("display", "");
                },
                complete: function () {
                    $("#service-loader-wrapper").css("display", "none");
                },
                success: function (data) {
                    if (data.Code === "00") {
                        for (var ii = 0; ii < data.Data.length ; ii++) {
                            $scope.MoreList.push(data.Data[ii]);
                        }
                    }

                    if (!$scope.$$phase)
                        $scope.$apply();
                },
                error: ""
            });
        }
        if (!$scope.$$phase)
            $scope.$apply();
    };

    $scope.ChangePanel = function (status) {
        $scope.PanelStatus = status;
        if ($scope.PanelStatus === 2) {
            $.ajax({
                url: "/news/get-promo-news-by-alias",
                data: {
                    pageNo: 1,
                    pageSize: 8
                },
                type: "POST",
                beforeSend: function () {
                    $("#service-loader-wrapper").css("display", "");
                },
                complete: function () {
                    $("#service-loader-wrapper").css("display", "none");
                },
                success: function (data) {
                    if (data.Code === "00") {
                        $scope.ProNews = data.Data;
                    }

                    if (!$scope.$$phase)
                        $scope.$apply();
                },
                error: ""
            });
        }

        $scope.ProPageNo = 1;
        if (!$scope.$$phase)
            $scope.$apply();
    }

    $scope.LoadMoreProNews = function () {
        if ($scope.ProPageNo <= 8) {
            $scope.ProPageNo = $scope.ProPageNo + 1;
            $.ajax({
                url: "/news/get-promo-news-by-alias",
                data: {
                    pageNo: $scope.ProPageNo,
                    pageSize: 8
                },
                type: "POST",
                beforeSend: function () {
                    $("#service-loader-wrapper").css("display", "");
                },
                complete: function () {
                    $("#service-loader-wrapper").css("display", "none");
                },
                success: function (data) {
                    if (data.Code === "00") {
                        for (var ii = 0; ii < data.Data.length ; ii++) {
                            $scope.ProNews.push(data.Data[ii]);
                        }
                    }

                    if (!$scope.$$phase)
                        $scope.$apply();
                },
                error: ""
            });
        }
        if (!$scope.$$phase)
            $scope.$apply();
    };
}]);
