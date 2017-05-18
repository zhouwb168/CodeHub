path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Factory.Services"
appcmd delete site "Wodeyun.Project.Factory.Services"

appcmd add apppool /name:"Wodeyun.Project.Factory.Services"
appcmd set apppool "Wodeyun.Project.Factory.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.Factory.Services"^
                /bindings:http/*:16007:^
                /physicalPath:"%cd%\Wodeyun.Project.Factory.Services"
appcmd set site "Wodeyun.Project.Factory.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.Factory.Services"
