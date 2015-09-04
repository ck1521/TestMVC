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

    self.author = {
        id: ko.observable(),
        name: ko.observable(),
        publisher: ko.observable(),
    };
    

    self.deleteAuthor = function (form) {
        self.sending(true);
        return true;
    };


    self.sending = ko.observable(false);
    self.showDeleteModalAjax = function (data, event) {
        $.get(
            $(event.target).attr('href'),
            function (d) {
                self.author.id(d.id);
                self.author.name(d.name);
                self.author.publisher(d.publisher);
                $('#deleteModal').modal('show');
            });
    };
    

    self.deleteAuthorAjax = function (form) {
        self.sending(true);
        $.ajax({
            url: '/api/authors/' + self.author.id(),
            method: 'delete',
            contentType: 'application/json',
        })
        .success(self.saveSuccess)
        .error(self.saveError)
        .complete(function () { self.sending(false); });
    };

    self.saveSuccess = function () {
        $('.body-content').prepend(
            '<div class="alert alert-success"><strong>Success!</strong>The author has been removed.</div>'
            );
        setTimeout(function () { location.href = '../'; }, 1000);
    };

    self.saveError = function () {
        $('.body-content').prepend(
             '<div class="alert alert-danger"><strong>Error!</strong>Some error happened.</div>'
            );
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