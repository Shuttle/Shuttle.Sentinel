import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import api from '~/api';
import each from 'can-util/js/each/';

const Map = DefineMap.extend(
    'user',
    {
        seal: false
    },
    {
        id: 'string',
        username: 'string',
        dateRegistered: 'date',
        registeredBy: 'string'
    });

const Model = DefineMap.extend(
    'user-model',
    {
        get () {
            return new Promise((resolve, reject) => {
                api.get('users')
                    .then(function(response) {
                        const result = new DefineList();

                        each(response.data, (item) => { result.push(new Map(item)) });
                        
                        resolve(result);
                    })
                    .catch(reject);
            });
        }
    });

export default new Model();