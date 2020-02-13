const Dal = require('./backend/Dal');
var classNames =['getIntsTable', 'DalProgrammability','DalDatabases',  'DalTables', 'DalTypes',
 'Mssql', 'Mysql', 'Sqlite', 'ShardBuilder', 'Table','TableColumn','TableColumnTypes', 'ProgrammableTypes', 'Programmable', 'IndexTypes'];
classNames.forEach((className)=>{
	Object.defineProperty(Dal, className, {
	  get: function(){return require('./backend/'+className)}
	});
});
module.exports= Dal;