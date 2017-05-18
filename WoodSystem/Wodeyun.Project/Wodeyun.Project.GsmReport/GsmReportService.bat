path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmReport.Services"
appcmd delete site "Wodeyun.Project.GsmReport.Services"

appcmd add apppool /name:"Wodeyun.Project.GsmReport.Services"
appcmd set apppool "Wodeyun.Project.GsmReport.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmReport.Services"^
                /bindings:http/*:14007:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmReport.Services"
appcmd set site "Wodeyun.Project.GsmReport.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmReport.Services"
