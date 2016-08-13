import QUnit from 'steal-qunit';
import { ViewModel } from './add';

// ViewModel unit tests
QUnit.module('sentinel/role/add');

QUnit.test('Has message', function(){
  var vm = new ViewModel();
  QUnit.equal(vm.attr('message'), 'This is the sentinel-role-add component');
});
