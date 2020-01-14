module.exports = new (function(params){
	const path = require('path');
	const DalDatabases = require('./DalDatabases');
	const Programmable = require('./Programmable');
	const Core = require('core');
	const DalLog = require('./DalLog');
	const Iterator = Core.Iterator;
	const DalProgrammability =require('./DalProgrammability');
	const DalTables =require('./DalTables');
	this.build = function(params){
		return new Promise((resolve, reject)=>{
			const programmablePaths = params.programmablePaths;
			if(!programmablePaths)throw new Error('No programmablePaths provided');
			const shardHost = params.shardHost;
			if(!shardHost)throw new Error('No host provided');
			const name = params.name;
			if(!name)throw new Error('No name provided');
			const createShard = params.createShard;
			if(!createShard)throw new Error('No createShard provided');
			const tables = params.tables;
			if(!tables)throw new Error('No tables provided');
			var newDatabaseConfiguration;
			createDatabase(shardHost.getDatabaseConfiguration(), name).then((newDatabaseConfigurationIn)=>{
				createTables(newDatabaseConfigurationIn, tables).then(()=>{
					newDatabaseConfiguration = newDatabaseConfigurationIn;
					populateDatabaseWithProgrammables(programmablePaths, new DalProgrammability(newDatabaseConfigurationIn)).then(()=>{
						createShard(newDatabaseConfigurationIn, shardHost).then((shard)=>{
							shard.update().then(()=>{
								resolve(shard);
							}).catch(error);
						}).catch(error);
					}).catch(error);	
				}).catch(error);	
			}).catch(error);
			
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
		return DalDatabases.createDatabase(currentDatabaseConfiguration, name);
	}
	function createTables(databaseConfiguration, tables){
		return new Promise((resolve, reject)=>{
			var iterator = new Iterator(tables);
			next();
			function next(){
				if(!iterator.hasNext()){
					resolve();
					return;
				}
				var table = iterator.next();
				DalTables.createTable(databaseConfiguration, table).then(next).catch(reject);
			}
		});
	}
	function populateDatabaseWithProgrammables(programmablePaths, dalProgrammability){
		return new Promise((resolve, reject)=>{
				console.log('getProgrammables');
			getProgrammables(programmablePaths).then((programmables)=>{
				var iterator = new Iterator(programmables);
				console.log('iterator');
				nextProgrammable();
				function nextProgrammable(){
				console.log('nextProgrammable');
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
	function getProgrammables(programmablePaths){
		return new Promise((resolve, reject)=>{
			var programmables =[];
			var iteratorProgrammablePaths = new Iterator(programmablePaths);
			next();
			function next(){
					console.log('n');
				if(!iteratorProgrammablePaths.hasNext()){
					console.log('next');
					resolve(programmables);
					return;
				}
				var programmablePath = iteratorProgrammablePaths.next();
				Programmable.fromFile(programmablePath).then((programmable)=>{
					console.log('n2');
					programmables.push(programmable);
					next();
				}).catch(reject);
			}
		});
	}
})();