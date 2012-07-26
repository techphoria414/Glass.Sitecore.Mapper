/// <reference path='http://cdnjs.cloudflare.com/ajax/libs/knockout/2.1.0/knockout-min.js' />
/// <reference path='http://code.jquery.com/jquery.min.js' />

$(function () {

    function ViewModel() {
        var self = this;

        self.details = ko.observable(null);

    }

    var view = new ViewModel();
    ko.applyBindings(view);

    var cls = getParameterByName("cls");


    $.getJSON("/Details/Get.gls?name=" + cls, function (data) {
        view.details = GlassDetails(data);
    });

});