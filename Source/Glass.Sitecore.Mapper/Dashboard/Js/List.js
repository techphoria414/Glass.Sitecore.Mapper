/// <reference path='http://cdnjs.cloudflare.com/ajax/libs/knockout/2.1.0/knockout-min.js' />
/// <reference path='http://code.jquery.com/jquery.min.js' />

$(function () {

    function ViewModel() {
        var self = this;
        self.classes = ko.observableArray([]);

        self.AddClass = function (name) {
            var cls = new GlassClass(name);
            self.classes.push(cls);
        };
    }

    var view = new ViewModel();
    ko.applyBindings(view);


    $.getJSON("/List/Classes.gls", function (data) {

        $.each(data, function (i, cls) {
            view.AddClass(cls.Name);
        });

    });

});