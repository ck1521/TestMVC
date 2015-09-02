function AuthorViewModel(author) {
    var self = this;

    self.saveCompleted = ko.observable(false);
    self.sending = ko.observable(false);

    self.isCreating = (author.id === 0);

    self.author =
    {
        id: author.id,
        name: ko.observable(author.name),
        publisher: ko.observable(author.publisher),
    };

    self.validateAndSave = function (form) {
        if (!$(form).valid()) {
            return false;
        }
        self.sending(true);
        self.author.__RequestVerificationToken = form[0].value;
        $.ajax({
            url: '/api/authors',
            method: (self.isCreating) ? 'post' : 'put',
            contentType: 'application/json',
            data: ko.toJS(self.author)
        })
        .success(self.saveSuccess)
        .error(self.saveError)
        .complete(function () { self.sending(false); });
    };

    self.saveSuccess = function () {
        self.saveCompleted(true);

        $('.body-content').prepend(
            '<div class="alert alert-success"><strong>Success!</strong>The author has been saved.</div>'
            );

        if (self.isCreating) {
            setTimeout(function () {
                location.href = '../';
            }, 1000);

        }
        else {
            setTimeout(function () { location.href = './'; }, 1000);
        }
    };

    self.saveError = function () {
        $('.body-content').prepend(
             '<div class="alert alert-success"><strong>Error!</strong>Some error happened.</div>'
            );
    };

}