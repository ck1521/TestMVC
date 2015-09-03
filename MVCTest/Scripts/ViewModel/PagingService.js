function PagingService(resultList)
{
    var self = this;
    self.queryOptions = {
        currentPage: ko.observable(),
        totalPages: ko.observable(),
        pageSize: ko.observable(),
        sortField: ko.observable(),
        sortOrder: ko.observable()
    };
    self.entities = ko.observableArray();

    self.updateResultList = function (resultList) {
        self.queryOptions.currentPage(resultList.queryOptions.currentPage);
        self.queryOptions.totalPages(resultList.queryOptions.totalPages);
        self.queryOptions.pageSize(resultList.queryOptions.pageSize);
        self.queryOptions.sortField(resultList.queryOptions.sortField);
        self.queryOptions.sortOrder(resultList.queryOptions.sortOrder);

        self.entities(resultList.results);
    };

    self.updateResultList(resultList);

    self.sortEntitiesBy = function (data, event) {
        var softField = $(event.target).data('sortField');
        if (softField === self.queryOptions.sortField() && (self.queryOptions.sortOrder() === "ASC")) {
            self.queryOptions.sortOrder("DESC");
        }
        else {
            self.queryOptions.sortOrder("ASC");
        }
        self.queryOptions.sortField(softField);
        self.queryOptions.currentPage(1);
        self.fetchEntities(event);
    };

    self.previousPage = function (data, event) {
        if (self.queryOptions.currentPage() > 1) {
            self.queryOptions.currentPage(self.queryOptions.currentPage() - 1);
            self.fetchEntities(event);
        }
    };

    self.nextPage = function (data, event) {
        if (self.queryOptions.currentPage() < self.queryOptions.totalPages()) {
            self.queryOptions.currentPage(self.queryOptions.currentPage() + 1);
            self.fetchEntities(event);
        }
    };

    self.fetchEntities = function (event) {
        var url = '/api/' + $(event.currentTarget).attr('href');    // jsfiddle.net/misteroneill/kmn4A/3/
        url += "?sortField=" + self.queryOptions.sortField();
        url += "&sortOrder=" + self.queryOptions.sortOrder();
        url += "&currentPage=" + self.queryOptions.currentPage();
        url += "&pageSize=" + self.queryOptions.pageSize();

        $.ajax({
            dataType: 'json',
            url: url
        }).success(function (data) { self.updateResultList(data); })
            .error(function () {
            $('.body-content').
                prepend('<div class="alert alert-danger"><strong>Error!</strong> There was an error fetching the data</div');
        });
    };

    self.buildSortIcon = function (sortField) {
        return ko.pureComputed(function () {
            var sortIcon = 'sort';
            if (self.queryOptions.sortField === sortField) {
                sortIcon += '-by-alphabet';
            }
            if (self.queryOptions.sortOrder() === "DESC") {
                sortIcon += '-alt';
            }

            return 'glyphicon glyphicon-' + sortIcon;
        });
    };

    self.buildPreviousClass = ko.pureComputed(function () {
        var className = 'previous';

        if (self.queryOptions.currentPage() === 1) {
            className += ' disabled';
        }
        return className;
    });

    self.buildNextClass = ko.pureComputed(function () {
        var className = 'next';

        if (self.queryOptions.currentPage() === self.queryOptions.totalPages) {
            className += ' disabled';
        }
        return className;
    });
}
