const Dal = require('./backend/Dal');
var classNames =['getIntsTable', 'DalDatabases', 'DalProgrammability', 'DalTables', 'DalTypes', 'DatabaseTypes',
 'Mssql', 'Mysql', 'Sqlite', 'ShardBuilder', 'Table','TableColumn','TableColumnTypes', 'ProgrammableTypes', 'Programmable', 'IndexTypes'];
classNames.forEach((className)=>{
	Object.defineProperty(Dal, className, {
	  get: function(){return require('./backend/'+className)}
	});
});
module.exports= Dal;