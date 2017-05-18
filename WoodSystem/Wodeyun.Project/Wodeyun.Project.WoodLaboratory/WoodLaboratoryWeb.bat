path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodLaboratory.Web"
appcmd delete site "Wodeyun.Project.WoodLaboratory.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodLaboratory.Web"
appcmd set apppool "Wodeyun.Project.WoodLaboratory.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodLaboratory.Web"^
                /bindings:http/*:17010:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodLaboratory.Web"
appcmd set site "Wodeyun.Project.WoodLaboratory.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodLaboratory.Web"
