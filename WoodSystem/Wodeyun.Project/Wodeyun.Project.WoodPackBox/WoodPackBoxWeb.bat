path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPackBox.Web"
appcmd delete site "Wodeyun.Project.WoodPackBox.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodPackBox.Web"
appcmd set apppool "Wodeyun.Project.WoodPackBox.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodPackBox.Web"^
                /bindings:http/*:17008:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodPackBox.Web"
appcmd set site "Wodeyun.Project.WoodPackBox.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodPackBox.Web"
