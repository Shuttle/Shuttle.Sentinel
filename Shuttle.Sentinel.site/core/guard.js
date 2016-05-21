Sentinel.guard = {
    againstUndefined: function(value, name) {
        if (value != undefined) {
            return;
        }

        throw new Error('Value named \'' + name + '\' may not be undefined.');
    }
};