import Component from 'can-component';
import DefineMap from 'can-define/map/';
import stache from 'can/view/stache/';
import template from './table.stache!';
import localisation from '~/localisation';
import click from '~/components/click';

export const ViewModel = DefineMap.extend({
    define: {
        emptyMessage: {
            get: function() {
                return this.attr('emptyMessage') || 'table-empty-message';
            }
        },

        containerClass: {
          get: function(value) {
              return value || '';
          }
        },

        buttonClass: {
          get: function(value) {
              return value || '';
          }
        },

        columns: {
            value: new can.List()
        },

        rows: {
            value: new can.List()
        },

        shouldShowEmptyMessage: {
            get: function() {
                return this.attr('rows.length') === 0 && !!this.attr('emptyMessage');
            }
        }
    },

    _rowClick: function(row) {
        if (this.attr('rowClick')) {
            this.attr('rowClick')(row);
        } else {
            click.on(row);
        }
    }
});

export default Component.extend({
    tag: 'sentinel-table',
    template,
    viewModel: ViewModel,
    helpers: {
        columnTitle(column) {
            if (!!column.columnTitleTemplate) {
                return stache(column.columnTitleTemplate)(column);
            } else {
                return localisation.value(column.columnTitle || '');
            }
        },
        columnClass(column) {
            return column.columnClass || '';
        },
        columnValue(row, column) {
            if (!column.attributeName) {
                throw new Error('The column requires an \'attributeName\'');
            }

            return typeof(row.attr) === 'function' ? row.attr(column.attributeName) : row[column.attributeName];
        },
        template(row, column) {
            let stacheTemplate = column.template;

            if (!stacheTemplate) {
                throw new Error('Specify a template for the column.');
            }

            return stache(stacheTemplate)(row);
        },
        rowClass(row) {
            return typeof(row.attr) === 'function' ? row.attr('rowClass') : row['rowClass'];
        },
        buttonContext(row, column) {
            var context = column.attr('buttonContext');

            return !!context ? context : row;
        }
    }
});