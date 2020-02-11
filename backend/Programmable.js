const DatabaseTypes=require('./DatabaseTypes');
const Core = require('core');
const StringsHelper = Core.StringsHelper;
const fs = require('fs'), path = require('path');
const ProgrammableTypes = require('./ProgrammableTypes');
function Programmable(params){
	const databaseType = params.databaseType;
	if(!databaseType)throw new Error('No databaseType provided');
	const programmableType = params.type;
	if(!programmableType)throw new Error('No programmableType provided');
	var definition = params.definition;
	if(!definition)throw new Error('Definition is empty');
	if(!params.name)params.name = getNameFromDefinition(definition);
	this.getProgrammableType = function(){
		return programmableType;
	};
	this.getName= function(){
		return params.name;
	};
	this.getDefinition = function(){
		return definition;
	};
	this.getDatabaseType = function(){
		return databaseType;
	};
	this.getCreateDefinition = getCreateDefinition;
	this.getAlterDefinition = function(){
		if(databaseType===DatabaseTypes.MYSQL)
			return getCreateDefinition();
		return parseDefinition(definition, 'alter');
	};
	this.getCreateOrAlterDefinition=function(){
		if(databaseType===DatabaseTypes.MYSQL)
			return getCreateDefinition();
		return parseDefinition(definition, 'create or alter');
	};
	this.getDeleteIfExists = function(){
		var programmableTypeStr = ProgrammableTypes.toString(programmableType, databaseType);
		if(databaseType===DatabaseTypes.MYSQL){
			return 'drop '+programmableTypeStr+' if exists '+params.name;
		}
		throwNotImplemented();
	};
	this.toFile = function(filePath){
		return new Promise((resolve, reject)=>{
			fs.writeFile(filePath, definition, "utf8", (err)=>{
				if(err){
					reject(err);
					return;
				}
				resolve();
			});
		});
	};
	function getCreateDefinition(){
		return parseDefinition(definition, 'create');
	}
};
module.exports = Programmable;
Programmable.fromFile=function(filePath, programmableType, databaseType){
	return new Promise((resolve, reject)=>{
		fs.readFile(filePath, "utf8", (err, sql)=>{
			if(err){
				reject(err);
				return;
			}
			if(!sql) {
				reject(new Error('Empty definition'));
				return;
			}
			if(!programmableType){
				programmableType = getProgrammableTypeFromDefinition(sql, databaseType);
			}
			const fileNameNoExtension = path.basename(filePath, path.extname(filePath));
			const programmable = new Programmable({definition:sql, name:fileNameNoExtension, 
				type:programmableType, databaseType:databaseType});
			checkStoredProcedureMatchesFileName(fileNameNoExtension, programmable);
			resolve(programmable);
		});
	});
};
function checkStoredProcedureMatchesFileName(fileName, programmable){
	var storedProcedureName = getProgrammableNameFromDefinition(programmable);
	if(storedProcedureName!==fileName)throw new Error('The file name "'+fileName+'" did not match the stored procedure name "'+storedProcedureName+'"');
}
function getProgrammableTypeFromDefinition(sql, databaseType){
	switch(databaseType){
		case DatabaseTypes.MYSQL:
			const regExp = new RegExp('DEFINER *= *`[a-zA-Z0-9_-]+` *@ *`(?:%|[a-zA-Z0-9_-]+)` +([a-zA-Z]+) `[a-zA-Z0-9_-]+` *[(]','i');
			const res = regExp.exec(sql);
			return ProgrammableTypes.parse(res[1], databaseType);
		default:
			throwNotImplemented();
	}
}
function getProgrammableNameFromDefinition(programmable){
	const databaseType = programmable.getDatabaseType();
	switch(databaseType){
		case DatabaseTypes.MYSQL:
			const programmableType = programmable.getProgrammableType();
			const regExp = new RegExp(ProgrammableTypes.toString(programmableType, databaseType)+' +`([a-zA-Z0-9_-]+)`\(\)','i');
			const res = regExp.exec(programmable.getCreateDefinition());
			return res[1];
		default:
			throwNotImplemented();
	}
}
function parseDefinition(str, toDo){
	str = StringsHelper.replaceAll(str, "old\\s+value", "new value");
	str = StringsHelper.replaceAll(str, "create\\s+view", toDo+" view");
	str = StringsHelper.replaceAll(str, "create\\s+function", toDo+" function");
	str =  StringsHelper.replaceAll(str, "create\\s+procedure", toDo+" procedure");
	str = StringsHelper.replaceAll(str, "alter\\s+view", toDo+" view");
	str = StringsHelper.replaceAll(str, "alter\\s+function", toDo+" function");
	str = StringsHelper.replaceAll(str, "create\\s+definer", toDo+" definer");
	str = StringsHelper.replaceAll(str, "alter\\s+definer", toDo+" definer");
	return StringsHelper.replaceAll(str, "alter\\s+procedure", toDo+" procedure");
}
function getNameFromDefinition(definition){
	return '';
}
function throwNotImplemented(){
	throw new Error('Not implemented');
}