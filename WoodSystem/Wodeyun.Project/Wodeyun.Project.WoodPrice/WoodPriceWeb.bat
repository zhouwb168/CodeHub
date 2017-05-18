path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPrice.Web"
appcmd delete site "Wodeyun.Project.WoodPrice.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodPrice.Web"
appcmd set apppool "Wodeyun.Project.WoodPrice.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodPrice.Web"^
                /bindings:http/*:17023:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodPrice.Web"
appcmd set site "Wodeyun.Project.WoodPrice.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodPrice.Web"
