path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodMachine.Web"
appcmd delete site "Wodeyun.Project.WoodMachine.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodMachine.Web"
appcmd set apppool "Wodeyun.Project.WoodMachine.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodMachine.Web"^
                /bindings:http/*:17013:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodMachine.Web"
appcmd set site "Wodeyun.Project.WoodMachine.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodMachine.Web"
