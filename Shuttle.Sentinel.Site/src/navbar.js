import guard from 'shuttle-guard';
import stache from 'can-stache';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';

export const NavbarControlMap = DefineMap.extend({
    stache: {
        type: 'string',
        get: function (value) {
            if (!value) {
                throw new Error('The \'NavbarControlMap\' does not have a \'view\' specified.')
            }

            return value;
        }
    },
    view: {
        type: 'string',
        get() {
            return stache(this.stache)(this.viewModel);
        }
    },
    viewModel: {
        type: '*'
    }
});

export const NavbarControlList = DefineList.extend({
    '#': NavbarControlMap,
    clear() {
        this.replace([]);
    }
});

export const Navbar = DefineMap.extend({
    controls: {
        Default: NavbarControlList
    },
    addButton(options) {
        guard.againstUndefined(options, 'options')
        guard.againstUndefined(options.type, 'options.type')

        var permission = options.permission || '';
        var type = options.type.toLowerCase();
        var click = options.click;

        if (options.type !== 'back' && !click) {
            switch (type) {
                case 'add':
                case 'refresh':
                case 'remove': {
                    click = type;
                    break;
                }
                default: {
                    throw new Error('No \'click\' method name has been specified.')
                }
            }
        }

        switch (type) {
            case 'back': {
                this.controls.push({
                    stache: '<cs-button-back text:from="\'back\'" elementClass:from="\'btn-sm mr-2\'"/>'
                });
                break;
            }
            case 'refresh': {
                this.controls.push({
                    viewModel: options.viewModel,
                    stache: `<cs-button-refresh click:from="@${click}" elementClass:from="'btn-sm mr-2'"/>`
                });
                break;
            }
            case 'add': {
                this.controls.push({
                    viewModel: options.viewModel,
                    stache: `<cs-button click:from="@${click}" elementClass:from="\'btn-sm mr-2\'" permission:from="\'${permission}\'" text:from="\'${options.text || 'add'}\'"/>`
                });
                break;
            }
            case 'remove': {
                this.controls.push({
                    viewModel: options.viewModel,
                    stache: `<cs-button-remove click:from="@${click}" elementClass:from="\'btn-sm mr-2\'" permission:from="\'${permission}\'"/>`
                });
                break;
            }
            default: {
                throw new Error(`Unhandled button type '${options.type}'.`);
            }
        }
    }
});

export default new Navbar();