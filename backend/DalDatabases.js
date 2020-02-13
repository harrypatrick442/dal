module.exports= new(function(configuration){
	const Mysql = require('./Mysql');
	const Mssql = require('./Mssql');
	const {DatabaseTypes}=require('enums');
	const DatabaseConfiguration = require('configuration').DatabaseConfiguration;
	this.createDatabase = function(currentDatabaseConfiguration, name){
		switch(currentDatabaseConfiguration.getDatabaseType()){
			case DatabaseTypes.MYSQL:
				var statement = 'CREATE DATABASE '+name+';';
				return _createDatabaseMysql(currentDatabaseConfiguration, name, statement);
			case DatabaseTypes.MSSQL:
				var statement = "CREATE DATABASE "+name+';';
				return _createDatabaseMssql(currentDatabaseConfiguration, name, statement);
			default:
				throwNotImplemented();
		}
	};
	this.createOrRecreateDatabase = function(currentDatabaseConfiguration, name){
		return new Promise((resolve, reject)=>{
			switch(currentDatabaseConfiguration.getDatabaseType()){
				case DatabaseTypes.MYSQL:
					deleteDatabase(currentDatabaseConfiguration, name).then(()=>{
						var statement = "CREATE DATABASE "+name+';';
						_createDatabaseMysql(currentDatabaseConfiguration, name, statement).then(resolve).catch(reject);
					}).catch(reject);
					return;
				case DatabaseTypes.MSSQL:
					var statement = "If(db_id(N'"+name+"') IS NOT NULL)DROP DATABASE "+name+"; CREATE DATABASE "+name+';';
					_createDatabaseMssql(currentDatabaseConfiguration, name, statement).then(resolve).catch(reject);
					return;
				default:
					throwNotImplemented();
			}
		});
	};
	
	this.deleteDatabase = deleteDatabase;
	function deleteDatabase(currentDatabaseConfiguration, name){
		switch(currentDatabaseConfiguration.getDatabaseType()){
			case DatabaseTypes.MSSQL:
				var statement = "If(db_id(N'"+name+"') IS NOT NULL)DROP DATABASE "+name+';';
				var dalMssql = new Dal(currentDatabaseConfiguration);
				return dalMssql.raw(statement);
			case DatabaseTypes.MYSQL:
				var statement = "DROP DATABASE IF EXISTS "+name+ ';';
				var dalMysql = new Mysql(currentDatabaseConfiguration);
				console.log(statement);
				return dalMysql.raw(statement);
		}
	};
	function _createDatabaseMssql(currentDatabaseConfiguration, name, statement){
		return new Promise((resolve, reject)=>{
			var dal = new Mssql(currentDatabaseConfiguration);
			dal.raw(statement).then(()=>{
				resolve(new DatabaseConfiguration({user:currentDatabaseConfiguration.getUser(), password:currentDatabaseConfiguration.getPassword(),
				server:currentDatabaseConfiguration.getServer(), database:name}));
			}).catch(reject);
		});
	}
	function _createDatabaseMysql(currentDatabaseConfiguration, name, statement){
		return new Promise((resolve, reject)=>{
			var dal = new Mysql(currentDatabaseConfiguration);
			dal.raw(statement).then(()=>{
				resolve(new DatabaseConfiguration({user:currentDatabaseConfiguration.getUser(), password:currentDatabaseConfiguration.getPassword(),
				server:currentDatabaseConfiguration.getServer(), database:name, databaseType:currentDatabaseConfiguration.getDatabaseType()}));
			}).catch(reject);
		});
	}
})();
