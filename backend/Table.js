const Core = require('core');
const TableColumnTypes = require('./TableColumnTypes');
const DatabaseTypes = require('./DatabaseTypes');
const Table = function(params){
	var name = params.name;
	if(!name)throw new Error('No name provided');
	var columns = params.columns;
	if(!columns)throw new Error('No columns provided');
	this.getCreate = getCreate;
	this.getCreateTableType = getCreateTableType;
	this.getUpdate = getUpdate;
	function getUpdate(){
		var str = "IF(object_id('"+name+"') is not null) begin drop table "+name+" end;";
		str+=getCreate();
		return str;
	}
	function getCreate(databaseType){
		if(!databaseType)throw new Error('No databaseType provided');
		var str = 'CREATE TABLE '+name;
		return _getCreate(str, databaseType);
	}
	function getCreateTableType(){
		var str = 'CREATE TYPE [dbo].['+name+'] AS TABLE';
		return _getCreate(str);
	}
	function _getCreate(str, databaseType){
		str+='(';
		var first = true;
		var primaryKeyColumns=[];
		const columnBracketing = getColumnBracketing(databaseType);
		const uniqueColumns=[], indices=[];
		columns.forEach((column)=>{
			if(first)first = false;
			else str+=',';
			str+=columnBracketing[0]+column.getName()+columnBracketing[1]+' '+column.getType()+getPrecisionScaleLength(column)+' ';
			str+=column.getNullable()?' NULL':' NOT NULL';
			if(column.getAutoIncrement())
				str+=getAutoIncrement(databaseType);
			if(column.getUnique()){
				var includeInColumnAndUnique = getUnique(databaseType);
				if(includeInColumnAndUnique[0]){
					str+=includeInColumnAndUnique[1];
				}
			}
			if(column.getPrimaryKey())
				primaryKeyColumns.push(column);
			if(column.getIndex()){
				indices.push(new Index( getIndexName(name, column.getName(), column.getIndex()), column.getName(), column.getIndex()));
			}
		});
		if(primaryKeyColumns.length>0){
			str+=', PRIMARY KEY(';
			var first = true;
			primaryKeyColumns.forEach((primaryKeyColumn)=>{
				if(first)first = false;
				else str+=',';
				str+=columnBracketing[0]+primaryKeyColumn.getName()+columnBracketing[1];
			});
			str+=')';
		}
		if(uniqueColumns.length>0){
			str+=', UNIQUE(';
			var first = true;
			uniqueColumns.forEach((uniqueColumn)=>{				
				if(first)first = false;
				else str+=',';
				str+=columnBracketing[0]+uniqueColumn+columnBracketing[1];
			});
			str+=')';
		}
		if(indices.length>0){
			str+=', INDEX(';
			var first = true;
			indices.forEach((index)=>{				
				if(first)first = false;
				else str+=',';
				str+=columnBracketing[0]+index.getColumn()+columnBracketing[1];
			});
			str+=')';
		}
		str +=');';
		//str+=createIndices(indices, databaseType);
		console.log(str);
		return str;
	}
	function createIndices(indices, databaseType){
		var str='';
		switch(databaseType){
			case DatabaseTypes.MYSQL:
				indices.forEach((index)=>{
					str+='CREATE INDEX '+index.getName()+' ON '+name+'('+index.getColumn()+');';
				});
				break;
			case DatabaseTypes.MSSQL:
				throwNotImplemented();
			default:throwNotImplemented();
		}
		return str;
	}
	function getPrecisionScaleLength(column){
		var type = column.getType();
		if(type===TableColumnTypes.DECIMAL){
			var precision = column.getPrecision();
			if(precision===undefined||precision===null)throw new Error('Precision not supplied for '+type);
			var scale = column.getScale();
			if(scale===undefined||scale===null)throw new Error('Scale not supplied for '+type);
			return '('+preicsion+','+scale+')';
		}
		var length = column.getLength();
		if(length===undefined||length===null)return '';
		return '('+length+')';
	}
	function getAutoIncrement(databaseType){
		switch(databaseType){
			case DatabaseTypes.MSSQL:
				return ' IDENTITY(1,1) ';
			case DatabaseTypes.MYSQL:
				return ' AUTO_INCREMENT ';
			default:
				throwNotImplemented();
		}
	}
	function getUnique(databaseType){
		switch(databaseType){
			case DatabaseTypes.MSSQL:
				return [false, ' UNIQUE '];
			case DatabaseTypes.MYSQL:
				return [true, ' UNIQUE '];
			default:
				throwNotImplemented();
		}
	}
	function getColumnBracketing(databaseType){
		switch(databaseType){
			case DatabaseTypes.MYSQL:
				return ['`','`'];
			default:
				return ['[',']'];
		}
	}
	function Index(name, column, indexType){
		if(!name)throw new Error('No name provided');
		if(!column)throw new Error('No column provided');
		this.getColumn = function(){
			return column;
		};
		this.getName = function(){
			return name;
		};
	}
	function getIndexName(tableName, columnName, indexType){
		return 'IX_'+tableName+'_'+columnName;
	}
};

module.exports = Table;
module.exports = Table;