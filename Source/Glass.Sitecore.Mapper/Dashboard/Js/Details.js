/// <reference path='http://cdnjs.cloudflare.com/ajax/libs/knockout/2.1.0/knockout-min.js' />
/// <reference path='http://code.jquery.com/jquery.min.js' />

$(function () {
    var cls = getParameterByName("cls");

    ModalOn();

    $.getJSON("/Details/Get.gls?name=" + cls, function (data) {
        var viewModel = ko.mapping.fromJS(data);
        viewModel.QueryUrl = "/Query.gls?cls=" + viewModel.Name();

        ko.applyBindings(viewModel);

        ModalOff();
    });

});