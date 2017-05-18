path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodMachine.Services"
appcmd delete site "Wodeyun.Project.WoodMachine.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodMachine.Services"
appcmd set apppool "Wodeyun.Project.WoodMachine.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodMachine.Services"^
                /bindings:http/*:16013:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodMachine.Services"
appcmd set site "Wodeyun.Project.WoodMachine.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodMachine.Services"
