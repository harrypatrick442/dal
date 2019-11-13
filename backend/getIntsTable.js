const sql = require('mssql');

const each = require('core').each;
const VALUE='value';
module.exports = function(ints){
	var intsTable = new sql.Table();
	intsTable.columns.add(VALUE,sql.Int);
	each(ints, function(i){
		intsTable.rows.add(i);
	});
	return intsTable;
};