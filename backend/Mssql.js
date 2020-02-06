const CALL="CALL ";
var sql = require("mssql");
const Core = require('core');
var each = Core.each;
var Mssql = function(configuration){
	if(!configuration)throw new Error('No configuration provided');
	const config = configuration.toJSON();
	this.nonQuery = function(params){
		return new Promise(function(resolve, reject){
			var storedProcedure = params.storedProcedure;
			var parameters = params.parameters;
			var connection = new sql.ConnectionPool(config);
			connection.connect().then(function(connection) {
				var request = new sql.Request(connection);
				setInputs(parameters, request);
				request.execute(storedProcedure).then(resolve).catch(function(err) {
					console.error(err);
					reject(err);
				});
			});
		});
	};
	this.query = function(params){
		return new Promise(function(resolve, reject){
			var storedProcedure = params.storedProcedure;
			var parameters = params.parameters?params.parameters:[];
			var callback= params.callback;
			var connection = new sql.ConnectionPool(config);
			connection.connect().then(function(connection) {
				var request = new sql.Request(connection);
				setInputs(parameters, request);
				request.execute(storedProcedure).then(function(result) {
					callback&&callback(result);//gradually removing this.
					resolve(result);
				}).catch(function(err) {
					console.error(err);
					reject(err);
				});
			})
			.catch(function(err) {
				console.error(err);
				reject(err);
			});
		});
	};
	this.raw = function(definition){
		return new Promise(function(resolve, reject){
			var connection = new sql.ConnectionPool(config);
			connection.connect().then(function(connection) {
				var request = new sql.Request(connection);
				request.query(definition).then(resolve).catch(reject);
			}).catch(reject);
		});
	};
	this.bulkInsert = function(params){
		return new Promise((resolve, reject)=>{
			var table = params.table;
			var callback= params.callback;
			var connection = new sql.ConnectionPool(config);
			connection.connect().then(function(connection) {
				var request = new sql.Request(connection);
				request.bulk(table).then(function(result) {
					callback&&callback(result);
					resolve(result);
				}).catch(function(err) {
					console.error(err.message); 
					reject(err);
				});
			});
		});
	};
	function setInputs(parameters, request){
		each(parameters, function(parameter){
			if(parameter.out)
				request.output(parameter.name, parameter.type, parameter.value);
			else{
				if(parameter.type)
					request.input(parameter.name, parameter.type, parameter.value);
				else 
					request.input(parameter.name, parameter.value);
			}
		});
	}
};
Mssql.sql=sql;
module.exports = Mssql;
