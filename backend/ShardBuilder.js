module.exports = new (function(params){
	const path = require('path');
	const DalDatabases = require('./DalDatabases');
	const Programmable = require('./Programmable');
	const Core = require('core');
	const DalLog = require('./DalLog');
	const Iterator = Core.Iterator;
	const DalProgrammables =require('./DalProgrammables');
	this.build = function(params){
		const programmablePaths = params.programmablePaths;
		if(!programmablePaths)throw new Error('No programmablePaths provided');
		const shardHost = params.shardHost;
		if(!shardHost)throw new Error('No host provided');
		const name = params.name;
		if(!name)throw new Error('No name provided');
		const createShard = params.createShard;
		if(!createShard)throw new Error('No createShard provided');
		return new Promise((resolve, reject)=>{
			var newDatabaseConfiguration;
			createDatabase(shardHost.getDatabaseConfiguration(), name).then((newDatabaseConfigurationIn)=>{
				newDatabaseConfiguration = newDatabaseConfigurationIn;
				populateDatabaseWithProgrammables(programmablePaths, new DalProgrammables(newDatabaseConfigurationIn)).then(()=>{
					createShard(newDatabaseConfigurationIn, shardHost).then((shard)=>{
						shard.update().then(()=>{
							resolve(shard);
						}).catch(error);
					}).catch(reject);
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
	function populateDatabaseWithProgrammables(programmablePaths, dalProgrammables){
		return new Promise((resolve, reject)=>{
			getProgrammables(programmablePaths).then((programmables)=>{
				var iterator = new Iterator(programmables);
				nextProgrammable();
				function nextProgrammable(){
					if(!iterator.hasNext()){
						resolve();
						return;
					}
					var programmable = iterator.next();
					dalProgrammables.updateProgrammable(programmable).then(()=>{
						populateDatabaseWithProgrammable(programmablePaths, dalProgrammables).then(nextProgrammable).catch(reject);
					}).catch(reject);
				}
			}).catch(reject);
		});
	}
	function populateDatabaseWithProgrammable(){
		return DalProgrammability.executeDefinition(programmable.getCreateDefinition());
	}
	function getProgrammables(programmablePaths){
		return new Promise((resolve, reject)=>{
			var programmables =[];
			var iteratorProgrammablePaths = new Iterator(programmablePaths);
			next();
			function next(){
				if(!iteratorProgrammablePaths.hasNext()){
					resolve(programmables);
					return;
				}
				var programmablePath = iteratorProgrammablePaths.next();
				Programmable.fromFile(programmablePath).then((programmable)=>{
					programmables.push(programmable);
					next();
				}).catch(reject);
			}
		});
	}
})();