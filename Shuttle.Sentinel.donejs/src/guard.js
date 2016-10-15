var guard = {
    againstUndefined: function(value, name) {
        if (value) {
            return;
        }

        throw new Error('\'' + name + '\' may not be undefined/null.');
    }
}

export default guard;