angular.module('crawlerApp.directives', ['crawlerApp.services'])
  //.directive('boardPanel', ['$rootScope','$compile', 'Board', 'Website', function ($rootScope, $compile, Board, Website) {
  //   return {
  //    restrict: 'E',
  //    templateUrl: '/tpl/directives/boardPanel.tpl.html',
  //    scope: {
  //        board: '@'
  //    },
  //    link: function(scope, el, attrs) {        

  //        var element = (el.find("a"));
  //        $compile(element)(scope);

  //        scope.contentRevisionList = [];
  //        scope.selectedBoardContentsList = [];
  //        scope.SelectedcontentRevisionList = [];

  //        Board.getBoard(scope.boardId, function (data) {
  //            scope.board = data;

  //            Website.getWebsite(scope.board.id, function (website) {
  //                scope.website = website;
  //            })
  //        }, function (msg) {
  //            alret(msg);
  //        });

  //        scope.websiteList = [];
  //        Website.getWebsiteList(function (data) {
  //            scope.websiteList = data;
              
  //        }, function (msg) {
  //            alret(msg);
  //        });
  //    }
  //   };
  //}])

.directive('contentList', ['$rootScope', '$stateParams', 'Board', 'Website', function ($rootScope, $stateParams, Board, Website) {
    return {
        restrict: 'E',
        templateUrl: '/tpl/directives/contentList.tpl.html',
        scope: {
            boardId: '@'
        },
        link: function (scope, el, attrs) {
            scope.boardId = $stateParams.websiteId;
            Board.getBoard(scope.boardId, function (data) {
                console.log("getBoard");
                console.log(data);
                scope.board = data;
                Website.getWebsite(scope.board.id, function (website) {
                    console.log("getWebsite");
                    console.log(website);
                    scope.website = website;
                })
            }, function (msg) {
                alret(msg);
            });

            scope.websiteList = [];
            Website.getWebsiteList(function (data) {
                scope.websiteList = data;
                console.log("getWebsiteList");
                console.log(scope.websiteList);
            }, function (msg) {
                alret(msg);
            });
        }
    };
}]);