Sentinel.ViewModel = can.Map.extend('ViewModel', {
}, {
    _localizedAttributes: [],

    init: function () {
        var self = this;

        this.resolve('_localizationService');

        this.bind('change', function (ev, attr, how, newVal, oldVal) {
        	if (attr.indexOf('_localizedAttributes') > -1) {
		        return;
        	}

	        var errors = self.errors(attr);

            if (errors && errors[attr].length > 0) {
                self.attr(attr + 'ValidationMessage', errors[attr][0]);
            } else {
                self.attr(attr + 'ValidationMessage', undefined);
            }

            if (!self._shouldLocalize(attr)) {
                return;
            }

            self._localizeAttributeValue(attr);
        });
    },

    _registerLocalizedAttribute: function (attr) {
        this._localizedAttributes.push(attr);

        this._localizeAttributeValue(attr);
    },

    _shouldLocalize: function (attr) {
        return $.inArray(attr, this._localizedAttributes) > -1;
    },

    _localizeAttributeValue: function (attr) {
    	var value = this.attr(attr);
    	var translation = this._localizationService.value(value);

    	if (translation !== value) {
		    this.attr(attr, translation);
	    }

        return translation;
    },

    hasErrors: function () {
    	return this.errors() != undefined;
    }
});

can.extend(Sentinel.ViewModel.prototype, Sentinel.Dependency);
