path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmReport.Web"
appcmd delete site "Wodeyun.Project.GsmReport.Web"

appcmd add apppool /name:"Wodeyun.Project.GsmReport.Web"
appcmd set apppool "Wodeyun.Project.GsmReport.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmReport.Web"^
                /bindings:http/*:15007:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmReport.Web"
appcmd set site "Wodeyun.Project.GsmReport.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmReport.Web"
