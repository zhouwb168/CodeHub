path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodLaboratoryConfirme.Web"
appcmd delete site "Wodeyun.Project.WoodLaboratoryConfirme.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodLaboratoryConfirme.Web"
appcmd set apppool "Wodeyun.Project.WoodLaboratoryConfirme.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodLaboratoryConfirme.Web"^
                /bindings:http/*:17020:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodLaboratoryConfirme.Web"
appcmd set site "Wodeyun.Project.WoodLaboratoryConfirme.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodLaboratoryConfirme.Web"
