const CALL="CALL ";
const Core = require('core');
const mysql  = require('mysql2');
var Mssql = function(configuration){
	if(!configuration)throw new Error('No configuration provided');
	const config = configuration.toJSON();
	this.call = function(params, connection){
		return new Promise((resolve, reject)=>{
			var storedProcedure = params.storedProcedure;
			if(!storedProcedure)throw new Error('No storedProcedure property provided');
			var parameters = params.parameters;
			var hadConnection = connection?true:false;
			if(!hadConnection){
				createConnection().then(part2).catch(reject);
			}
			else {part2(connection);}
			function part2(connection){
				var str = `CALL ${ storedProcedure }(`;
				var len = parameters.length;
				if(len-- >0){
					str+='?';
					while(len-- >0){
						str+=',?';
					}
				}
				str+=')';
				connection.query(str, parameters, function (error, results, fields) {
					if(!hadConnection)endConnection(connection);
					if (error){ reject(error);return;}
					resolve(results);
				});
			}
		});
	};
	this.bulkInsertObjects = function(tableName, objectsArray, connection){
		if(objectsArray.length<1)return new Promise((resolve)=>{ resolve();});
		var keys = Object.keys(objectsArray[0]);
		var values = objectsArray.map( obj => keys.map( key => obj[key]));
		return bulkInsert(tableName, keys, values, connection);
	};
	this.bulkInsert = bulkInsert;
	this.raw = raw;
	function bulkInsert(tableName, columns, rows, connection){
		return new Promise((resolve, reject)=>{
			var sql = 'INSERT INTO ' + tableName + ' (' + columns.join(',') + ') VALUES ?';
			var hadConnection = connection?true:false;
			if(!hadConnection){
				createConnection().then(part2).catch(reject);
			}
			else {part2(connection);}
			function part2(connection){
				connection.query(sql, [rows], function (error, results, fields) {
					if(!hadConnection)endConnection(connection);
					if (error){ reject(error);return;}
					resolve(results);
				});
			}
		});
	};
	function raw(sql, values, connection){
		return new Promise((resolve, reject)=>{
			var hadConnection = connection?true:false;
			if(!hadConnection){
				createConnection().then(part2).catch(reject);
			}
			else {part2(connection);}
			function part2(connection){
				connection.query({sql:sql, values:values}, function (error, results, fields) {
					if(!hadConnection)endConnection(connection);
					if (error){ reject(error);return;}
					resolve(results);
				});
			}
		});
	};
	function createConnection(){
		return new Promise((resolve, reject)=>{
			var _done =false;
			const obj = {
				host: '192.168.0.33',//configuration.getServer(),
				user: configuration.getUser(),
				password: configuration.getPassword(),
				database:configuration.getDatabase(),
				debug: false,
				typeCast: typeCast
			};
			const port = configuration.getPort();
			if(port){
				obj.port = port;
			}
			const connection =  mysql.createConnection(obj);
			connection.connect();
			connection.on('error', done);
			done();
			function done(err){
				if(_done)return;
				_done = true;
				if(err)return (err);
				else	resolve(connection);
			}
		});
	}
	function endConnection(connection){ connection.end();}
	function typeCast( field, useDefaultTypeCasting ) {
		if ( ( field.type === "BIT" ) && ( field.length === 1 ) ) {
			var bytes = field.buffer();
			return( bytes[ 0 ] === 1 );
		}
		return( useDefaultTypeCasting() ); 
	}
};
module.exports = Mssql;
