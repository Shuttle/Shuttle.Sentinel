import can from 'can';
import Map from 'can/map/';
import stache from 'can/view/stache/';
import template from './table.stache!';

export const ViewModel = Map.extend({
    define: {
        containerClass: {
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
        columnValue(row, column) {
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