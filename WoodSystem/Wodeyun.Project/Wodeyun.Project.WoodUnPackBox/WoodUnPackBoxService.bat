path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodUnPackBox.Services"
appcmd delete site "Wodeyun.Project.WoodUnPackBox.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodUnPackBox.Services"
appcmd set apppool "Wodeyun.Project.WoodUnPackBox.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodUnPackBox.Services"^
                /bindings:http/*:16009:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodUnPackBox.Services"
appcmd set site "Wodeyun.Project.WoodUnPackBox.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodUnPackBox.Services"
