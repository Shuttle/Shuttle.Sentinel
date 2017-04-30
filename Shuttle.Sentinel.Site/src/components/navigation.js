import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import Component from 'can-component';
import view from './navigation.stache!';
import security from '~/security';
import each from 'can-util/js/each/';
import navigationMap from '~/navigation-map';

var ViewModel = DefineMap.extend({
    security: { value: security },
    navigationItems: {
        get: function() {
            var result = new DefineList();

            each(navigationMap, function(item) {
                var add = false;
                var navigationItem = new DefineMap({
                    href: item.href,
                    text: item.text,
                    items: new DefineList()
                });

                if (!item.permission || security.hasPermission(item.permission)) {
                    if (item.items !== undefined) {
                        each(item.items, function(subitem) {
                            if (!subitem.permission || security.hasPermission(subitem.permission)) {
                                add = true;

                                navigationItem.items.push(new DefineMap({
                                    href: subitem.href,
                                    text: subitem.text
                                }));
                            }
                        });
                    } else {
                        add = true;
                    }

                    if (add) {
                        result.push(navigationItem);
                    }
                }
            });

            return result;
        }
    }
});

export default Component.extend({
    tag: 'sentinel-navigation',
    view,
    ViewModel
});