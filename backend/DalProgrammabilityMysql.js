const {DatabaseTypes}=require('enums');
const Mysql =require('./Mysql');
const Programmable = require('./Programmable'), ProgrammableTypes = require('./ProgrammableTypes');
const {throwNotImplemented, Iterator}= require('core');
module.exports= function(configuration){
	const dal = new Mysql(configuration);
	const {throwNotImplemented} = require('core');
	this.updateProgrammable = function(programmable){
		return new Promise((resolve, reject)=>{
			if(programmable.getDatabaseType()!==DatabaseTypes.MYSQL)throw new Error('Programmable was not for DatabaseType MSSQL');
			deleteProgrammable(programmable).then(()=>{
				dal.raw(programmable.getCreateDefinition()).then(resolve).catch(reject)
			}).catch(reject);
		});
	};
	this.deleteProgrammable = deleteProgrammable;
	function deleteProgrammable(programmable){
		return dal.raw(programmable.getDeleteIfExists());
	};
	this.readProgrammables = function(){
		return new Promise((resolve, reject)=>{
			const procedures=[];
			dal.raw(`SELECT ROUTINE_NAME as name, 
							ROUTINE_TYPE as type 
					FROM INFORMATION_SCHEMA.ROUTINES
					WHERE ROUTINE_SCHEMA = '`+configuration.getDatabase()
					+`' AND (ROUTINE_TYPE = 'PROCEDURE' OR ROUTINE_TYPE = 'FUNCTION')`)
			.then((rows)=>{
					const iterator = new Iterator(rows);
					next();
					function next(){
						if(!iterator.hasNext()){
							resolve(procedures);
							return;
						}
						const row = iterator.next();
						const programmableType =ProgrammableTypes.parse(row.type);
						const showCreateType=getShowCreateType(programmableType);
						getDefinitionUsingShowCreate(showCreateType, row.name).then((definition)=>{
							row.definition = definition;
							if(!row.definition)throw new Error('Failed to retreive programmable for procedure '+row.name);
							row.databaseType = DatabaseTypes.MYSQL;
							row.type = programmableType;
							procedures.push(new Programmable(row));
							next();
						}).catch(reject);
					}
			}).catch(reject);
		});
		function getDefinitionUsingShowCreate(showCreateType, procedureName){
			return new Promise((resolve, reject)=>{
				dal.raw('show create '+showCreateType+' '+procedureName).then((definitions)=>{
					var definition = definitions[0];
					Object.keys(definition).forEach((key)=>{
						definition[key.toLowerCase()]=definition[key];
					});
					definition = definition['create '+showCreateType];
					resolve(definition);
				}).catch(reject);
			});
			
		}
		function getShowCreateType(programmableType){
			switch(programmableType){
				case ProgrammableTypes.STORED_PROCEDURE:
					return 'procedure';
				case ProgrammableTypes.SCALAR_FUNCTION:
					return 'function';
				default:
					throwNotImplemented();
					break;
			}
		}
	};
};
