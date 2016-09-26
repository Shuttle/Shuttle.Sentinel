import can from 'can';
import Map from 'can/map/';
import template from './table.stache!';
import localisation from 'sentinel/localisation';

export const ViewModel = Map.extend({
    define: {
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
        }
    }
});