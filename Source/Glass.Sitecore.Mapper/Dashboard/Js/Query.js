/// <reference path='http://cdnjs.cloudflare.com/ajax/libs/knockout/2.1.0/knockout-min.js' />
/// <reference path='http://code.jquery.com/jquery.min.js' />

$(function () {

    function ViewModel() {
        var self = this;
        self.classes = ko.observableArray([]);
        self.results = ko.observableArray([]);
        self.AddClass = function (name) {
            var cls = new GlassClass(name);
            self.classes.push(cls);
        };
        self.FirstLoad = true;
    }

    var view = new ViewModel();
    var $glassClass = $("#glassClass");
    var $scPath = $("#scPath");
    var $button = $("#queryButton");

    ko.applyBindings(view);





    $.getJSON("/List/Classes.gls", function (data) {

        $.each(data, function (i, cls) {
            view.AddClass(cls.Name);
        });

        if (view.FirstLoad) {
            if (getParameterByName("cls") != '') {
                $glassClass.val(getParameterByName("cls"));
            }
            view.FirstLoad = false;
        }

    });

    $scPath.autocomplete({
        source: "/Query/Paths.gls",
        minLength: 2
    });

    $button.click(function (event) {

        ModalOn();

        event.preventDefault();

        var cls = $glassClass.val();
        var path = $scPath.val();

        var url = "/Query/Query.gls?cls=" + cls + "&path=" + path;

        view.results.removeAll();


        $.getJSON(url, function (data) {

            for (var i = 0; i < data.length; i++) {
                view.results.push(data[i]);
            }

            $(".queryLink").click(function (event) {
                event.preventDefault();
                var $link = $(this);
                var path = $link.attr("path");
                var cls = $link.attr("cls");

                $scPath.val(path);
                $glassClass.val(cls);
                $button.click();

            });

            ModalOff();

        });
    });



});

