path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodReport.Services"
appcmd delete site "Wodeyun.Project.WoodReport.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodReport.Services"
appcmd set apppool "Wodeyun.Project.WoodReport.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodReport.Services"^
                /bindings:http/*:16011:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodReport.Services"
appcmd set site "Wodeyun.Project.WoodReport.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodReport.Services"
