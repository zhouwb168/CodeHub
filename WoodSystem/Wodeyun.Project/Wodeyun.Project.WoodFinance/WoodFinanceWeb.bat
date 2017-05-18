path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodFinance.Web"
appcmd delete site "Wodeyun.Project.WoodFinance.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodFinance.Web"
appcmd set apppool "Wodeyun.Project.WoodFinance.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodFinance.Web"^
                /bindings:http/*:17019:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodFinance.Web"
appcmd set site "Wodeyun.Project.WoodFinance.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodFinance.Web"
