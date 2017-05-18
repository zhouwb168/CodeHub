path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodLaboratory.Services"
appcmd delete site "Wodeyun.Project.WoodLaboratory.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodLaboratory.Services"
appcmd set apppool "Wodeyun.Project.WoodLaboratory.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodLaboratory.Services"^
                /bindings:http/*:16010:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodLaboratory.Services"
appcmd set site "Wodeyun.Project.WoodLaboratory.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodLaboratory.Services"
