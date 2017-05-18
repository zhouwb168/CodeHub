path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodUnPackBox.Web"
appcmd delete site "Wodeyun.Project.WoodUnPackBox.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodUnPackBox.Web"
appcmd set apppool "Wodeyun.Project.WoodUnPackBox.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodUnPackBox.Web"^
                /bindings:http/*:17009:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodUnPackBox.Web"
appcmd set site "Wodeyun.Project.WoodUnPackBox.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodUnPackBox.Web"
