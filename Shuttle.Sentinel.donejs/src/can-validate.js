/**
* @module {can.Construct} can-validate Can-Validate
* @parent can-validate-plugin
* @group can-validate.validators 0 Validate Methods
* @group can-validate.utils 1 Utilities
* @description
*  The core of can.validate, this script adds abstraction methods to can.validate
*   that call corresponding methods in the chosen shim. The shim does any value and
*   validation option manipulation, calls library validation methods, and returns errors.
*
*  Shims, in the case of errors, should return an array of errors. Otherwise, the
*   shim should return undefined.
*
* @signature 'can.validate'
* Validate is a can.Construct that is set to can.validate. It is possible to create
* an instance for the purpose of storing errors in the instance. It is recommended
* to use can.Map, can.Map.define, and can.Map.validate instead.
*
* @body
*
* ## Initialization
* Require/Import `can-validate` into your application after `can` and `can.validate`
*  will be available immediately. Make sure to also load your libarary and the
*  appropriate shim.
* ```js
* import 'validatejs';
* import 'can-validate/can-validate';
* import 'can-validate/shims/validatejs.shim';
*```
* You can use any of the methods at `can.validate`.
*
* @demo ./can-validate/demo.html
*
*
*/

import can from 'can';

// add methods to can
var Validate = can.Construct.extend({

	/*
	* @description The current validator ID to use when can.validate methods are called.
	*/
	_validatorId: '',

	/*
	* @description A map of validator libraries loaded into can.validate.
	*/
	_validators: {},

	/**
	* @function can-validate.validator Validator
	* @parent can-validate.utils
	* @description Registers a library with can.validate. The last one registered is the default library.
	* Override the default by changing `_validatorId` to the key of the desired registered library.
	* ```js
	* Validate.register('validatejs',validatejs);
	* ```
	* @param {string} id The key name of the validator library.
	* @param {object|function} validator The validator libarary. Only pass instances.
	*/
	validator: function () {
		return this._validators[this._validatorId];
	},

	/**
	* @function can-validate.register Register
	* @parent can-validate.utils
	* @description Registers a library with can.validate. The last one registered is the default library.
	* Override the default by changing `_validatorId` to the key of the desired registered library.
	* ```js
	* Validate.register('validatejs',validatejs);
	* ```
	* @param {string} id The key name of the validator library.
	* @param {object|function} validator The validator libarary. Only pass instances.
	*/
	register: function (id, validator) {
		this._validatorId = id;
		this._validators[id] = validator;
	},

	/**
	* @function can-validate.isValid Is Valid
	* @parent can-validate.validators
	* @description Registers a library with can.validate. The last one registered is the default library.
	* Override the default by changing `_validatorId` to the key of the desired registered library.
	* ```js
	* Validate.isValid('', {}, 'myVal');
	* ```
	* @param {*} value A value of any type to validate.
	* @param {object} options Validation config object, structure depends on the
	*  validation library used.
	* @param {string} name Optional. The key name of the value.
	*/
	isValid: function () {
		//!steal-remove-start
		if (!this._validatorId) {
			can.dev.warn('A validator library is required for can.validate to work properly.');
		}
		//!steal-remove-end
		return this.validator().isValid.apply(this, arguments);
	},

	/**
	* @function can-validate.once Once
	* @parent can-validate.validators
	* @description Registers a library with can.validate. The last one registered is the default library.
	* Override the default by changing `_validatorId` to the key of the desired registered library.
	* ```js
	* Validate.once('', {}, 'myVal');
	* ```
	* @param {*} value A value of any type to validate.
	* @param {object} options Validation config object, structure depends on the
	*  validation library used.
	* @param {string} name Optional. The key name of the value.
	*/
	once: function () {
		//!steal-remove-start
		if (!this._validatorId) {
			can.dev.warn('A validator library is required for can.validate to work properly.');
		}
		//!steal-remove-end
		return this.validator().once.apply(this, arguments);
	},

	/**
	* @function can-validate.validate Validate
	* @parent can-validate.validators
	* @description
	* ```js
	* Validate.validate({},{});
	* ```
	* @param {object} values A map of the different objects to validate.
	* @param {object} options Validation config object, structure depends on the
	*  validation library used.
	*/
	validate: function () {
		var validateArgs = arguments;
		//!steal-remove-start
		if (!this._validatorId) {
			can.dev.warn('A validator library is required for can.validate to work properly.');
		}
		if (typeof arguments[0] !== 'object') {
			can.dev.warn('Attempting to pass single value to validate, use can.validator.once instead.');
		}
		//!steal-remove-end
		return this.validator().validate.apply(this, validateArgs);
	}
}, {});

can.validate = Validate;

export default can;
