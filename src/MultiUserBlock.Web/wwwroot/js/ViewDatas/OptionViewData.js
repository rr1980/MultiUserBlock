
window.ViewModels = (function (module) {
    module.OptionViewData = function (data) {
        var self = this;
        ko.mapping.fromJS(data, {}, self);

        self.onClickSave = function () { };
        self.onClickDelete = function () { };

    };
    return module;
}(this.ViewModels || {}));