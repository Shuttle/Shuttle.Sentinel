Sentinel.DependencyContainer = can.Construct.extend({
    _container: {},

    _guardName: function (name) {
        if (name.indexOf('_') !== 0) {
            throw new Error('All dependency names have to start with an underscore.');
        }
    },

    contains: function (name) {
        this._guardName(name);

        return this._container[name] != undefined;
    },

    resolve: function (name) {
        this._guardName(name);

        if (!this.contains(name) && !this._registerComponent(name)) {
            throw new Error('Could not resolve dependency with \'' + name + '\' as no such implementation has been registered.');
        }

        return this._container[name];
    },

    register: function (name, instance) {
        Sentinel.guard.againstUndefined(name, 'name');
        Sentinel.guard.againstUndefined(instance, 'instance');

        this._guardName(name);

        this._container[name] = instance;
    },

    _registerComponent: function (name) {
        var componentInstanceName;
        var componentTypeName;
        var componentNamespace;

        Sentinel.guard.againstUndefined(name, 'name');

        this._guardName(name);

        componentInstanceName = name.substr(1);
        componentTypeName = componentInstanceName.substr(0, 1).toUpperCase() + componentInstanceName.substr(1);
        componentNamespace =
            name.endsWith('Service')
            ? 'Services'
            : name.endsWith('Repository')
            ? 'Repositories'
            : name.endsWith('Control')
            ? 'Controls'
            : undefined;

        if (!componentNamespace) {
            return false;
        }

        if (Sentinel[componentNamespace][componentInstanceName] != undefined) {
        	this.register(name, Sentinel[componentNamespace][componentInstanceName]);

            Sentinel.logger.debug('[registered service instance] : instance name = \'Sentinel.' + componentNamespace + '.' + name + '\'');

            return true;
        }

        if (Sentinel[componentNamespace][componentTypeName] != undefined) {
        	this.register(name, new Sentinel[componentNamespace][componentTypeName]());

            Sentinel.logger.debug('[registered service instance from type] : instance type = \'Sentinel.' + componentNamespace + '.' + componentTypeName + '\'');

            return true;
        }

        return false;
    }
});

Sentinel.container = new Sentinel.DependencyContainer();

Sentinel.Dependency = {
    resolve: function (name) {
        if (this[name]) {
            return;
        }

        this[name] = Sentinel.container.resolve(name);
    }
};