
const fs = require('fs'),path=require('path');

const cs_files = fs.readdirSync(__dirname).filter(function(fn){return fn.endsWith('.cs')});
const txt_files = fs.readdirSync(__dirname).filter(function(fn){return fn.endsWith('.txt')});
const cs_paths = cs_files.map(function(fn){ return path.join(__dirname,fn)});
const txt_paths = txt_files.map(function(fn){ return path.join(__dirname,fn)});
const cs_data  = cs_paths.map(function(fn){return fs.readFileSync(fn,'utf8')});
const txt_data  = txt_paths.map(function(fn){return fs.readFileSync(fn,'utf8')});
const cs_outpaths = cs_files.map(function(fn){ return path.join(__dirname,'..',fn)});

const dir = {};

cs_files.forEach(function (fn,index){
    dir[fn]={
        path : cs_paths[index],
        name : fn,
        original : cs_data[index],
        fixed : cs_data[index],
        outpath : cs_outpaths[index]
    };
});

txt_files.forEach(function (fn,index){
    dir[fn]={
        path : txt_paths[index],
        name : fn,
        original : txt_data[index],
        fixed : txt_data[index]
    };
});

function fix(src) {
    const splits = src.split('/*>[');
    if (splits.length===1) return src;

    return splits.map(function (line,index){
        if (index===0) return line;
        const parts = line.split(']<*/');
        if (parts.length===2) {
            const fn = parts[0];
            const fixed = dir[fn].fixed;
            return fixed + parts[1] ;
        }
        return line;
    }).join('');
}

cs_files.forEach(function(fn,ix){
   dir[fn].fixed = fix(dir[fn].fixed);
   if (fs.existsSync(dir[fn].outpath)) {
        fs.writeFileSync(dir[fn].outpath,dir[fn].fixed);
   }
});

 
