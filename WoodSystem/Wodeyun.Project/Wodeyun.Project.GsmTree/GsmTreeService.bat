path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmTree.Services"
appcmd delete site "Wodeyun.Project.GsmTree.Services"

appcmd add apppool /name:"Wodeyun.Project.GsmTree.Services"
appcmd set apppool "Wodeyun.Project.GsmTree.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmTree.Services"^
                /bindings:http/*:14005:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmTree.Services"
appcmd set site "Wodeyun.Project.GsmTree.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmTree.Services"
