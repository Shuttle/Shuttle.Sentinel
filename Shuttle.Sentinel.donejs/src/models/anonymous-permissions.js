import Model from 'can/Model/';
import configuration from 'sentinel/configuration';

var AnonymousPermissions = Model.extend({
	findAll: `GET ${configuration.controllerUrl('AnonymousPermissions')}`,
	parseModels: function(result) {
		return result.data;
	},
	parseModel: function(model) {
		return model;
	}
}, {});

export default AnonymousPermissions;