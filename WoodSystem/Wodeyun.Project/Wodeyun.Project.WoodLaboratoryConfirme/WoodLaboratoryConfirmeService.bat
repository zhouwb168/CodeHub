path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodLaboratoryConfirme.Services"
appcmd delete site "Wodeyun.Project.WoodLaboratoryConfirme.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodLaboratoryConfirme.Services"
appcmd set apppool "Wodeyun.Project.WoodLaboratoryConfirme.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodLaboratoryConfirme.Services"^
                /bindings:http/*:16020:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodLaboratoryConfirme.Services"
appcmd set site "Wodeyun.Project.WoodLaboratoryConfirme.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodLaboratoryConfirme.Services"
