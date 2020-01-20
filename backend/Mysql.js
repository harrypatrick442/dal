const CALL="CALL ";
var sql = require("mssql");
const Core = require('core');
const mysql  = require('node-mysql');
var Mssql = function(configuration){
	if(!configuration)throw new Error('No configuration provided');
	const config = configuration.toJSON();
	this.nonQuery = function(params){
		throwNotImplemented();
	};
	this.query = function(params){
		throwNotImplemented();
	};
	this.raw = function(definition){
		throwNotImplemented();
	};
	this.bulkInsertObjects = function(tableName, objectsArray, connection){
		if(objectsArray.length<1)return new Promise((resolve)=>{ resolve();});
		var keys = Object.keys(objectArray[0]);
		var values = objectArray.map( obj => keys.map( key => obj[key]));
		return bulkInsert(tableName, keys, values, connection);
	};
	this.bulkInsert = bulkInsert;
	function bulkInsert(tableName, columns, rows, connection){
		return new Promise((resolve, reject)=>{
			var sql = 'INSERT INTO ' + tableName + ' (' + columns.join(',') + ') VALUES ?';
			var hadConnection = connection?true:false;
			if(!hadConnection)connection = mysql.createConnection();
			connection.query(sql, [rows], function (error, results, fields) {
				if(!hadConnection)connection.end();
				if (error){ reject(error);return;}
				resolve(results);
			});
		});
	};
};
Mssql.sql=sql;
module.exports = Mssql;
