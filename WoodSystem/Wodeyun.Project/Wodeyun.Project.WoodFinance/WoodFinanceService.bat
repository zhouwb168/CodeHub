path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodFinance.Services"
appcmd delete site "Wodeyun.Project.WoodFinance.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodFinance.Services"
appcmd set apppool "Wodeyun.Project.WoodFinance.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodFinance.Services"^
                /bindings:http/*:16019:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodFinance.Services"
appcmd set site "Wodeyun.Project.WoodFinance.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodFinance.Services"
