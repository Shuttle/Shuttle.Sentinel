import can from 'can';
import Map from 'can/map/';
import stache from 'can/view/stache/';
import template from './table.stache!';
import localisation from 'sentinel/localisation';

export const ViewModel = Map.extend({
    define: {
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
        }
    }
});

export default can.Component.extend({
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
        }
    }
});