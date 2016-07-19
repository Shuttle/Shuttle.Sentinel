import Model from 'can/Model/';
import configuration from 'sentinel/configuration';

var RegisterSession = Model.extend({
	create: `POST ${configuration.getApiUrl('Sessions')}`
}, {});

export default RegisterSession;