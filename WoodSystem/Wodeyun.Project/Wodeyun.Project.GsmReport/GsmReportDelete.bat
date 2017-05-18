path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmReport.Services"
appcmd delete site "Wodeyun.Project.GsmReport.Services"

appcmd delete apppool "Wodeyun.Project.GsmReport.Web"
appcmd delete site "Wodeyun.Project.GsmReport.Web"
