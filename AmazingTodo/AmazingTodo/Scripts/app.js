﻿var TodoApp = angular.module("TodoApp", ['ngResource', 'ngRoute']).
    config(function ($routeProvider) {
        $routeProvider.
            when('/', { controller: ListCtrl, templateUrl: 'List.html' }).
            when('/new', { controller: CreateCtrl, templateUrl: 'details.html' }).
            when('/edit/:itemId', { controller: EditCtrl, templateUrl: 'details.html' }).
            otherwise({ redirectTo: '/' });
    });

//.directive('greet', function () {
//    return {
//        template: '<h2>Greetings from {{from}} to my dear {{to}}</h2>',
//        controller: function ($scope, $element, $attrs) {
//            $scope.from = $attrs.from;
//            $scope.to = $attrs.greet;
//        }
//    };
//});

TodoApp.factory('Todo', function ($resource) {
    return $resource('/api/todo/:id', { id: '@id' }, { update: { method: 'PUT' } });
});

var CreateCtrl = function ($scope, $location, Todo) {
    $scope.action = "Add";
    $scope.save = function () {
        Todo.save($scope.item, function () {
            $location.path('/');
        });
    };
};

var EditCtrl = function ($scope, $routeParams, $location, Todo) {
    $scope.action = "Update";
    var id = $routeParams.itemId;
    $scope.item = Todo.get({ id: id });

    $scope.save = function () {
        Todo.update({ id: $scope.item.TodoItemId }, $scope.item, function () {
            $location.path('/');
        });
    };
};

var ListCtrl = function ($scope, $location, Todo) {
    $scope.search = function () {
        //$scope.items = Todo.query({ sort: $scope.sort_order, desc: $scope.is_desc });
        Todo.query({
            q:$scope.query,
            sort: $scope.sort_order,
            desc: $scope.is_desc,
            offset: $scope.offset,
            limit: $scope.limit
        },
            function (data) {
                $scope.more = data.length === 20;
                $scope.items = $scope.items.concat(data);
        });
    };

    $scope.sort = function (col) {
        if ($scope.sort_order === col) {
            $scope.is_desc = !$scope.is_desc;
        } else {
            $scope.sort_order = col;
            $scope.is_desc = false;
        }
        $scope.reset();
    }
    
    $scope.show_more = function () {
        $scope.offset += $scope.limit;
        $scope.search();
    };
    $scope.has_more = function () {
        return $scope.more;
    };

    $scope.reset = function () {

        $scope.limit = 20;
        $scope.offset = 0;
        $scope.items = [];
        $scope.more = true;
        $scope.search();
    };

    $scope.delete = function () {
        debugger;
        var itemId = this.item.TodoItemId;
        console.log(itemId);
        Todo.delete({ id: itemId }, function () {
            $('#todo_' + itemId).fadeOut();
        });
    };

    //$scope.

    $scope.sort_order = "Priority";
    $scope.is_desc = false;

    $scope.reset();
};

//var ListCtrl = function ($scope, $location) {
//    $scope.test = "testing";
//};
