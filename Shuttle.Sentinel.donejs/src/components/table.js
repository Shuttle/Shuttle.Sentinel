import can from 'can';
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
    helpers:{
        columnValue(row){
            return fields;
        }
    }});