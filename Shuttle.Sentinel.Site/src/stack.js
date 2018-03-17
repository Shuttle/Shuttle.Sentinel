import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import guard from "shuttle-guard";

export const Stack = DefineMap.extend({
    items: {
        Default: DefineList
    },

    put: function (name, value) {
        guard.againstUndefined(name, 'name');

        this.remove(name);
        this.items.push({name: name, value: value});
    },

    pop: function (name) {
        guard.againstUndefined(name, 'name');

        let result;
        let removeIndex = 1;

        this.items.forEach(function (item, index) {
            if (item.name === name) {
                result = item.value;
                removeIndex = index;

                return false;
            }

            return true;
        });

        if (removeIndex > 1) {
            this.splice(removeIndex, 1);
        }

        return result;
    },

    peek: function (name) {
        guard.againstUndefined(name, 'name');

        let result;

        this.items.forEach(function (item, index) {
            if (item.name === name) {
                result = item.value;

                return false;
            }

            return true;
        });

        return result;
    },

    remove: function (name) {
        guard.againstUndefined(name, 'name');

        let removeIndex = -1;

        this.items.forEach(function (item, index) {
            if (item.name === name) {
                removeIndex = index;

                return false;
            }

            return true;
        });

        if (removeIndex > -1) {
            this.items.splice(removeIndex, 1);
        }
    }
});

export default new Stack();