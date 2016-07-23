import Model from 'can/Model/';
import configuration from 'sentinel/configuration';

var RegisterSession = Model.extend({
	create: `POST ${configuration.controllerUrl('Sessions')}`
}, {});

export default RegisterSession;