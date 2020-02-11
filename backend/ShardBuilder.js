module.exports = new (function(params){
	const path = require('path');
	const DalDatabases = require('./DalDatabases');
	const Programmable = require('./Programmable');
	const Core = require('core');
	const DalLog = require('./DalLog');
	const Iterator = Core.Iterator;
	const DalProgrammability =require('./DalProgrammability');
	const DalTables =require('./DalTables');
	const DalTypes = require('./DalTypes');
	this.build = function(params){
		return new Promise((resolve, reject)=>{
			const programmablePaths = params.programmablePaths, shardHost = params.shardHost,
			name = params.name, createShard = params.createShard, tables = params.tables, tableTypes = params.tableTypes;
			if(!programmablePaths)throw new Error('No programmablePaths provided');
			if(!shardHost)throw new Error('No host provided');
			if(!name)throw new Error('No name provided');
			if(!createShard)throw new Error('No createShard provided');
			if(!tables)throw new Error('No tables provided');
			if(tableTypes===undefined)throw new Error('No tableTypes provided');
			var newDatabaseConfiguration;
			console.log(shardHost.getDatabaseConfiguration().toJSON());
			const existingDatabaseConfiguration = shardHost.getDatabaseConfiguration();
			createDatabase(existingDatabaseConfiguration, name).then((newDatabaseConfigurationIn)=>{
				createTables(newDatabaseConfigurationIn, tables).then(()=>{
					newDatabaseConfiguration = newDatabaseConfigurationIn;
					if(tableTypes&&tableTypes.length>=0)
						createTableTypes(newDatabaseConfigurationIn, tableTypes).then(part2).catch(error);	
					else part2();
				}).catch(error);	
			}).catch(error);
			function part2(){
				populateDatabaseWithProgrammables(programmablePaths, new DalProgrammability(newDatabaseConfiguration), existingDatabaseConfiguration.getDatabaseType()).then(()=>{
					createShard(newDatabaseConfiguration, shardHost).then((shard)=>{
						if(shard.update){
							shard.update().then(()=>{
								resolve(shard);
							}).catch(error);
							return;
						}
						resolve(shard);
					}).catch(error);
				}).catch(error);
			}
			function error(err){
				if(newDatabaseConfiguration){
					DalDatabases.deleteDatabase(newDatabaseConfiguration).then(doReject).catch((err)=>{
						DalLog.error(new Error('Error deleting database '+newDatabaseConfiguration.getDatabase()+' while cleaning up after creating new shard faile'));
						doReject();
					});
					return;
				}
				doReject();
				function doReject(){
					reject(err);
				}
			}
		});
	};
	function createDatabase(currentDatabaseConfiguration, name){
		return DalDatabases.createOrRecreateDatabase(currentDatabaseConfiguration, name);
	}
	function createTables(existingDatabaseConfiguration, tables){
		return createTX(DalTables.createTable, existingDatabaseConfiguration, tables)
	}
	function createTableTypes(existingDatabaseConfiguration, tableTypes){
		return createTX(DalTypes.createTableTypes, existingDatabaseConfiguration, tableTypes)
	}
	function createTX(func, existingDatabaseConfiguration, tables){
		return new Promise((resolve, reject)=>{
			var iterator = new Iterator(tables);
			next();
			function next(){
				if(!iterator.hasNext()){
					resolve();
					return;
				}
				var table = iterator.next();
				func(existingDatabaseConfiguration, table).then(next).catch(reject);
			}
		});
	}
	function populateDatabaseWithProgrammables(programmablePaths, dalProgrammability, databaseType){
		return new Promise((resolve, reject)=>{
			getProgrammables(programmablePaths, databaseType).then((programmables)=>{
				var iterator = new Iterator(programmables);
				nextProgrammable();
				function nextProgrammable(){
					if(!iterator.hasNext()){
						resolve();
						return;
					}
					var programmable = iterator.next();
					dalProgrammability.updateProgrammable(programmable).then(nextProgrammable).catch(reject);
				}
			}).catch(reject);
		});
	}
	function getProgrammables(programmablePaths, databaseType){
		return new Promise((resolve, reject)=>{
			var programmables =[];
			var iteratorProgrammableOrProgrammablePaths = new Iterator(programmablePaths);
			next();
			function next(){
				if(!iteratorProgrammableOrProgrammablePaths.hasNext()){
					resolve(programmables);
					return;
				}
				var programmableOrProgrammablePath = iteratorProgrammableOrProgrammablePaths.next();
				if(programmableOrProgrammablePath instanceof Programmable){
					programmables.push(programmable);
					next();
					return;
				}
				Programmable.fromFile(programmableOrProgrammablePath, null, databaseType).then((programmable)=>{
					programmables.push(programmable);
					next();
				}).catch(reject);
			}
		});
	}
})();