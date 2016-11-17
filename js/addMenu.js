function addMenu(){
  //点击大的新建菜单
  if(arguments.length==2){
    var html="<li class='btli' id='btli0'><a href='#' class='menuitem' addr='"+arguments[1]+"'>"+arguments[0]+"</a><div class='subdiv'><ul class='subul'><li class='fuhao'><a href='#'>+</a></li></ul></div></li><li class='btlif'><a href='#' class='jia'>+</a></li>";
  }else if(arguments.length==3){
    var html="<li class='btli' id='"+arguments[0]+"'><a href='#' class='menuitem' addr='"+arguments[2]+"'>"+arguments[1]+"</a><div class='subdiv'><ul class='subul'><li class='fuhao'><a href='#'>+</a></li></ul></div></li>";
  }
  return html;
}
function addSubMenu(num,value,address){
  var html="<li class='subli'><a href='#' addr='"+address+"' id='"+num+"'>"+value+"</a></li>";
  return html;
}
function getText(val) {
   var len = 0;
   for (var i = 0; i < val.length; i++) {
        var length = val.charCodeAt(i);
   if(length>=0&&length<=128){
         len += 1;
  }else{
         len += 2;
  }
}
  return len;
}