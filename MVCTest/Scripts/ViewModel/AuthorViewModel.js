function AuthorViewModel() {
    var self = this;

    self.saveCompleted = ko.observable(false);
    self.sending = ko.observable(false);

    self.author =
    {
        name: ko.observable(),
        publisher: ko.observable(),
    };

    self.validateAndSave = function (form) {
        if (!$(form).valid()) {
            return false;
        }
        self.sending(true);
        self.author.__RequestVerificationToken = form[0].value;
        $.ajax({
            url: 'Create',
            method: 'POST',
            contentType: 'application/x-www-form-urlencoded',
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
        //setTimeout(function () { location.href = './'; }, 1000);
    };

    self.saveError = function () {
        $('.body-content').prepend(
             '<div class="alert alert-success"><strong>Error!</strong>Some error happened.</div>'
            );
    };

}