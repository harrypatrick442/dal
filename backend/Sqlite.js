const sqlite3 = require('sqlite3');
module.exports = function(params){
	if(!params)params={};
	var readOnly = params.readOnly;
	var traceCallback = params.traceCallback;
	var profileCallback = params.profileCallback;
	var path = params.path;
	var _db,disposed = false;
	this.all = function(sql, parameters){
		return new Promise((resolve, reject)=>{
			getDb().then((db)=>{
				db.all(sql, paraemters, function(err, rows) {
					if(err){
						reject(err);
						return;
					}
					resolve(rows);
				});	
			}).catch(reject);
		});
	};
	this.each = function(sql, parameters, callback){
		return new Promise((resolve, reject)=>{
			var _done=false;
			getDb().then((db)=>{
				db.each(sql, parameters, function(err, row) {
					if(err){
						done(err);
						return;
					}
					callback(row);
				}, done);	
			}).catch(reject);
			function done(err, nRows){
				if(_done)return;
				_done=true;
				if(err)reject(err);
				else resolve(nRows);
			}
		});
	};
	this.exec = function(sql){
		return new Promise((resolve, reject)=>{
			getDb().then((db)=>{
				db.exec(sql, function(err) {
					if(err){
						reject(err);
						return;
					}
					resolve();
				});	
			}).catch(reject);
		});
	};
	this.beginGet = function(sql, parameters, callback){
		return new Promise((resolve, reject)=>{
			getDb().then((db)=>{
					var stmt = db.prepare(sql, parameters,(err)=>{
					if(err){
						reject(err);
						return;
					}
					stmt.get(function(err, row) {
						if(err){
							reject(err);
							return;
						}
						resolve(new GetHandle(stmt, callback));
					});
				}, done);	
			}).catch(reject);
		});
	};
	this.run = function(sql, parameters){
		return new Promise((resolve, reject)=>{
			getDb().then((db)=>{
				db.run(sql, parameters);
			}).catch(reject);
		});
	};
	function getDb(){
		return new Promise((resolve, reject)=>{
			checkNotDisposed();
			if(_db){
				resolve(_db);
				return;
			}
			console.log(path);
			if(traceCallback||profileCallback){
				console.log('verbose');
				sqlite3.verbose();
			}
			_db = new sqlite3.Database(path, sqlite3.OPEN_READWRITE, (err)=>{
				if(err){reject(err);return;}
				if(traceCallback)
					_db.on('trace', traceCallback);
				if(profileCallback)
					_db.on('profile', profileCallback);
				resolve(_db);
			});
		});
	}
	function dispose(){
		if(disposed)return;
		disposed=true;
		db.close();
	}
	function checkNotDisposed(){
		if(disposed)throw new Error('Already disposed');
	}
	function GetHandle(stmt){
		var ended=false;
		this.finalize = function(){
			if(ended)return;
			ended=true;
			stmt.finalize();
		};
		this.next = function(){
			if(ended)throw new Error('Already finalized');
			stmt.get();
		};
	}
};