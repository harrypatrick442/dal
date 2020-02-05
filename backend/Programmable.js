const DatabaseTypes=require('./DatabaseTypes');
const Core = require('core');
const StringsHelper = Core.StringsHelper;
const fs = require('fs');
function Programmable(params){
	const databaseType = params.databaseType;
	if(!databaseType)throw new Error('No databaseType provided');
	var definition = params.definition;
	if(!definition)throw new Error('Definition is empty');
	console.log(definition);
	if(!params.name)params.name = getNameFromDefinition(definition);
	this.getProgrammableType = function(){
		params.type
	};
	this.getName= function(){
		return params.name;
	};
	this.getDefinition = function(){
		return definition;
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
	this.toFile = function(){
		return new Promise((resolve, reject)=>{
			fs.writeFile(path, definition, "utf8", (err)=>{
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
/*Programmable.fromSqlReader = function(){
	throwNotImplemented();
};*/
Programmable.fromFile=function(path){
	return new Promise((resolve, reject)=>{
		fs.readFile(path, "utf8", (err, sql)=>{
			if(err){
				reject(err);
				return;
			}
			if(!sql) {
				reject(new Error('Empty definition'));
				return;
			}
			resolve(new Programmable({definition:sql}));
		});
	});
};
function parseDefinition(str, toDo){
	str = str.toLowerCase();
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