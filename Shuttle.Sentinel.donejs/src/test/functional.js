import F from 'funcunit';
import QUnit from 'steal-qunit';

F.attach(QUnit);

QUnit.module('shuttle-sentinel-donejs functional smoke test', {
  beforeEach() {
    F.open('../development.html');
  }
});

QUnit.test('shuttle-sentinel-donejs main page shows up', function() {
  F('title').text('shuttle-sentinel-donejs', 'Title is set');
});
