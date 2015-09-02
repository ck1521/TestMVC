function AuthorIndexViewModel(authors) {
    var self = this;
    self.authors = authors;

    if (typeof PagingService === 'function')
    {
        self.pagingService = new PagingService(authors);
    }

    self.showDeleteModal = function (data, event) {
        self.sending = ko.observable(false);
        $.get($(event.target).attr('href'), function (d) {
            $('.body-content').prepend(d);
            $('#deleteModal').modal('show');

            ko.applyBindings(self, document.getElementById('deleteModal'));
        });
    };

    self.deleteAuthor = function (form) {
        self.sending(true);
        return true;
    };

}
//var viewModel = new AuthorIndexViewModel(@Html.HtmlConvertToJson(Model));
//viewModel.AjaxDelete =  function AjaxDelete(id)
//{
//    var token = $('input[name=__RequestVerificationToken]').val();
//    $.post('/Authors/Delete',
//        {
//            id:id,
//            '__RequestVerificationToken':token,
//        },
//        function(data)
//        {
//            alert(data);
//        });
//}
//ko.applyBindings(viewModel);